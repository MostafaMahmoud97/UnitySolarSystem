using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthControllerScript : MonoBehaviour
{
   
    public void Logout()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("LoginScene");
    }
}
