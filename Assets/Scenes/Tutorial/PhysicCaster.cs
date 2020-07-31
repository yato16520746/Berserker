using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicCaster : MonoBehaviour
{
    enum CasterType
    {
        Ray, 
        Box,
        Phere
    }

    [SerializeField] CasterType _castertype;    

    private void OnDrawGizmos()
    {
        if (_castertype == CasterType.Ray)
        {
            float maxDistance = 10f;
            RaycastHit hit;

            bool isHit = Physics.Raycast(transform.position, transform.forward, out hit, maxDistance);
            if (isHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
            }
        }
        else if (_castertype == CasterType.Box)
        {
            float maxDistance = 10f;
            RaycastHit hit;

            bool isHit = Physics.BoxCast(transform.position, transform.lossyScale / 2, 
                transform.forward, out hit, transform.rotation, maxDistance);
            if (isHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
                Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance, transform.lossyScale);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
            }
        }
        else if (_castertype == CasterType.Phere)
        {
            float maxDistance = 10f;
            RaycastHit hit;

            bool isHit = Physics.SphereCast(transform.position, transform.lossyScale.x / 2, transform.forward, out hit,
                maxDistance);

            if (isHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
                Gizmos.DrawWireSphere(transform.position + transform.forward * hit.distance, transform.lossyScale.x / 2);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
            }
        }
    }
}
