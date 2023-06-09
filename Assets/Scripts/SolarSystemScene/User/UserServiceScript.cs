using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserServiceScript : MonoBehaviour
{

    [SerializeField] private Text UserName;
    [SerializeField] private Text UserEmail;
    [SerializeField] private Text LeadName;
    [SerializeField] private Text LeadAddress;

    // Start is called before the first frame update
    void Start()
    {
        GetData();
    }

    private void GetData()
    {
        UserName.text = PlayerPrefs.GetString("name");
        UserEmail.text = PlayerPrefs.GetString("email");
        LeadName.text = PlayerPrefs.GetString("lead_name");
        LeadAddress.text = PlayerPrefs.GetString("lead_name");
    }

    
    public void BackScene()
    {
        SceneManager.LoadScene("LeadsListScene");
    }

}
