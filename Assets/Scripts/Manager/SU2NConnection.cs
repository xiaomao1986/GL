using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.Globalization;




/// <summary>
/// Native 通信
/// </summary>
public class SU2NConnection : MonoBehaviour
{
    /// <summary>
    /// 以下回调无参数，直接调用
    /// </summary>
    //关闭主菜单
    public const string U2N_N_CloseMainMenu = "U2N_N_CloseMainMenu";
    //打开主场景
    public const string U2N_N_OpenMainScene = "U2N_N_OpenMainScene";
    //关闭主场景
    public const string U2N_N_CloseMainScene = "U2N_N_CloseMainScene";
    //关闭附近页面
    public const string U2N_N_CloseNearby = "U2N_N_CloseNearby";
    //关闭故事页面
    public const string U2N_N_CloseStory = "U2N_N_CloseStory";
    //请求关闭世界页面
    public const string U2N_N_CloseWorld = "U2N_N_CloseWorld";
    //关闭地图
    public const string U2N_N_CloseMap = "U2N_N_CloseMap";
    //关闭背包
    public const string U2N_N_CloseBag = "U2N_N_CloseBag"; 
    //关闭分享
    public const string U2N_N_CheeseBack = "U2N_N_CheeseBack";
    //开启app后第一次启动相机
    public const string U2N_N_StartAppCamera = "U2N_N_StartAppCamera";

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="bool isOk">是否登录成功</param>
    /// <param name="string errorCode">错误信息</param>
    public const string U2N_N_CloseLogin = "U2N_N_CloseLogin";

    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="bool isOk"> true 表示发图   false 表示不发图 </param>
    /// <param name="JsonData json">上传的图片信息json字符串</param>
    public const string U2N_N_CloseSendStory = "U2N_N_CloseSendStory";

    /// <summary>
    /// 关闭分享
    /// </summary>
    /// <param name="string err"> 如果不为null或“” 则表示错误内容 </param>
    /// <param name="string type">上传的图片信息json字符串</param>
    public const string U2N_N_CheeseShare = "U2N_N_CheeseShare";
    
    /// <summary>
    /// 请求GPS返回
    /// </summary>
    /// <param name="bool isOk">请求是否成功</param>
    /// <param name="string errorCode">错误信息</param>
    /// <param name="System.DateTime time">GPS的时间</param>
    /// <param name="float longitude">经度</param>
    /// <param name="float latitude">纬度</param>
    /// <param name="float altitude">高度</param>
    /// <param name="float LAccuracy">水平误差</param>
    /// <param name="float HAccuracy">高度误差</param>
    public const string U2N_N_GPS = "U2N_N_GPS";

#region 主菜单页面
    public void U2N_U_OpenMainMenu(Callback<bool,string> _callback)
    {
        ConnectionCall("U2N_U_OpenMainMenu", _callback,null);
    }
#endregion

#region 附近页面
    public void U2N_U_OpenNearby(Callback<bool, string> _callback)
    {
        ConnectionCall("U2N_U_OpenNearby", _callback,null);
    }
#endregion

#region 查看故事页面
    /// <summary>
    /// 查看故事页面
    /// </summary>
    /// <param name="_storyID">故事id</param>
    /// <param name="_callback"></param>
    public void U2N_U_OpenStory(JsonData _storyID,
        Callback<bool, string> _callback)
    {
        JsonData data = new JsonData();
        data["storyID"] = _storyID == null ? "" : _storyID;
        ConnectionCall("U2N_U_OpenStory", _callback, data);
    }
#endregion
    
#region 请求打开世界页面
    public void U2N_U_OpenWorld(Callback<bool, string> _callback)
    {
        ConnectionCall("U2N_U_OpenWorld", _callback,null);
    }
#endregion

#region 地图页面

    public void U2N_U_OpenMap(Callback<bool, string> _callback)
    {
        ConnectionCall("U2N_U_OpenMap", _callback,null);
    }
#endregion
#region 背包页面

    public void U2N_U_OpenBag(Callback<bool, string> _callback)
    {
        ConnectionCall("U2N_U_OpenBag", _callback, null);
    }
#endregion
    

#region 打开登录页面
    public void U2N_U_OpenLogin(Callback<bool, string> _callback)
    {
        ConnectionCall("U2N_U_OpenLogin", _callback,null);
    }
#endregion

#region 打开发布故事页面
    public void U2N_U_OpenSendStory(Callback<bool, string> _callback)
    {
        ConnectionCall("U2N_U_OpenSendStory", _callback,null);
    }
#endregion

