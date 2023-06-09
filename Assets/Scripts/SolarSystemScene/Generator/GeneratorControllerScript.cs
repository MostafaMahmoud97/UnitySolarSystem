using Microsoft.Maps.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorControllerScript : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject GeneratorGameObject;
    [SerializeField] private GameObject ControllerGeneratorPanel;

    [Header("Map Render")]
    [SerializeField] private GameObject map;

    [Header("Suggest Panel Message")]
    [SerializeField] private GameObject SuggestationPanel;
    [SerializeField] private Text CurrentGeneratorText;
    [SerializeField] private GameObject WarningPanel;
    [SerializeField] private Text WarningText;

    private RaycastHit raycastHit;

    private bool AddGeneratorCheck;
    private bool SelectedGeneratorCheck;
    private bool MoveGeneratorCheck;

    private GameObject InitGenerator;
    private GameObject SelectedGenerator;

    private int NumberGenerator;

    private bool CheckRotateXAxisUp;
    private bool CheckRotateXAxisDown;
    private bool CheckRotateZAxisUp;
    private bool CheckRotateZAxisDown;
    private bool CheckRotateYAxisUp;
    private bool CheckRotateYAxisDown;

    private void Awake()
    {
        AddGeneratorCheck = false;
        SelectedGeneratorCheck = false;
        MoveGeneratorCheck = false;
        NumberGenerator = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SelectGenerator();
    }

    private void FixedUpdate()
    {
        AddGenerator();
        MoveGenerator();
        RotateXAxisUp();
        RotateXAxisDown();
        RotateZAxisUp();
        RotateZAxisDown();
        RotateYAxisUp();
        RotateYAxisDown();
    }

    public void CheckAddGenerator()
    {
        if (!SelectedGeneratorCheck && !MoveGeneratorCheck)
        {
            AddGeneratorCheck = !AddGeneratorCheck;
            if (AddGeneratorCheck)
            {
                InitGenerator = Instantiate(GeneratorGameObject, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                Destroy(InitGenerator);
            }
        }
        else
        {
            if (SelectedGeneratorCheck)
            {
                ShowWarningLetterPanel("Please Close Selected Generator");
            }else if (MoveGeneratorCheck)
            {
                ShowWarningLetterPanel("Please Close Move Generator");
            }
        }
    }

    private void AddGenerator()
    {
        if (AddGeneratorCheck && !SelectedGeneratorCheck && !MoveGeneratorCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (map.GetComponent<MapRenderer>().Raycast(ray, out MapRendererRaycastHit raycastHit))
            {
                InitGenerator.transform.position = new Vector3(raycastHit.Point.x, raycastHit.Point.y + 0.0112264f, raycastHit.Point.z);
                if (Input.GetMouseButtonDown(0))
                {
                    NumberGenerator++;
                    ShowNumberGenerator();
                    AddGeneratorCheck = false;
                }
            }
        }
    }

    private void SelectGenerator()
    {
        if (Input.GetMouseButtonDown(0) && !AddGeneratorCheck && !MoveGeneratorCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform.tag == "Generator")
                {
                    if (!SelectedGeneratorCheck)
                    {
                        SelectedGenerator = raycastHit.transform.gameObject;
                        SelectedGeneratorCheck = true;
                        ControllerGeneratorPanel.SetActive(true);
                    }
                    else
                    {
                        SelectedGenerator = null;
                        SelectedGeneratorCheck = false;
                        ControllerGeneratorPanel.SetActive(false);
                    }
                }
            }
        }
    }

    public void CheckMoveGenerator()
    {
        MoveGeneratorCheck = true;
    }

    private void MoveGenerator()
    {
        if (MoveGeneratorCheck)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (map.GetComponent<MapRenderer>().Raycast(ray, out MapRendererRaycastHit raycastHit))
            {
                SelectedGenerator.transform.position = new Vector3(raycastHit.Point.x, raycastHit.Point.y + 0.0112264f, raycastHit.Point.z);
                if (Input.GetMouseButtonDown(0))
                {
                    MoveGeneratorCheck = false;
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

        if (SelectedGeneratorCheck && CheckRotateXAxisUp)
        {
            SelectedGenerator.transform.Rotate(15f * Time.deltaTime, 0f, 0f);
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

        if (SelectedGeneratorCheck && CheckRotateXAxisDown)
        {
            SelectedGenerator.transform.Rotate(-15f * Time.deltaTime, 0f, 0f);
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

        if (SelectedGeneratorCheck && CheckRotateYAxisUp)
        {
            SelectedGenerator.transform.Rotate(0f, 15f * Time.deltaTime, 0f);
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

        if (SelectedGeneratorCheck && CheckRotateYAxisDown)
        {
            SelectedGenerator.transform.Rotate(0f, -15f * Time.deltaTime, 0f);
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
        if (SelectedGeneratorCheck && CheckRotateZAxisUp)
        {
            SelectedGenerator.transform.Rotate(0f, 0f, 15f * Time.deltaTime);
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
        if (SelectedGeneratorCheck && CheckRotateZAxisDown)
        {
            SelectedGenerator.transform.Rotate(0f, 0f, -15f * Time.deltaTime);
        }
    }

    public void DeleteGenerator()
    {
        if (SelectedGeneratorCheck)
        {
            Destroy(SelectedGenerator);
            AddGeneratorCheck = false;
            SelectedGeneratorCheck = false;
            ControllerGeneratorPanel.SetActive(false);
            MoveGeneratorCheck = false;
            SelectedGenerator = null;
            NumberGenerator--;
            ShowNumberGenerator();
        }
    }

    private void ShowNumberGenerator()
    {
        CurrentGeneratorText.text = "Number Current Generator : " + NumberGenerator;
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
