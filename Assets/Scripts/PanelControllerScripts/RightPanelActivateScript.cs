using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPanelActivateScript : MonoBehaviour
{

    [SerializeField] private GameObject WeatherPanel;


    public void DisibleAndEnableGameObject(int x)
    {

        this.DisapleAllPanel(x);

        if (x == 1)
        {
            WeatherPanel.SetActive(!WeatherPanel.activeInHierarchy);
        }
        

    }

    private void DisapleAllPanel(int x)
    {
        

    }
}