    #region 调用截屏
    public void U2N_U_Cheese(string _url, Callback<bool, string> _callback)
    {
        JsonData data = new JsonData();
        data["url"] = _url;

        ConnectionCall("U2N_U_Cheese", _callback, data);
    }
    #endregion


    #region 调用分享
    /// <summary>
    /// 打开风向界面
    /// </summary>
    /// <param name="_url">分享到的网络地址</param>
    /// <param name="_title">信息标题</param>
    /// <param name="_view_title">对话框的标题</param>
    /// <param name="_content">分享的内容</param>
    /// <param name="_weibo_content">微博内容</param>
    /// <param name="_imagepath">图片路径  图片的网络地址</param>
    /// <param name="_callback">成功失败回调</param>
    public void U2N_U_OpenShare(
        string _url,
        string _title, 
        string _view_title,
        string _content,
        string _weibo_content,
        string _imagepath,
        Callback<bool, string> _callback)
    {
        JsonData data = new JsonData();
        data["url"] = _url;
        data["title"] = _title;
        data["view_title"] = _view_title;
        data["content"] = _content;
        data["weibo_content"] = _weibo_content;
        data["imagepath"] = _imagepath;

        ConnectionCall("U2N_U_OpenShare", _callback, data);
    }
    #endregion


    #region 请求GPS数据
    public void U2N_U_GPS(Callback<bool, string> _callback)
    {
        ConnectionCall("U2N_U_GPS", _callback,null);
    }
#endregion

#region 处理回调函数事件
    
