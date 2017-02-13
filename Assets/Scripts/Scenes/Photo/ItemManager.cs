using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using DG.Tweening;
using com.imysky.camera;
public class ItemManager
{

    private GameObject PhotoRoot = null;
    private GameObject FindConfig = null;
    private RequestsHttpdata m_RequestsHttpdata;
    private string http = "http://220.231.48.254:6080";
    private string pathjson = "/v1/node/circle_geo_list.json?";
    private PhotoConfigArray m_PhotoConfigArray = null;
    public ItemManager(GameObject obj)
    {
        m_PhotoConfigArray = new PhotoConfigArray();
        this.PhotoRoot = obj;
        CreateFindConfig();
        m_RequestsHttpdata = new RequestsHttpdata();
    }

    public void Delete()
    {
        PhotoRoot = null;
        MonoBehaviour.Destroy(FindConfig);
        m_RequestsHttpdata.Delete();
        m_RequestsHttpdata = null;
    }


    #region  物体管理器  增 删 查 功能
    private Dictionary<string, ItemData> m_itemGameObjectDictinary = new Dictionary<string, ItemData>();



    public bool shaixuan(int i)
    {
        bool isshaixuan = false;
        PhotoScene.Instance.itemManager.isOpenxunbao(false);
        foreach (var item in m_itemGameObjectDictinary)
        {
            if (int.Parse(item.Value.itemJson["cardType"].ToString())==i)
            {
                isshaixuan = true;
                continue;
            }
            item.Value.item.gameObject.SetActive(false);
        }
        isRootRotation = true;
        return isshaixuan;
    }
    public void OpenItem()
    {
       
        foreach (var item in m_itemGameObjectDictinary)
        {
            item.Value.item.gameObject.SetActive(true);
        }
        isRootRotation = true;
        PhotoScene.Instance.itemManager.isOpenxunbao(false);
    }

    public void isOpenxunbao(bool isopen)
    {
            foreach (var item in m_itemGameObjectDictinary)
            {
                if (item.Value.cardType>3)
                {
                    item.Value.item.gameObject.SetActive(isopen);
                }else
                {
                    item.Value.item.gameObject.SetActive(!isopen);
                }
            }
        isRootRotation = false;
    }


    // 添加
    public void AddItmeDicitinary(string tmpkey, ItemData tmpItemData)
    {
        if (m_itemGameObjectDictinary.ContainsKey(tmpkey) == false)
        {
            m_itemGameObjectDictinary.Add(tmpkey, tmpItemData);
        }
        else
        {
            Debug.Log("   you    AddItmeDicitinary ");
        }
    }
    //删除 
    public bool RemoveItemDictinary(string tmpkey)
    {
        if (m_itemGameObjectDictinary.ContainsKey(tmpkey))
        {
            m_itemGameObjectDictinary[tmpkey].OnDestroy();
            m_itemGameObjectDictinary.Remove(tmpkey);
            return true;
        }
        return false;
    }

    //删除全部
    public bool RemoveItemDictinary2()
    {
        if (m_itemGameObjectDictinary.Count!=0)
        {

            foreach (var item in m_itemGameObjectDictinary)
            {
                m_itemGameObjectDictinary[item.Key].OnDestroy();
            }
            ISArrangement = false;
            m_itemGameObjectDictinary.Clear();
            return true;
        }
        return false;
    }

    //查找
    public ItemData FindItemData(string tmpKey)
    {
        if (m_itemGameObjectDictinary.ContainsKey(tmpKey))
        {
            return m_itemGameObjectDictinary[tmpKey];
        }
        return null;
    }
    //按物体查找
    public ItemData FindItemData(GameObject tmpObj)
    {
        foreach (KeyValuePair<string, ItemData> item in m_itemGameObjectDictinary)
        {
            if (item.Value.item.gameObject == tmpObj)
            {
                return item.Value;
            }
        }
        return null;
    }
    //按位置查找物体
    public ItemData FindItemData(Vector3 pos)
    {
        foreach (KeyValuePair<string, ItemData> item in m_itemGameObjectDictinary)
        {
            if (item.Value.RootObjPos == pos)
            {
                return item.Value;
            }
        }
        return null;
    }

