using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using com.imysky.camera;
using DG.Tweening;
using UnityEngine.UI;
public class PhotoScene : Scene
{
    public ItemManager itemManager;
    public static PhotoScene Instance;
    public SpecialEffectsUI specialEffectsUI;
    public ImageManager2 imageManager;
    public CollectPhoto collectPhoto = null;
    private GameObject photoUIobj = null;
    public int itmeClipID = 0;
    public GameObject Royboj = null;

    public ClickUi clickUi = null;
    public Color itemClick_color;
    public GameObject PhotoCameraObj = null;

    public Photoprompt photoprompt = null;

    private bool IsHeip = false;
    public bool IsClickPhtoto = false;
    public int jiaocheng = 0;

    private PhotoNotReachable m_PhotoNotReachable = null;

    // Use this for initialization
    void Start()
    {

    }
    void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            m_PhotoNotReachable.Hide();
            SCameraManager.currentCamera.Stop(null);
        }
        else
        {
            SCameraManager.currentCamera.Start(null);
            itemManager.MsgGps(true);
        }
    }

    public void RequestAnUpdate()
    {
        itemManager.MsgGps(true);
    }

    private void Updatephoto()
    {
        photoprompt.Off();
        itemManager.MsgGps(false);
    }
    public void TimeUpData()
    {
        MsgBase.SendMsg("BaseButtonEvnet2");
        speed = 0;
        StartLoading(0);
        isLoadData = true;
    }

    public void Collectphoto()
    {
        m_callback(100, false);
        collectPhoto.Open();
    }
    private Callback callback;
    public void PhotoInvoke(float time, Callback _callback)
    {
        callback = _callback;
        Invoke("InvokeCallback", time);
    }
    private void InvokeCallback()
    {
        callback();
    }

    private void EventDrag(GameObject obj, MySkyInputEvent.DragState tmpDragState, Vector2 pos, Vector2 speed, Vector3 newPos)
    {
        if (IsHeip)
        {
            return;
        }
        itemManager.ItemDrag(obj, tmpDragState, pos, speed, newPos);
    }
    private void OnClickPhoto(GameObject obj, MySkyInputEvent.ClickState ClickState, Vector2 pos)
    {
        if (obj==null)
        {
            MsgBase.SendMsg("BaseButtonEvnet2");
        }
        if (IsHeip || IsClickPhtoto)
        {
            return;
        }

        itemManager.ItemClick(obj, pos, specialEffectsUI, ClickState);
    }
    public void UpPhoto()
    {
        // m_callback(100, false);
        specialEffectsUI.clickShow(UpEnd);
    }
    private void UpEnd()
    {
        Debug.Log("  发送  拍照请求  U2N_U_OpenSendStory ");
        MsgBase.SendMsg<Callback<bool, string>>("U2N_U_OpenSendStory", 
            delegate(bool a, string  s) 
            {
                if (a)
                {
                    //错误
                    specialEffectsUI.clickHide(MsgBase.ShowUI);
                }
            });
    }
    private Vector3 ve = Vector3.zero;
    private float Speed = 0;
    private bool IsAngle = false;

    private bool IsAngle02 = false;
    public bool IsUpPhoto = false;
    int iiii = 0;
    float n1 = 116.4281f;

    public Text tt;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            JsonData jd4 = new JsonData();
            jd4["Event"] = "EventClickItme";
            Native.Instance.NativeToUnity(jd4.ToJson());


        }
        if (Input.GetKeyDown(KeyCode.T))
        {

           // bool isRemoveItem = itemManager.RemoveItemDictinary2();
            System.DateTime T = new System.DateTime();
            n1 = n1 + 10;
            Messenger.Broadcast<bool, string, System.DateTime, float, float, float, float, float>("U2N_N_GPS", true, "eeeeeeeeee", T, n1, 39.90639f, 123, 123, 123);
  
        }
        if (IsUpPhoto)
        {
            MsgBase.SendMsg<bool, float>("ShowBlur", true, 10);
        }
        if (itemManager != null && isClose == false)
        {
            itemManager.RootRotation();
        }
        IsAngle = IsUserAngle();
        if (photoprompt != null && IsAngle02==false)
        {
            if (IsAngle && jiaocheng == 1 && IsClickPhtoto==false)
            {
                IsClickPhtoto = true;
                MsgBase.SendMsg<bool>("ISslide", true);
                photoprompt.ShowUItishi3(true);
                photoprompt.Off();
                return;
            }
            if (jiaocheng == 0 && isLoadData == false && IsUpPhoto==false)
            {
                photoprompt.Show(IsAngle);
            }
        }

     
      // My_debugNet.SendDebugLog("IsUpPhoto---" + IsUpPhoto + "---isLoadData---" + isLoadData + "---jiaocheng---" + jiaocheng + "---IsAngle---" + IsAngle);
    }
    /// <summary>
    /// 当前没有网络 
    /// </summary>
    /// 

    public void isNotReachable(bool isShow)
    {
        if (itemManager.isNotReachable)
        {
            Debug.Log("----Hide-----");
            m_PhotoNotReachable.Hide();
        }
        if (isShow && itemManager.isNotReachable)
        {
          m_PhotoNotReachable.Show();
        }

    }



    public void PhotoNotReachable()
    {
        IsLoadingData = false;
        StopCoroutine(LoadingData(100));
        m_callback(0, false);
        MsgBase.SendMsg<string, bool>("SetButtonLose", "UPphoto", false);
        MsgBase.SendMsg<bool>("isRefresh", false);
        photoprompt.Off();
        m_PhotoNotReachable.Show();
    }

    public void HidePhotoNotReachable()
    {
        m_PhotoNotReachable.Hide();
    }
    /// <summary>
    ///  按钮事件  重试
    /// </summary>
    public void PhotoNotReachableButtonEvent()
    {
        Debug.Log("----PhotoNotReachableButtonEvent-----" );
        m_PhotoNotReachable.Hide();
        Updatephoto();
    }
    private Callback<float, bool> m_callback;
    protected override void OnOpen(Callback<float, bool> callback)
    {
        m_callback = callback;
        MsgBase.SendMsg<Callback>("OffGyroControllerCallback", OffGyroallback);
    }

    private void OffGyroallback()
    {
        IsHeip = false;
        PhotoCameraObj = new GameObject();
        PhotoCameraObj.name = "PhotoCameraObj";
        PhotoCameraObj.transform.SetParent(SCameraManager.currentCamera.gameObject.transform.parent.gameObject.transform);
        PhotoCameraObj.transform.localPosition = Vector3.zero;
        SCameraManager.currentCamera.gameObject.transform.localPosition = Vector3.zero;
        Royboj = Instantiate(Resources.Load("photo/Rotation")) as GameObject;
        Royboj.SetActive(false);
        Royboj.transform.SetParent(PhotoCameraObj.transform);
        Royboj.transform.localPosition = new Vector3(0, -2.5f, 6.22f);
        MsgBase.SendMsg("OpenGyroController");
        InitUI();
        Init();
        MsgBase.ShowUI();
        try
        {
            itemManager.MsgGps(false);
        }
        catch (System.Exception e)
        {

            Debug.Log(""+e);
        }
     
        PhotoCameraObj.transform.localPosition = Vector3.zero;
    }
    private void InitUI()
    {
        photoUIobj = uiRoot.GetUIView().gameObject;
        photoUIobj.name = "PhotoUIroot";
        GameObject g = Instantiate(Resources.Load("UI/Photo/Image")) as GameObject;
        g.transform.SetParent(photoUIobj.transform);
        specialEffectsUI = g.AddComponent<SpecialEffectsUI>();
        specialEffectsUI.Init();

 
        clickUi = new ClickUi(photoUIobj);
        if (collectPhoto == null)
        {
            collectPhoto = new CollectPhoto(specialEffectsUI, PhotoCameraObj);
        }

  



    }
    private GameObject Gyro = null;
    private void Init()
    {
        imageManager = gameObject.AddComponent<ImageManager2>();
        Instance = this;
        itemManager = new ItemManager(gameObject);
        photoprompt = new Photoprompt(itemManager);
        m_PhotoNotReachable = new PhotoNotReachable(PhotoNotReachableButtonEvent);
        MsgBase.MsgAdd<GameObject, MySkyInputEvent.DragState, Vector2, Vector2, Vector3>("EventDrag", EventDrag);
        MsgBase.MsgAdd<GameObject, MySkyInputEvent.ClickState, Vector2>("EventClick", OnClickPhoto);
        MsgBase.MsgAdd("Updatephoto", Updatephoto);
        MsgBase.MsgAdd<Callback,bool>("SetIsAngle02", SetIsAngle02);
    }
    private void SetIsAngle02(Callback arg1, bool arg2)
    {
        IsAngle02 = arg2;
        if (arg1!=null)
        {
            arg1();
        }
    }

    private void isheip(bool ish)
    {
        IsHeip = ish;
    }

    private void EndUI()
    {
        // MsgBase.SendMsg("HomePageMagShowUI");
        // MsgBase.SendMsg("photoUIeventShowUI");

    }

    public void SetHttpData(bool isLaod, Dictionary<string, JsonData> _tmpDictionary, bool isCode)
    {

        if (isEnable)
        {
            return;
        }
        if (itemManager == null)
        {
            SendDebug("  itemManager    kong");
            return;
        }
        if (isCode == false)
        {
            bool isRemoveItem=itemManager.RemoveItemDictinary2();
            IsLoadingData = false;
            StopCoroutine(LoadingData(100));
            m_callback(0, false);
            isLoadData = false;
            MsgBase.SendMsg<string, bool>("SetButtonLose", "UPphoto", false);
            MsgBase.SendMsg<bool>("isRefresh", false);
            SetIsAngle02(delegate() { MsgBase.SendMsg<bool, float>("ShowBlur", true, 10); }, false);
           // uiRoot.ShowAlert(null, "这里未发现故事", "留下我的故事", delegate() { UpPhoto(); }, "到世界逛逛", delegate() { MsgBase.SendMsg<Callback<bool, string>>("U2N_U_OpenWorld", null); }, true);
            return;
        }
        if (isLaod == false)
        {
          //  LoadFailure();
            return;
        }
        itemManager.SetData(_tmpDictionary);
    }
    private void LoadFailure()
    {
        IsLoadingData = false;
        m_callback(0, false);
       // uiRoot.ShowAlert(null, "加载失败，请重试", "重试", delegate() { Updatephoto(); }, null, null, true);
        SetIsAngle02(delegate() { MsgBase.SendMsg<bool, float>("ShowBlur", true, 10); }, false);
        isLoadData = false;
        MsgBase.SendMsg<string, bool>("SetButtonLose", "UPphoto", false);
        MsgBase.SendMsg<bool>("isRefresh", false);
    }


    private bool isClose = false;
    private Callback Closecallback=null;
    protected override void OnClose(Callback callback)
    {
        if (callback!=null)
        {
            Closecallback = callback;
        }
        isClose = true;
        MsgBase.MsgRemove("Updatephoto", Updatephoto);
        MsgBase.MsgRemove<GameObject, MySkyInputEvent.DragState, Vector2, Vector2, Vector3>("EventDrag", EventDrag);
        MsgBase.MsgRemove<GameObject, MySkyInputEvent.ClickState, Vector2>("EventClick", OnClickPhoto);
        Delete();
    }
    private void Delete()
    {
        StopCoroutine(photoprompt.SetPrompt());
        itemManager.Delete();
        Destroy(imageManager);
        Destroy(photoUIobj);
        collectPhoto.Delete();
        collectPhoto = null;
        clickUi.Delete();
        clickUi = null;
        photoprompt.Delete();
        photoprompt = null;
        m_PhotoNotReachable.Delete();
        m_PhotoNotReachable = null;
        Destroy(Royboj);
        Destroy(PhotoCameraObj);
        if (Closecallback!=null)
        {
            Closecallback();
        }
    }
    public void StartLoading(float t)
    {
        IsLoadingData = true;
        SetIsAngle02(delegate() { MsgBase.SendMsg<bool, float>("ShowBlur", false, 10); }, true);
        MsgBase.SendMsg<bool>("isRefresh", true);
        MsgBase.SendMsg<string, bool>("SetButtonLose", "UPphoto", true);
        StartCoroutine(LoadingData(t));

    }
    // 请求数据  创建数据 分发数据
    float speed = 0;
    public bool IsLoadingData = false;
    IEnumerator LoadingData(float t = 0)
    {
        while (speed < 99 && IsLoadingData)
        {
            speed = speed + t;
            m_callback(speed, true);
            yield return new WaitForSeconds(0.001f);
        }
    }
    private bool UpDateItme = false;
    public bool isLoadData = false;
    public void LoadingEnd()
    {
        if (UpDateItme)
        {
            int k = itemManager.CreateitemSnu;
            if (k == 0)
            {
                Invoke("waitShowReminder",1);
                return;
            }
            else
            {
               // SUIManager.instance.reminder.ShowReminder("发现" + k + "个新故事", 3);
            }
            itemManager.CreateitemSnu = 0;
        }
        else
        {
            if (itemManager.GetItmeDataObjectCount()==0)
            {
       
            }
           // SUIManager.instance.reminder.ShowReminder("周围发现" + itemManager.GetItmeDataObjectCount() + "个故事", 3);
            itemManager.CreateitemSnu = 0;
            UpDateItme = true;
            StartCoroutine(photoprompt.SetPrompt());
        }
        IsLoadingData = false;
        if (PlayerPrefs.GetInt("3DStart") == 2)
        {
         //   Invoke("Startjiaocheng", 3f);
        }

        SetIsAngle02(delegate() { MsgBase.SendMsg<bool, float>("ShowBlur", true, 10); }, false);
        StopCoroutine(LoadingData(100));
        m_callback(100, true);
        isLoadData = false;
        MsgBase.SendMsg<string, bool>("SetButtonLose", "UPphoto", false);
        MsgBase.SendMsg<bool>("isRefresh", false);
    }
    private void waitShowReminder()
    {
        IsLoadingData = false;
        StopCoroutine(LoadingData(100));
        m_callback(100, true);
        isLoadData = false;
        MsgBase.SendMsg<string, bool>("SetButtonLose", "UPphoto", false);
        MsgBase.SendMsg<bool>("isRefresh", false);
        //SUIManager.instance.reminder.ShowReminder("没有发现新故事", 3);
        SetIsAngle02(delegate() { MsgBase.SendMsg<bool, float>("ShowBlur", true, 10); }, false);
    }
    private void Startjiaocheng()
    {
        jiaocheng = 1;
    }
    public static void SendDebug(string log)
    {
        Debug.Log(log);
    }
    public JsonData jsond()
    {
        Dictionary<string, ItemData> tmp = itemManager.GetItemObjectDict();

        foreach (var t in tmp)
        {
            return t.Value.itemJson;
        }
        return null;
    }

    private readonly float SHOW_PHOTO_ANGLE = 50f;
    public bool IsUserAngle()
    {
        Vector3 q1 = PhotoCameraObj.transform.forward;
        Vector3 q2 = new Vector3(0, -1f, 0);
        float angle = Vector3.Angle(q1, q2);
        //如果没有竖立起来
        if (angle < SHOW_PHOTO_ANGLE)
        {
            return false;
        }
        return true;
    }

    public bool isEnable = false;

    public bool movableUI = false;
    void OnEnable()
    {
        isEnable = false;
        MsgBase.ShowUI();
        if (itemManager!=null)
        {
            itemManager.MsgGps(true);
            foreach (var item in itemManager.GetItemObjectDict())
	        {
                item.Value.AnimatorPlay2();
	        }
            
        }
        if (PhotoCameraObj != null)
        {
            PhotoCameraObj.SetActive(true);
        }
        if (photoprompt != null)
        {
            StartCoroutine(photoprompt.SetPrompt());
        }
    
        IsHeip = false;
        IsClickPhtoto = false;
        MsgBase.SendMsg<string, bool>("SetButtonLose", "UPphoto", false);
        MsgBase.SendMsg<bool>("isRefresh", false);
        if (movableUI)
        {
            //HomePageMag.Initacne.HomePageChildDictionary["movableUI"].SetActive(true);
        }
    }
    void OnDisable()
    {
        isEnable = true;
        IsHeip = true;
        if (photoprompt != null)
        {
            photoprompt.Off();
        }
        PhotoCameraObj.SetActive(false);
      //  HomePageMag.Initacne.HomePageChildDictionary["movableUI"].SetActive(false);

    }

}