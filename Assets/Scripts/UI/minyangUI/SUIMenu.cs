using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
public class SUIMenu : UIAnimation 
{

    public string jsonDataName;
    public string ParentPathName;

    public  Dictionary<string, GameObject> MenuDictionary = new Dictionary<string, GameObject>();

  public  void LoadMenuJsonConfig(string json)
    {
        JsonData jd = JsonMapper.ToObject(json);
        gameObject.name = jd["Name"].ToString();

        jsonDataName = jd["Name"].ToString();
        ParentPathName = jd["parentURL"].ToString();
        if (gameObject.GetComponent<RectTransform>() == null)
            gameObject.AddComponent<RectTransform>();
      
        Vector3 RootV3=new Vector3();
        RootV3.x = float.Parse(jd["position"]["x"].ToString());
        RootV3.y = float.Parse(jd["position"]["y"].ToString());
        RootV3.z = float.Parse(jd["position"]["z"].ToString());
        Vector2 RootV2 = new Vector2();
        RootV2.x = float.Parse(jd["sizeDelta"]["w"].ToString());
        RootV2.y = float.Parse(jd["sizeDelta"]["h"].ToString());
        gameObject.GetComponent<RectTransform>().localPosition = RootV3;
        gameObject.GetComponent<RectTransform>().sizeDelta = RootV2;
        gameObject.GetComponent<RectTransform>().localScale=Vector2.one;
        for (int i = 0; i < jd["Child"].Count; i++)
        {
            JsonData tmp = jd["Child"][i];
            string s = tmp["PrefabName"].ToString();
            GameObject obj = Instantiate(Resources.Load("UI/" + jd["parentURL"].ToString() + "/" + s)) as GameObject;
            obj.transform.SetParent(gameObject.transform);
            obj.name = s;
            if (s!="BaseButton")
            {
                //obj.SetActive(false);
            }
            Vector3 v = new Vector3();
            v.x = float.Parse(tmp["position"]["x"].ToString());
            v.y = float.Parse(tmp["position"]["y"].ToString());
            v.z = float.Parse(tmp["position"]["z"].ToString());
            obj.GetComponent<RectTransform>().localPosition = v;
            obj.GetComponent<RectTransform>().localScale = Vector3.one;

            Vector2 v2 = new Vector2();
            v2.x = float.Parse(tmp["sizeDelta"]["w"].ToString());
            v2.y = float.Parse(tmp["sizeDelta"]["h"].ToString());
            obj.GetComponent<RectTransform>().sizeDelta = v2;
            MenuDictionary.Add(s, obj);
        }
        foreach (var item in MenuDictionary)
        {
            if (item.Key != "Backround")
            {
                item.Value.GetComponent<RectTransform>().SetParent(MenuDictionary["Backround"].transform);
                item.Value.GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }

    }
  public void AnimPlay(bool isBase,DG.Tweening.TweenCallback callback=null)
  {
      base.MenuPlay(MenuDictionary, isBase, callback);
  }

}
