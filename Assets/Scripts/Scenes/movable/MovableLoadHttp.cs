using UnityEngine;
using System.Collections;
using LitJson;

public class MovableLoadHttp 
{


    public MovableLoadHttp()
    {
        
    }

    public void LoadJson(Callback<JsonData>  _callback,string Url)
    {
        MovableScene.Instance.StartCoroutine(HttpWWW(Url, _callback));
    }

    private IEnumerator HttpWWW(string Url, Callback<JsonData> _callback)
    {
        WWW whttp = new WWW(Url);
        yield return whttp;
        if (whttp.error==null)
        {
            JsonData jd = JsonMapper.ToObject(whttp.text);
            _callback(jd);
        }
        else
        {
            

        }
        
    }

    public IEnumerator Texture2DWWW(string Url, Callback<Texture2D> _callback)
    {
        WWW whttp = new WWW(Url);
        yield return whttp;
        if (whttp.error == null)
        {
            //My_debugNet.SendDebugLog("Texture2DWWW----" );
            _callback(whttp.texture);
        }
        else
        {
           // My_debugNet.SendDebugLog("whttp.error ----" + whttp.error.ToString());
            _callback(null);
        }
    }


    public void PostLoadJson(Callback<JsonData> _callback, string Url)
    {
        MovableScene.Instance.StartCoroutine(PostLoadJson(Url, _callback));
    }
    private IEnumerator PostLoadJson(string Url, Callback<JsonData> _callback)
    {
        WWWForm wfForm = new WWWForm();



        wfForm.AddField("activity_id",1);
       



        WWW whttp = new WWW(Url, wfForm);
        yield return whttp;
        if (whttp.error == null)
        {
            JsonData jd = JsonMapper.ToObject(whttp.text);
            _callback(jd);
        }
        else
        {
            Debug.Log("--whttp.error--" + whttp.error.ToString());

        }
    }

}
