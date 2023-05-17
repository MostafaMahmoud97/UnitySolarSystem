using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.Maps.Unity;
using System.Linq;

public class MeasureAreaScript : MonoBehaviour
{

    [Header("Map Render")]
    public GameObject map;

    [SerializeField] private Camera mainCamira;
    [SerializeField] private GameObject InitPoint;


    private List<GameObject> MesurePoints;
    private RaycastHit raycastHit;
    private bool CheckAddPointBtnClick = true;
    private LineRenderer lineRenderer;

    private List<Transform> Targets;
    private List<Vector2> Points;
    private List<float> Slops;


    // Start is called before the first frame update
    void Start()
    {
        MesurePoints = new List<GameObject>();
        lineRenderer = mainCamira.GetComponent<LineRenderer>();
        Targets = new List<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        this.AddPoint();
        
        if (MesurePoints.Count > 1)
        {
            this.DrawLineBetweenPoints();
        }

        if (Targets.Count >= 3)
        {
            this.CalcSlops();
            ConvertVec3ToVec2New();
            /*
            for (int i = 0; i < Points.Count; i++)
            {
                Debug.Log(Points.ElementAt(i));
            }
            */
            Debug.Log(CalcAreaOfPoligon());

        }

    }

    private void AddPoint()
    {
        if (CheckAddPointBtnClick && Input.GetMouseButtonDown(0))
        {
            var ray = mainCamira.ScreenPointToRay(Input.mousePosition);

            if (map.GetComponent<MapRenderer>().Raycast(ray, out MapRendererRaycastHit hitInfo))
            {
                Vector3 Position = new Vector3(hitInfo.Point.x, hitInfo.Point.y, hitInfo.Point.z);
                GameObject NewPoint = Instantiate(InitPoint, Position, Quaternion.identity);
                MesurePoints.Add(NewPoint);
                Targets.Add(NewPoint.transform);
                if (MesurePoints.Count > 1)
                {
                    this.lineRenderer.enabled = true;

                }
                else
                {
                    this.lineRenderer.enabled = false;
                }
            }
        }
    }

    private void DrawLineBetweenPoints()
    {
        if (CheckAddPointBtnClick)
        {
            lineRenderer.positionCount = MesurePoints.Count;
            for (int i = 0; i < MesurePoints.Count; i++)
            {
                lineRenderer.SetPosition(i, MesurePoints.ElementAt(i).transform.position);
            }
        }
        else
        {
            lineRenderer.positionCount = MesurePoints.Count + 1;
            for (int i = 0; i < MesurePoints.Count; i++)
            {
                lineRenderer.SetPosition(i, MesurePoints.ElementAt(i).transform.position);
            }
            lineRenderer.SetPosition(MesurePoints.Count, MesurePoints.ElementAt(0).transform.position);
        }

    }

    void CalcSlops()
    {
        Slops = new List<float>();
        for (int i = 1; i < Targets.Count; i++)
        {
            if ((Targets.ElementAt(i).position.x - Targets.ElementAt(i - 1).position.x) == 0)
            {
                Slops.Add(float.PositiveInfinity);
                continue;
            }
            float slop = (Targets.ElementAt(i).position.z - Targets.ElementAt(i - 1).position.z) / (Targets.ElementAt(i).position.x - Targets.ElementAt(i - 1).position.x);
            Slops.Add(slop);
        }
    }

    void ConvertVec3ToVec2New()
    {
        Points = new List<Vector2>();
        Vector2 IntialPoint = new Vector2(Targets.ElementAt(0).position.x, Targets.ElementAt(0).position.z);

        Points.Add(IntialPoint);

        for (int i = 1; i < Targets.Count; i++)
        {
            float Distance = Vector3.Distance(Targets.ElementAt(i - 1).position, Targets.ElementAt(i % Targets.Count).position);
            // For real distance
            Distance *= 55.9f;
            

            if (Slops.ElementAt(i - 1) == float.PositiveInfinity)
            {
                if (Targets.ElementAt(i).position.z < Targets.ElementAt(i - 1).position.z)
                {

                    float Y2 = Points.ElementAt(i - 1).y - Distance;
                    float X2 = Points.ElementAt(i - 1).x;
                    Vector2 point = new Vector2(X2, Y2);
                    Points.Add(point);
                }
                else
                {
                    float Y2 = Points.ElementAt(i - 1).y + Distance;
                    float X2 = Points.ElementAt(i - 1).x;
                    Vector2 point = new Vector2(X2, Y2);
                    Points.Add(point);
                }
                continue;
            }

            // to remove sqrt
            Distance = Distance * Distance;
            //Calc Y equation for slop
            // Y2 = SX2 - SX1 - (Y1)
            //X Value
            float X2_Val = Slops.ElementAt(i - 1);
            float last_equation = (-1f * (Slops.ElementAt(i - 1) * Points.ElementAt(i - 1).x)) + (Points.ElementAt(i - 1).y);


            //Concatitinat between 2 equetion sqrt((Y2 - Y1)^2 + (X2 - X1)^2) = Distance
            //We will Compination Y2 By This

            // Add Y1 to Last Equation
            float ConcLastEquation = last_equation + (Points.ElementAt(i - 1).y * -1);


            // final Distance^2 = (SX2 - ConcLastEquation)^2 + (x2 - x1)^2

            // first part
            float a1 = X2_Val * X2_Val;
            float b1 = 2f * ConcLastEquation * X2_Val;
            float c1 = ConcLastEquation * ConcLastEquation;


            // last part
            float a2 = 1f;
            float b2 = 2f * Points.ElementAt(i - 1).x * -1f;
            float c2 = Points.ElementAt(i - 1).x * Points.ElementAt(i - 1).x;


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
                Points.Add(point);
            }
            else if (Delta > 0)
            {

                float X2_1 = (-b3 + Mathf.Sqrt(Delta)) / (2 * a3);
                float X2_2 = (-b3 - Mathf.Sqrt(Delta)) / (2 * a3);

                if (Targets.ElementAt(i).position.x > Targets.ElementAt(i - 1).position.x)
                {
                    if (X2_1 > Points.ElementAt(i - 1).x)
                    {
                        float Y2 = X2_Val * X2_1 + last_equation;
                        Vector2 point = new Vector2(X2_1, Y2);
                        Points.Add(point);
                    }
                    else
                    {
                        float Y2 = X2_Val * X2_2 + last_equation;
                        Vector2 point = new Vector2(X2_2, Y2);
                        Points.Add(point);
                    }


                }
                else
                {
                    if (X2_1 < Points.ElementAt(i - 1).x)
                    {
                        float Y2 = X2_Val * X2_1 + last_equation;
                        Vector2 point = new Vector2(X2_1, Y2);
                        Points.Add(point);
                    }
                    else
                    {
                        float Y2 = X2_Val * X2_2 + last_equation;
                        Vector2 point = new Vector2(X2_2, Y2);
                        Points.Add(point);
                    }
                }

            }
            else
            {
                Debug.Log("error");
            }


        }


    }


    float CalcAreaOfPoligon()
    {
        float area = 0;
        for (int i = 0; i < Points.Count; i++)
        {
            Vector2 va = Points.ElementAt(i);
            Vector2 vb = Points.ElementAt((i + 1) % Points.Count);
            float width = vb.x - va.x;
            float height = (vb.y + va.y) / 2;

            area += (width * height);
        }

        return Mathf.Abs(area);
    }
}
