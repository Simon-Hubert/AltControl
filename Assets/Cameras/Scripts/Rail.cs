using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField] private bool _isLoop;
    private Transform[] _nodes;
    private float _length;

    private void Start() {
        UpdateNodes();
    }

    public void UpdateNodes() {
        _nodes = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            _nodes[i] = transform.GetChild(i);
        }

        float l = 0;
        for (int i = 1; i < _nodes.Length; i++) {
            l += (_nodes[i - 1].position - _nodes[i].position).magnitude;
        }
        if (_isLoop) {
            l += (_nodes[^1].position - _nodes[0].position).magnitude;
        }
        
        _length = l;
    }
    
    public float GetLength() => _length;

    public Vector3 GetPosition(float distance) {
        float l = 0;
        
        UpdateNodes();
        if (_nodes == null || _nodes.Length <= 0) {
            throw new ArgumentException("_nodes est pas bon");
        }
        
        if (distance < 0) {
            return _nodes[0].position;
        }
        
        for (int i = 1; i < _nodes.Length; i++) {
            if (l + (_nodes[i - 1].position - _nodes[i].position).magnitude >= distance) {
                float remainder = distance - l;
                return _nodes[i-1].position + remainder * (_nodes[i].position - _nodes[i -1].position).normalized;
            }
            l += (_nodes[i - 1].position - _nodes[i].position).magnitude;
        }

        return _nodes[^1].position;
    }

    public Vector3 GetNearestPoint(Vector3 target) {
        Vector3 nearest = new Vector3();
        float minDist = float.MaxValue;
        for (int i = 1; i < _nodes.Length; i++) {
            Vector3 proj = MathUtils.GetNearestPointOnSegment(_nodes[i - 1].position, _nodes[i].position, target);
            float dist = (target - proj).magnitude;
            if ( dist < minDist) {
                minDist = dist;
                nearest = proj;
            }
        }

        return nearest;
    }
    

    private void OnDrawGizmos() {
        UpdateNodes();
        if (_nodes != null) {
            for (int i = 1; i < _nodes.Length; i++) {
                Gizmos.DrawLine(_nodes[i-1].position, _nodes[i].position);
            }
            if (_isLoop) {
                Gizmos.DrawLine(_nodes[^1].position, _nodes[0].position);
            }
        }
    }
}
