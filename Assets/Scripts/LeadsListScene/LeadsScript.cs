using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using static AuthController;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeadsScript : MonoBehaviour
{

    [SerializeField] private GameObject LeadCardPrefabe;
    [SerializeField] private Transform CardHolder;
    [SerializeField] private InputField SearchInput;

    [Serializable]
    public class Lead
    {
        public int id;
        public string full_name;
        public string address;
        public float lat;
        public float lon;
    }

    private string InitUrl = "https://crm.boxbyld.tech/unity/v1/leads?search=";
    private string URL;
    private string access_token;

    private List<GameObject> leadsArrayGameObject;


    private RaycastHit raycastHit;

    // Start is called before the first frame update
    void Start()
    {
        leadsArrayGameObject = new List<GameObject>();
        GetAccessToken();
        URL = InitUrl;
        StartCoroutine(GetLeads());
    }
    private void GetAccessToken()
    {
        access_token = "Bearer ";
        access_token += PlayerPrefs.GetString("access_token");
    }

    public void SearchLead()
    {
        DeleteAllLeads();
        URL = InitUrl+SearchInput.text;
        StartCoroutine(GetLeads());

    }

    IEnumerator GetLeads()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);

        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Authorization", access_token);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            // Access the response data as string
            string responseText = www.downloadHandler.text;

            // Deserialize the response into an object
            ErrorMessage responseObject = JsonUtility.FromJson<ErrorMessage>(responseText);
            Debug.Log(responseObject.error);
        }
        else
        {
            // Access the response data as string
            string responseText = www.downloadHandler.text;
            JSONArray jsonArray = JSON.Parse(responseText) as JSONArray;

            for (int i = 0; i < jsonArray.Count; i++)
            {
                JSONNode arrayElement = jsonArray[i];
                Lead lead = new Lead();
                lead.id = arrayElement["id"];
                lead.full_name = arrayElement["full_name"];
                lead.lat = arrayElement["lat"];
                lead.lon = arrayElement["lng"];
                lead.address = arrayElement["location"];
                InstansiateCard(lead);
            }
        }
    }

    private void InstansiateCard(Lead lead)
    {
        GameObject NewLead = Instantiate(LeadCardPrefabe, CardHolder);
        NewLead.transform.GetChild(1).gameObject.GetComponent<Text>().text = lead.full_name;
        NewLead.transform.GetChild(2).gameObject.GetComponent<Text>().text = lead.address;
        NewLead.GetComponent<Button>().onClick.AddListener(() => OnClickToSelectCard(lead));
        
        leadsArrayGameObject.Add(NewLead);
    }


    private void DeleteAllLeads()
    {

        
        foreach (GameObject g in leadsArrayGameObject)
        {
            Destroy(g);
        }
    }

    public void OnClickToSelectCard(Lead x)
    {
        if (x.lat != 0 && x.lon != 0)
        {
            PlayerPrefs.SetFloat("lat", x.lat);
            PlayerPrefs.SetFloat("lon", x.lon);
            PlayerPrefs.SetString("lead_name", x.full_name);
            PlayerPrefs.SetString("address", x.address);
            PlayerPrefs.Save();
            SceneManager.LoadScene("SolarSystemScene");
        }
        else
        {
            Debug.Log("No Lat And Lon For This Lead");
        }
        
    }
    
   
}