    //管理器 数据 个数
    public int GetItmeDataObjectCount()
    {
        return m_itemGameObjectDictinary.Count;
    }
    public Dictionary<string, ItemData> GetItemObjectDict()
    {
        return m_itemGameObjectDictinary;
    }
    #endregion
    #region   下载 更新 管理
    public bool isOpenLoad = false;
    public bool isNotReachable = false;
    public void MsgGps(bool _isLoad)
    {

        JsonData jd1 = new JsonData();
        jd1["Event"] = "NearScene";
        jd1["cardType"] = -1;
        Native.Instance.SendNative(jd1);
        TextAsset t = (TextAsset)Resources.Load("Send_NearScene");
        JsonData jd = JsonMapper.ToObject(t.text);
        Debug.Log("----" + jd.ToJson());
        //m_RequestsHttpdata.RequestsHttp("null", PhotoScene.Instance.SetHttpData, jd);
    }
    public void LoadJsonData(JsonData jd)
    {
        m_RequestsHttpdata.RequestsHttp("null", PhotoScene.Instance.SetHttpData, jd);
    }
   private float m_currentN=0;
   private float m_currentE = 0;
   public float m_TimeOnUpdateTow = 10;
   public bool isLoad = false;
   private void Load(float _longitude, float _latitude)
   {
       if (isLoad==false)
       {
           m_currentN = _latitude;
           m_currentE = _longitude;
           LoadHttp(_latitude, _longitude);
           m_TimeOnUpdateTow = 10;
           return;
       }
       if (Vector2.Distance(new Vector2(m_currentN, m_currentE), new Vector2(_latitude, _longitude)) > 0.00100F)
       {
           m_currentN = _latitude;
           m_currentE = _longitude;
           LoadHttp(_latitude, _longitude);
           m_TimeOnUpdateTow = 10;
           return;
       }
       if (Vector2.Distance(new Vector2(m_currentN, m_currentE), new Vector2(_latitude, _longitude)) > 0.00050F)
       {
           m_currentN = _latitude;
           m_currentE = _longitude;
           LoadHttp(_latitude, _longitude);
           m_TimeOnUpdateTow = 10;
           return;
       }
       MsgBase.SendMsg<string, bool>("SetButtonLose", "UPphoto", false);
       MsgBase.SendMsg<bool>("isRefresh", false);

   }

   private bool isLoadUPdateTime = false;
   public IEnumerator LoadUPdateTime()
   {
       while (isLoadUPdateTime)
       {
           Debug.Log("----isNotReachable---2--"+isNotReachable);
           m_TimeOnUpdateTow--;
           if (m_TimeOnUpdateTow <= 0 && isNotReachable)
           {
               m_TimeOnUpdateTow = 10;
               isLoadUPdateTime = false;
               PhotoScene.Instance.PhotoNotReachable();
           }
           yield return new WaitForSeconds(0.5F);
       }
       
   }
    private void GetGps(bool _isOk, string _errorCode, System.DateTime _time, float _longitude, float _latitude, float _altitude, float _LAccuracy, float _HAccuracy)
    {
        if (_longitude == 0 || _latitude==0)
        {
            PhotoScene.Instance.SetHttpData(false, null, true);
            return;
        }
            float currentN;
            float currentE;
            float Hiehgt;
            float LAccuracy;
            float HAccuracy;

            SkySceneManager.Instance.currentE = _longitude;
            SkySceneManager.Instance.currentN = _latitude;

        ///1.5.1增加 活动入口UI开启  为确保有GPS 数据 放在这里



         PhotoScene.Instance.StartCoroutine(loadmovable());


            currentN = _latitude;
            currentE = _longitude;
            Hiehgt = _altitude;
            LAccuracy = _LAccuracy;
            HAccuracy = _HAccuracy;
            Load(_longitude, _latitude);
    }

    private IEnumerator loadmovable()
    {
        WWW www = new WWW(http+"/v1/sys/get_sys_parameter.json");
        
        yield return www;
        if (www.error==null)
        {
            Debug.Log(www.text);
            JsonData jd = JsonMapper.ToObject(www.text);
            if (jd["ret_code"].ToString() == "0" && jd["err_msg"].ToString() == "ok")
            {
                if (jd["parameter"]["d3_advert"]["switch"].ToString() != "enable")
                {
                    PhotoScene.Instance.movableUI = false;
                    HomePageMag.Initacne.HomePageChildDictionary["movableUI"].SetActive(false);
                }
                else
                {
                    SetARxunbaoUI();
                }
            }
            else
            {
                SetARxunbaoUI();
            }
        }
        else
        {
            SetARxunbaoUI();

        }
    }


