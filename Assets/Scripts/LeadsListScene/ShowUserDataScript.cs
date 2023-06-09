using UnityEngine;
using UnityEngine.UI;

public class ShowUserDataScript : MonoBehaviour
{
    [SerializeField] private Text UserNameTxt;
    [SerializeField] private Text EmailTxt;

    private string nameUser;
    private string email;

    // Start is called before the first frame update
    void Start()
    {
        GetData();
        showData();
    }


    private void GetData()
    {
        nameUser = PlayerPrefs.GetString("name");
        email = PlayerPrefs.GetString("email");
    }

    private void showData()
    {
        UserNameTxt.text = nameUser;
        EmailTxt.text = email;
    }

}
