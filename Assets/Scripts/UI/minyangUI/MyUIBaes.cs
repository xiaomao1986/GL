using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using DG.Tweening;

public class MyUIBaes : MyUIevent
{


    public string jsonDataName;
    public string ParentPathName;

    public bool isLoad = false;


    private float LoadTime = 0;
    /// <summary>
    /// 子节点管理
    /// </summary>
    /// 
    public Dictionary<string, GameObject> HomePageChildDictionary = new Dictionary<string, GameObject>();

    public void Init(string json)
    {
      //  TextAsset HomePageJson = (TextAsset)Resources.Load("HomePage");
        LoadHomePageJsonConfig(json);
    }
    public void LoadHomePageJsonConfig(string json, Callback callback = null, Callback<float> callbackTime = null)
    {
        // gameObject.transform.SetParent(GameObject.Find("Canvas").gameObject.transform);
        JsonData jd = JsonMapper.ToObject(json);

        jsonDataName = jd["Name"].ToString();
        ParentPathName = jd["parentURL"].ToString();

        gameObject.name = jd["Name"].ToString();
        if (gameObject.GetComponent<RectTransform>() == null)
            gameObject.AddComponent<RectTransform>();
        Vector3 RootV3 = new Vector3();
        RootV3.x = float.Parse(jd["position"]["x"].ToString());
        RootV3.y = float.Parse(jd["position"]["y"].ToString());
        RootV3.z = float.Parse(jd["position"]["z"].ToString());
        Vector2 RootV2 = new Vector2();
        RootV2.x = float.Parse(jd["sizeDelta"]["w"].ToString());
        RootV2.y = float.Parse(jd["sizeDelta"]["h"].ToString());
        gameObject.GetComponent<RectTransform>().localPosition = RootV3;
        gameObject.GetComponent<RectTransform>().sizeDelta = RootV2;
        gameObject.GetComponent<RectTransform>().localScale = Vector2.one;
        for (int i = 0; i < jd["Child"].Count; i++)
        {
            JsonData tmp = jd["Child"][i];
            if ((string)tmp["label"] == "SUIButton")
            {
                CreateButton(tmp, jd["parentURL"].ToString());
            }
            if ((string)tmp["label"] == "SUIMenu")
            {
                CreateSUIMenu(tmp);
            }
            LoadTime += Time.deltaTime;
            if (callbackTime != null)
            {
                callbackTime(LoadTime);
            }
        }
        if (callback != null)
        {
            callback();
        }
      //  Play();
    }

    public void ButtonLose(string ButtonName,bool isLose)
    {
        if (HomePageChildDictionary.ContainsKey(ButtonName))
	    {
		   HomePageChildDictionary[ButtonName].GetComponent<SUIButton>().isLose=isLose;
	    }
    }


    public void CreateButton(JsonData tmp,string path)
    {
        string s = tmp["PrefabName"].ToString();
        GameObject obj = Instantiate(Resources.Load("UI/" + path + "/" + s)) as GameObject;
        obj.transform.SetParent(gameObject.transform);
        obj.name = s;
        obj.SetActive(false);
        Vector3 v = new Vector3();
        v.x = float.Parse(tmp["position"]["x"].ToString());
        v.y = float.Parse(tmp["position"]["y"].ToString());
        v.z = float.Parse(tmp["position"]["z"].ToString());
        obj.GetComponent<RectTransform>().localPosition = v;
        obj.GetComponent<RectTransform>().localScale = Vector3.one;
        Vector2 v2 = new Vector2();
        v2.x = float.Parse(tmp["sizeDelta"]["w"].ToString());
        v2.y = float.Parse(tmp["sizeDelta"]["h"].ToString());
        obj.GetComponent<UIAnimation>().AnimationType = int.Parse(tmp["AnimationType"].ToString());
        obj.GetComponent<UIAnimation>().ScriptType = int.Parse(tmp["ScriptType"].ToString());
        obj.GetComponent<UIAnimation>().direction = (UIAnimation.Direction)System.Enum.Parse(typeof(UIAnimation.Direction), tmp["Direction"].ToString());
        obj.GetComponent<RectTransform>().sizeDelta = v2;

        if (s=="mine")
        {
            obj.GetComponent<SUIButton>().isDownEvent = true;
        }

        HomePageChildDictionary.Add(s, obj);
    }
    public void CreateSUIMenu(JsonData tmp)
    {
        string path = tmp["jsonURL"].ToString();
        GameObject obj = new GameObject();
        obj.SetActive(true);
        TextAsset MenuJson = (TextAsset)Resources.Load(path);
        obj.transform.SetParent(gameObject.transform);
        obj.AddComponent<SUIMenu>().LoadMenuJsonConfig(MenuJson.text);
        obj.GetComponent<UIAnimation>().AnimationType = int.Parse(tmp["AnimationType"].ToString());
        obj.GetComponent<UIAnimation>().ScriptType = int.Parse(tmp["ScriptType"].ToString());
        obj.GetComponent<UIAnimation>().direction = (UIAnimation.Direction)System.Enum.Parse(typeof(UIAnimation.Direction), tmp["Direction"].ToString());
        HomePageChildDictionary.Add(obj.name, obj);

    }
    public void Play()
    {
        foreach (var item in HomePageChildDictionary)
        {
            if (item.Value.GetComponent<SUIMenu>() == null)
            {
                item.Value.GetComponent<SUIButton>().AnimPlay(true);
            }
        }
    }
    public void Quit()
    {
        foreach (var item in HomePageChildDictionary)
        {
            if (item.Value.GetComponent<SUIMenu>() == null)
            {
                item.Value.GetComponent<SUIButton>().AnimPlay(false);
            }
        }
    }

}
