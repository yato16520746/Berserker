using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMousePosition : MonoBehaviour
{
    [SerializeField] RectTransform _rectTransform;

    private void Start()
    {
        //Cursor.visible = false;

//#if UNITY_EDITOR
//#else
//#endif
    }

    void LateUpdate()
    {
        _rectTransform.position = Input.mousePosition;
    }
}
