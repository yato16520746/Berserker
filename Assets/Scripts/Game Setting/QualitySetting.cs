using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualitySetting : MonoBehaviour
{
    [SerializeField] List<Button> _qualityButtons;

    [SerializeField] string _quality;
    string[] names;

#if UNITY_EDITOR
    private void OnValidate()
    {
        names = QualitySettings.names;
    }
#endif

    public void Button_Quality(int level)
    {
        QualitySettings.SetQualityLevel(level, true);
        //_quality = names[level];

        for (int i = 0; i < _qualityButtons.Count; i++)
        {
            if (i == level)
            {
                _qualityButtons[i].interactable = false;
            }
            else
            {
                _qualityButtons[i].interactable = true;
            }
        }
    }
}
