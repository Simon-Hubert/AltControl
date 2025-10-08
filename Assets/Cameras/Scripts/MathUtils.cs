using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static Vector3 GetNearestPointOnSegment(Vector3 a, Vector3 b, Vector3 target) {
        Vector3 n = (b - a).normalized;
        float dot = Vector3.Dot(n, target - a);
        dot = Mathf.Clamp(dot, 0, (b - a).magnitude);
        return a + n * dot;
    }

    public static Vector3 LinearBezier(Vector3 a, Vector3 b, float t) {
        return Vector3.Lerp(a, b, t);
    }

    public static Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t) {
        return LinearBezier(LinearBezier(a, b, t), LinearBezier(b, c, t), t);
    }
    
    public static Vector3 CubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d,  float t) {
        return LinearBezier(QuadraticBezier(a, b, c, t), QuadraticBezier(b, c, d, t), t);
    }
}
