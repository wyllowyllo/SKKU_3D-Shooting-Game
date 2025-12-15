using System;
using UnityEngine;

public class TraceController : MonoBehaviour
{
    [Header("타겟")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _detectDistance = 5f;

    public Transform Target => _target;
    public float DistanceFromTarget => Vector3.Distance(transform.position, _target.position);
    public bool Detected => DistanceFromTarget <= _detectDistance;
    public Vector3 TargetPosition => _target.position;
    public float DetectDistance => _detectDistance;

    private void Awake()
    {
        if (_target == null)
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

  
   
   
}