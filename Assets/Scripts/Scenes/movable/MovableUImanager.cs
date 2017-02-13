using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.imysky.camera;
using DG.Tweening;
using LitJson;
using System.Collections.Generic;

public class MovableUImanager : MonoBehaviour
{


    public static MovableUImanager Instance;


    public Sprite backpackSprite1;
    public Sprite backpackSprite2;
    public Button ArrowButton;

    public Button backpackButton;
    public Image img;
    public GameObject Aim;

    private bool isUp;
    private bool isUp2;
    private bool isStartCoroutine = false;





    public GameObject UIstart;
    public GameObject UIend;
    public GameObject UIend2;
    public GameObject UIend3;
    public GameObject UIobtain;
    public GameObject UIcomplete;
    public GameObject UIBack;
    public GameObject UITime;


    public GameObject tubaoButton;
    public GameObject tubaoButton1;


    public Button zhuangtaiButton;
    public Button zhuangtaiList;
    public Text zhuangtaiText;


    public Button fenxiangButton;
    public Button fenxiangButton2;


    public Button LoadBackButton;
    void Start()
    {
        Instance = this;
        

        EventTriggerListener.Get(ArrowButton.gameObject).onDown = onArrowButtonDown;
        EventTriggerListener.Get(ArrowButton.gameObject).onUp = onArrowButtonUp;
        EventTriggerListener.Get(backpackButton.gameObject).onClick = onbackpackButton;

        EventTriggerListener.Get(fenxiangButton.gameObject).onClick = onfenxiangButton;
        EventTriggerListener.Get(fenxiangButton2.gameObject).onClick = onfenxiangButton;

        EventTriggerListener.Get(tubaoButton.gameObject).onClick = ontubaoButton;
        EventTriggerListener.Get(tubaoButton1.gameObject).onClick = ontubaoButton1;

        EventTriggerListener.Get(UIstart.transform.FindChild("Button").gameObject).onClick = onUIstartButtonUp;
        EventTriggerListener.Get(UIend.transform.FindChild("Button").gameObject).onClick = onUIendButtonUp;
        EventTriggerListener.Get(UIend2.transform.FindChild("Button").gameObject).onClick = onUIendButtonUp;
        EventTriggerListener.Get(UIcomplete.transform.FindChild("Button").gameObject).onClick = onUIendButtonUp;
        EventTriggerListener.Get(UIBack.transform.FindChild("Button").gameObject).onClick = onUIBackdButtonUp;
        EventTriggerListener.Get(UIend3.transform.FindChild("Button").gameObject).onClick = onUIBackdButtonUp;
        EventTriggerListener.Get(LoadBackButton.gameObject).onClick = onUIBackdButtonUp;

        EventTriggerListener.Get(zhuangtaiButton.gameObject).onClick = onzhuangtaiButton;
        EventTriggerListener.Get(zhuangtaiList.gameObject).onClick = onzhuangtaiList;
        isStartCoroutine = false;
        //List<MovableData> movableDataList = new List<MovableData>();

        //MovableData we=new MovableData();
        //we.activity_id = "123";
        //we.activity_site = "sdf";
        //we.activity_theme = "qweqwewe";
        //MovableData we1 = new MovableData();
        //we1.activity_id = "1232";
        //we1.activity_site = "sdf";
        //we1.activity_theme = "qweqwewe";
        //movableDataList.Add(we);
        //movableDataList.Add(we1);
        //SetUIstart(movableDataList);
    }

    

    private void onfenxiangButton(GameObject go)
    {
        string shareurl = PlayerPrefs.GetString("shareurl");
        string shareimagepath = PlayerPrefs.GetString("shareimagepath");
        string title = "快来参加AR夺宝，好东西免费抢了！";
        string view_title = "这么多宝贝免费抢！\n赶快告诉小伙伴们来抢宝贝了~";
        string content = "我用AR夺宝抢到了好多好东西，你也快来跟我一起抢吧！";
        string weibo_content = "我用AR夺宝抢到了好多好东西，你也快来跟我一起抢吧！请点击"+" "+ shareurl;
        My_debugNet.SendDebugLog("onfenxiangButton----"+ shareurl+ shareimagepath);
        MsgBase.SendMsg<string, string, string, string, string, string,Callback<bool, string>>
            ("U2N_U_OpenShare", shareurl, title, view_title, content, weibo_content, shareimagepath,null);
        My_debugNet.SendDebugLog("onfenxiangButton--122--" + shareurl + shareimagepath);
    }
    private void onzhuangtaiList(GameObject go)
    {
        zhuangtaiList.gameObject.SetActive(false);
    }
    private void onzhuangtaiButton(GameObject go)
    {
        zhuangtaiText.text = "<size=50>本区域正在进行的活动\n</size>";
        for (int i = 0; i < MovableScene.Instance.movableManager.movabledataList.Count; i++)
        {
            if (MovableScene.Instance.movableManager.movabledataList[i].ret_code=="0")
            {
                string activity_theme = MovableScene.Instance.movableManager.movabledataList[i].activity_theme;
                string end_time = MovableScene.Instance.movableManager.movabledataList[i].end_time;
                string s = MovableScene.Instance.movableManager.movabledataList[i].huodongzhuangtai;
                string s1 = "";
                if (s== "疯抢中")
                {
                     s1 = "<color=orange>"+ s + "</color>";
                }else
                {
                     s1 = "<color=silver>" + s + "</color>";
                }
                zhuangtaiText.text = zhuangtaiText.text+"<size=35>" + activity_theme + "  " + s1 + "  " + end_time + "结束\n</size>";
            }
        }
        zhuangtaiList.gameObject.SetActive(true);
    }
    private void ontubaoButton1(GameObject go)
    {
        tubaoButton1.SetActive(false);
    }
    private void ontubaoButton(GameObject go)
    {
        tubaoButton1.SetActive(true);
    }

