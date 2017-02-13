using UnityEngine;
using System.Collections;
using  System.Collections.Generic;
using LitJson;
public class PhotoTransform
{
    public Vector3 pos=Vector3.zero;
    public Vector3 pot = Vector3.zero;
    public Vector3 sca = Vector3.zero;
}



public class PhotoConfigArray  
{

    public List<PhotoTransform> PhotoTransformList = new List<PhotoTransform>();
    public  PhotoConfigArray()
    {
        TextAsset ArrangementJson = (TextAsset)Resources.Load("Arrangement");
        LoadArrangement(ArrangementJson.text);

    }
    public PhotoConfigArray(string  s)
    {
        TextAsset ArrangementJson = (TextAsset)Resources.Load(s);
        LoadArrangement(ArrangementJson.text);
    }


    private void LoadArrangement(string json)
    {
        JsonData jd = JsonMapper.ToObject(json);

        for (int i = 0; i < jd["Array"].Count; i++)
		{
            PhotoTransform tmp = new PhotoTransform();
            tmp.pos.x = float.Parse(jd["Array"][i]["Transform"]["pos"]["x"].ToString());
            tmp.pos.y = float.Parse(jd["Array"][i]["Transform"]["pos"]["y"].ToString());
            tmp.pos.z = float.Parse(jd["Array"][i]["Transform"]["pos"]["z"].ToString());

            tmp.pot.x = float.Parse(jd["Array"][i]["Transform"]["pot"]["x"].ToString());
            tmp.pot.y = float.Parse(jd["Array"][i]["Transform"]["pot"]["y"].ToString());
            tmp.pot.z = float.Parse(jd["Array"][i]["Transform"]["pot"]["z"].ToString());

            tmp.sca.x = float.Parse(jd["Array"][i]["Transform"]["scale"]["x"].ToString());
            tmp.sca.y = float.Parse(jd["Array"][i]["Transform"]["scale"]["y"].ToString());
            tmp.sca.z = float.Parse(jd["Array"][i]["Transform"]["scale"]["z"].ToString());
            PhotoTransformList.Add(tmp);
		}
    }
    public Vector3 GetPos(int Index)
    {
        if (PhotoTransformList.Count == 0 || PhotoTransformList.Count <= Index)
        {
            Vector3 v = Vector3.zero;
            return v;
        }
        return PhotoTransformList[Index].pos;
    }
}
