using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class BezierMath {
    private static float TANGENT_RES = 10000f;

    //cacluates point coordinates on a list of quadratic curves
    public static Vector3 PointOnPath(float t, List<Vector3> controlPoints) {
        Assert.AreEqual(0, (controlPoints.Count - 1) % 3, "must be a list of cubic beziers sharing start and end, plus a start point");
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

    public static Vector3 TangentAt(float t, List<Vector3> controlPoints) {
        return PointOnPath(Math.Max(0, t + 1f/TANGENT_RES), controlPoints) - PointOnPath(Math.Min(1, t - 1f/TANGENT_RES), controlPoints);
    }

}
