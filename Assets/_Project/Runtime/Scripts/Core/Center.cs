using UnityEngine;

public class Center : MonoBehaviour
{
    [SerializeField] private Transform a;
    [SerializeField] private Transform b;
    
    void Update() {
        transform.position = (a.position + b.position) / 2f;
        transform.right = (a.position - b.position).normalized;
    }
}
