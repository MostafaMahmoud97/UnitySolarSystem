using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPanelActionButtonsScript : MonoBehaviour
{
    [SerializeField] private GameObject AddProductsPanel;
    [SerializeField] private GameObject MeasurePanel;
    [SerializeField] private GameObject SettingPanel;
    [SerializeField] private GameObject UserInformationPanel;


    public void DisibleAndEnableGameObject(int x)
    {

        this.DisapleAllPanel(x);

        if (x == 1)
        {
            AddProductsPanel.SetActive(!AddProductsPanel.activeInHierarchy);
        } else if(x == 2)
        {
            MeasurePanel.SetActive(!MeasurePanel.activeInHierarchy);
        }
        else if (x == 3)
        {
            SettingPanel.SetActive(!SettingPanel.activeInHierarchy);
        }

        if (!AddProductsPanel.activeInHierarchy && !MeasurePanel.activeInHierarchy && !SettingPanel.activeInHierarchy)
        {
            UserInformationPanel.SetActive(true);
        }
        else
        {
            UserInformationPanel.SetActive(false);
        }

    }

    private void DisapleAllPanel(int x)
    {
        if(x == 1)
        {
            MeasurePanel.SetActive(false);
            SettingPanel.SetActive(false);
        }
        else if(x == 2)
        {
            AddProductsPanel.SetActive(false);
            SettingPanel.SetActive(false);
        }
        else if (x == 3)
        {
            AddProductsPanel.SetActive(false);
            MeasurePanel.SetActive(false);
        }

    }
}
