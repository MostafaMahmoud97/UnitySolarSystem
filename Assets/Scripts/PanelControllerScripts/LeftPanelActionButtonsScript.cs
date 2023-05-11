using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPanelActionButtonsScript : MonoBehaviour
{
    [SerializeField] private GameObject AddProductsPanel;


    public void DisibleAndEnableGameObject(int x)
    {

        this.DisapleAllPanel(x);

        if (x == 1)
        {
            AddProductsPanel.SetActive(!AddProductsPanel.activeInHierarchy);
        }


    }

    private void DisapleAllPanel(int x)
    {


    }
}
