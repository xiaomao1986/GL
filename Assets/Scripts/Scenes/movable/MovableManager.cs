using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LitJson;
using UnityEngine.UI;




public class MovableData
{
    public  string activity_id="";
    public  string begin_time="";
    public string end_time="";
    public Texture2D product_img_Texture2D = null;
    public Texture2D UIstart_img_Texture2D = null;
    public string ret_code = "";
    public string activity_theme = "";
    public string activity_profile = "";
    public string activity_site = "";

    public string activity_img_uri = "";
    public string product_img_uri = "";
    public int amount = 0;

    public bool isEnd = false;
    public bool isNumber = false;

    public string huodongzhuangtai = "疯抢中";


    public void SetTexture2Dproduct(GameObject obj)
    {
        if (product_img_Texture2D!=null)
        {
            obj.GetComponent<Renderer>().materials[2].mainTexture = product_img_Texture2D;
            return;
        }
        MovableScene.Instance.StartCoroutine(Texture2Dproduct(obj));
    }

    public void SetTexture2Dactivity(GameObject obj)
    {
        if (UIstart_img_Texture2D != null)
        {
            obj.GetComponent<Image>().sprite = Sprite.Create(UIstart_img_Texture2D, new Rect(0, 0, UIstart_img_Texture2D.width, UIstart_img_Texture2D.height), new Vector2(1, 1));
            return;
        }
        MovableScene.Instance.StartCoroutine(Texture2Dactivity(obj));
    }



    private IEnumerator Texture2Dproduct(GameObject obj)
    {
        WWW whttp = new WWW(product_img_uri);
        yield return whttp;
        if (whttp.error == null)
        {
            product_img_Texture2D=whttp.texture;
            obj.GetComponent<Renderer>().materials[2].mainTexture = product_img_Texture2D;
        }
    }
    private IEnumerator Texture2Dactivity(GameObject obj)
    {
        WWW whttp = new WWW(activity_img_uri);
        yield return whttp;
        if (whttp.error == null)
        {
            UIstart_img_Texture2D = whttp.texture;
            obj.GetComponent<Image>().sprite = Sprite.Create(UIstart_img_Texture2D, new Rect(0, 0,UIstart_img_Texture2D.width,UIstart_img_Texture2D.height), new Vector2(1, 1));
        }
    }
}

public class MovableManager
{



    public List<MovableData> movabledataList = new List<MovableData>();
    ///活動ID
    private string  activity_id ="";
    public Arrow arrow;
    private MovableLoadHttp movableLoadHttp = null;

    public GameObject MovableRootObj = null;


    private string http = "http://220.231.48.254:6080";
    private string pathjson = "v1/advert/activity_list.json?";
    private string beginjson = "v1/advert/activity_begin.json?";
    private string seckilljson = "v1/advert/seckill_product.json?";
    private string server_timejson = "/v1/sys/server_time.json";

    private string user_packjson = " v1/advert/user_pack.json?";
    private string activity_notice = "/v1/advert/activity_notice.json?";


    private string begin_time = "";
    private string end_time = "";

    public MovableConfigArray ConfigArray = null;