    private void SetARxunbaoUI()
    {
        //1月20日 关闭 寻宝 按钮提示教程
        //if (PlayerPrefs.GetInt("ARxunbaoUI")!=1)
        //{
        //    PlayerPrefs.SetInt("ARxunbaoUI", 1);
        //    HomePageMag.Initacne.ARxunbaoUIobj.SetActive(true);
        //    return;
        //}
        //PhotoScene.Instance.movableUI = true;
        //HomePageMag.Initacne.HomePageChildDictionary["movableUI"].SetActive(true);
    }

    public void LoadHttp(float _currentN, float _currentE)
    {
            string s = http + pathjson + "ring=1" + "&geo_lng=" + _currentE.ToString() + "&geo_lat=" + _currentN.ToString() + "&page=0";
            Debug.Log(s);
            PhotoScene.Instance.TimeUpData();
            
    }
    #endregion

    private List<string> ClearItemDictList = new List<string>();
    private bool ISArrangement = false;
    public void SetData(Dictionary<string, JsonData> _tmpDictionary)
    {
        Dictionary<string, JsonData> tmpDictionary = _tmpDictionary;

        if (_tmpDictionary==null)
        {
            // PhotoScene.Instance.SetHttpData(false, null, true);
            return;
        }
        if (_tmpDictionary.Count==0)
        {
           // PhotoScene.Instance.SetHttpData(false, null, true);
            return;
        }
        foreach (var item in tmpDictionary)
        {
            if (!m_itemGameObjectDictinary.ContainsKey(item.Key))
            {
                //TODO 创建 ItemData
                CreateitemSnu++;
                CreateItemData(item.Value);
            }
        }
        //TODO  启动排列
        if (ISArrangement == false)
        {
            ISArrangement = true;
            PhotoScene.Instance.StartCoroutine(ItemArrangement(true));
            return;
        }
        if (ClearItemDictList.Count != 0)
        {
            PhotoScene.Instance.StartCoroutine(ItemArrangement(true));
        }
        else
            PhotoScene.Instance.StartCoroutine(ItemArrangement(false));
        ClearItemDictList.Clear();
    }
    public int CreateitemSnu = 0;
    public void CreateItemData(JsonData jd)
    {
        ItemData tmpItemData = new ItemData(jd,PhotoRoot);

        m_itemGameObjectDictinary.Add(jd["cardId"].ToString(), tmpItemData);
    }

    private Find_Item_Config m_FindConfig = null;
    private void CreateFindConfig()
    {
        FindConfig = MonoBehaviour.Instantiate(Resources.Load("photo/Find_config")) as GameObject;
        FindConfig.name = "FindConfig";
        FindConfig.transform.SetParent(PhotoRoot.transform);
        m_FindConfig = FindConfig.GetComponent<Find_Item_Config>();
    }
    private Vector3[] vs = {new Vector3(0,1.5f,5), new Vector3(0, 1.5f, -5) , new Vector3(5, 1.5f, 0),new Vector3(-5, 1.5f, 0) };
    private IEnumerator ItemArrangement(bool isArrang)
    {
        if (isArrang)
        {
            int Next = 0;
            int Next2 = 0;
            foreach (var tmp in m_itemGameObjectDictinary)
            {
                tmp.Value.itemRootObj.gameObject.transform.SetParent(PhotoRoot.transform);
                Vector3 v = m_PhotoConfigArray.GetPos(Next);
                if (tmp.Value.cardType > 3)
                {
                    
                    tmp.Value.SetPos(vs[Next2]);
                    tmp.Value.Appearanc();
                    Next2++;
                    if (Next2>vs.Length)
                    {
                        Next2 = 0;
                    }
                }
                else
                {
                    v.y += 5;
                    tmp.Value.SetPos(v);
                    tmp.Value.Appearanc();
                    Next++;
                }
                yield return new WaitForSeconds(0.1f);
            }
            PhotoScene.Instance.LoadingEnd();
        }else
        {
            PhotoScene.Instance.LoadingEnd();
        }
    }

