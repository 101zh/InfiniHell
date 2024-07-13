using FastBezier;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvingProjectile : Projectile
{
    [SerializeField] private Transform target;//the destination
    [SerializeField] private Vector3 offset = new Vector3(4.58f, 4.52f, 0);

    private const float DistanceToTarget = 1;

    private Vector3 _initialPosition;
    private List<Vector3> _allPositions;
    private int _counter;

    private void Start()
    {
        _initialPosition = transform.position;
        _allPositions = new List<Vector3>(100);

        for (var i = 0; i < 100; i++)
        {
            var newPosition = CubicCurve(_initialPosition, _initialPosition + offset, _initialPosition + offset,
                target.position, (float)i / 100);
            _allPositions.Add(newPosition);
        }
    }

    private void Update()
    {
        if (_counter < _allPositions.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, _allPositions[_counter], speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _allPositions[_counter]) < DistanceToTarget) _counter++;
        }
    }

    private Vector3 CubicCurve(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end, float t)
    {
        return (((-start + 3 * (control1 - control2) + end) * t + (3 * (start + control2) - 6 * control1)) * t +
                3 * (control1 - start)) * t + start;
    }

    //since _initialPosition is set on start, the drawn curve is from (0,0,0) if the code is not executed
    private void OnDrawGizmosSelected()
    {
        _initialPosition = transform.position;
        for (var i = 0; i < 100; i += 10)
        {
            var newPosition = CubicCurve(_initialPosition, _initialPosition + offset, _initialPosition + offset,
                target.position, (float)i / 100);
            Gizmos.DrawSphere(newPosition, 0.2f);
        }
    }

    double ApproxLength()
    {
        return new FastBezier.Bezier2(new V3D(_initialPosition), new V3D(_initialPosition + offset), new V3D(target.position)).Length; ;
    }
}
