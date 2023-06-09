using Microsoft.Maps.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SolarPanelController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject SolarPanel;
    [SerializeField] private Material SolarPositionMaterial;
    [SerializeField] private Material SelectedMaterial;
    [SerializeField] private Material SolarMaterial;
    [SerializeField] private RightPanelActivateScript RightPanelScript;
    [SerializeField] private GameObject ControllerSolarPanel;
    [SerializeField] private MeasureAreaScript measureAreaScript;


    [Header("Map Render")]
    public GameObject map;

    [Header("Suggest Panel Message")]
    [SerializeField] private GameObject SuggestationPanel;
    [SerializeField] private Text SuggestSolarPanelText;
    [SerializeField] private Text CurrentSolarPanelText;
    [SerializeField] private GameObject WarningPanel;
    [SerializeField] private Text WarningText;


    private RaycastHit raycastHit;
    private GameObject GroupSelectedSolarPanel;
    private List<GameObject> SelectedSolarPanels;
    

    private bool AddPanelCheck;
    private bool MovePanelCheck;

    private bool CheckRotateXAxisUp;
    private bool CheckRotateXAxisDown;
    private bool CheckRotateZAxisUp; 
    private bool CheckRotateZAxisDown;
    private bool CheckRotateYAxisUp;
    private bool CheckRotateYAxisDown;


    private bool CheckContinueClickMouse;
    private bool AddInitPositionPanelsCheck;
    private List<GameObject> PositionPanelsRight;
    private List<GameObject> PositionPanelsUp;
    private List<GameObject> PositionPanelsDown;
    private List<GameObject> PositionPanelsLift;

    private int NumberCurrentPanels;
    private int NumberSuggestPanels;



    private void Awake()
    {
        AddPanelCheck = false;
        MovePanelCheck = false;
        
        CheckContinueClickMouse = false;
        AddInitPositionPanelsCheck = false;
        PositionPanelsRight = new List<GameObject>();
        PositionPanelsUp = new List<GameObject>();
        PositionPanelsDown = new List<GameObject>();
        PositionPanelsLift = new List<GameObject>();

        GroupSelectedSolarPanel = new GameObject("GroupSelectedPanel");
        SelectedSolarPanels = new List<GameObject>();

        NumberCurrentPanels = 0;
        NumberSuggestPanels = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AddIntialSolarPanel();
        SelectSolarPanel();
        CountinueClickMouseButtonZero();
        if (CheckContinueClickMouse)
        {
            AddPositionSolarPanel();
        }
        CalcNumberSuggestPanel();
    }

    private void FixedUpdate()
    {
        MoveSelectedSolarMethod();
        RotateXAxisUp();
        RotateXAxisDown();
        RotateZAxisUp();
        RotateZAxisDown();
        RotateYAxisUp();
        RotateYAxisDown();
    }

    public void AddPanelCheckActive()
    {
        
        if(NumberSuggestPanels > NumberCurrentPanels)
        {
            AddPanelCheck = true;
            InitAddNewSolar();
            if (ControllerSolarPanel.activeInHierarchy)
                ActiveAndInactiveRightControllerPanel();
        }
        else
        {
            ShowWarningLetterPanel("You Can't Add Solar Panel Number Suggest : " + NumberSuggestPanels + " And Curent Number : " + NumberCurrentPanels);
        }
        
        
    }

    private void AddIntialSolarPanel()
    {
        if(Input.GetMouseButtonDown(0) && AddPanelCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(map.GetComponent<MapRenderer>().Raycast(ray,out MapRendererRaycastHit hitInfo))
            {
                Instantiate(SolarPanel, new Vector3(hitInfo.Point.x, hitInfo.Point.y + 0.00169f, hitInfo.Point.z), Quaternion.identity);
                AddPanelCheck = false;
                NumberCurrentPanels++;
            }
        }
    }


    public void SelectSolarPanel()
    {
        if (Input.GetMouseButtonUp(0) && !AddPanelCheck && !MovePanelCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out raycastHit))
            {
                if(raycastHit.transform.tag == "SolarPanel")
                {
                    GameObject SolarSelected = raycastHit.transform.gameObject;

                    bool CheckSelectedSolar = CheckFindSelectedSolarPanel(SolarSelected);
                    if (CheckSelectedSolar) // If Find true
                    {
                        DeleteSolarFromSelectedSolarPanel(SolarSelected);
                        SolarSelected.GetComponent<MeshRenderer>().material = SolarMaterial;
                        SolarSelected.transform.SetParent(null);
                        if (ControllerSolarPanel.activeInHierarchy && SelectedSolarPanels.Count == 0)
                            ActiveAndInactiveRightControllerPanel();
                        
                    }
                    else
                    {
                        if (SelectedSolarPanels.Count == 0)
                        {
                            GroupSelectedSolarPanel.transform.position = SolarSelected.transform.position;
                        }
                        SolarSelected.transform.SetParent(GroupSelectedSolarPanel.transform);
                        SolarSelected.GetComponent<MeshRenderer>().material = SelectedMaterial;
                        AddSolarToSelectedSolarPanel(SolarSelected);
                        if (!ControllerSolarPanel.activeInHierarchy)
                            ActiveAndInactiveRightControllerPanel();
                    }
                }
            }
        }
    }


    private bool CheckFindSelectedSolarPanel(GameObject SolarPanelCheck)
    {
        foreach(GameObject g in SelectedSolarPanels)
        {
            if(g == SolarPanelCheck)
            {
                return true;
            }
        }

        return false;
    }

    private void InitAddNewSolar()
    {
        foreach (GameObject g in SelectedSolarPanels)
        {
            g.GetComponent<MeshRenderer>().material = SolarMaterial;
            g.transform.SetParent(null);
        }
        SelectedSolarPanels = new List<GameObject>();
    }

    private void AddSolarToSelectedSolarPanel(GameObject SolarToAdd)
    {
        SelectedSolarPanels.Add(SolarToAdd);
    }

    private void DeleteSolarFromSelectedSolarPanel(GameObject SolarDelete)
    {
        for(int i = 0;i < SelectedSolarPanels.Count; i++)
        {
            if(SelectedSolarPanels.ElementAt(i) == SolarDelete)
            {
                SelectedSolarPanels.RemoveAt(i);
                break;
            }
        }
    }

    private void ActiveAndInactiveRightControllerPanel()
    {
        RightPanelScript.DisibleAndEnableGameObject(3);
    }

    public void MoveSelectedSolarPanelCheck()
    {
        if(SelectedSolarPanels.Count > 0 && !AddPanelCheck)
        {
            MovePanelCheck = true;
        }
    }

    private void MoveSelectedSolarMethod()
    {
        if(MovePanelCheck && SelectedSolarPanels.Count > 0)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(map.GetComponent<MapRenderer>().Raycast(ray,out MapRendererRaycastHit hitInfo))
            {
                GroupSelectedSolarPanel.transform.position = new Vector3(hitInfo.Point.x, hitInfo.Point.y + 0.00169f, hitInfo.Point.z);
                if (Input.GetMouseButtonDown(0))
                {
                    MovePanelCheck = false;
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

        if (this.SelectedSolarPanels.Count > 0 && CheckRotateXAxisUp)
        {
           GroupSelectedSolarPanel.transform.Rotate(10f * Time.deltaTime, 0f, 0f);
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

        if (this.SelectedSolarPanels.Count > 0 && CheckRotateXAxisDown)
        {
            GroupSelectedSolarPanel.transform.Rotate(-10f * Time.deltaTime, 0f, 0f);
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

        if (this.SelectedSolarPanels.Count > 0 && CheckRotateYAxisUp)
        {
            GroupSelectedSolarPanel.transform.Rotate(0f,10f * Time.deltaTime, 0f);
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

        if (this.SelectedSolarPanels.Count > 0 && CheckRotateYAxisDown)
        {
            GroupSelectedSolarPanel.transform.Rotate(0f,-10f * Time.deltaTime, 0f);
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
        if (this.SelectedSolarPanels.Count > 0 && CheckRotateZAxisUp)
        {
            GroupSelectedSolarPanel.transform.Rotate(0f , 0f,10f * Time.deltaTime);
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
        if (this.SelectedSolarPanels.Count > 0 && CheckRotateZAxisDown)
        {
           GroupSelectedSolarPanel.transform.Rotate(0f,0f, -10f * Time.deltaTime);
        }
    }

    public void DeleteSelectedSolarPanel()
    {
        foreach(GameObject g in SelectedSolarPanels)
        {
            Destroy(g);
            NumberCurrentPanels--;
        }
        SelectedSolarPanels = new List<GameObject>();
        ActiveAndInactiveRightControllerPanel();
    }

    private void CountinueClickMouseButtonZero()
    {
        if(Input.GetMouseButtonDown(0) && !CheckContinueClickMouse)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out raycastHit))
            {
                if(raycastHit.transform.tag == "SolarPanel")
                {
                    CheckContinueClickMouse = true;
                    if (!AddInitPositionPanelsCheck&& SelectedSolarPanels.Count == 0)
                    {
                        AddInitPositionPanelsCheck = true;
                        InitPositionPanelRight(raycastHit.transform.gameObject);
                        InitPositionPanelLeft(raycastHit.transform.gameObject);
                        InitPositionPanelUp(raycastHit.transform.gameObject);
                        InitPositionPanelDown(raycastHit.transform.gameObject);
                    }
                }
            }

            

            
        }else if(Input.GetMouseButtonUp(0) && CheckContinueClickMouse)
        {
            CheckContinueClickMouse = false;
            AddInitPositionPanelsCheck = false;
            ClearPositionPanel();
        }
    }

    private void InitPositionPanelRight(GameObject SolarPanelX)
    {
        Vector3 NewPosition = SolarPanelX.transform.position;
        Vector3 Rotation = SolarPanelX.transform.localEulerAngles;
        for(int i = 0;i < 10; i++)
        {
            NewPosition.y -= 0.1f;
            GameObject New = Instantiate(SolarPanelX, NewPosition, Quaternion.identity);
            New.GetComponent<MeshRenderer>().material = SolarPositionMaterial;
            New.transform.tag = "SolarPanelPosition";
            New.transform.localEulerAngles = Rotation;
            New.transform.Translate(new Vector3(0.01840000041f, 0f, 0f));

            Vector3 PositionX = New.transform.position;
            PositionX.y += 0.1f;
            New.transform.position = PositionX;

            PositionPanelsRight.Add(New);
            NewPosition = New.transform.position;
        }
    }

    private void InitPositionPanelLeft(GameObject SolarPanelX)
    {
        Vector3 NewPosition = SolarPanelX.transform.position;
        Vector3 Rotation = SolarPanelX.transform.localEulerAngles;
        for (int i = 0; i < 10; i++)
        {
            NewPosition.y -= 0.1f;
            GameObject New = Instantiate(SolarPanelX, NewPosition, Quaternion.identity);
            New.GetComponent<MeshRenderer>().material = SolarPositionMaterial;
            New.transform.tag = "SolarPanelPosition";
            New.transform.localEulerAngles = Rotation;
            New.transform.Translate(new Vector3(-0.01840000041f, 0f, 0f));
           
            Vector3 PositionX = New.transform.position;
            PositionX.y += 0.1f;
            New.transform.position = PositionX;
         
            PositionPanelsLift.Add(New);
            NewPosition = New.transform.position;
        }
    }

    private void InitPositionPanelUp(GameObject SolarPanelX)
    {
        Vector3 NewPosition = SolarPanelX.transform.position;
        Vector3 Rotation = SolarPanelX.transform.localEulerAngles;
        for (int i = 0; i < 3; i++)
        {
            NewPosition.y -= 0.1f;
            GameObject New = Instantiate(SolarPanelX, NewPosition, Quaternion.identity);
            New.GetComponent<MeshRenderer>().material = SolarPositionMaterial;
            New.transform.tag = "SolarPanelPosition";
            New.transform.localEulerAngles = Rotation;
            New.transform.Translate(new Vector3(0f, 0f, 0.043099999f));
          
            Vector3 PositionX = New.transform.position;
            PositionX.y += 0.1f;
            New.transform.position = PositionX;
           
            PositionPanelsUp.Add(New);
            NewPosition = New.transform.position;
        }
    }

    private void InitPositionPanelDown(GameObject SolarPanelX)
    {
        Vector3 NewPosition = SolarPanelX.transform.position;
        Vector3 Rotation = SolarPanelX.transform.localEulerAngles;
        for (int i = 0; i < 3; i++)
        {
            NewPosition.y -= 0.1f;
            GameObject New = Instantiate(SolarPanelX, NewPosition, Quaternion.identity);
            New.GetComponent<MeshRenderer>().material = SolarPositionMaterial;
            New.transform.tag = "SolarPanelPosition";
            New.transform.localEulerAngles = Rotation;
            New.transform.Translate(new Vector3(0f, 0f, -0.043099999f));

            Vector3 PositionX = New.transform.position;
            PositionX.y += 0.1f;
            New.transform.position = PositionX;
            
            PositionPanelsDown.Add(New);
            NewPosition = New.transform.position;
        }
    }

    private void ClearPositionPanel()
    {
        foreach(GameObject g in PositionPanelsRight)
        {
            if(g.tag == "SolarPanelPosition")
            {
                Destroy(g);
            }
        }
        foreach (GameObject g in PositionPanelsLift)
        {
            if (g.tag == "SolarPanelPosition")
            {
                Destroy(g);
            }
        }
        foreach (GameObject g in PositionPanelsUp)
        {
            if (g.tag == "SolarPanelPosition")
            {
                Destroy(g);
            }
        }
        foreach (GameObject g in PositionPanelsDown)
        {
            if (g.tag == "SolarPanelPosition")
            {
                Destroy(g);
            }
        }

        PositionPanelsRight = new List<GameObject>();
        PositionPanelsLift = new List<GameObject>();
        PositionPanelsUp = new List<GameObject>();
        PositionPanelsDown = new List<GameObject>();
    }

    private void AddPositionSolarPanel()
    {
        Vector3 MousePos = Input.mousePosition;

        Ray ray = cam.ScreenPointToRay(MousePos);
        if(Physics.Raycast(ray,out raycastHit))
        {
            if(raycastHit.transform.tag == "SolarPanelPosition")
            {
                if(NumberSuggestPanels > NumberCurrentPanels)
                {
                    GameObject solar = raycastHit.transform.gameObject;
                    solar.tag = "SolarPanel";
                    solar.GetComponent<MeshRenderer>().material = SolarMaterial;
                    NumberCurrentPanels++;
                }
                else
                {
                    ShowWarningLetterPanel("You Can't Add Solar Panel Number Suggest : " + NumberSuggestPanels + " And Curent Number : " + NumberCurrentPanels);
                }

            }
        }
    }


    private void CalcNumberSuggestPanel()
    {
        float num = Mathf.Floor(measureAreaScript.totalArea / 3);
        // AreaPanel = 3 M Squer
        int NumConv = (int) num - 1;

        if(NumConv == -1)
        {
            NumberSuggestPanels = 0;
        }
        else
        {
            NumberSuggestPanels = NumConv;
        }

        SuggestSolarPanelText.text = "Number Suggest Solar : " + NumberSuggestPanels;
        CurrentSolarPanelText.text = "Number Current Solar : " + NumberCurrentPanels;
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
