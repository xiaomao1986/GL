using UnityEngine;
using System.Collections;
using UnityEditor;
using LitJson;
public class UISetConfig : Editor
{

    public void CreateJson(GameObject obj,string jsonDataName,string ParentPath)
    {
        JsonData jd = new JsonData();
        JsonData RootpositionJson = new JsonData();
        JsonData RootsizeDeltaJson = new JsonData();
        JsonData jd2 = new JsonData();
        int i = 0;
        foreach (Transform item in obj.transform)
        {
            JsonData tmp = new JsonData();
            JsonData TransformJson = new JsonData();
            JsonData positionJson = new JsonData();
            JsonData sizeDeltaJson = new JsonData();


            tmp["PrefabName"] = item.name;
            positionJson["x"] = item.GetComponent<RectTransform>().localPosition.x;
            positionJson["y"] = item.GetComponent<RectTransform>().localPosition.y;
            positionJson["z"] = item.GetComponent<RectTransform>().localPosition.z;
            sizeDeltaJson["w"] = item.GetComponent<RectTransform>().sizeDelta.x;
            sizeDeltaJson["h"] = item.GetComponent<RectTransform>().sizeDelta.y;
            tmp["position"] = positionJson;
            tmp["sizeDelta"] = sizeDeltaJson;
            tmp["AnimationType"] = item.GetComponent<UIAnimation>().AnimationType;
            tmp["ScriptType"] = item.GetComponent<UIAnimation>().ScriptType;
            tmp["Direction"] = item.GetComponent<UIAnimation>().direction.ToString();
            if (item.GetComponent<SUIButton>() != null)
            {
                tmp["label"] = "SUIButton";
                string s = item.GetComponent<SUIButton>().m_ButtonMsg.ToString();
                tmp["ButtonEvent"] = "" + s;
            }
            if (item.GetComponent<SUIMenu>() != null)
            {
                i++;
                string path = Mingyang_MenuEditor.CreateJson(item.gameObject, item.GetComponent<SUIMenu>().jsonDataName,item.GetComponent<SUIMenu>().ParentPathName);
                tmp["label"] = "SUIMenu";
                tmp["jsonURL"] = item.GetComponent<SUIMenu>().jsonDataName;
            }
            else
            {
                // "Assets/Resources/UI/HomePage/"
                MyParent.make(item.gameObject, "Assets/Resources/UI/" + ParentPath + "/" + item.name + ".prefab");
              //  MyParent.make(item.gameObject, "Assets/Resources/UI/Photo/" + item.name + ".prefab");
            }
            jd2.Add(tmp);
        }
        RootpositionJson["x"] = obj.GetComponent<RectTransform>().localPosition.x;
        RootpositionJson["y"] = obj.GetComponent<RectTransform>().localPosition.y;
        RootpositionJson["z"] = obj.GetComponent<RectTransform>().localPosition.z;
        RootsizeDeltaJson["w"] = obj.GetComponent<RectTransform>().sizeDelta.x;
        RootsizeDeltaJson["h"] = obj.GetComponent<RectTransform>().sizeDelta.y;
        jd["parentURL"] = ParentPath;
        jd["Name"] = obj.name;
        jd["position"] = RootpositionJson;
        jd["sizeDelta"] = RootsizeDeltaJson;
        jd["Child"] = jd2;
        Debug.Log(jd.ToJson());
        string filepath = Application.dataPath + "/Resources";
        My_UIEditorToos.CreateFile(filepath, jsonDataName, jd);
    }

	
}