    private Tweener posTweener;
    private ItemData tmpItemData = null;
    private float Speed = 0;
    private float Speed1 = 0;
    private MySkyInputEvent.DragState m_tmpDragState = MySkyInputEvent.DragState.OnDrag;


    private void SetRoybojColor(bool isShow)
    {
        if (isShow)
        {
            Color r = PhotoScene.Instance.Royboj.GetComponent<Renderer>().material.color;
            PhotoScene.Instance.Royboj.SetActive(isShow);
            PhotoScene.Instance.Royboj.GetComponent<Renderer>().material.DOColor(new Color(r.r, r.g, r.b, 1), 1);
        }else
        {
            Color r = PhotoScene.Instance.Royboj.GetComponent<Renderer>().material.color;
            PhotoScene.Instance.Royboj.GetComponent<Renderer>().material.DOColor(new Color(r.r, r.g, r.b, 0), 1);
        }
    }

    public bool isRootRotation = true;
    public void RootRotation()
    {
        if (isRootRotation==false)
        {
            return;
        }
        float speedValue1 = 0.025f;
        Vector3 V = PhotoRoot.transform.rotation.eulerAngles;
        PhotoRoot.transform.rotation = Quaternion.Euler(new Vector3(0, Speed, 0) + V);
        if (Speed==0)
        {
           SetRoybojColor(false);
        }
        else if (Speed!=0&&m_tmpDragState != MySkyInputEvent.DragState.OnDrag)
        {
            SetRoybojColor(true);
            PhotoScene.Instance.Royboj.transform.localRotation = Quaternion.Euler(new Vector3(0, Speed, 0) + V);
            m_TimeOnUpdateTow = 10;
        }
        if (m_tmpDragState != MySkyInputEvent.DragState.OnDrag)
        {
            if (Speed > 0)
            {
                Speed += -Speed * speedValue1;
                if (Speed < 0.1f)
                {
                    Speed = 0;
                    m_tmpDragState = MySkyInputEvent.DragState.OnDrag;
                }
            }
            else if (Speed < 0)
            {
                Speed += -Speed * speedValue1;
                if (Speed > -0.1f)
                {
                    Speed = 0;
                    m_tmpDragState = MySkyInputEvent.DragState.OnDrag;
                }
            }else
            {
                m_tmpDragState = MySkyInputEvent.DragState.OnDrag;
            }
        }
    }
    public void ItemDrag(GameObject obj, MySkyInputEvent.DragState tmpDragState, Vector2 pos, Vector2 speed, Vector3 newPos)
    {
        float speedValue1 = 10f;
        float speedValue2 = 20f;
        m_tmpDragState = tmpDragState;
        if (tmpDragState == MySkyInputEvent.DragState.OnDrag)
        {
            MsgBase.QuitBaseUI();
            PhotoScene.Instance.Royboj.SetActive(true);
            Speed = speed.x * speedValue1 * Time.deltaTime;
            Speed1 = speed.x * speedValue1 * Time.deltaTime;
            if (Speed <= -speedValue2)
            {
                Speed = -speedValue2;
                Speed1 = -speedValue2;
            }
            else if (Speed >= speedValue2)
            {
                Speed = speedValue2;
                Speed1 = speedValue2;
            }
        }
        if (tmpDragState == MySkyInputEvent.DragState.Start)
        {
            if (obj == null)
            {
            
            }
            tmpItemData = FindItemData(obj);
            if (tmpItemData != null)
            {
                MsgBase.QuitBaseUI();
                tmpItemData.item.BoderGlow.SetActive(true);
                tmpItemData.item.gameObject.transform.DOKill();
            }
        }
        if (tmpItemData != null)
        {
            if (tmpDragState == MySkyInputEvent.DragState.OnDrag)
            {
                m_TimeOnUpdateTow = 30;
                tmpItemData.AnimatorStop();
                tmpItemData.item.BoderGlow.SetActive(true);
                SetRoybojColor(false);
                Vector3 v = new Vector3(tmpItemData.item.transform.position.x, newPos.y, tmpItemData.item.transform.position.z);
                tmpItemData.item.transform.position = v;
            }
            if (tmpDragState == MySkyInputEvent.DragState.End)
            {
                ClickItemData.item.Photo.GetComponent<Renderer>().material.color = ClickItemData.item.tmpColor;
                tmpItemData.item.BoderGlow.SetActive(false);
                MoveItemData = tmpItemData;
                posTweener = tmpItemData.item.gameObject.transform.DOLocalMove(Vector3.zero, 1);
                posTweener.OnComplete(endMove);
            }
        }
    }
    private ItemData MoveItemData = null;

