using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (MeshFilter), typeof (MeshRenderer))]
public class Bezier3D2 : MonoBehaviour {
    public int resolution = 12;
    public int waypointResolution = 12;
    public float thickness = 0.25f;
    public float breadth = 0.25f;
    public Vector3[] points = new Vector3[4];
    private List<Vector3> controlPoints;

#if UNITY_EDITOR
    // instead of @script ExecuteInEditMode()
    [ContextMenu("Update mesh")]
    public void UpdateMesh() {
        Debug.Log("Constructing sQuad from ContextMenu");
        controlPoints = AdaptControlPoints();
        var mesh = CreateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
#endif

    private void OnDrawGizmos() {
        controlPoints = AdaptControlPoints();
        Gizmos.color = Color.blue;
        var waypointsVects = GenerateWaypoints();
        foreach (var wp in waypointsVects) {
            Gizmos.DrawWireSphere(wp + transform.position, 0.5f);
        }
    }

    private void OnDrawGizmosSelected() {
        controlPoints = AdaptControlPoints();
        Gizmos.color = Color.green;
        foreach (var p in points) {
            Gizmos.DrawWireSphere(p + transform.position, 0.5f);
        }

        Gizmos.color = Color.white;
        var mesh = CreateMesh();
        Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation);
    }

    private List<Vector3> AdaptControlPoints() {
        List<Vector3> controls = new List<Vector3>();
        //generate the end and control points
        for (int i = 0; i < points.Length; i++) {
            if ((i > 2 && i < points.Length - 1 && (i + 1) % 2 == 0 && points.Length > 4) ||
                (i == points.Length - 1 && i > 3 && i % 2 == 0)) {
                controls.Add((points[i - 1] + points[i]) / 2f);
            }
            controls.Add(points[i]);
        }
        return controls;
    }

    public void Awake() {
//        var mesh = CreateMesh();
//        GetComponent<MeshFilter>().mesh = mesh;
//        GetComponent<MeshCollider>().sharedMesh = mesh;
        controlPoints = AdaptControlPoints();
    }

    private List<Vector3> GenerateWaypoints() {
        List<Vector3> waypoints = new List<Vector3>();
        for (int s = 0; s < waypointResolution; s++) {
            float t = ((float) s) / waypointResolution;
            waypoints.Add(BezierMath.PointOnPath(t, controlPoints));
        }
        return waypoints;
    }

    public Mesh CreateMesh() {
        Mesh mesh;

        mesh = new Mesh();

        float scaling = 1;
        float width = breadth / 2f;
        float height = thickness / 2f;
        List<Vector3> vertList = new List<Vector3>();
        List<int> triList = new List<int>();
        List<Vector2> uvList = new List<Vector2>();
        Vector3 upNormal = new Vector3(0, -1, 0);

        triList.AddRange(new int[] {
            2, 1, 0, //start face
            0, 3, 2
        });

        float length = 0;
        for (int s = 0; s < resolution; s++) {
            float t0 = ((float) s) / resolution;
            float t1 = ((float) s + 1) / resolution;

            Vector3 p0 = BezierMath.PointOnPath(t0, controlPoints);
            Vector3 p1 = BezierMath.PointOnPath(t1, controlPoints);

            Vector3 dir1 = BezierMath.TangentAt(t0, controlPoints);
            Vector3 dir2 = BezierMath.TangentAt(t1, controlPoints);
            dir1.Normalize();
            dir2.Normalize();
            Vector3 right1 = -Vector3.Cross(upNormal, dir1).normalized * width;
            Vector3 right2 = -Vector3.Cross(upNormal, dir2).normalized * width;

            Vector3 up = upNormal * height;

            int curTriIdx = vertList.Count;

            Vector3[] segmentVerts = new Vector3[] {
                p0 - right1 - up,
                p0 - right1 + up,
                p0 + right1 + up,
                p0 + right1 - up,
            };
            vertList.AddRange(segmentVerts);

            Vector2[] uvs = new Vector2[] {
                new Vector2(0, length),
                new Vector2(0, length),
                new Vector2(1, length),
                new Vector2(1, length)
            };
            uvList.AddRange(uvs);

            int[] segmentTriangles = new int[] {
                curTriIdx + 6, curTriIdx + 5, curTriIdx + 1, //left face
                curTriIdx + 1, curTriIdx + 2, curTriIdx + 6,
                curTriIdx + 7, curTriIdx + 3, curTriIdx + 0, //right face
                curTriIdx + 0, curTriIdx + 4, curTriIdx + 7,
                curTriIdx + 1, curTriIdx + 5, curTriIdx + 4, //top face
                curTriIdx + 4, curTriIdx + 0, curTriIdx + 1,
                curTriIdx + 3, curTriIdx + 7, curTriIdx + 6, //bottom face
                curTriIdx + 6, curTriIdx + 2, curTriIdx + 3
            };
            triList.AddRange(segmentTriangles);

            length += (p1 - p0).magnitude / breadth;

            // final segment fenceposting: finish segment and add end face
            if (s == resolution - 1) {
                curTriIdx = vertList.Count;

                vertList.AddRange(new Vector3[] {
                    p1 - right2 - up,
                    p1 - right2 + up,
                    p1 + right2 + up,
                    p1 + right2 - up,
                });

                uvList.AddRange(new Vector2[] {
                        new Vector2(0, length),
                        new Vector2(0, length),
                        new Vector2(1, length),
                        new Vector2(1, length)
                    }
                );
                triList.AddRange(new int[] {
                    curTriIdx + 0, curTriIdx + 1, curTriIdx + 2, //end face
                    curTriIdx + 2, curTriIdx + 3, curTriIdx + 0
                });
            }
        }

        mesh.vertices = vertList.ToArray();
        mesh.triangles = triList.ToArray();
        mesh.uv = uvList.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        //mesh.Optimize();

        return mesh;
    }

    public Vector3 TangentAt(float t) {
        return BezierMath.TangentAt(t, controlPoints);
    }

    public Vector3 GetPointAt(float t, float offset = 0) {
        Vector3 p = BezierMath.PointOnPath(Mathf.Clamp(t, 0, 1f), controlPoints);
        Vector3 dir = TangentAt(t);
        return transform.TransformPoint(p + Vector3.Cross(dir, Vector3.up).normalized * offset);
    }

    public void UpdateTransformTo(Transform tf, float t, float offset = 0) {
        Vector3 transformedPoint = GetPointAt(t, offset);
        Vector3 dir = TangentAt(t);
        tf.position = transformedPoint;
        tf.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    public float GetProgress(Vector3 position) {
        Vector3 objPos = transform.InverseTransformPoint(position);
        float minMag = (BezierMath.PointOnPath(0, controlPoints) - objPos).sqrMagnitude;
        float minT = 0;
        for (int s = 0; s < waypointResolution; s++) {
            float t = ((float) s) / waypointResolution;
            var mag = (BezierMath.PointOnPath(t, controlPoints) - objPos).sqrMagnitude;
            if (mag < minMag) {
                minT = t;
                minMag = mag;
            }
        }
        return minT;
    }

    public Vector3 GetClosestPoint(Vector3 position) {
        float t = GetProgress(position);
        return GetPointAt(t);
    }
}
