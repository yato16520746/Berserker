using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerDamage : MonoBehaviour
{
    public int Damage = 10;
    [SerializeField] LayerMask _mask;
    [SerializeField] float _radius = 0.2f;
    [SerializeField] float _distance;
    [SerializeField] List<GameObject> _explosions;

    [Header("Audio")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _clipImpact;
    [SerializeField] float _volumeImpact = 1f;
    [SerializeField] AudioClip _clipShieldImpact;
    [SerializeField] float _volumeShieldImpact = 1f;


    [Space]
    [SerializeField] bool _debug = true;

    RaycastHit[] hits;
    List<GameObject> _alreadyHits;

    private void OnEnable()
    {
        if (_alreadyHits == null)
        {
            _alreadyHits = new List<GameObject>();
        }
        _alreadyHits.Clear();


    }

    void Update()
    {
        Vector3 position = transform.position;

        // check Paladin Shield
        RaycastHit hit2;
        bool isHit = Physics.SphereCast(position, _radius, transform.forward, out hit2, _distance, _mask);
        if (isHit)
        {
            if (hit2.collider.name == "Paladin Shield")
            {
                foreach (GameObject explosion in _explosions)
                {
                    if (!explosion.activeSelf)
                    {
                        explosion.transform.position = hit2.transform.position;
                        explosion.SetActive(true);

                        break;
                    }
                }

                if (_clipShieldImpact)
                {
                    _audioSource.PlayOneShot(_clipShieldImpact, _volumeShieldImpact);
                }

                gameObject.SetActive(false);
                return;
            }
        }

        //
        hits = Physics.SphereCastAll(position, _radius, transform.forward, _distance, _mask);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (_alreadyHits.Contains(hit.collider.gameObject))
                {
                    continue;
                }

                _alreadyHits.Add(hit.collider.gameObject);

                foreach (GameObject explosion in _explosions)
                {
                    if (!explosion.activeSelf)
                    {
                        explosion.transform.position = hit.transform.position;
                        explosion.SetActive(true);

                        break;
                    }
                }

                EnemyHealth eHealth = hit.collider.GetComponent<EnemyHealth>();
                if (eHealth)
                {
                    eHealth.AddDamage(Damage);

                    if (_clipImpact)
                    {
                        _audioSource.PlayOneShot(_clipImpact, _volumeImpact);
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!_debug)
        {
            return;
        }

        RaycastHit hit;
        Vector3 position = transform.position;
        bool isHit = Physics.SphereCast(position, _radius, transform.forward, out hit, _distance, _mask);

        if (isHit)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawRay(position, transform.forward * _distance);
        Gizmos.DrawWireSphere(position + transform.forward * _distance, _radius);

    }
#endif
}