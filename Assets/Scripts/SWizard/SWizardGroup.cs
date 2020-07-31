using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWizardGroup : MonoBehaviour
{
    [SerializeField] GameObject _wizard;
    [SerializeField] List<GameObject> _meteors;

    bool _active = false;

    private void OnEnable()
    {
        _active = true;
        _wizard.SetActive(true);
    }

    void Update()
    {
        if (_active)
        {
            if (!_wizard.activeSelf && !_meteors[0].activeSelf && !_meteors[0].activeSelf)
            {
                gameObject.SetActive(false);
                _active = false;
            }
        }
    }
}
