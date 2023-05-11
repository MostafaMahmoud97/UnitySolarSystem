using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCameraScript : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Vector3 previousPosition;
    private Vector3 previousRotation;


    [Header("Zoom Controller")]
    public float ZoomChange;
    public float SmoothChange;
    public float MinSize, MaxSize;

    [Header("Transform Controller")]
    public float TransformChange;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.ChangePosition();

        this.ChangeRotation();

        this.ZoomInAndOut();
    }
    public void ChangePosition()
    {
        // Transform Position
        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);

            cam.transform.Translate(direction.x * TransformChange, 0f, direction.y * TransformChange);

            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
    }
    public void ChangeRotation()
    {
        // Transform Rotation
        if (Input.GetMouseButtonDown(1))
        {
            previousRotation = cam.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 direction = previousRotation - cam.ScreenToViewportPoint(Input.mousePosition);

            cam.transform.Rotate(new Vector3(1f, 0f, 0f), direction.y * 180);
            cam.transform.Rotate(new Vector3(0f, 1f, 0f), -direction.x * 180, Space.World);

            previousRotation = cam.ScreenToViewportPoint(Input.mousePosition);
        }
    }

    public void ZoomInAndOut()
    {
        // Zoom In And Out
        if (Input.mouseScrollDelta.y > 0)
        {
            cam.fieldOfView -= ZoomChange * Time.deltaTime * SmoothChange;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            cam.fieldOfView += ZoomChange * Time.deltaTime * SmoothChange;
        }

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, MinSize, MaxSize);
    }
}
