using Es.InkPainter.Sample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelControl : MonoBehaviour
{
    public static ModelControl Instance;

    public Transform ModelTransform;
    public List<Transform> ModelGroup;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform item in ModelTransform)
        {
            ModelGroup.Add(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseModel()
    {
        MousePainter.Instance.ResetMaterial();
        foreach (Transform item in ModelGroup)
        {
            item.gameObject.SetActive(false);
            if (item.name == "Cylinder003")
            {
                item.localEulerAngles = new Vector3(-90, 0, 90);
            }
            else
            {
                item.localEulerAngles = new Vector3(-90, 0, 0);
            }
        }
        
    }
}