    public MovableManager(GameObject obj)
    {
       // PlayerPrefs.SetString("native_baseURL", "http://192.168.1.30:6080");
        http = PlayerPrefs.GetString("native_baseURL");
        MovableRootObj = obj;
        movableLoadHttp = new MovableLoadHttp();
        arrow = new Arrow();
        ConfigArray = new MovableConfigArray();
        string url = http + activity_notice + "page=0&page_rows=100";
        Debug.Log("-------------------" + url);
        movableLoadHttp.LoadJson(SetUIScrollJsonData, url);
    }
    private void SetUIScrollJsonData(JsonData jd)
    {
        Debug.Log("-------------------"+ jd.ToJson());
        for (int i = 0; i < jd["result"].Count; i++)
        {
            string activity_theme = jd["result"][i]["activity_theme"].ToString();
            string activity_site = jd["result"][i]["activity_site"].ToString();
            string amount = jd["result"][i]["amount"].ToString();
            string begin_time = jd["result"][i]["begin_time"].ToString();
            string end_time = jd["result"][i]["end_time"].ToString();
            string Tiem = begin_time + "-" + end_time;
            MovableUImanager.Instance.SetUIScroll(Tiem, activity_site, activity_theme, amount);
        }
    }
    public void Delete()
    {
        movableLoadHttp = null;
        ConfigArray.Delete();
        ConfigArray = null;
        arrow.Delete();
        arrow = null;
    }
    float currentN=0;
    float currentE=0;
    public void GetGps2(float _longitude, float _latitude)
    {
        Debug.Log("-====GetGps2==" + _longitude.ToString());
        currentN = _latitude;
        currentE = _longitude;
        string geo_lat = "geo_lat=" + _latitude.ToString();
        string geo_lng = "geo_lng=" + _longitude.ToString();
        string uid ="uid="+ PlayerPrefs.GetString("uid");
        Debug.Log("发送消息 请求活动");
        //My_debugNet.SendDebugLog("发送消息 请求活动");
        //       messenger.broadcast<string, string[], callback<string, jsondata>, bool, int>
        //("u2n_u_http", user_packjson, new string[] { uid, "page = 0", "page_rows = 1" },
        //delegate (string err, jsondata js)
        //{
        //    if (err == null || err.equals(""))
        //    {
        //        my_debugnet.senddebuglog("pathjson请求" + js.tojson());
        //       /// user_pack(js, uid, geo_lat, geo_lng);
        //    }
        //    else
        //    {
        //        debug.log("pathjson请求 失败");
        //        my_debugnet.senddebuglog("pathjson请求 失败");
        //    }
        //}, true, 1);

        Messenger.Broadcast<string, string[], Callback<string, JsonData>, bool, int>
          ("U2N_U_HTTP", pathjson, new string[] { uid, geo_lat, geo_lng, "page = 0", "page_rows = 100" },
          delegate (string err, JsonData js2)
          {
              if (err == null || err.Equals(""))
              {
                  //My_debugNet.SendDebugLog("pathjson请求JsonMovable===" + js.ToJson());
                  JsonMovable(js2);
              }
              else
              {
                  Debug.Log("pathjson请求 失败");
                        // My_debugNet.SendDebugLog("pathjson请求 失败");
                    }
          }, true, 1);


    }
    private void user_pack(JsonData js, string uid, string geo_lat, string geo_lng)
    {
        int total_rows = int.Parse(js["total_rows"].ToString());
        if (total_rows >0)
            MovableUImanager.Instance.SetbackpackImage(2);
        else
            MovableUImanager.Instance.SetbackpackImage(1);
    }
    private void LoadHttpJson()
    {
        MovableScene.Instance.Load(0);
        string URL = http + pathjson + "uid=10000&geo_lat=10&geo_lng=10&page=0&page_rows=2";
       // movableLoadHttp.LoadJson(JsonMovable, URL);
    }
    private void JsonMovable(JsonData jd)
    {
        MovableScene.Instance.Load(1);

        if (jd["ret_code"].ToString() != "0" || jd["err_msg"].ToString() != "ok")
        {
            Debug.Log("  错误 ：----" + jd["ret_code"].ToString());
            return;
        }
        JsonData result = jd["result"];
        if (result.Count==0)
        {
            Debug.Log("  错误 ：没有 活动" );
            MovableUImanager.Instance.BeingBack("-1001");
            return;
        }
        else
        {
            for (int i = 0; i < result.Count; i++)
            {
                MovableData tmpdata = new MovableData();
                tmpdata.activity_id = result[i]["activity_id"].ToString();
                tmpdata.begin_time = result[i]["begin_time"].ToString();
                tmpdata.end_time = result[i]["end_time"].ToString();
                tmpdata.activity_theme = result[i]["activity_theme"].ToString();
                tmpdata.activity_profile = result[i]["activity_profile"].ToString();
                tmpdata.activity_site = result[i]["activity_site"].ToString();
                tmpdata.activity_img_uri = result[i]["activity_img_uri"].ToString();
                tmpdata.product_img_uri = result[i]["d3_ad_product"]["product_img_uri"].ToString();
                tmpdata.amount = int.Parse(result[i]["amount"].ToString());
                movabledataList.Add(tmpdata);
            }
            if (movabledataList.Count==0)
            {
                MovableUImanager.Instance.BeingBack("-1001");
                return;
            }
            Loadbegin(MovableUImanager.Instance.BeingBack, movabledataList[0].activity_id);
            MovableUImanager.Instance.SetUIstart(movabledataList);
        }
    }