    public void endMove()
    {
        MoveItemData.AnimatorPlay();
        MoveItemData = null;
    }
    private SpecialEffectsUI m_effectsUI;
    private ItemData ClickItemData = null;
    private bool IsItemClick = false;
    public void ItemClick(GameObject obj, Vector2 pos, SpecialEffectsUI _effectsUI,MySkyInputEvent.ClickState ClickState)
    {
    
        if (IsItemClick==true)
        {
            return;
        }
        if (ClickState==MySkyInputEvent.ClickState.Down)
	    {
              ClickItemData = FindItemData(obj);


            Debug.Log("====" + ClickItemData.cardType);
             if (ClickItemData != null&& ClickItemData.cardType>3)
             {
                return;
             }
              if (ClickItemData != null)
              {
                  PhotoScene.Instance.clickUi.ClickUiObj.GetComponent<RectTransform>().position = new Vector3(pos.x, pos.y, 0);
                  PhotoScene.Instance.clickUi.ClickUiObj.SetActive(true);
                  ClickItemData.item.Photo.GetComponent<Renderer>().material.color = new Vector4(0.5f, 0.5f, 0.5f, 1);
                  ClickItemData.item.BoderGlow.SetActive(true);
              }
            if (m_tmpDragState != MySkyInputEvent.DragState.OnDrag)
            {
                Speed = 0;
                m_tmpDragState = MySkyInputEvent.DragState.OnDrag;
                return;
            }
        }
        else
        {
            if (ClickItemData != null && ClickItemData.cardType > 3)
            {
                if (ClickItemData.cardType==4)
                {
                    ClickItemData.item.GetComponent<Animation>().Play();
                    ClickItemData.item.Clip.SetActive(true);
                    PhotoScene.Instance.StartCoroutine(Doyhongdong(ClickItemData.nid));
                }else
                {
                    ClickItemData.item.Clip.SetActive(true);
                    PhotoScene.Instance.StartCoroutine(Doyhongdong(ClickItemData.nid));
                }
                return;
            }

            m_effectsUI = _effectsUI;
            ClickItemData = FindItemData(obj);
            if (ClickItemData != null)
            {
                m_TimeOnUpdateTow = 30;
                ClickItemData.item.Photo.GetComponent<Renderer>().material.color = ClickItemData.item.tmpColor;
                ClickItemData.item.BoderGlow.SetActive(false);
                MsgBase.QuitUI();
                IsItemClick = true;
                PhotoScene.Instance.clickUi.specialEffectsUI2.clickShow(ItemClickShowEnd);
            }
        }
    }

    private IEnumerator Doyhongdong(string s)
    {
        yield return new WaitForSeconds(2);
        RemoveItemDictinary(s);
    }

    private void ItemClickShowEnd()
    {
        JsonData jsonData = null;
        if (ClickItemData != null)
        {
            jsonData = ClickItemData.itemJson;
            if (jsonData == null)
            {
                PhotoScene.SendDebug(" 单张点击  jsonData    空");
                return;
            }
        }
        // MsgBase.SendMsg<JsonData, Callback<bool, string>>("U2N_U_OpenStory", jsonData, null);
        JsonData jd = new JsonData();
        jd["Event"] = "EventClickItme";
        jd["data"] = jsonData;
        Debug.Log(jd.ToJson());
        Native.Instance.SendNative(jd);
        ClickItemData =null;
    }
    public void StoryCallback()
    {
        if ( PhotoScene.Instance.clickUi.specialEffectsUI2 != null)
        {
             IsItemClick = false;
             PhotoScene.Instance.clickUi.specialEffectsUI2.clickHide(HideCallback);
        }
    }
    private void HideCallback()
    {
        //MsgBase.ShowUI();
    }
    Queue<Vector3> UpPhotoPosQueue = new Queue<Vector3>();
    private int UpPoints = 0;

