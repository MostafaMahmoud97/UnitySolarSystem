using Microsoft.Maps.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HVACControllerScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject HVACGameObject;
    [SerializeField] private GameObject ControllerHVACPanel;

    [Header("Map Render")]
    public GameObject map;

    [Header("Suggest Panel Message")]
    [SerializeField] private GameObject SuggestationPanel;
    [SerializeField] private Text CurrentHVACText;
    [SerializeField] private GameObject WarningPanel;
    [SerializeField] private Text WarningText;

    private RaycastHit raycastHit;

    private bool AddHVACCheck;
    private bool SelectedHVACCheck;
    private bool MoveHVACCheck;

    private GameObject InitHVAC;
    private GameObject SelectedHVAC;

    [HideInInspector] public int NumberHVAC;

    private bool CheckRotateXAxisUp;
    private bool CheckRotateXAxisDown;
    private bool CheckRotateZAxisUp;
    private bool CheckRotateZAxisDown;
    private bool CheckRotateYAxisUp;
    private bool CheckRotateYAxisDown;

    private void Awake()
    {
        AddHVACCheck = false;
        SelectedHVACCheck = false;
        MoveHVACCheck = false;
        NumberHVAC = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SelectHVAC();
    }

    private void FixedUpdate()
    {
        AddHVAC();
        MoveHVAC();
        RotateXAxisUp();
        RotateXAxisDown();
        RotateZAxisUp();
        RotateZAxisDown();
        RotateYAxisUp();
        RotateYAxisDown();

    }

    public void CheckAddHVAC()
    {
        if(!SelectedHVACCheck && !MoveHVACCheck)
        {
            AddHVACCheck = !AddHVACCheck;
            if (AddHVACCheck)
            {
                InitHVAC = Instantiate(HVACGameObject, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                Destroy(InitHVAC);
            }
        }
        else
        {
            if (SelectedHVACCheck)
            {
                ShowWarningLetterPanel("Please Close Selected HVAC");
            }
            else if (MoveHVACCheck)
            {
                ShowWarningLetterPanel("Please Close Move HVAC");
            }
        }
    }

    private void AddHVAC()
    {
        if (AddHVACCheck && !SelectedHVACCheck && !MoveHVACCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (map.GetComponent<MapRenderer>().Raycast(ray, out MapRendererRaycastHit raycastHit))
            {
                InitHVAC.transform.position = new Vector3(raycastHit.Point.x, raycastHit.Point.y + 0.0175754f, raycastHit.Point.z);
                if (Input.GetMouseButtonDown(0))
                {
                    AddHVACCheck = false;
                    NumberHVAC++;
                    ShowNumberHVAC();
                }
            }
        }
    }

    private void SelectHVAC()
    {
        if (Input.GetMouseButtonDown(0) && !AddHVACCheck && !MoveHVACCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform.tag == "HVAC")
                {
                    if (!SelectedHVACCheck)
                    {
                        SelectedHVAC = raycastHit.transform.gameObject;
                        SelectedHVACCheck = true;
                        ControllerHVACPanel.SetActive(true);
                    }
                    else
                    {
                        SelectedHVAC = null;
                        SelectedHVACCheck = false;
                        ControllerHVACPanel.SetActive(false);
                    }
                }
            }
        }
    }

    public void CheckMoveHVAC()
    {
        MoveHVACCheck = true;
    }

    private void MoveHVAC()
    {
        if (MoveHVACCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (map.GetComponent<MapRenderer>().Raycast(ray, out MapRendererRaycastHit raycastHit))
            {
                SelectedHVAC.transform.position = new Vector3(raycastHit.Point.x, raycastHit.Point.y + 0.0175754f, raycastHit.Point.z);
                if (Input.GetMouseButtonDown(0))
                {
                    MoveHVACCheck = false;
                }
            }
        }
    }

    public void CheckRotateXAxisUpMethodActive()
    {
        this.CheckRotateXAxisUp = true;
    }

    public void CheckRotateXAxisUpMethodInactive()
    {
        this.CheckRotateXAxisUp = false;
    }

    private void RotateXAxisUp()
    {

        if (SelectedHVACCheck && CheckRotateXAxisUp)
        {
            SelectedHVAC.transform.Rotate(15f * Time.deltaTime, 0f, 0f);
        }
    }

    public void CheckRotateXAxisDownMethodActive()
    {
        this.CheckRotateXAxisDown = true;
    }

    public void CheckRotateXAxisDownMethodInactive()
    {
        this.CheckRotateXAxisDown = false;
    }

    private void RotateXAxisDown()
    {

        if (SelectedHVACCheck && CheckRotateXAxisDown)
        {
            SelectedHVAC.transform.Rotate(-15f * Time.deltaTime, 0f, 0f);
        }
    }

    public void CheckRotateYAxisUpMethodActive()
    {
        this.CheckRotateYAxisUp = true;
    }

    public void CheckRotateYAxisUpMethodInactive()
    {
        this.CheckRotateYAxisUp = false;
    }

    private void RotateYAxisUp()
    {

        if (SelectedHVACCheck && CheckRotateYAxisUp)
        {
            SelectedHVAC.transform.Rotate(0f, 15f * Time.deltaTime, 0f);
        }
    }

    public void CheckRotateYAxisDownMethodActive()
    {
        this.CheckRotateYAxisDown = true;
    }

    public void CheckRotateYAxisDownMethodInactive()
    {
        this.CheckRotateYAxisDown = false;
    }

    private void RotateYAxisDown()
    {

        if (SelectedHVACCheck && CheckRotateYAxisDown)
        {
            SelectedHVAC.transform.Rotate(0f, -15f * Time.deltaTime, 0f);
        }
    }


    public void CheckRotateZAxisUpMethodActive()
    {
        this.CheckRotateZAxisUp = true;
    }

    public void CheckRotateZAxisUpMethodInactive()
    {
        this.CheckRotateZAxisUp = false;
    }

    private void RotateZAxisUp()
    {
        if (SelectedHVACCheck && CheckRotateZAxisUp)
        {
            SelectedHVAC.transform.Rotate(0f, 0f, 15f * Time.deltaTime);
        }
    }

    public void CheckRotateZAxisDownMethodActive()
    {
        this.CheckRotateZAxisDown = true;
    }

    public void CheckRotateZAxisDownMethodInactive()
    {
        this.CheckRotateZAxisDown = false;
    }

    private void RotateZAxisDown()
    {
        if (SelectedHVACCheck && CheckRotateZAxisDown)
        {
            SelectedHVAC.transform.Rotate(0f, 0f, -15f * Time.deltaTime);
        }
    }

    public void DeleteHVAC()
    {
        if (SelectedHVACCheck)
        {
            Destroy(SelectedHVAC);
            AddHVACCheck = false;
            SelectedHVACCheck = false;
            ControllerHVACPanel.SetActive(false);
            MoveHVACCheck = false;
            SelectedHVAC = null;
            NumberHVAC--;
            ShowNumberHVAC();
        }
    }

    private void ShowNumberHVAC()
    {
        CurrentHVACText.text = "Number Current HVAC : " + NumberHVAC;
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
