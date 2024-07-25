using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
[ExecuteInEditMode()]
public class SplineSampler : MonoBehaviour
{
    [SerializeField]
    private SplineContainer _splineContainer;

    [SerializeField]
    private int _splineIndex;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float _time;

    float3 position;
    float3 tangent;
    float3 upVector;

    // Update is called once per frame
    private void Update()
    {
        _splineContainer.Evaluate(_splineIndex, _time, out position, out tangent, out upVector);
    }

    private void onDrawGizmos()
    {
        Handles.matrix = transform.localToWorldMatrix;
        Handles.SphereHandleCap(0, position, Quaternion.identity, 0.1f, EventType.Repaint);
    }
}