    private void onbackpackButton(GameObject go)
    {
       MsgBase.SendMsg<Callback<bool, string>>("U2N_U_OpenBag", null);
    }
    public void SetUIdebugText(string  t)
    {
        //UIDebug.SetActive(true);
       // UIDebug.transform.FindChild("Text").GetComponent<Text>().text = "错误  编码：---"+t;
    }
    public void SetUITimeText(string t)
    {
        UITime.SetActive(true);
        UITime.transform.FindChild("Text2").GetComponent<Text>().text = t;
    }
    public void onUIBackdButtonUp(GameObject go)
    {
        Debug.Log("onUIBackdButtonUp");
        LoadBackButton.gameObject.SetActive(false);
        MovableScene.Instance.Load(100);
        //HomePageMag.Initacne.HomePageChildDictionary["movableUI"].SetActive(true);
        HomePageMag.Initacne.SetHomeButtonEvnetEnumEnd();
        MsgBase.SendMsg<string>("OnOpenScene", "PhotoScene");
        MsgBase.SendMsg<string>("RemoveScene", "MovableScene");
    }
    private void onUIendButtonUp(GameObject go)
    {
        UIend.gameObject.SetActive(false);
        UIend2.gameObject.SetActive(false);
        UIcomplete.gameObject.SetActive(false);
    }

    public bool isIstartButtonUp = false;

    public void onUIstartButtonUp(GameObject go)
    {
        if (isButtonEvnet == false || isIstartButtonUp)
        {
            return;
        }
        Debug.Log("onUIstartButtonUp");
       
        isIstartButtonUp = true;

        if (MovableScene.Instance.movableManager.movabledataList.Count != 0)
        {
            MovableScene.Instance.movableManager.MovableStartSum = 0;
            MovableScene.Instance.movableManager.Loadbegin(BeingBack,
                MovableScene.Instance.movableManager.movabledataList[0].activity_id);
        }
    }

