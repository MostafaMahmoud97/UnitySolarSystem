using Microsoft.Maps.Unity;
using UnityEngine;
using UnityEngine.UI;

public class InverterScript : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject InverterGameObject;
    [SerializeField] private GameObject ControllerInverterPanel;

    [Header("Map Render")]
    public GameObject map;

    [Header("Suggest Panel Message")]
    [SerializeField] private GameObject SuggestationPanel;
    [SerializeField] private Text SuggestInverterText;
    [SerializeField] private Text CurrentInverterText;
    [SerializeField] private GameObject WarningPanel;
    [SerializeField] private Text WarningText;

    private RaycastHit raycastHit;


    private bool AddInverterCheck;
    private bool SelectedInverterCheck;
    private bool MoveInverterCheck;

    private GameObject InitInverter;

    [HideInInspector] public int NumberInverter;
    private int NumberSuggestInverter;

    private bool CheckRotateYAxisDown;
    private bool CheckRotateYAxisUp;

    private void Awake()
    {
        AddInverterCheck = false;
        SelectedInverterCheck = false;
        MoveInverterCheck = false;

        CheckRotateYAxisDown = false;
        CheckRotateYAxisUp = false;
        NumberInverter = 0;
        NumberSuggestInverter = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        DesplayTextSuggestation();
    }

    // Update is called once per frame
    void Update()
    {
        SelectInverter();
        
    }

    private void FixedUpdate()
    {
        AddInverter();
        MoveInverter();

        RotateYAxisUp();
        RotateYAxisDown();
    }

    public void CheckAddInverter()
    {
        if(NumberSuggestInverter > NumberInverter)
        {
            AddInverterCheck = !AddInverterCheck;
            if (AddInverterCheck)
            {
                InitInverter = Instantiate(InverterGameObject, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                Destroy(InitInverter);
            }
        }
        else
        {
            ShowWarningLetterPanel("You Can't Add Inverter Number Suggest : " + NumberSuggestInverter + " And Curent Number : " + NumberInverter);

        }

    }

    private void AddInverter()
    {
        if (AddInverterCheck && !SelectedInverterCheck && !MoveInverterCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(map.GetComponent<MapRenderer>().Raycast(ray,out MapRendererRaycastHit raycastHit))
            {
                InitInverter.transform.position = new Vector3(raycastHit.Point.x,raycastHit.Point.y,raycastHit.Point.z);
                InitInverter.transform.Translate(new Vector3(0, 0, 0.0031485f));
                if (Input.GetMouseButtonDown(0))
                {
                    NumberInverter++;
                    AddInverterCheck = false;
                    CurrentInverterText.text = "Number Current Inverter : " + NumberInverter;
                }
            }
        }
    }

    private void SelectInverter()
    {
        if(Input.GetMouseButtonDown(0) && !AddInverterCheck && !MoveInverterCheck) 
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out raycastHit))
            {
                if(raycastHit.transform.tag == "Inverter")
                {
                    if (!SelectedInverterCheck)
                    {
                        SelectedInverterCheck = true;
                        ControllerInverterPanel.SetActive(true);
                    }
                    else
                    {
                        SelectedInverterCheck = false;
                        ControllerInverterPanel.SetActive(false);
                    }
                }
            }
        }
    }

    public void CheckMoveInverter()
    {
        MoveInverterCheck = true;
    }

    private void MoveInverter()
    {
        if (MoveInverterCheck)
        {
            Ray ray = cam.ScreenPointToRay (Input.mousePosition); 
            if(map.GetComponent<MapRenderer>().Raycast(ray,out MapRendererRaycastHit raycastHit))
            {
                InitInverter.transform.position = new Vector3(raycastHit.Point.x,raycastHit.Point.y,raycastHit.Point.z);
                InitInverter.transform.Translate(new Vector3(0, 0, 0.0031485f));
                if (Input.GetMouseButtonDown(0))
                {
                    MoveInverterCheck = false;
                }
            }
        }
    }

    public void CheckRotateYAxisUpMethodActive()
    {
        CheckRotateYAxisUp = true;
    }

    public void CheckRotateYAxisUpMethodInactive()
    {
        CheckRotateYAxisUp = false;
    }

    private void RotateYAxisUp()
    {

        if (SelectedInverterCheck && CheckRotateYAxisUp)
        {
            InitInverter.transform.Rotate(0f, 10f * Time.deltaTime, 0f);
        }
    }

    public void CheckRotateYAxisDownMethodActive()
    {
        CheckRotateYAxisDown = true;
    }

    public void CheckRotateYAxisDownMethodInactive()
    {
        CheckRotateYAxisDown = false;
    }

    private void RotateYAxisDown()
    {

        if (SelectedInverterCheck && CheckRotateYAxisDown)
        {
            InitInverter.transform.Rotate(0f, -10f * Time.deltaTime, 0f);
        }
    }

    public void DeleteInverter()
    {
        if (SelectedInverterCheck)
        {
            Destroy(InitInverter);
            AddInverterCheck = false;
            SelectedInverterCheck = false;
            ControllerInverterPanel.SetActive(false);
            MoveInverterCheck = false;
            InitInverter = null;
            NumberInverter--;
            CurrentInverterText.text = "Number Current Inverter : " + NumberInverter;
        }
    }

    private void DesplayTextSuggestation()
    {
        SuggestInverterText.text = "Number Suggest Inverter : " + NumberSuggestInverter;
        CurrentInverterText.text = "Number Current Inverter : " + NumberInverter;
    }

    private void ShowWarningLetterPanel(string text)
    {
        SuggestationPanel.SetActive(false);
        WarningText.text = text;
        WarningPanel.SetActive(true);
        Invoke("DisableWarningLetterPanel", 5f);
    }

    private void DisableWarningLetterPanel()
    {
        WarningPanel.SetActive(false);
        SuggestationPanel.SetActive(true);
    }

}