    private void Native_Callback_Update(JsonData _jd)
    {
        TestValue( _jd,"fun");
        TestValue(_jd, "fun_id");
        string fun = _jd["fun"].ToString();
        string fun_id = _jd["fun_id"].ToString();
        switch(fun)
        {
            case U2N_N_OpenMainScene:
            case U2N_N_CloseMainScene:
            case U2N_N_CloseMainMenu:
            case U2N_N_CloseNearby:
            case U2N_N_CloseStory:
            case U2N_N_CloseWorld:
            case U2N_N_CloseMap:
            case U2N_N_CloseBag:
            case U2N_N_CheeseBack:
            case U2N_N_StartAppCamera:
                {
                    Messenger.Broadcast(fun);
                }
                break;
            case U2N_N_CloseLogin:
                {
                    TestValue( _jd, "data");
                    JsonData data = _jd["data"];
                    TestValue(data, "isOk");
                    TestValue( data, "errorCode");
                    Messenger.Broadcast<bool,string>(fun,data["isOk"].Equals("0"), data["errorCode"].ToString());
                }
                break;
            case U2N_N_CloseSendStory:
                {
                    TestValue(_jd, "data");
                    JsonData data = _jd["data"]; 
                    
                    TestValue(data, "isOk");

                    bool isOk = data["isOk"].Equals("0");
                    JsonData storyJson = null;
                    try 
	                {	        
		                storyJson = data["storyJson"];
	                }
	                catch (System.Exception)
	                {
		                storyJson = null;
	                }
                    Messenger.Broadcast<bool, JsonData>(fun, isOk, storyJson);
                }
                break;
            case U2N_N_CheeseShare:
                {
                    TestValue(_jd, "data");
                    JsonData data = _jd["data"];

                    TestValue(data, "err");
                    TestValue(data, "type");

                    Messenger.Broadcast<string, string>(fun, data["err"].ToString(), data["type"].ToString());
                }
                break;
            case U2N_N_GPS:
                {
                    TestValue(_jd, "data");
                    JsonData data  = _jd["data"];

                    TestValue(data, "isOk");
                    TestValue(data, "errorCode");
                    TestValue(data, "longitude");
                    TestValue(data, "latitude");
                    TestValue(data, "altitude");
                    TestValue(data, "LAccuracy");
                    TestValue(data, "HAccuracy");

                    bool isOk = data["isOk"].Equals("0");
                    string errorCode = data["errorCode"].ToString();
                    System.DateTime time = System.DateTime.Now;
                    try
                    {
                        time = System.DateTime.ParseExact(data["time"].ToString(), "yyyy-MM-dd hh:mm:ss", new CultureInfo("zh-CN", true));
                    }
                    catch (System.Exception e)
                    {
                        isOk = false;
                        errorCode = "错误：U2N 协议：U2N_N_GPS 时间错误 日期必须是 yyyy-MM-dd hh:mm:ss 格式的字符串。";
                        Debug.LogError("错误：U2N 协议：U2N_N_GPS 时间错误 日期必须是 yyyy-MM-dd hh:mm:ss 格式的字符串。");
                    }
                    try
                    {
                        float longitude = float.Parse(data["longitude"].ToString());
                        float latitude = float.Parse(data["latitude"].ToString());
                        float altitude = float.Parse(data["altitude"].ToString());
                        float LAccuracy = float.Parse(data["LAccuracy"].ToString());
                        float HAccuracy = float.Parse(data["HAccuracy"].ToString());
                        Messenger.Broadcast<bool, string, System.DateTime, float, float, float, float, float>
                        (fun, isOk, errorCode, time, longitude, latitude, altitude, LAccuracy, HAccuracy);
                    }
                    catch (System.Exception)
                    {
                        throw new ValueErrException("浮点类型错误");
                    }
                }
                break;
            default:
                throw new System.Exception("错误：U2N Native_CallBack 对不起，没有找到" + fun + "的处理方法。");
        }

        JsonData jd = new JsonData();
        jd["event"] = "native";
        jd["fun_id"] = fun_id;
        jd["type"] = "response";
        jd["fun"] = fun;
        jd["ret_code"] = "0";
        jd["err_msg"] = "";

        SendMessage(jd);
    }
#endregion

#region 发送接收的基础函数
    private void ConnectionCall(string fun, Callback<bool, string> _callback, JsonData data)
    {
        if (m_connectionMap.ContainsKey(fun))
        {
            if(_callback!=null)_callback(true, "对不起队列中 还有没有处理完成的" + fun + "事件！不可以连续调用");
            return;
        }
        Connection c = new Connection(fun, _callback);

        JsonData jd = new JsonData();
        jd["event"] = "native";
        jd["fun_id"] = "" + c.fun_id;
        jd["type"] = "request";
        jd["fun"] = fun;

        if (data != null) jd["data"] = data;
        else
        {
            JsonData d = new JsonData();
            d["Test"] = "";
            jd["data"] = d;
        }
        string rStr = SendMessage(jd);
        if (System.String.IsNullOrEmpty(rStr))
        {
            if (_callback!=null) _callback(true, "错误： Native端没有收到" + fun + "消息。");
        }
        else
        {
            //m_connectionMap.Add(fun, c);
            //if (Application.platform != RuntimePlatform.WindowsEditor && Application.platform != RuntimePlatform.OSXEditor) Native_CallBack(rStr);
        }
    }
    private struct Connection
    {
        public Connection(string fun, Callback<bool, string> callback, bool isContinuous = true)
        {
            this.fun = fun;
            this.fun_id = ""+m_all_funid++;
            this.callback = callback;
            this.tick = System.DateTime.Now.Ticks;
            this.isContinuous = isContinuous;
        }
        public string fun;
        //该调用的id值
        public string fun_id;
        //回调方法 bool turew
        public Callback<bool, string> callback;
        //发起时间
        public long tick;
        //是否是需要立即返回
        public bool isContinuous;
    }
    private static int m_all_funid = 0;
    private Dictionary<string, Connection> m_connectionMap = new Dictionary<string, Connection>();
    private IEnumerator UpdateConnection()
    {
        if (Application.platform != RuntimePlatform.WindowsEditor &&
            Application.platform != RuntimePlatform.OSXEditor)
        while(true)
        {
            yield return new WaitForSeconds(1f);
            int state = 0;
            string key = "";
            foreach (KeyValuePair<string, Connection> item in m_connectionMap)
            {
                //如果超时，那么报错
                if (System.DateTime.Now.Ticks - item.Value.tick > 5000)
                {
                    state = 1;
                    key = item.Key;
                    break;
                }
            }

            switch (state)
            {
                case 1:
					if (m_connectionMap[key].callback!=null) m_connectionMap[key].callback(true, "错误： U2N 该信息请求超时。");
					Debug.LogError("错误： U2N  该信息请求超时。");
                    m_connectionMap.Remove(key);
                    break;
            }
        }
        
    }

#if UNITY_ANDROID
    private AndroidJavaObject m_javaObject;
#endif
#if UNITY_IOS
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void Sky_Unity_CallBack(string jsStr);

