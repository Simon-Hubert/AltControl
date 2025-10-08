using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Curve
{
    [SerializeField] private Vector3 a, b, c, d;

    private Vector3 GetPosition(float t) {
        return MathUtils.CubicBezier(a, b, c, d, t);
    }

    public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix) {
        return localToWorldMatrix.MultiplyPoint(GetPosition(t));
    }

    public void DrawGizmo(Color col, Matrix4x4 localToWorldMatrix) {
        Vector3 worldA = localToWorldMatrix.MultiplyPoint(a);
        Vector3 worldB = localToWorldMatrix.MultiplyPoint(b);
        Vector3 worldC = localToWorldMatrix.MultiplyPoint(c);
        Vector3 worldD = localToWorldMatrix.MultiplyPoint(d);

        Gizmos.color = col;
        Gizmos.DrawSphere(worldA, 0.1f);
        Gizmos.DrawSphere(worldB, 0.1f);
        Gizmos.DrawSphere(worldC, 0.1f);
        Gizmos.DrawSphere(worldD, 0.1f);

        Handles.color = col;
        Handles.DrawDottedLine(worldA,worldB, 5f);
        Handles.DrawDottedLine(worldC,worldD, 5f);
        
        
        Handles.DrawBezier(worldA, worldD, worldB, worldC, col, Texture2D.whiteTexture, 1f);
    }
}
