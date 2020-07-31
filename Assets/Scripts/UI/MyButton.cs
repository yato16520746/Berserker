using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour
{
    bool _press;
    public bool Press { get { return _press; } }

    public void BUTTON_Down()
    {
        _press = true;
    }

    public void BUTTON_Up()
    {
        _press = false;
    }
}
