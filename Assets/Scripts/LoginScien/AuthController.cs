using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AuthController : MonoBehaviour
{

    [Serializable]
    public class User
    {
        public int id;
        public string name;
        public string email;
        public string access_token;
    }

    [Serializable]
    public class ErrorMessage
    {
        public string error;
        public int code;
    }

    [SerializeField] private GameObject ClosePanel;
    [SerializeField] private InputField Email;
    [SerializeField] private InputField Password;

    private string LoginUrl = "http://crm.boxbyld.tech/unity/v1/user/login";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableAndDesableClosePanel ()
    {
        ClosePanel.SetActive(!ClosePanel.activeInHierarchy);
    }

    public void CloseAPP()
    {
        Application.Quit();
    }

    public void Login()
    {
        if(Email.text != "" && Email.text.Contains('@') && Password.text != "" && Password.text.Length >= 8)
        {
            StartCoroutine(LoginRequest());
        }
        else
        {
            Debug.Log("Please enter correct email and Password");
        }
        
    }

    IEnumerator LoginRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", Email.text);
        form.AddField("password", Password.text);
        UnityWebRequest www = UnityWebRequest.Post(LoginUrl,form);

        www.SetRequestHeader("Accept", "application/json");

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

            // Deserialize the response into an object
            User responseObject = JsonUtility.FromJson<User>(responseText);

            DeleteAllDataSave();
            SaveData(responseObject);
            loadNextScene();

        }
    }


    private void SaveData(User user)
    {
        PlayerPrefs.SetInt("id", user.id);
        PlayerPrefs.SetString("name", user.name);
        PlayerPrefs.SetString("email", user.email);
        PlayerPrefs.SetString("access_token", user.access_token);
        PlayerPrefs.Save();
    }

    private void DeleteAllDataSave()
    {
        PlayerPrefs.DeleteAll();
    }

    private void loadNextScene()
    {
        SceneManager.LoadScene("LeadsListScene");
    }
}
