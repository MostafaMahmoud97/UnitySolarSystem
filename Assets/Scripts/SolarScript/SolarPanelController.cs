using Microsoft.Maps.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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


    [Header("Map Render")]
    public GameObject map;


    private RaycastHit raycastHit;
    private GameObject GroupSelectedSolarPanel;
    private List<GameObject> SelectedSolarPanels;
    

    private bool AddPanelCheck;
    private bool MovePanelCheck;

    private bool CheckRotateUp; 
    private bool CheckRotateLeft; 
    private bool CheckRotateRight; 
    private bool CheckRotateDown;

    private bool CheckContinueClickMouse;
    private bool AddInitPositionPanelsCheck;
    private List<GameObject> PositionPanelsRight;
    private List<GameObject> PositionPanelsUp;
    private List<GameObject> PositionPanelsDown;
    private List<GameObject> PositionPanelsLift;



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
    }

    private void FixedUpdate()
    {
        MoveSelectedSolarMethod();
        RotateUp();
        RotateDown();
        RotateLeft();
        RotateRight();
    }

    public void AddPanelCheckActive()
    {
        AddPanelCheck = true;
        InitAddNewSolar();
        if (ControllerSolarPanel.activeInHierarchy)
            ActiveAndInactiveRightControllerPanel();
        
    }

    private void AddIntialSolarPanel()
    {
        if(Input.GetMouseButtonDown(0) && AddPanelCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(map.GetComponent<MapRenderer>().Raycast(ray,out MapRendererRaycastHit hitInfo))
            {
                Instantiate(SolarPanel, new Vector3(hitInfo.Point.x, hitInfo.Point.y + 0.0011622f, hitInfo.Point.z), Quaternion.identity);
                AddPanelCheck = false;
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
                GroupSelectedSolarPanel.transform.position = new Vector3(hitInfo.Point.x, hitInfo.Point.y + 0.0011643f, hitInfo.Point.z);
                if (Input.GetMouseButtonDown(0))
                {
                    MovePanelCheck = false;
                }
            }

        }
    }

    public void CheckRotateUpMethodActive()
    {
        this.CheckRotateUp = true;
    }

    public void CheckRotateUpMethodInactive()
    {
        this.CheckRotateUp = false;
    }

    private void RotateUp()
    {

        if (this.SelectedSolarPanels.Count > 0 && CheckRotateUp)
        {
           GroupSelectedSolarPanel.transform.Rotate(10f * Time.deltaTime, 0f, 0f);
        }
    }

    public void CheckRotateDownMethodActive()
    {
        this.CheckRotateDown = true;
    }

    public void CheckRotateDownMethodInactive()
    {
        this.CheckRotateDown = false;
    }

    private void RotateDown()
    {

        if (this.SelectedSolarPanels.Count > 0 && CheckRotateDown)
        {
            GroupSelectedSolarPanel.transform.Rotate(-10f * Time.deltaTime, 0f, 0f);
        }
    }

    public void CheckRotateLeftMethodActive()
    {
        this.CheckRotateLeft = true;
    }

    public void CheckRotateLeftMethodInactive()
    {
        this.CheckRotateLeft = false;
    }

    private void RotateLeft()
    {
        if (this.SelectedSolarPanels.Count > 0 && CheckRotateLeft)
        {
            GroupSelectedSolarPanel.transform.Rotate(0f , 10f * Time.deltaTime, 0f);
        }
    }

    public void CheckRotateRightMethodActive()
    {
        this.CheckRotateRight = true;
    }

    public void CheckRotateRightMethodInactive()
    {
        this.CheckRotateRight = false;
    }

    private void RotateRight()
    {
        if (this.SelectedSolarPanels.Count > 0 && CheckRotateRight)
        {
           GroupSelectedSolarPanel.transform.Rotate(0f, -10f * Time.deltaTime, 0f);
        }
    }

    public void DeleteSelectedSolarPanel()
    {
        foreach(GameObject g in SelectedSolarPanels)
        {
            Destroy(g);
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
                GameObject solar = raycastHit.transform.gameObject;
                solar.tag = "SolarPanel";
                solar.GetComponent<MeshRenderer>().material = SolarMaterial;
            }
        }
    }

}
