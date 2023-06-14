using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.Maps.Unity;
using System.Linq;
using MeasureAreaTools;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class MeasureAreaScript : MonoBehaviour
{

    [Header("Map Render")]
    public GameObject map;

    [Header("Settings")]
    [SerializeField] private Camera mainCamira;
    [SerializeField] private GameObject InitPoint;
    [SerializeField] private GameObject LineRenderMap;

    //Calc Total Area
    [Header("Area Description Panel")]
    [SerializeField] private GameObject AreaDescriptionPanel;
    [SerializeField] private Text LabelTotalArea;
    [SerializeField] private Text LabelArea;
    [HideInInspector] public float totalArea;

    private RaycastHit raycastHit;
    private bool CheckAddPointBtnClick = false;
    private List<AreaClass> areaClasses;
    private AreaClass InitialClassArea;
    private GameObject PointTrack;

    // For Report
    [HideInInspector] public List<float> AreaReport;


    //Select Any Point
    private GameObject PointArea;
    private AreaClass SelectedAreaClass;
    private bool checkSelectedPoint;
    private bool checkMovePoint;

    


    // Start is called before the first frame update
    void Start()
    {
        areaClasses = new List<AreaClass>();

        //Select Point
        PointArea = null;
        checkSelectedPoint = false;
        checkMovePoint = false;
        //calc area
        totalArea = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //this.TrackPoint();
        this.AddPoint();
        
        if (CheckAddPointBtnClick &&InitialClassArea.MesurePoints.Count > 1)
        {
            this.DrawLineBetweenPoints(InitialClassArea);
        }


        //Move Point Select
        if (!CheckAddPointBtnClick)
        {
            SelectPointToMove();
            MoveSelectedPoint();
        }

    }

    private void FixedUpdate()
    {
        ManageCalcTotalArea();
    }

    
    private void AddPoint()
    {
        if (CheckAddPointBtnClick && Input.GetMouseButtonUp(0) )
        {
            
            var ray = mainCamira.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit) && InitialClassArea.MesurePoints.Count > 1)
            {
                if(raycastHit.transform.tag == "Start_Point_Area")
                {
                    CheckAddPointBtnClick = false;
                    DrawLineBetweenPoints(InitialClassArea);
                    areaClasses.Add(InitialClassArea);
                }
            }
           

            if (CheckAddPointBtnClick) { 
                if (map.GetComponent<MapRenderer>().Raycast(ray, out MapRendererRaycastHit hitInfo))
                {
                    PointTrack = Instantiate(InitPoint, hitInfo.Point, Quaternion.identity);
                    if(InitialClassArea.MesurePoints.Count == 0)
                    {
                        PointTrack.transform.tag = "Start_Point_Area";
                    }
                    InitialClassArea.MesurePoints.Add(PointTrack);
                    InitialClassArea.Targets.Add(PointTrack.transform);
                    if (InitialClassArea.MesurePoints.Count > 1)
                    {
                        InitialClassArea.lineRendererMap.GetComponent<LineRenderer>().enabled = true;

                    }
                    else
                    {
                        InitialClassArea.lineRendererMap.GetComponent<LineRenderer>().enabled = false;
                    }
                    PointTrack = null;
                }
            }

        }
    }
    

    
    private void DrawLineBetweenPoints(AreaClass areaClassX)
    {
        if (CheckAddPointBtnClick)
        {
            areaClassX.lineRendererMap.GetComponent<LineRenderer>().positionCount = areaClassX.MesurePoints.Count;
            for (int i = 0; i < areaClassX.MesurePoints.Count; i++)
            {
                areaClassX.lineRendererMap.GetComponent<LineRenderer>().SetPosition(i, areaClassX.MesurePoints.ElementAt(i).transform.position);
            }
        }
        else
        {
            areaClassX.lineRendererMap.GetComponent<LineRenderer>().positionCount = areaClassX.MesurePoints.Count + 1;
            for (int i = 0; i < areaClassX.MesurePoints.Count; i++)
            {
                areaClassX.lineRendererMap.GetComponent<LineRenderer>().SetPosition(i, areaClassX.MesurePoints.ElementAt(i).transform.position);
            }
            areaClassX.lineRendererMap.GetComponent<LineRenderer>().SetPosition(areaClassX.MesurePoints.Count, areaClassX.MesurePoints.ElementAt(0).transform.position);
        }

    }

    void DrawLineBetweenPointsForClasses(AreaClass areaClassX)
    {
        areaClassX.lineRendererMap.GetComponent<LineRenderer>().positionCount = areaClassX.MesurePoints.Count + 1;
        for (int i = 0; i < areaClassX.MesurePoints.Count; i++)
        {
            areaClassX.lineRendererMap.GetComponent<LineRenderer>().SetPosition(i, areaClassX.MesurePoints.ElementAt(i).transform.position);
        }
        areaClassX.lineRendererMap.GetComponent<LineRenderer>().SetPosition(areaClassX.MesurePoints.Count, areaClassX.MesurePoints.ElementAt(0).transform.position);
    }


    void CalcSlops(AreaClass areaClass)
    {
        areaClass.Slops = new List<float>();
        for (int i = 1; i < areaClass.Targets.Count; i++)
        {
            if ((areaClass.Targets.ElementAt(i).position.x - areaClass.Targets.ElementAt(i - 1).position.x) == 0)
            {
                areaClass.Slops.Add(float.PositiveInfinity);
                continue;
            }
            float slop = (areaClass.Targets.ElementAt(i).position.z - areaClass.Targets.ElementAt(i - 1).position.z) / (areaClass.Targets.ElementAt(i).position.x - areaClass.Targets.ElementAt(i - 1).position.x);
            areaClass.Slops.Add(slop);
        }
    }

    

    
    void ConvertVec3ToVec2New(AreaClass areaClass)
    {
        areaClass.Points = new List<Vector2>();
        Vector2 IntialPoint = new Vector2(areaClass.Targets.ElementAt(0).position.x, areaClass.Targets.ElementAt(0).position.z);

        areaClass.Points.Add(IntialPoint);

        for (int i = 1; i < areaClass.Targets.Count; i++)
        {
            float Distance = Vector3.Distance(areaClass.Targets.ElementAt(i - 1).position, areaClass.Targets.ElementAt(i % areaClass.Targets.Count).position);
            // For real distance
            Distance *= 55.9f;
            

            if (areaClass.Slops.ElementAt(i - 1) == float.PositiveInfinity)
            {
                if (areaClass.Targets.ElementAt(i).position.z < areaClass.Targets.ElementAt(i - 1).position.z)
                {

                    float Y2 = areaClass.Points.ElementAt(i - 1).y - Distance;
                    float X2 = areaClass.Points.ElementAt(i - 1).x;
                    Vector2 point = new Vector2(X2, Y2);
                    areaClass.Points.Add(point);
                }
                else
                {
                    float Y2 = areaClass.Points.ElementAt(i - 1).y + Distance;
                    float X2 = areaClass.Points.ElementAt(i - 1).x;
                    Vector2 point = new Vector2(X2, Y2);
                    areaClass.Points.Add(point);
                }
                continue;
            }

            // to remove sqrt
            Distance = Distance * Distance;
            //Calc Y equation for slop
            // Y2 = SX2 - SX1 - (Y1)
            //X Value
            float X2_Val = areaClass.Slops.ElementAt(i - 1);
            float last_equation = (-1f * (areaClass.Slops.ElementAt(i - 1) * areaClass.Points.ElementAt(i - 1).x)) + (areaClass.Points.ElementAt(i - 1).y);


            //Concatitinat between 2 equetion sqrt((Y2 - Y1)^2 + (X2 - X1)^2) = Distance
            //We will Compination Y2 By This

            // Add Y1 to Last Equation
            float ConcLastEquation = last_equation + (areaClass.Points.ElementAt(i - 1).y * -1);


            // final Distance^2 = (SX2 - ConcLastEquation)^2 + (x2 - x1)^2

            // first part
            float a1 = X2_Val * X2_Val;
            float b1 = 2f * ConcLastEquation * X2_Val;
            float c1 = ConcLastEquation * ConcLastEquation;


            // last part
            float a2 = 1f;
            float b2 = 2f * areaClass.Points.ElementAt(i - 1).x * -1f;
            float c2 = areaClass.Points.ElementAt(i - 1).x * areaClass.Points.ElementAt(i - 1).x;


            // Concatinate 2 Equetion
            float a3 = a1 + a2;
            float b3 = b1 + b2;
            float c3 = c1 + c2 + (Distance * -1f);


            float Delta = b3 * b3 - 4f * a3 * c3;


            if (Delta == 0)
            {
                float X2 = (-1f * b3) / (2 * a3);
                // Y2 = SX2 + lastEquation
                float Y2 = X2_Val * X2 + last_equation;
                Vector2 point = new Vector2(X2, Y2);
                areaClass.Points.Add(point);
            }
            else if (Delta > 0)
            {

                float X2_1 = (-b3 + Mathf.Sqrt(Delta)) / (2 * a3);
                float X2_2 = (-b3 - Mathf.Sqrt(Delta)) / (2 * a3);

                if (areaClass.Targets.ElementAt(i).position.x > areaClass.Targets.ElementAt(i - 1).position.x)
                {
                    if (X2_1 > areaClass.Points.ElementAt(i - 1).x)
                    {
                        float Y2 = X2_Val * X2_1 + last_equation;
                        Vector2 point = new Vector2(X2_1, Y2);
                        areaClass.Points.Add(point);
                    }
                    else
                    {
                        float Y2 = X2_Val * X2_2 + last_equation;
                        Vector2 point = new Vector2(X2_2, Y2);
                        areaClass.Points.Add(point);
                    }


                }
                else
                {
                    if (X2_1 < areaClass.Points.ElementAt(i - 1).x)
                    {
                        float Y2 = X2_Val * X2_1 + last_equation;
                        Vector2 point = new Vector2(X2_1, Y2);
                        areaClass.Points.Add(point);
                    }
                    else
                    {
                        float Y2 = X2_Val * X2_2 + last_equation;
                        Vector2 point = new Vector2(X2_2, Y2);
                        areaClass.Points.Add(point);
                    }
                }

            }
            else
            {
                Debug.Log("error");
            }


        }


    }

    
    float CalcAreaOfPoligon(AreaClass areaClass)
    {
        float area = 0;
        for (int i = 0; i < areaClass.Points.Count; i++)
        {
            Vector2 va = areaClass.Points.ElementAt(i);
            Vector2 vb = areaClass.Points.ElementAt((i + 1) % areaClass.Points.Count);
            float width = vb.x - va.x;
            float height = (vb.y + va.y) / 2;

            area += (width * height);
        }

        return Mathf.Abs(area);
    }
    

    public void CheckClickAddArea()
    {
        if (!CheckAddPointBtnClick)
        {
            this.InitalClassArea();

            CheckAddPointBtnClick = true;
        }
        else
        {
            CheckAddPointBtnClick = false;
            this.DestroyAnyThing(InitialClassArea);
            InitialClassArea = null;
        }
    }

    private void InitalClassArea()
    {
        
        InitialClassArea = new AreaClass();
        InitialClassArea.MesurePoints = new List<GameObject>();
        GameObject lineRenderX = Instantiate(LineRenderMap, new Vector3(0, 0, 0), Quaternion.identity);
        InitialClassArea.lineRendererMap = lineRenderX;
        InitialClassArea.Targets = new List<Transform>();
    }

    private void DestroyAnyThing(AreaClass area)
    {
        if (area.MesurePoints != null)
        {
            foreach (GameObject gameObject in area.MesurePoints)
            {
                Destroy(gameObject);
            }
        }

        Destroy(area.lineRendererMap);
        Destroy(PointTrack);
        area.Targets = null;
        area.Points = null;
        area.Slops = null;
        PointTrack = null;
    }

    private void ManageCalcTotalArea()
    {
        totalArea = 0;
        float area = 0;
        AreaReport = new List<float>();
        //Calac selected Point area
        if (PointArea != null)
        {
            AreaClass areaClassX = GetAreaClassFromPoint(PointArea);
            if(areaClassX.Targets.Count >= 3)
            {
                this.CalcSlops(areaClassX);
                this.ConvertVec3ToVec2New(areaClassX);
                //Debug.Log("Area : "+CalcAreaOfPoligon(areaClass));
                area = CalcAreaOfPoligon(areaClassX);
            }
        }
        //Calc total Area
        foreach (AreaClass areaClass in areaClasses)
        {
            if (areaClass.MesurePoints.Count > 1)
            {
                DrawLineBetweenPointsForClasses(areaClass);
            }
            if (areaClass.Targets.Count >= 3)
            {

                this.CalcSlops(areaClass);
                this.ConvertVec3ToVec2New(areaClass);
                //Debug.Log("Area : "+CalcAreaOfPoligon(areaClass));
                float areax = CalcAreaOfPoligon(areaClass);
                totalArea += areax;
                AreaReport.Add(areax);
            }

        }


        if (totalArea != 0)
        {

            ShowAreaDescriptionPanel(true, totalArea,area);
        }
    }

    //==============================================>
    //SelectPoint
    private void SelectPointToMove()
    {
        if (!CheckAddPointBtnClick && Input.GetMouseButtonDown(0) && !checkMovePoint)
        {
            var ray = mainCamira.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform.tag == "PointArea" || raycastHit.transform.tag == "Start_Point_Area")
                {
                    AreaDescriptionPanel.SetActive(!AreaDescriptionPanel.activeInHierarchy);
                    if (!checkSelectedPoint)
                    {
                        PointArea = raycastHit.transform.gameObject;
                        checkSelectedPoint = true;
                    }
                    else
                    {
                        checkSelectedPoint = false;
                    }
                }
                    
            }
        }
    }

    //Move Point
    public void CheckMovePointMethod()
    {
        checkMovePoint = true;
    }

    private void MoveSelectedPoint()
    {
        if(!CheckAddPointBtnClick && PointArea != null && checkMovePoint)
        {
            var ray = mainCamira.ScreenPointToRay(Input.mousePosition);
            if (map.GetComponent<MapRenderer>().Raycast(ray, out MapRendererRaycastHit hitInfo))
            {
                PointArea.transform.position = hitInfo.Point;
                if (Input.GetMouseButtonDown(0))
                {
                    checkMovePoint = false;
                }
            }
            
        }
    }

    //==============================================>
    //Calc Area
    private void ShowAreaDescriptionPanel(bool check, float totalArea,float area)
    {
        LabelArea.text = "Area : "+Math.Round(area, 2) + " m\u00b2";
        LabelTotalArea.text = "Total Area : "+Math.Round(totalArea,2) + " m\u00b2";
    }


    //Delete Area Class
    public void DeleteAreaClass()
    {
        if(!CheckAddPointBtnClick && PointArea != null)
        {
            bool check = false;
            for (int i = 0; i < areaClasses.Count; i++)
            {

                for(int j = 0; j< areaClasses.ElementAt(i).MesurePoints.Count; j++)
                {
                    if (PointArea == areaClasses.ElementAt(i).MesurePoints.ElementAt(j))
                    {
                        DestroyAnyThing(areaClasses.ElementAt(i));
                        areaClasses.RemoveAt(i);
                        AreaDescriptionPanel.SetActive(false);
                        checkSelectedPoint = false;
                        PointArea = null;
                        check = true;
                        break;
                    }
                }
                if (check)
                {
                    break;
                }
                
            }
        }
    }

    private AreaClass GetAreaClassFromPoint(GameObject PointSelected)
    {
        for (int i = 0; i < areaClasses.Count; i++)
        {
            for (int j = 0; j < areaClasses.ElementAt(i).MesurePoints.Count; j++)
            {
                if (PointSelected == areaClasses.ElementAt(i).MesurePoints.ElementAt(j))
                {
                    return areaClasses.ElementAt(i);
                }
            }
        }
        return null;
    }
}
