using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class WorksData
{
    public string Model_name;
    public string Texture_Path;
    public string Jpg_path;
}

public class WorksDataGroup
{
    public WorksData[] worksDatas;
}


public class WorksDataControl : MonoBehaviour
{
    public static WorksDataControl Instance;
    public List<WorksData> worksDatas = new List<WorksData>();
    private string WorksJsonDataPath = "/Resources/WorksDatas";
    private string WorksJsonDataName = "WorksJsonDatas.Json";

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (System.IO.Directory.Exists(Application.dataPath+ WorksJsonDataPath + "/" + WorksJsonDataName))
        {
            string str = Resources.Load<TextAsset>(WorksJsonDataPath + "/" + WorksJsonDataName).text;
            WorksDataGroup worksDatasGroup = JsonConvert.DeserializeObject<WorksDataGroup>(str);
            for (int i = 0; i < worksDatasGroup.worksDatas.Length; i++)
            {
                worksDatas.Add(worksDatasGroup.worksDatas[i]);
            }
        }
        //else
        //{
        //    worksDatas = null;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveFile(string msg, string FilePath)
    {
        var fss = new System.IO.FileStream(FilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        var sws = new System.IO.StreamWriter(fss);
        sws.Write(msg);
        sws.Close();
        fss.Close();
        Debug.Log("保存数据成功===" + msg);
    }

    private void OnDestroy()
    {
        if (worksDatas.Count > 0)
        {
            WorksDataGroup worksDataGroup = new WorksDataGroup();
            worksDataGroup.worksDatas = worksDatas.ToArray();
            string str = JsonConvert.SerializeObject(worksDataGroup);
            SaveFile(str, Application.dataPath+"/" +WorksJsonDataPath + "/" + WorksJsonDataName);
        }
    }
}
