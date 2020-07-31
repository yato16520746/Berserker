using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonB_WarningLine : MonoBehaviour
{
    [SerializeField] Transform _parent;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Transform _shootPoint;
    [SerializeField] float _maxRange = 30f;

    readonly float _timeCheck = 0.2f;
    float _count = 0f;

    void Update()
    {
        _count -= Time.deltaTime;
        if (_count < 0f)
        {
            Check();
            _count = _timeCheck;
        }    
    }

    void Check()
    {
        Vector3 point2 = new Vector3(0f, 0f, 0f);

        // check obstacles
        Ray ray = new Ray();
        ray.origin = _shootPoint.position;
        ray.direction = _parent.transform.rotation * Vector3.forward;

        RaycastHit hit;
        int mask = LayerMask.GetMask(Layer.Environment);

        if (Physics.Raycast(ray, out hit, _maxRange, mask))
        {
            // phía trước có gì đó
            point2.z = (hit.point - transform.position).magnitude;
        }
        else
        {
            // phía trước ko có gì
            point2.z = _maxRange;
        }

        _lineRenderer.SetPosition(1, point2);
    }

    void Event_Disable()
    {
        gameObject.SetActive(false);
    }
}
