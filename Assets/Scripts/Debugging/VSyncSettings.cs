using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSyncSettings : MonoBehaviour
{
    public void OnButton(int i)
    {
        switch (i)
        {
            case 1:
                Application.targetFrameRate = 60;
                break;
            case 2:
                QualitySettings.vSyncCount = 1;
                break;
            default:
                break;
        }
    }
}