    private ItemData tmpUpItemData = null;
    public void UpPhoto(bool isUP ,JsonData jd)
    {
        if (isUP==false)
        {
            PhotoScene.Instance.specialEffectsUI.clickHide(UPHide2);
            return;
        }
        PhotoScene.Instance.IsUpPhoto = true;
        if (GetItmeDataObjectCount() < 20)
        {
            UpPoints = GetItmeDataObjectCount();
        }
        tmpUpItemData = new ItemData(jd,PhotoRoot);
        tmpUpItemData.isUp = true;
        Vector3 tmpV3 = m_PhotoConfigArray.GetPos(UpPoints);
        ItemData tmpFindItemData = FindItemData(tmpV3);
        if (tmpFindItemData != null)
        {
            bool isRemove = RemoveItemDictinary(tmpFindItemData.nid);
            if (isRemove)
            {
                tmpUpItemData.SetPos(tmpV3);
                m_itemGameObjectDictinary.Add(jd["cardId"].ToString(), tmpUpItemData);
                tmpUpItemData.itemRootObj.transform.SetParent(PhotoRoot.transform);
                tmpUpItemData.item.gameObject.SetActive(true);
                PhotoScene.Instance.specialEffectsUI.clickHide(UPHide);
            }
            else
            {
                PhotoScene.SendDebug("上传出错  已有相片 未能删除");
            }
        }
        else
        {
            tmpUpItemData.SetPos(tmpV3);
            m_itemGameObjectDictinary.Add(jd["cardId"].ToString(), tmpUpItemData);
            tmpUpItemData.itemRootObj.transform.SetParent(PhotoRoot.transform);
            tmpUpItemData.item.gameObject.SetActive(true);
            MsgBase.SendMsg<bool, float>("ShowBlur", true, 10);
            PhotoScene.Instance.specialEffectsUI.clickHide(UPHide);
        }
    }
    private void UPHide2()
    {
        MsgBase.ShowUI();
    }
    public void UPHide()
    {
       // GameObject g = MonoBehaviour.Instantiate(Resources.Load("photo/UploadEffects")) as GameObject;
       // g.transform.SetParent(PhotoScene.Instance.PhotoCameraObj.transform);
       // g.transform.localPosition = new Vector3(0, 0, 8);
        MsgBase.ShowUI();
       
        PhotoScene.Instance.PhotoInvoke(0.4f,UploadEffectsMoveEnd);
      //  g.AddComponent<UploadEffectsMove>().Move(tmpUpItemData, UploadEffectsMoveEnd);
        UpPoints++;
        if (UpPoints >= 20)
        {
            UpPoints = 0;
        }

    }
    private void UploadEffectsMoveEnd()
    {
        PhotoScene.Instance.IsUpPhoto = false;
        SUIManager.instance.reminder.ShowReminder("照片已成功挂天上^_^", 3);
        //tmpUpItemData.Appearanc();
    }
    public int Findphotos()
    {
        //最物体的点位
        Vector3 tempGameObject = Vector3.zero;
        Vector3 tempPoint =PhotoScene.Instance.PhotoCameraObj.transform.forward;

        tempPoint *= 10f;
        float baseDistance = 9999999;
        foreach (KeyValuePair<string, ItemData> item in m_itemGameObjectDictinary)
        {
            Vector3 tp = item.Value.item.Photo.transform.position;
            float distance = Vector3.Distance(tempPoint, tp);
            if (baseDistance > distance)
            {
                baseDistance = distance;
                tempGameObject = tp;
            }
        }
        //检查物体在左边还是右边  备注，和forward Cross之后point的y是负数则说明在右边，正数则在左面
        Vector3 v3 = Vector3.Cross(tempGameObject, tempPoint);
        if (v3.y < 0)
            return 2;
        else
            return 1;
    }
    public bool IsSvreenPhoto()
    {
        if (GetItmeDataObjectCount() == 0)
        {
            return false;
        }
        List<ItemData> tmpList = new List<ItemData>(m_itemGameObjectDictinary.Values);
        for (int i = 0; i < tmpList.Count; i++)
        {
            if (tmpList[i].item.isRendering == true)
            {
                return true;
            }
        }
        return false;
    }

}