	[System.Runtime.InteropServices.DllImport("__Internal")]
	private static extern void Sky_Unity_CallBack_log(string jsStr);
#endif

    /// <summary>
    /// 向Native发送数据
    /// </summary>
    /// <param name="jd">数据内容</param>
    private string SendMessage(JsonData jd)
    {
        string returnString = null;
        try
        {
            string data = jd.ToJson();
            Debug.Log("向Native发请求：" + data);
#if UNITY_ANDROID
            if(Application.platform == RuntimePlatform.Android)
                returnString = m_javaObject.Call<string>("Unity_CallBack", data);
#endif
#if UNITY_IOS
            Sky_Unity_CallBack(data);
#endif
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
        return returnString;
    }
    /// <summary>
    /// Native的回调方法
    /// </summary>
    /// <param name="s">回调参数</param>
    private void NativeHttp_CallBack(string s)
    {
        try
        {
            Debug.Log("NativeHttp_CallBack 返回的参数为：" + s);
            if (s == null || s.Equals(""))
            {
                Debug.LogError("错误：U2N NativeHttp_CallBack Native的回调方法 出错，返回的参数不可以为空。");
                return;
            }

            JsonData jd = JsonMapper.ToObject(s);

            if (!jd.Contains("event"))
                throw new System.Exception("错误：U2N NativeHttp_CallBack 返回的参数必须有 event 字段");
            //查看是否是http类型的请求。
            if (UpdateRequestNativeHttp(jd))
                return;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// Native的回调方法
    /// </summary>
    /// <param name="s">回调参数</param>
    private void Native_CallBack(string s)
    {
        try
        {
            Debug.Log("Native_CallBack 返回的参数为：" + s);
            if (s == null || s.Equals(""))
            {
                Debug.LogError("错误：U2N Native_CallBack Native的回调方法 出错，返回的参数不可以为空。");
                return;
            }

            JsonData jd = JsonMapper.ToObject(s);

            if (!jd.Contains("event"))
                throw new System.Exception("错误：U2N Native_CallBack 返回的参数必须有 event 字段");
            //查看是否是http类型的请求。
            if (UpdateRequestNativeHttp(jd))
                return;

            if (!jd.Contains("type"))
                Debug.LogError("(" + s + ")错误：U2N Native_CallBack 返回的参数必须有 type 字段。");
            string type = jd["type"].ToString();

            //请求的数据
            if (type.Equals("request"))
            {
                Native_Callback_Update(jd);
            }
            //回调的消息
            else if (type.Equals("response"))
            {
                if (!jd.Contains("ret_code")) Debug.LogError("错误：U2N Native_CallBack 返回的参数必须有 ret_code 字段。");
                if (!jd.Contains("fun")) Debug.LogError("错误：U2N Native_CallBack 返回的参数必须有 fun 字段。");
                if (!jd.Contains("err_msg")) Debug.LogError("错误：U2N Native_CallBack 返回的参数必须有 err_msg 字段。");
                if (!jd.Contains("fun_id")) Debug.LogError("错误：U2N Native_CallBack 返回的参数必须有 fun_id 字段。");

                string ret_code = jd["ret_code"].ToString();
                string err_msg = jd["err_msg"].ToString();
                string fun_id = jd["fun_id"].ToString();
                string fun = jd["fun"].ToString();

                if (m_connectionMap.ContainsKey(fun) &&
                    //判断返回的ID是否是发送的ID
                    fun_id.Equals(m_connectionMap[fun].fun_id))
                {
                    if (!ret_code.Equals("0"))
                        m_connectionMap[fun].callback(true, err_msg);
                    else
                    {
                        m_connectionMap[fun].callback(false, err_msg);
                    }
                    m_connectionMap.Remove(fun);
                }

            }
            else
            {
                Debug.LogError("错误：U2N Native_CallBack 返回的type值 必须是 “request”或“response”");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        
    }
#endregion

    /// <summary>
    /// 单例
    /// </summary>
    private static SU2NConnection m_instance = null;

    private void Awake()
    {
        if(!gameObject.name.Equals( "Manager" ))
        {
            MonoBehaviour.Destroy(this);
            Debug.LogError("错误： U2N 对不起SNativeConnection只能挂在name==Manager的对象上面。");
            return;
        }
        if (m_instance != null)
        {
            MonoBehaviour.Destroy(this);
			Debug.LogError("错误： U2N 对不起SNativeConnection只能创建一个实例对象。");
            return;
        }
        m_instance = this;

#if UNITY_ANDROID
        try
        {
            if(Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass javaClass = new AndroidJavaClass("com.imysky.skyalbum.unity.Unity_Receive");
                m_javaObject = javaClass.CallStatic<AndroidJavaObject>("getInstance");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
#endif

        StartCoroutine(UpdateConnection());

        Messenger.AddListener<Callback<bool, string>>("U2N_U_OpenMainMenu", U2N_U_OpenMainMenu);
        Messenger.AddListener<Callback<bool, string>>("U2N_U_OpenNearby", U2N_U_OpenNearby);
        Messenger.AddListener<JsonData, Callback<bool, string>>("U2N_U_OpenStory", U2N_U_OpenStory);
        Messenger.AddListener<Callback<bool, string>>("U2N_U_OpenWorld", U2N_U_OpenWorld);
        Messenger.AddListener<Callback<bool, string>>("U2N_U_OpenMap", U2N_U_OpenMap);
        Messenger.AddListener<Callback<bool, string>>("U2N_U_OpenBag", U2N_U_OpenBag);
        Messenger.AddListener<Callback<bool, string>>("U2N_U_OpenLogin", U2N_U_OpenLogin);
        Messenger.AddListener<Callback<bool, string>>("U2N_U_OpenSendStory", U2N_U_OpenSendStory);
        Messenger.AddListener<string, string, string, string, string, string, Callback<bool, string>>("U2N_U_OpenShare", U2N_U_OpenShare);
        Messenger.AddListener<Callback<bool, string>>("U2N_U_GPS", U2N_U_GPS);
        Messenger.AddListener<string, Callback<bool, string>>("U2N_U_Cheese", U2N_U_Cheese);
        Messenger.AddListener< string , string[] , Callback< string, JsonData > , bool ,int > ("U2N_U_HTTP", SendHttpGet);

    }

    private void OnDestroy()
    {
        Messenger.RemoveListener<Callback<bool, string>>("U2N_U_OpenMainMenu", U2N_U_OpenMainMenu);
        Messenger.RemoveListener<Callback<bool, string>>("U2N_U_OpenNearby", U2N_U_OpenNearby);
        Messenger.RemoveListener<JsonData, Callback<bool, string>>("U2N_U_OpenStory", U2N_U_OpenStory);
        Messenger.RemoveListener<Callback<bool, string>>("U2N_U_OpenWorld", U2N_U_OpenWorld);
        Messenger.RemoveListener<Callback<bool, string>>("U2N_U_OpenMap", U2N_U_OpenMap);
        Messenger.RemoveListener<Callback<bool, string>>("U2N_U_OpenBag", U2N_U_OpenBag);
        Messenger.RemoveListener<Callback<bool, string>>("U2N_U_OpenLogin", U2N_U_OpenLogin);
        Messenger.RemoveListener<Callback<bool, string>>("U2N_U_OpenSendStory", U2N_U_OpenSendStory);
        Messenger.RemoveListener<string, string, string, string, string, string, Callback<bool, string>>("U2N_U_OpenShare", U2N_U_OpenShare);
        Messenger.RemoveListener<Callback<bool, string>>("U2N_U_GPS", U2N_U_GPS);
        Messenger.RemoveListener<string, Callback<bool, string>>("U2N_U_Cheese", U2N_U_Cheese);
        Messenger.RemoveListener<string, string[], Callback<string, JsonData>, bool, int>("U2N_U_HTTP", SendHttpGet);
    }
    public static void Create()
    {
        GameObject g = GameObject.Find("Manager");
        SU2NConnection s;
        if (g != null)
        {
            s = g.GetComponent<SU2NConnection>();
            if (s == null)
                s = g.AddComponent<SU2NConnection>();
        }
        else
        {
            g = new GameObject();
            g.name = "Manager";
            s = g.AddComponent<SU2NConnection>();
        }
    }

    private string debugstring = "xxx";

    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0, 0, 400, 300), "xxxxxxxxx"))
    //    {
    //        Debug.Log("Button is Down!   ====== ");
    //        //Messenger.Broadcast<string, string[], Callback<string, JsonData>, bool, int>
    //        //    ("U2N_U_HTTP",
    //        //    "v1/node/newbie_guide_list.json",
    //        //    new string[] { "page_size=3" },
    //        //    delegate (string err, JsonData js)
    //        //    {
    //        //        debugstring = "err:" + err + "   js:" + js.ToString();
    //        //    }, false, 1);

    //        Messenger.AddListener<bool, string>("U2N_N_CloseLogin", 
    //            delegate (bool b, string s) {
    //                debugstring = "err:" + b + "   js:" +s;
    //            });
    //        Messenger.Broadcast<Callback<bool, string>>("U2N_U_OpenLogin",null);
    //        //if (!_jd.Contains("data")) Debug.LogError("错误：U2N Native_CallBack 返回的参数必须有 data 字段。");
    //        //JsonData data = data = _jd["data"];
    //        //Messenger.Broadcast<bool, string>(fun, data["isOk"].Equals("0"), data["errorCode"].ToString());
    //    }
    //    GUI.Label(new Rect(0, 300, 400, 300), debugstring);
    //}


#region 处理HTTP请求

    /// <summary>
    ///处理返回的http请求。
    /// </summary>
    /// <param name="jd"></param>
    /// <returns></returns>
    private bool UpdateRequestNativeHttp(JsonData jd)
    {
        if (jd["event"].Equals("Native_Http"))
        {
            if (!jd.Contains("ret_code"))
                throw new System.Exception("错误：U2N NativeHttp_CallBack 返回的参数必须有 ret_code 字段");
            if (!jd.Contains("fun"))
                throw new System.Exception("错误：U2N NativeHttp_CallBack 返回的参数必须有 fun 字段");

            if (m_http_requests.ContainsKey(jd["fun"].ToString()))
            {
                string err = null;
                if (jd["ret_code"].Equals("-1")) err = "用户认证失效，并且没有登录";
                m_http_requests[jd["fun"].ToString()](err, jd["data"]);
            }
            return true;
        }
        return false;
    }

    private Dictionary<string, Callback<string, JsonData>> m_http_requests = new Dictionary<string, Callback<string, JsonData>>();

    /// <summary>
    /// 通知Native进行Http
    /// </summary>
    /// <param name="url"></param>
    /// <param name="values"></param>
    /// <param name="callback"></param>
    /// <param name="isLogin"></param>
    /// <param name="type"></param>
    public void SendHttpGet(string url, string[] values, Callback<string, JsonData> callback, bool isLogin = false,int type = 1)
    {
        if(m_http_requests.ContainsKey(url))
        {
            m_http_requests.Remove(url);
        }

        m_http_requests.Add(url, callback);

        JsonData jd = new JsonData();
        jd["event"] = "Native_Http";
        jd["fun"] = url;
        jd["type"] = type;
        jd["userinfo"] = isLogin ? 1:0;
        jd["url"] = url;

        JsonData data = new JsonData();

        foreach( string v in values)
        {
            JsonData itemData = new JsonData();
            string[] vs = v.Split('=');
            if (vs.Length < 2)
                throw new System.Exception("传入的 key 或者 value 不对。");

            itemData["key"] = vs[0].Trim();
            itemData["value"] = vs[1].Trim();

            data.Add(itemData);
        }

        jd["data"] = data;

        string rStr = SendMessage(jd);
    }
    #endregion

    private void TestValue(JsonData js, string value)
    {
        if (!js.Contains(value))
            throw new System.Exception("错误：SU2NConnection 返回的参数必须有 " + value + " 字段。");
    }
}

public class NotHaveKeyException : System.Exception
{
    public NotHaveKeyException(string s):base(s){}
}
public class ValueErrException : System.Exception
{
    public ValueErrException(string s) : base(s) { }
}