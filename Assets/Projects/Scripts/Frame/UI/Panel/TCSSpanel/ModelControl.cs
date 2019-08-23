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
}