    private void onArrowButtonUp(GameObject go)
    {
        if (isButtonEvnet==false)
        {
            return;
        }
        isUp2 = true;
    }
    private void onArrowButtonDown(GameObject go)
    {
        if (MovableScene.Instance.isCrossbowAnim==false)
        {
            MovableScene.Instance.fire();
        }
        //if (isButtonEvnet == false)
        //{
        //    return;
        //}
        //isUp = false;
        //isUp2 = false;
        //if (isStartCoroutine == false)
        //{
        //    img.fillAmount = 0f;
        //    isStartCoroutine = true;
        //   // StartCoroutine(grow());
        //}
    }
    private IEnumerator grow()
    {
        while (true)
        { 
            //if (isUp)
            //{
            //    isStartCoroutine = false;
            //    break;
            //}
            //if (img.fillAmount == 0)
            //{
            //    Debug.Log("ssssssssssssss222ssssssssss");
            //    MovableScene.Instance.fire();
            //}
            //img.fillAmount += 0.5f * Time.deltaTime;
            //if (img.fillAmount>=1)
            //{
            //    img.fillAmount = 0f;
            //    MovableScene.Instance.Crossbow.GetComponent<Animator>().speed = 1;
            //    MsgBase.SendMsg("LoadLoadArrow");
            //    if (isUp2==true)
            //    {
            //        img.fillAmount = 1f;
            //        isStartCoroutine = false;
            //        isUp = true;
            //        break;
            //    }
            //}
            yield return null;
        }
      //  isStartCoroutine = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="texture2D">图片</param>
    /// <param name="activity_theme">活动</param>
    /// <param name="begin_time">开始时间</param>

    public void SetUIstart(Texture2D texture2D, string activity_theme, string begin_time, string activity_site)
    {
       // UIstart.transform.FindChild("Image").GetComponent<Image>().sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(1, 1));
       // UIstart.transform.FindChild("Text1").GetComponent<Text>().text = activity_theme;
      //  UIstart.transform.FindChild("Text3").GetComponent<Text>().text = begin_time;
      //  UIstart.transform.FindChild("Text2").GetComponent<Text>().text = "地点："+activity_site;

    }
    public UI_Control_ScrollFlow _ScrollFlow;
    public void SetUIstart(List<MovableData> movableDataList)
    {
      //  My_debugNet.SendDebugLog("--movableDataList---" + movableDataList.Count + "===" + movableDataList[0].activity_id);
        for (int i = 0; i < movableDataList.Count; i++)
        {
            try
            {
                GameObject g = Instantiate(Resources.Load("Image")) as GameObject;
                g.transform.SetParent(UIstart.transform.FindChild("Drag"));
                g.GetComponent<RectTransform>().localPosition = Vector3.zero;
                g.name = "Image" + i.ToString();
                movableDataList[i].SetTexture2Dactivity(g);
                g.GetComponent<UIMovableData>().UIdata = movableDataList[i];
            }
            catch (Exception e)
            {
                //My_debugNet.SendDebugLog("--Exception---" + e.ToString());
            }
       
            // g.GetComponent<Image>().sprite = Sprite.Create(t2d,new Rect(0,0,t2d.width,t2d.height),new Vector2(1,1));
        }
        _ScrollFlow.Refresh();
        UIstart.SetActive(false);
    }



    private bool isUIcomplete = false;
    private bool isUIend = false;
    public void BeingBack(string msg)
    {
        MovableScene.Instance.Load(100);
        UIBack.SetActive(true);
        MovableScene.Instance.ArrowManager.SetActive(true);
        fenxiangButton.gameObject.SetActive(true);
        Debug.Log("msg---="+msg);
       // My_debugNet.SendDebugLog("msg---=" + msg);
        if (msg == "0")
        {
            isButtonEvnet = true;
            tubaoButton.SetActive(true);
            zhuangtaiButton.gameObject.SetActive(true);
            //MovableScene.Instance.movableManager.StartTime();
            MsgBase.SendMsg<bool, float>("ShowBlur", true, 10);
            ISshow(true);
            MovableScene.Instance.ArrowManager.SetActive(true);
            UIstart.SetActive(false);
        }
        if (msg == "-301")
        {
            //活动不存在：
            return;
        }
        if (msg == "-302")
        {
            for (int i = 0; i < MovableScene.Instance.movableManager.movabledataList.Count; i++)
            {
                if (MovableScene.Instance.movableManager.movabledataList[i].activity_id == MovableScene.Instance.movableManager.huodongID)
                {
                    if (MovableScene.Instance.movableManager.movabledataList[i].isEnd == false)
                    {
                        UIend.gameObject.SetActive(true);
                        UIend.transform.FindChild("Text").GetComponent<Text>().text = MovableScene.Instance.movableManager.movabledataList[i].activity_theme + "已结束," + MovableScene.Instance.movableManager.movabledataList[i].amount.ToString() + "份" + MovableScene.Instance.movableManager.movabledataList[i].activity_theme + "已抢完,您可以继续参加其他活动。";
                        MovableScene.Instance.movableManager.movabledataList[i].isEnd = true;
                    }
                }
            }

            return;
        }
        if (msg == "-303")
        {
            Debug.Log("msg-2--=" + msg);
            //活动还没有开始
          //  My_debugNet.SendDebugLog("msg-2--=" + msg);
            UIstart.SetActive(true);
            UIstart.transform.FindChild("Text4").gameObject.SetActive(true);
            UIstart.transform.FindChild("Text4").GetComponent<Text>().text = "活动还没有开始 请耐心等待！！！";
            Invoke("EndUIstart4", 3);
            ISshow(false);
            MovableScene.Instance.ArrowManager.SetActive(false);
            return;
        }
        if (msg == "-304")
        {
            //商品已抢完
           
            //   您晚到一步，宝贝已被抢完，继续射击仅提供娱乐，明天还有新宝贝哦！
            for (int i = 0; i < MovableScene.Instance.movableManager.movabledataList.Count; i++)
            {
                if (MovableScene.Instance.movableManager.movabledataList[i].activity_id == MovableScene.Instance.movableManager.huodongID)
                {
                    MovableScene.Instance.movableManager.movabledataList[i].huodongzhuangtai = "已抢完";
                    if (MovableScene.Instance.movableManager.movabledataList[i].isNumber == false)
                    {
                        UIend2.gameObject.SetActive(true);
                        UIend2.transform.FindChild("Text").GetComponent<Text>().text = MovableScene.Instance.movableManager.movabledataList[i].amount.ToString() + "份" + MovableScene.Instance.movableManager.movabledataList[i].activity_theme + "已被抢完。下次活动早点来吧！";
                        MovableScene.Instance.movableManager.movabledataList[i].isNumber = true;
                    }
                }
            }

        }
        if (msg == "-305")
        {
            //已参加该活动
            //My_debugNet.SendDebugLog("isUIcomplete--=" + isUIcomplete);
            if (isUIcomplete)
            {
                return;
            }
            isUIcomplete = true;
            UIcomplete.SetActive(true);
            //MovableScene.Instance.movableManager.isGift = true;
        }
        if (msg == "-1000")
        {
            //已经射过了
            if (isUIcomplete)
            {
                return;
            }
            isUIcomplete = true;
            UIcomplete.SetActive(true);
           // MovableScene.Instance.movableManager.isGift = true;

        }
        if (msg == "-1001")
        {
            //，没有活动
            UIend3.SetActive(true);
            UIstart.SetActive(false);
            isButtonEvnet = false;
            ISshow3(true);
            //MovableScene.Instance.movableManager.isGift = true;
        }
        if (msg == "-1002")
        {
            SetUIdebugText("-1002");
        }
       
    }

    private bool isButtonEvnet = true;
    private void EndUIstart4()
    {
        UIstart.transform.FindChild("Text4").gameObject.SetActive(false);
    }

    public void seckil(Texture2D texture2D,GameObject obj)
    {
        //射到奖品
       // My_debugNet.SendDebugLog("--seckil-===========-" + t);
     //   UIobtain.transform.FindChild("Text1").GetComponent<Text>().text = t;
        ISshow2(false);
        UIobtain.SetActive(true);
        UIobtain.GetComponent<RectTransform>().localScale=Vector3.zero;
        var screenPos = SCameraManager.currentCamera.camera.WorldToScreenPoint(obj.transform.position);
        Vector3 v = UIobtain.GetComponent<RectTransform>().localPosition;

        UIobtain.GetComponent<RectTransform>().localPosition = screenPos;
       Tweener pos= UIobtain.GetComponent<RectTransform>().DOLocalMove(v, 0.5f);
        pos.OnComplete(delegate()
        {
            Invoke("MoveUIobtain",3);
        });
        UIobtain.GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f);
        UIobtain.GetComponent<Image>().sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(1, 1));
        //My_debugNet.SendDebugLog("--UIobtain-=========222==-" + t);
    }

    private void MoveUIobtain()
    {
        Tweener pos2 = UIobtain.GetComponent<RectTransform>().DOLocalMove(backpackButton.GetComponent<RectTransform>().localPosition, 0.5f);
        backpackButton.GetComponent<Animator>().Play("BackPackButton");
        UIobtain.GetComponent<RectTransform>().DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f);
        pos2.OnComplete(delegate()
        {
            UIobtain.SetActive(false);
            ISshow2(true);
            SetbackpackImage(2);
            backpackButton.GetComponent<Animator>().Play("State");

        });
    }
    public void ISshow(bool Active)
    {
        ArrowButton.gameObject.SetActive(Active);
        backpackButton.gameObject.SetActive(Active);
        Aim.gameObject.SetActive(Active);
    }
    public void ISshow3(bool Active)
    {
        ArrowButton.gameObject.SetActive(Active);
        backpackButton.gameObject.SetActive(Active);
        Aim.gameObject.SetActive(Active);
        MovableScene.Instance.ArrowManager.SetActive(Active);
    }
    public void ISshow2(bool Active)
    {
        ArrowButton.gameObject.SetActive(Active);
       // backpackButton.gameObject.SetActive(Active);
        Aim.gameObject.SetActive(Active);
    }
    public void SetbackpackImage(int i)
    {
        switch (i)
        {
            case 1:
                backpackButton.GetComponent<Image>().sprite = backpackSprite1;
                break;
            case 2:
                backpackButton.GetComponent<Image>().sprite = backpackSprite2;
                break;
        }
    }

    public GameObject Drid;
    public void SetUIScroll(string Time, string activity_site, string activity_theme, string amount)
    {
        GameObject item = Instantiate(Resources.Load("movable/UI/Item")) as GameObject;
        item.GetComponent<UISetScrollItem>().SetText(Time, activity_site, activity_theme, amount);
        item.transform.SetParent(Drid.transform);
        item.GetComponent<RectTransform>().localScale = Vector3.one;
    }
}
