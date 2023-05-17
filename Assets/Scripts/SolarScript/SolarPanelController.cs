using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanelController : MonoBehaviour
{

    [SerializeField] private Camera cam;
    public GameObject SolarPanel;
    bool AddPanelCheck = false;
    RaycastHit raycastHit;

    [Header("Select Solar")]
    public Material SelectedMaterial;
    public GameObject MovePanel;

    [Header("Map Render")]
    public GameObject map;


    private Rigidbody SolarRigidBody;
    private GameObject SelectSolar;
    private Material OldMaterial;
    private bool CheckSelectSolar = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
