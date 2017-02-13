using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
public class RequestsHttpdata  
{
    private Callback<bool, Dictionary<string, JsonData>,bool> LaodCallback = null;
    private string m_HttpUrl="";
    public void RequestsHttp(string HttpUrl, Callback<bool, Dictionary<string, JsonData>,bool> _callback,JsonData jd)
    {
        LaodCallback = _callback;
        m_HttpUrl = HttpUrl;
        IsLoadTime = false;
        LoadTime = 10;
        LoadOk = false;
        ParseJsonGL(jd);
       // PhotoScene.Instance.StartCoroutine(LoadData(HttpUrl));
       // PhotoScene.Instance.StartCoroutine(LoadTimes());
    }

    private float LoadTime = 10;
    private bool IsLoadTime = false;
    private bool LoadOk = false;
    private IEnumerator LoadData(string url)
    {
        WWW www = new WWW(url);
        yield return www;
       
        if (www.error == null)
        {
            JsonData jd = JsonMapper.ToObject(www.text);
            ParseJson(jd);
            LoadOk = true;
            LoadTime = 10;
        }
        else
        {
           // LaodCallback(false, null,true);
        }
    }
    private IEnumerator LoadTimes()
    {
        while (LoadTime > 0)
        {
            LoadTime--;
            yield return new WaitForSeconds(0.2f);
        }
        if (LoadOk == false && IsLoadTime==false)
        {
            LaodCallback(false, null, true);
        }
    }


    private Dictionary<string, JsonData> m_LoadDataDictionary = new Dictionary<string, JsonData>();

   private void ParseJsonGL(JsonData jd)
    {

        for (int i = 0; i < jd["data"]["data"].Count; i++)
        { 
            JsonData tmpJD = jd["data"]["data"][i];

            Debug.Log("tmpJD---" + tmpJD.ToJson());
            string  Nid= tmpJD["cardId"].ToString();
            m_LoadDataDictionary.Add(Nid, tmpJD);
        }

        for (int i = 0; i < jd["data_redbag"]["data"].Count; i++)
        {
            JsonData tmpJD = jd["data_redbag"]["data"][i];
            Debug.Log("tmpJD--data_redbag-" + tmpJD.ToJson());
            string Nid = tmpJD["cardId"].ToString();
            m_LoadDataDictionary.Add(Nid, tmpJD);
        }

        for (int i = 0; i < jd["data_chest"]["data"].Count; i++)
        {
            JsonData tmpJD = jd["data_chest"]["data"][i];
            Debug.Log("tmpJD--data_chest-" + tmpJD.ToJson());
            string Nid = tmpJD["cardId"].ToString();
            m_LoadDataDictionary.Add(Nid, tmpJD);
        }
        if (LaodCallback != null)
        {
            if (m_LoadDataDictionary.Count == 0)
            {
                LaodCallback(false, null, true);
                return;
            }
            LaodCallback(true, m_LoadDataDictionary, true);
        }


    }


    private void ParseJson(JsonData jd)
    {
        string ret_code = (string)jd["ret_code"];
        if (ret_code!="0")
        {
            IsLoadTime = true;
            LaodCallback(false, null, false);
            return;
        }
        string err_msg = (string)jd["err_msg"];
        if (err_msg != "ok")
        {
            IsLoadTime = true;
            LaodCallback(false, null, false);
            return;
        }
        if (jd["data"].Count==0)
        {
            IsLoadTime = true;
            LaodCallback(false, null, false);
            return;
        }

        m_LoadDataDictionary.Clear();
        PhotoScene.Instance.StartLoading(1);
        for (int i = 0; i < jd["data"].Count; i++)
        {
            try
            {
                JsonData data = jd["data"][i];
                if (!m_LoadDataDictionary.ContainsKey(data["nid"].ToString()))
                {
                    m_LoadDataDictionary.Add(data["nid"].ToString(), data);
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("222" + e.ToString());
            }
        }
        if (LaodCallback!=null)
        {
            if (m_LoadDataDictionary.Count==0)
            {
                 LaodCallback(false, null, true);
                 return;
            }
            LaodCallback(true, m_LoadDataDictionary, true);
        }
        LoadOk = false;
    }
    public void Delete()
    {
        //PhotoScene.Instance.StopCoroutine(LoadTimes());
        PhotoScene.Instance.StopCoroutine(LoadData(m_HttpUrl));
    }


}