    private Callback<string> msgCallback = null;
    public void Loadbegin(Callback<string> _msgCallback, string activity_id)
    {
        msgCallback = _msgCallback;
        string Activity_id = "activity_id=" + activity_id;
        Messenger.Broadcast<string, string[], Callback<string, JsonData>, bool, int>
        ("U2N_U_HTTP", beginjson, new string[] { Activity_id },
        delegate(string err, JsonData js)
        {
            if (err == null || err.Equals(""))
            {
               Getactivity_begin(js);
                My_debugNet.SendDebugLog("Loadbegin-----" + js.ToJson());
            }
            else
            {
                Debug.Log("beginjson请求 失败");
            }
        }, false, 1);
    }
    public int MovableStartSum = 0;
    private void Getactivity_begin(JsonData arg1)
    {
        MovableStartSum++;
        if (arg1["ret_code"].ToString() != "0")
        {
            movabledataList[MovableStartSum - 1].ret_code = arg1["ret_code"].ToString();
            if (MovableStartSum == movabledataList.Count)
            {
                CreateBalloon();
                return;
            }
            Loadbegin(MovableUImanager.Instance.BeingBack, movabledataList[MovableStartSum].activity_id);
        }
        else
        {
            movabledataList[MovableStartSum - 1].ret_code = arg1["ret_code"].ToString();
            if (MovableStartSum == movabledataList.Count)
            {
                CreateBalloon();
                return;
            }
            Loadbegin(MovableUImanager.Instance.BeingBack, movabledataList[MovableStartSum].activity_id);
        }
       
    }

