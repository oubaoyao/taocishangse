using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public GameObject Model;
    [SerializeField]
    public Material[] OldModelMaterial;
    [SerializeField]
    private Material[] NewModelMaterial;

    // Start is called before the first frame update
    void Start()
    {
        //OldModelMaterial = Model.GetComponent<MeshRenderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            NewModelMaterial = Model.GetComponent<MeshRenderer>().materials;
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Model.GetComponent<MeshRenderer>().materials= OldModelMaterial;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            Model.GetComponent<MeshRenderer>().materials= NewModelMaterial;
        }

    }
}
