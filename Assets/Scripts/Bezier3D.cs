using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bezier3D : MonoBehaviour {
    public Vector3 start = new Vector3(0, 0, 0);
    public Vector3 end = new Vector3(1, 1, 0);
    public Vector3 handle1 = new Vector3(0, 1, 0);
    public Vector3 handle2 = new Vector3(1, 0, 0);
    public int resolution = 12;
    public float thickness = 0.25f;
    public float breadth = 0.25f;

    private void OnDrawGizmos() {
        var mesh = CreateMesh();
        Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation);

        Gizmos.DrawWireSphere(start + transform.position, 0.25f);
        Gizmos.DrawWireSphere(end + transform.position, 0.25f);
        Gizmos.DrawWireSphere(handle1 + transform.position, 0.25f);
        Gizmos.DrawWireSphere(handle2 + transform.position, 0.25f);

        /*Vector3 upNormal = new Vector3(0, -1, 0);
        float width = breadth / 2f;
        float height = thickness / 2f;
        float scaling = 1;

        for (int s = 0; s < resolution; s++) {
            float t0 = ((float) s) / resolution;
            float t1 = ((float) s + 1) / resolution;

            Vector3 p0 = PointOnPath(t0, start, handle1, handle2, end) + transform.position;
            Vector3 p1 = PointOnPath(t1, start, handle1, handle2, end) + transform.position;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(p0, p1);

            Vector3 dir1 = PointOnPath(Math.Max(0, t0 - 1f/resolution), start, handle1, handle2, end) - PointOnPath(Math.Min(1, t0 + 1f/resolution), start, handle1, handle2, end);
            Vector3 dir2 = PointOnPath(Math.Max(0, t1 - 1f/resolution), start, handle1, handle2, end) - PointOnPath(Math.Min(1, t1 + 1f/resolution), start, handle1, handle2, end);
            dir1.Normalize();
            dir2.Normalize();
            Vector3 right1 = Vector3.Cross(upNormal, dir1) * width;
            Vector3 right2 = Vector3.Cross(upNormal, dir2) * width;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(p0, p0 + right1);
            Gizmos.DrawLine(p0, p0 - right1);
            Gizmos.DrawLine(p0 + right1, p1 + right2);
            //Gizmos.DrawLine(p1 + right1, p2 + right2);
            Gizmos.DrawLine(p0 - right1, p1 - right2);
            //Gizmos.DrawLine(p1 - right1, p2 - right2);
        }*/
    }

    public void Start() {
        var mesh = CreateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    //cacluates point coordinates on a quadratic curve
    public static Vector3 PointOnPath(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
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

            Vector3 p0 = PointOnPath(t0, start, handle1, handle2, end);
            Vector3 p1 = PointOnPath(t1, start, handle1, handle2, end);

            Vector3 dir1 = PointOnPath(Math.Max(0, t0 - 1f/resolution), start, handle1, handle2, end) - PointOnPath(Math.Min(1, t0 + 1f/resolution), start, handle1, handle2, end);
            Vector3 dir2 = PointOnPath(Math.Max(0, t1 - 1f/resolution), start, handle1, handle2, end) - PointOnPath(Math.Min(1, t1 + 1f/resolution), start, handle1, handle2, end);
            dir1.Normalize();
            dir2.Normalize();
            Vector3 right1 = Vector3.Cross(upNormal, dir1) * width;
            Vector3 right2 = Vector3.Cross(upNormal, dir2) * width;

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
}