    public bool isGift = false;
    public void CreateBalloon()
    {
        //MovableData tmp01 = new MovableData();
        //tmp01.activity_id = "qweeeqweqwe";
        //tmp01.activity_theme = "qweeeeee";
        //tmp01.activity_site = "qweeeeee";
        //MovableData tmp02 = new MovableData();
        //tmp02.activity_id = "qweeeqweqwe2";
        //tmp02.activity_theme = "qweeeeee2";
        //tmp02.activity_site = "qweeeeee2";

        List<MovableData> tmpDatas=new List<MovableData>();
        //tmpDatas.Add(tmp01);
        //tmpDatas.Add(tmp02);
        for (int i = 0; i < movabledataList.Count; i++)
        {
            if (movabledataList[i].ret_code == "0")
            {
                MovableData tmp = movabledataList[i];
                tmpDatas.Add(tmp);
            }
        }
        int k = 0;
        if (tmpDatas.Count < MovableScene.Instance.BalloonCount && tmpDatas.Count!=0)
        {
            k = MovableScene.Instance.BalloonCount / tmpDatas.Count;
        }
        else
        {
            k = 1;
        }
        if (tmpDatas.Count==0)
        {
            k = MovableScene.Instance.BalloonCount;
            MovableUImanager.Instance.BeingBack("-303");
            MovableUImanager.Instance.isIstartButtonUp = false;
        }
        else
        {
            MovableUImanager.Instance.BeingBack("0");
        }
        int j = 0;
            for (int m = 0; m < tmpDatas.Count; m++)
            {
                for (int i = 0; i < k; i++)
                {
                    balloonData tmp = balloonData.Create();
                    tmp.gameObject.SetActive(false);
                    tmp.gameObject.transform.parent.gameObject.transform.SetParent(MovableRootObj.transform);
                    Vector3 v = ConfigArray.GetPos(j);
                    tmp.gameObject.transform.parent.gameObject.transform.localPosition = v;
                    tmp.gameObject.transform.parent.gameObject.transform.LookAt(MovableRootObj.transform);
                    float ey = tmp.gameObject.transform.parent.gameObject.transform.localRotation.eulerAngles.y;
                    tmp.gameObject.transform.parent.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, ey, 0));
                    tmp.movabledata = tmpDatas[m];
                    tmp.SetTexture2D();
                    tmp.Activity_id = tmpDatas[m].activity_id;
                    tmp.Move();
                    j++;
                }
            }
        MovableUImanager.Instance.isIstartButtonUp = false;
        MovableScene.Instance.Load(100);
    }

    public void RequestGift(GameObject obj, string _Activity_id)
    {
        if (isGift == false)
        {
            MovableScene.Instance.Load(0);
            string uid = "uid=" + PlayerPrefs.GetString("uid");
            string Activity_id = "activity_id=" + _Activity_id.ToString();
            Messenger.Broadcast<string, string[], Callback<string, JsonData>, bool, int>
            ("U2N_U_HTTP", seckilljson, new string[] { uid, Activity_id },
            delegate(string err, JsonData js)
            {
                if (err == null || err.Equals(""))
                {
                    Getactivity_seckill(js, obj, _Activity_id);
                }
                else
                {
                    Debug.Log("seckilljson请求 失败");
                }
            }, true, 2);
        }
        else
        {
            MovableScene.Instance.Load(100);
        }
    }

    public String huodongID = "";
    private void Getactivity_seckill(JsonData arg1,GameObject obj,string id)
    {
        MovableScene.Instance.Load(1);
        if (arg1["ret_code"].ToString() != "0")
        {
            huodongID = id;
            MovableUImanager.Instance.BeingBack(arg1["ret_code"].ToString());
            MovableScene.Instance.Load(100);
            return;
        }
        string Name = arg1["result"]["business_name"].ToString();
        string product_name = arg1["result"]["product_name"].ToString();
        string product_profile = arg1["result"]["product_profile"].ToString();
        string created_at = arg1["result"]["created_at"].ToString();
        string text = created_at + product_name;
        for (int i = 0; i < movabledataList.Count; i++)
        {
            if (movabledataList[i].activity_id == id)
            {
                MovableUImanager.Instance.seckil(movabledataList[i].product_img_Texture2D, obj);
            }
        }
        
        MovableScene.Instance.Load(100);
    }
    public void StartTime()
    {
        string url = http + server_timejson;
        movableLoadHttp.LoadJson(SetTime, url);
    }

    private int Days = 0;
    private int Hours = 0;
    private int Minutes = 0;
    private int Seconds = 0;
    public void SetTime(JsonData jd)
    {
        if (jd["ret_code"].ToString() != "0" || jd["err_msg"].ToString() != "ok")
        {
            return;
        }
        begin_time =jd["result"]["server_time"].ToString();

        DateTime begin_timedate = new DateTime();
        begin_timedate = Convert.ToDateTime(begin_time);

        DateTime end_timedate = new DateTime();
        end_timedate = Convert.ToDateTime(end_time);

        TimeSpan ts = Function.DateDiff1(begin_timedate, end_timedate);
        Days = int.Parse(ts.Days.ToString());
        Hours = int.Parse(ts.Hours.ToString());
        Minutes = int.Parse(ts.Minutes.ToString());
        Seconds = int.Parse(ts.Seconds.ToString());
        string dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
        Debug.Log("ExecDateDiff-----" + dateDiff);
       MovableScene.Instance.StartCoroutine(Tiem());

    }
    private bool isTime = true;
    private IEnumerator Tiem()
    {
        while (isTime)
        {
            if (Seconds <= 0)
            {
                Minutes--;
                if (Minutes < 0)
                {
                    Hours--;
                    if (Hours <= 0)
                    {
                        Days--;
                        if (Days <= 0)
                        {
                            Days = 0;
                            Hours = 0;
                        }
                        else
                        {
                            Hours = 23;
                        }
                    }
                    Minutes = 59;
                }
                Seconds = 60;
            }
            Seconds--;
            string dateDiff = Days.ToString() + "天" + Hours.ToString() + "小时" + Minutes.ToString() + "分钟" + Seconds.ToString() + "秒";
            Debug.Log("Seconds====" + dateDiff);

      
            int Hours1 = 24*Days;
            Hours1 = Hours + Hours1;

            string dateDiff1 = Hours1.ToString() + ":" + Minutes.ToString() + ":" + Seconds.ToString();
            MovableUImanager.Instance.SetUITimeText(dateDiff1);
            if (Seconds == 0 && Minutes == 0 && Hours == 0 && Days == 0)
            {
                isTime = false;
            }
            yield return new WaitForSeconds(1f);
        }
    }

}
