using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public GameObject Model;
    [SerializeField]
    public Material OldModelMaterial;
    [SerializeField]
    private Material NewModelMaterial;
    public RenderTexture canvasTexture;

    // Start is called before the first frame update
    void Start()
    {
        OldModelMaterial = Model.GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    OldModelMaterial.mainTexture = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
        //}

        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    Model.GetComponent<MeshRenderer>().materials= OldModelMaterial;
        //}

        //if(Input.GetKeyDown(KeyCode.E))
        //{
        //    Model.GetComponent<MeshRenderer>().materials= NewModelMaterial;
        //}

    }
}
