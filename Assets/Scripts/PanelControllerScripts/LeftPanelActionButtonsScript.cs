using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPanelActionButtonsScript : MonoBehaviour
{
    [SerializeField] private GameObject AddProductsPanel;
    [SerializeField] private GameObject MeasurePanel;


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


    }

    private void DisapleAllPanel(int x)
    {
        if(x == 1)
        {
            MeasurePanel.SetActive(false);
        }else if(x == 2)
        {
            AddProductsPanel.SetActive(false);
        }

    }
}
