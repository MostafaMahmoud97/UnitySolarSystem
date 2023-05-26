using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPanelActivateScript : MonoBehaviour
{

    [SerializeField] private GameObject WeatherPanel;
    [SerializeField] private GameObject AddProductSolarPanel;
    [SerializeField] private GameObject ControllerSolarPanel;
    


    public void DisibleAndEnableGameObject(int x)
    {

        this.DisapleAllPanel(x);

        if (x == 1)
        {
            WeatherPanel.SetActive(!WeatherPanel.activeInHierarchy);
        }else if(x == 2)
        {
            AddProductSolarPanel.SetActive(!AddProductSolarPanel.activeInHierarchy);
        }else if(x == 3)
        {
            ControllerSolarPanel.SetActive(!ControllerSolarPanel.activeInHierarchy);
        }
        

    }

    private void DisapleAllPanel(int x)
    {
        if (x == 1)
        {
            AddProductSolarPanel.SetActive(false);
            ControllerSolarPanel.SetActive(false);
        }
        else if(x == 2)
        {
            WeatherPanel.SetActive(false);
            ControllerSolarPanel.SetActive(false);
        }
        else if(x == 3)
        {
            AddProductSolarPanel.SetActive(false);
            WeatherPanel.SetActive(false);
        }

    }
}
