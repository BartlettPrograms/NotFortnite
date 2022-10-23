using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickHelper : MonoBehaviour
{
    [SerializeField] private Image[] JoystickInterferance;
    
    public void ButtonHitOff()
    {
        foreach (Image image in JoystickInterferance)
        {
            image.raycastTarget = false;
        }
    }

    public void ButtonHitOn()
    {
        foreach (Image image in JoystickInterferance)
        {
            image.raycastTarget = true;
        }
    }
}
