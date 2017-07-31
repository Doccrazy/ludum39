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
    private List<GameObject> _waypoints;
    public GameObject waypointObj;

    public List<GameObject> Waypoints {
        get { return _waypoints; }
    }

#if UNITY_EDITOR
    // instead of @script ExecuteInEditMode()
    [ContextMenu("Update mesh")]
    public void UpdateMesh() {
        Debug.Log("Constructing sQuad from ContextMenu");
        var mesh = CreateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
#endif

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        var waypointsVects = GenerateWaypoints();
        foreach (var wp in waypointsVects) {
            Gizmos.DrawWireSphere(wp + transform.position, 0.5f);
        }

//        var controlPoints = AdaptControlPoints();
//
//        Gizmos.color = Color.red;
//        foreach (var cp in controlPoints) {
//            Gizmos.DrawWireSphere(cp + transform.position, 0.25f);
//        }
//        Vector3 upNormal = new Vector3(0, -1, 0);
//        float width = breadth / 2f;
//        float height = thickness / 2f;
//        float scaling = 1;
//
//        for (int s = 0; s < resolution; s++) {
//            float t0 = ((float) s) / resolution;
//            float t1 = ((float) s + 1) / resolution;
//
//            Vector3 p0 = PointOnPath(t0, controlPoints) + transform.position;
//            Vector3 p1 = PointOnPath(t1, controlPoints) + transform.position;
//
//            Gizmos.color = Color.green;
//            Gizmos.DrawLine(p0, p1);
//
//            Vector3 dir1 = PointOnPath(Math.Max(0, t0 - 1f/resolution), controlPoints) - PointOnPath(Math.Min(1, t0 + 1f/resolution), controlPoints);
//            Vector3 dir2 = PointOnPath(Math.Max(0, t1 - 1f/resolution), controlPoints) - PointOnPath(Math.Min(1, t1 + 1f/resolution), controlPoints);
//            dir1.Normalize();
//            dir2.Normalize();
//            Vector3 right1 = Vector3.Cross(upNormal, dir1) * width;
//            Vector3 right2 = Vector3.Cross(upNormal, dir2) * width;
//
//            Gizmos.color = Color.red;
//            Gizmos.DrawLine(p0, p0 + right1);
//            Gizmos.DrawLine(p0, p0 - right1);
//            Gizmos.DrawLine(p0 + right1, p1 + right2);
//            //Gizmos.DrawLine(p1 + right1, p2 + right2);
//            Gizmos.DrawLine(p0 - right1, p1 - right2);
//            //Gizmos.DrawLine(p1 - right1, p2 - right2);
//        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        foreach (var p in points) {
            Gizmos.DrawWireSphere(p + transform.position, 0.5f);
        }

        Gizmos.color = Color.white;
        var mesh = CreateMesh();
        Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation);
    }

    private List<Vector3> AdaptControlPoints() {
        List<Vector3> controlPoints = new List<Vector3>();
        //generate the end and control points
        for (int i = 0; i < points.Length; i++) {
            if ((i > 2 && i < points.Length - 1 && (i + 1) % 2 == 0 && points.Length > 4) ||
                (i == points.Length - 1 && i > 3 && i % 2 == 0)) {
                controlPoints.Add((points[i - 1] + points[i]) / 2f);
            }
            controlPoints.Add(points[i]);
        }
        return controlPoints;
    }

    public void Start() {
//        var mesh = CreateMesh();
//        GetComponent<MeshFilter>().mesh = mesh;
//        GetComponent<MeshCollider>().sharedMesh = mesh;
        _waypoints = GenerateWaypointObjs();
    }

    private List<Vector3> GenerateWaypoints() {
        List<Vector3> waypoints = new List<Vector3>();
        var controlPoints = AdaptControlPoints();
        for (int s = 0; s < waypointResolution; s++) {
            float t = ((float) s) / waypointResolution;
            waypoints.Add(PointOnPath(t, controlPoints));
        }
        return waypoints;
    }

    private List<GameObject> GenerateWaypointObjs() {
        List<GameObject> waypoints = new List<GameObject>();
        var controlPoints = AdaptControlPoints();
        for (int s = 0; s < waypointResolution; s++) {
            float t = ((float) s) / waypointResolution;
            Vector3 dir = TangentAt(t, controlPoints);
            var p = PointOnPath(t, controlPoints);
            var o = Instantiate(waypointObj, transform, false);
            o.transform.localPosition = p;
            o.transform.localRotation = Quaternion.LookRotation(dir, Vector3.up);
            waypoints.Add(o);
        }
        return waypoints;
    }

    //cacluates point coordinates on a quadratic curve
    public static Vector3 PointOnPath(float t, List<Vector3> controlPoints) {
        int bezierCount = (controlPoints.Count - 1) / 3;
        int nBezier = Math.Min((int) (t * bezierCount), bezierCount - 1);
        int startIdx = nBezier * 3;
        Vector3 p0 = controlPoints[startIdx];
        Vector3 p1 = controlPoints[startIdx + 1];
        Vector3 p2 = controlPoints[startIdx + 2];
        Vector3 p3 = controlPoints[startIdx + 3];

        t = (t - (nBezier * 1f / bezierCount)) * bezierCount;

        float u, uu, uuu, tt, ttt;
        Vector3 p;

        u = 1 - t;
        uu = u * u;
        uuu = uu * u;

        tt = t * t;
        ttt = tt * t;

        p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    public Mesh CreateMesh() {
        var controlPoints = AdaptControlPoints();

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

            Vector3 p0 = PointOnPath(t0, controlPoints);
            Vector3 p1 = PointOnPath(t1, controlPoints);

            Vector3 dir1 = TangentAt(t0, controlPoints);
            Vector3 dir2 = TangentAt(t1, controlPoints);
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

    private Vector3 TangentAt(float t0, List<Vector3> controlPoints) {
        return PointOnPath(Math.Max(0, t0 + 1f/resolution), controlPoints) - PointOnPath(Math.Min(1, t0 - 1f/resolution), controlPoints);
    }

    public Vector3 GetPointAt(float t) {
        return PointOnPath(Math.Min(Math.Max(0, t), 1f), AdaptControlPoints()) + transform.position;
    }

    public void UpdateTransformTo(Transform tf, float t, float offset = 0) {
        var controlPoints = AdaptControlPoints();
        Vector3 p = PointOnPath(t % 1f, controlPoints);
        Vector3 dir = TangentAt(t, controlPoints);
        tf.position = transform.TransformPoint(p + Vector3.Cross(dir, Vector3.up).normalized * offset);
        tf.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    public float GetProgress(Vector3 position) {
        var controlPoints = AdaptControlPoints();
        Vector3 objPos = transform.InverseTransformPoint(position);
        float minMag = (PointOnPath(0, controlPoints) - objPos).sqrMagnitude;
        float minT = 0;
        for (int s = 0; s < waypointResolution; s++) {
            float t = ((float) s) / waypointResolution;
            var mag = (PointOnPath(t, controlPoints) - objPos).sqrMagnitude;
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
