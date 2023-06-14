using Microsoft.Maps.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeasurePitchDegree : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject PitchDegreeTool;
    [SerializeField] private GameObject ControllerPitchDegreePanel;

    [Header("Map Render")]
    public GameObject map;

    [Header("Suggest Panel Message")]
    [SerializeField] private GameObject SuggestationPanel;
    [SerializeField] private Text CurrentPitchDegreeText;
    [SerializeField] private GameObject WarningPanel;
    [SerializeField] private Text WarningText;

    private RaycastHit raycastHit;

    private bool AddPitchDegreeCheck;
    private bool SelectedPitchDegreeToolCheck;

    private bool MovePitchDegreeCheck;


    private GameObject InitPitchDegreeTool;
    [HideInInspector] public List<GameObject> PitchDegreeTools;
    private GameObject SelectedPitchDegreeTool;


    private bool CheckRotateYAxisUp;
    private bool CheckRotateYAxisDown;

    private bool CheckMoveUp;
    private bool CheckMoveDown;
    private bool CheckMoveLeft;
    private bool CheckMoveRight;


    private void Awake()
    {
        AddPitchDegreeCheck = false; 
        SelectedPitchDegreeToolCheck = false;
        MovePitchDegreeCheck = false;
        PitchDegreeTools = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SelectPitchDegree();
        CalcReports();
    }

    private void FixedUpdate()
    {
        AddPitchDegreeTool();
        MovePitchDegreeTool();
        RotateYAxisUp();
        RotateYAxisDown();
        MoveUp();
        MoveDown();
        MoveLeft();
        MoveRight();
    }

    public void CheckAddPitchDegreeTool()
    {
        if (!SelectedPitchDegreeToolCheck && !MovePitchDegreeCheck)
        {
            AddPitchDegreeCheck = !AddPitchDegreeCheck;
            if (AddPitchDegreeCheck)
            {
                InitPitchDegreeTool = Instantiate(PitchDegreeTool, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                Destroy(InitPitchDegreeTool);
            }
        }
        else
        {
            if (SelectedPitchDegreeToolCheck)
            {
                ShowWarningLetterPanel("Please Close Selected Pitch Degree Tool");
            }
            else if (MovePitchDegreeCheck)
            {
                ShowWarningLetterPanel("Please Close Move Pitch Degree Tool");
            }
        }
    }

    private void AddPitchDegreeTool()
    {
        if (AddPitchDegreeCheck && !SelectedPitchDegreeToolCheck && !MovePitchDegreeCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (map.GetComponent<MapRenderer>().Raycast(ray, out MapRendererRaycastHit raycastHit))
            {
                InitPitchDegreeTool.transform.position = new Vector3(raycastHit.Point.x, raycastHit.Point.y, raycastHit.Point.z+ 0.0031485f);
                if (Input.GetMouseButtonDown(0))
                {
                    PitchDegreeTools.Add(InitPitchDegreeTool);
                    AddPitchDegreeCheck = false;
                }
            }
        }
    }

    private void SelectPitchDegree()
    {
        if (Input.GetMouseButtonDown(0) && !AddPitchDegreeCheck && !MovePitchDegreeCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform.tag == "PitchDegreePoint")
                {
                    if (!SelectedPitchDegreeToolCheck)
                    {
                        SelectedPitchDegreeTool = raycastHit.transform.parent.gameObject;
                        SelectedPitchDegreeToolCheck = true;
                        ControllerPitchDegreePanel.SetActive(true);
                    }
                    else
                    {
                        SelectedPitchDegreeTool = null;
                        SelectedPitchDegreeToolCheck = false;
                        ControllerPitchDegreePanel.SetActive(false);
                    }
                }
            }
        }
    }

    public void CheckMovePitchDegree()
    {
        MovePitchDegreeCheck = true;
    }


    private void MovePitchDegreeTool()
    {
        if (MovePitchDegreeCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (map.GetComponent<MapRenderer>().Raycast(ray, out MapRendererRaycastHit raycastHit))
            {
                SelectedPitchDegreeTool.transform.position = new Vector3(raycastHit.Point.x, raycastHit.Point.y, raycastHit.Point.z + 0.0031485f);
                if (Input.GetMouseButtonDown(0))
                {
                    MovePitchDegreeCheck = false;
                }
            }
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

        if (SelectedPitchDegreeToolCheck && CheckRotateYAxisUp)
        {
            SelectedPitchDegreeTool.transform.Rotate(0f, 15f * Time.deltaTime, 0f);
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

        if (SelectedPitchDegreeToolCheck && CheckRotateYAxisDown)
        {
            SelectedPitchDegreeTool.transform.Rotate(0f, -15f * Time.deltaTime, 0f);
        }
    }



    public void CheckMoveUpMethodActive()
    {
        this.CheckMoveUp = true;
    }

    public void CheckMoveUpMethodInactive()
    {
        this.CheckMoveUp = false;
    }

    private void MoveUp()
    {

        if (SelectedPitchDegreeToolCheck && CheckMoveUp)
        {
            Transform UpDownPoint = SelectedPitchDegreeTool.transform.Find("topBottom");
            UpDownPoint.Translate(0f, 0.02f * Time.deltaTime, 0f);
        }
    }

    public void CheckMoveDownMethodActive()
    {
        this.CheckMoveDown = true;
    }

    public void CheckMoveDownMethodInactive()
    {
        this.CheckMoveDown = false;
    }

    private void MoveDown()
    {

        if (SelectedPitchDegreeToolCheck && CheckMoveDown)
        {
            Transform UpDownPoint = SelectedPitchDegreeTool.transform.Find("topBottom");
            UpDownPoint.Translate(0f, -0.02f * Time.deltaTime, 0f);
        }
    }

    public void CheckMoveRightMethodActive()
    {
        this.CheckMoveRight = true;
    }

    public void CheckMoveRightMethodInactive()
    {
        this.CheckMoveRight = false;
    }

    private void MoveRight()
    {

        if (SelectedPitchDegreeToolCheck && CheckMoveRight)
        {
            Transform LeftRightPoint = SelectedPitchDegreeTool.transform.Find("leftRight");
            LeftRightPoint.Translate(0f, 0f, 0.02f * Time.deltaTime);
        }
    }


    public void CheckMoveLeftMethodActive()
    {
        this.CheckMoveLeft = true;
    }

    public void CheckMoveLeftMethodInactive()
    {
        this.CheckMoveLeft = false;
    }

    private void MoveLeft()
    {

        if (SelectedPitchDegreeToolCheck && CheckMoveLeft)
        {
            Transform LeftRightPoint = SelectedPitchDegreeTool.transform.Find("leftRight");
            LeftRightPoint.Translate(0f, 0f,-0.02f * Time.deltaTime);
        }
    }

    private void CalcReports()
    {
        foreach(GameObject g in PitchDegreeTools)
        {
            float Rise = GetRise(g);
            float Run = GetRun(g);
            CalcPitchDegree(Rise, Run);

        }
    }

    private float GetRise(GameObject PitchDegreeGameObject)
    {
        Transform CenterPoint = PitchDegreeGameObject.transform.Find("center");
        Transform TopBottomPoint = PitchDegreeGameObject.transform.Find("topBottom");

        float Rise = Vector3.Distance(CenterPoint.position, TopBottomPoint.position) * 55.9f;
        return Rise;
    }

    private float GetRun(GameObject PitchDegreeGameObject)
    {
        Transform CenterPoint = PitchDegreeGameObject.transform.Find("center");
        Transform LeftRightPoint = PitchDegreeGameObject.transform.Find("leftRight");

        float Run = Vector3.Distance(CenterPoint.position, LeftRightPoint.position) * 55.9f;
        return Run;
    }

    private void CalcPitchDegree(float Rise,float Run)
    {
        float Pitch = Rise / (Run / 12f);
        float slop = (Rise / Run) * 100f;
        double Angle = Math.Atan(Rise / Run) * (180 / Math.PI);

        Debug.Log("Pitch = " + Math.Floor(Pitch) + " / 12");
        Debug.Log("slop = " + Math.Floor(slop) + "%");
        Debug.Log("Angle = " + Math.Floor(Angle) + " °");
    }

    public void DeletePitchDegree()
    {
        if (SelectedPitchDegreeToolCheck && !MovePitchDegreeCheck)
        {
            foreach(GameObject g in PitchDegreeTools)
            {
                if(g == SelectedPitchDegreeTool)
                {
                    PitchDegreeTools.Remove(g);
                    break;
                }
            }
            Destroy(SelectedPitchDegreeTool);
            SelectedPitchDegreeTool = null;
            SelectedPitchDegreeToolCheck = false;
            ControllerPitchDegreePanel.SetActive(false);

        }
    }

   

    private void ShowWarningLetterPanel(string text)
    {
        SuggestationPanel.SetActive(false);
        WarningText.text = text;
        WarningPanel.SetActive(true);
        Invoke("DisableWarningLetterPanel", 5f);
    }
}
