using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportControllerScript : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private GameObject ContollerGameObject;

    [Header("Center Panel")]
    [SerializeField] private GameObject CenterPanel;
    [SerializeField] private Transform CenterPanelPosition;

    [Header("Main")]
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject MainDetailPanel;

    [Header("Pitch")]
    [SerializeField] private GameObject PitchPanel;
    [SerializeField] private GameObject PitchDetailPanel;


    private GameObject InitAreaPanel;
    private GameObject InitSolarPanel;
    private GameObject InitInverterPanel;
    private GameObject InitHVACPanel;
    private GameObject InitGeneratorPanel;
    private GameObject InitPitchPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableAndEnableCenterPanel()
    {
        CenterPanel.SetActive(!CenterPanel.activeInHierarchy);
        if (CenterPanel.activeInHierarchy)
        {
            ShowAreaDetails();
            ShowSolarPanelDetails();
            ShowInverterPanelDetails();
            ShowHVACPanelDetails();
            ShowGeneratorPanelDetails();
            ShowPitchPanelDetails();
        }
    }

    private void ShowAreaDetails()
    {
        Destroy(InitAreaPanel);
        MeasureAreaScript measureAreaScript = ContollerGameObject.GetComponent<MeasureAreaScript>();
        if(measureAreaScript.AreaReport.Count > 0 )
        {
            InitAreaPanel = Instantiate(MainPanel, CenterPanelPosition);

            InitAreaPanel.transform.Find("Title").GetComponent<Text>().text = "Area Panel Description";

            Transform Content = InitAreaPanel.transform.Find("ContentMainPanel").Find("Content");
            GameObject areaDetail;

            foreach (float x in measureAreaScript.AreaReport)
            {
                areaDetail= Instantiate(MainDetailPanel, Content);
                areaDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Area : " + Math.Round(x, 2) + " m\u00b2"; 
            }

            areaDetail = Instantiate(MainDetailPanel, Content);
            areaDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Total Area : " + Math.Round(measureAreaScript.totalArea, 2) + " m\u00b2";
        }
    }

    private void ShowSolarPanelDetails()
    {
        Destroy(InitSolarPanel);
        SolarPanelController solarPanelController = ContollerGameObject.GetComponent<SolarPanelController>();
        if (solarPanelController.NumberCurrentPanels > 0)
        {
            InitSolarPanel = Instantiate(MainPanel, CenterPanelPosition);
            InitSolarPanel.transform.Find("Title").GetComponent<Text>().text = "Solar Panel Description";

            Transform Content = InitSolarPanel.transform.Find("ContentMainPanel").Find("Content");
            GameObject solarDetail;

            solarDetail = Instantiate(MainDetailPanel, Content);
            solarDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Suggest Solar : " + solarPanelController.NumberSuggestPanels;

            solarDetail = Instantiate(MainDetailPanel, Content);
            solarDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Current Solar : " + solarPanelController.NumberCurrentPanels;
        }
    }

    private void ShowInverterPanelDetails()
    {
        Destroy(InitInverterPanel);
        InverterScript inverterScript = ContollerGameObject.GetComponent<InverterScript>();
        InitInverterPanel = Instantiate(MainPanel, CenterPanelPosition);
        InitInverterPanel.transform.Find("Title").GetComponent<Text>().text = "Inverter Panel Description";

        Transform Content = InitInverterPanel.transform.Find("ContentMainPanel").Find("Content");
        GameObject inverterDetail;

        if (inverterScript.NumberInverter > 0)
        {
            inverterDetail = Instantiate(MainDetailPanel, Content);
            inverterDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Inverter Status : Yes";
        }
        else
        {
            inverterDetail = Instantiate(MainDetailPanel, Content);
            inverterDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Inverter Status : No";
        }

        inverterDetail = Instantiate(MainDetailPanel, Content);
        inverterDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Current Inverter : " + inverterScript.NumberInverter;
    }


    private void ShowHVACPanelDetails()
    {
        Destroy(InitHVACPanel);
        HVACControllerScript hVACControllerScript = ContollerGameObject.GetComponent<HVACControllerScript>();
        InitHVACPanel = Instantiate(MainPanel, CenterPanelPosition);
        InitHVACPanel.transform.Find("Title").GetComponent<Text>().text = "HVAC Panel Description";

        Transform Content = InitHVACPanel.transform.Find("ContentMainPanel").Find("Content");
        GameObject HvacDetail;

        if (hVACControllerScript.NumberHVAC > 0)
        {
            HvacDetail = Instantiate(MainDetailPanel, Content);
            HvacDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Inverter Status : Yes";
        }
        else
        {
            HvacDetail = Instantiate(MainDetailPanel, Content);
            HvacDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Inverter Status : No";
        }

        HvacDetail = Instantiate(MainDetailPanel, Content);
        HvacDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Current Inverter : " + hVACControllerScript.NumberHVAC;
    }

    private void ShowGeneratorPanelDetails()
    {
        Destroy(InitGeneratorPanel);
        GeneratorControllerScript generatorControllerScript = ContollerGameObject.GetComponent<GeneratorControllerScript>();
        InitGeneratorPanel = Instantiate(MainPanel, CenterPanelPosition);
        InitGeneratorPanel.transform.Find("Title").GetComponent<Text>().text = "Generator Panel Description";

        Transform Content = InitGeneratorPanel.transform.Find("ContentMainPanel").Find("Content");
        GameObject GeneratorDetail;

        if (generatorControllerScript.NumberGenerator > 0)
        {
            GeneratorDetail = Instantiate(MainDetailPanel, Content);
            GeneratorDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Generator Status : Yes";
        }
        else
        {
            GeneratorDetail = Instantiate(MainDetailPanel, Content);
            GeneratorDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Generator Status : No";
        }

        GeneratorDetail = Instantiate(MainDetailPanel, Content);
        GeneratorDetail.transform.Find("MainTxt").GetComponent<Text>().text = "Current Generator : " + generatorControllerScript.NumberGenerator;
    }

    private void ShowPitchPanelDetails()
    {
        Destroy(InitPitchPanel);
        MeasurePitchDegree measurePitchDegree = ContollerGameObject.GetComponent<MeasurePitchDegree>();

        if (measurePitchDegree.PitchDegreeTools.Count > 0)
        {
            InitPitchPanel = Instantiate(PitchPanel, CenterPanelPosition);
            InitPitchPanel.transform.Find("Title").GetComponent<Text>().text = "Pitch Panel Description";

            Transform Content = InitPitchPanel.transform.Find("ContentMainPanel").Find("Content");
            GameObject PitchDetail;

            foreach(GameObject g in measurePitchDegree.PitchDegreeTools)
            {
                float Rise = GetRise(g);
                float Run = GetRun(g);


                float Pitch = Rise / (Run / 12f);
                float slop = (Rise / Run) * 100f;
                double Angle = Math.Atan(Rise / Run) * (180 / Math.PI);

                PitchDetail = Instantiate(PitchDetailPanel, Content);
                PitchDetail.transform.Find("PitchTxt").GetComponent<Text>().text = "Pitch : "+ Math.Floor(Pitch) +"/12";
                PitchDetail.transform.Find("SlopTxt").GetComponent<Text>().text = "Slop : "+Math.Floor(slop)+"%";
                PitchDetail.transform.Find("AngleTxt").GetComponent<Text>().text = "Angle : "+Math.Floor(Angle) + "°";
            }

            
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
}
