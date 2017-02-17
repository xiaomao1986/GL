using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using LitJson;

public class HomePageUI : Scene
{

    public GameObject ui;

    public Button Backbutton;
    public Button Upbutton;
    public Button Minubutton;


    public Button[] MinuButtons;
    public Sprite[] Sprites;
    public Sprite[] Sprites2;


    public GameObject MinuObj;


    public GameObject tishi;

    public bool isToggle=true;

    public Toggle toggle;
    public Button[] jiaochengs;
    void Start()
    {
      //  PlayerPrefs.DeleteAll();
        if (PlayerPrefs.GetInt("GUIDE3DGL") == 0)
        {
            jiaochengs[0].gameObject.SetActive(true);    
        }else
        {
            MsgBase.SendMsg("OnOpenScene", "PhotoScene");
            ui.SetActive(true);
        }
        EventTriggerListener.Get(jiaochengs[0].gameObject).onClick = onjiaochengClick_00;
        EventTriggerListener.Get(jiaochengs[1].gameObject).onClick = onjiaochengClick_01;
        EventTriggerListener.Get(jiaochengs[2].gameObject).onClick = onjiaochengClick_02;
        EventTriggerListener.Get(jiaochengs[3].gameObject).onClick = onjiaochengClick_03;
        EventTriggerListener.Get(jiaochengs[5].gameObject).onClick = onjiaochengClick_04;


        EventTriggerListener.Get(Backbutton.gameObject).onClick = onBackbuttonClick;
        EventTriggerListener.Get(Upbutton.gameObject).onClick = onUpbuttonClick;
        EventTriggerListener.Get(Minubutton.gameObject).onClick = onMinubuttonClick;

        EventTriggerListener.Get(MinuButtons[0].gameObject).onClick = onClick_00;
        EventTriggerListener.Get(MinuButtons[1].gameObject).onClick = onClick_01;
        EventTriggerListener.Get(MinuButtons[2].gameObject).onClick = onClick_02;
        EventTriggerListener.Get(MinuButtons[3].gameObject).onClick = onClick_03;
        EventTriggerListener.Get(MinuButtons[4].gameObject).onClick = onClick_04;
    }

    private void onjiaochengClick_00(GameObject go)
    {
        jiaochengs[0].gameObject.SetActive(false);
        jiaochengs[1].gameObject.SetActive(true);
    }
    private void onjiaochengClick_01(GameObject go)
    {
        jiaochengs[1].gameObject.SetActive(false);
        jiaochengs[2].gameObject.SetActive(true);
    }
    private void onjiaochengClick_02(GameObject go)
    {
        jiaochengs[2].gameObject.SetActive(false);
        jiaochengs[3].gameObject.SetActive(true);
    }
    private void onjiaochengClick_03(GameObject go)
    {
        jiaochengs[3].gameObject.SetActive(false);
        jiaochengs[4].gameObject.SetActive(true);
    }
    private void onjiaochengClick_04(GameObject go)
    {
        jiaochengs[4].gameObject.SetActive(false);
        MsgBase.SendMsg("OnOpenScene", "PhotoScene");
        ui.SetActive(true);
        if (isToggle)
        {
            PlayerPrefs.SetInt("GUIDE3DGL", 2);
        }
       
    }
    //动态
    private void onClick_00(GameObject go)
    {
        UpMinuButton(0);
      bool isItem = PhotoScene.Instance.itemManager.shaixuan(0);
        if (isItem==false)
        {
            tishi.SetActive(true);
            tishi.transform.FindChild("Text").GetComponent<Text>().text = "没有找到动态 卡片~~~！！！";
        }
        Invoke("Endtishi", 1);
    }
    //用户
    private void onClick_01(GameObject go)
    {
        UpMinuButton(1);
        bool isItem = PhotoScene.Instance.itemManager.shaixuan(1);
        if (isItem == false)
        {
            tishi.SetActive(true);
            tishi.transform.FindChild("Text").GetComponent<Text>().text = "没有找到用户 卡片~~~！！！";
        }
        Invoke("Endtishi", 1);
    }
    //默认
    private void onClick_02(GameObject go)
    {
        UpMinuButton(2);
        PhotoScene.Instance.itemManager.OpenItem();
    }
    //游戏
    private void onClick_03(GameObject go)
    {
        UpMinuButton(3);
        bool isItem = PhotoScene.Instance.itemManager.shaixuan(3);
        if (isItem == false)
        {
            tishi.SetActive(true);
            tishi.transform.FindChild("Text").GetComponent<Text>().text = "没有找到用户 游戏~~~！！！";
        }
        Invoke("Endtishi", 1);
    }

    private void Endtishi()
    {
        tishi.SetActive(false);
    }

    //寻宝
    private void onClick_04(GameObject go)
    {
        UpMinuButton(4);
        PhotoScene.Instance.itemManager.isOpenxunbao(true);
    }

    private void UpMinuButton(int i)
    {
        MinuButtons[i].transform.FindChild("Back").GetComponent<Image>().sprite = Sprites[i];

        for (int k = 0; k < MinuButtons.Length; k++)
        {
            if (k==i)
            {
                continue;
            }
            MinuButtons[k].transform.FindChild("Back").GetComponent<Image>().sprite = Sprites2[k];
        }

    }
    public void OnValueChange(bool ist)
    {
        isToggle = toggle.isOn;
    }
    private void onBackbuttonClick(GameObject go)
    {
        ///返回 事件
        JsonData jd4 = new JsonData();
        jd4["Event"] = "closeactivity";
        Native.Instance.SendNative(jd4);
    }
    private void onUpbuttonClick(GameObject go)
    {
        ///拍照事件
        JsonData jd4 = new JsonData();
        jd4["Event"] = "photograph";
        Native.Instance.SendNative(jd4);
    }


    private bool isMinubutton = false;
    private void onMinubuttonClick(GameObject go)
    {

        Debug.Log("123");
        ///列表按钮 事件
        if (!isMinubutton)
        {
            isMinubutton = true;
            Debug.Log("Back");
            MinuObj.GetComponent<RectTransform>().DOScale(Vector3.one,0.2f);
            Minubutton.gameObject.transform.FindChild("Back").gameObject.SetActive(false);
            Minubutton.gameObject.transform.FindChild("Back1").gameObject.SetActive(true);
        }
        else
        {
            isMinubutton = false;
            Debug.Log("Back1");
            Minubutton.gameObject.transform.FindChild("Back1").gameObject.SetActive(false);
            Minubutton.gameObject.transform.FindChild("Back").gameObject.SetActive(true);
            MinuObj.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.2f);
        }
       
    }




    protected override void OnOpen(Callback<float, bool> callback)
    {
     

    }
    private Callback<float, bool> m_callback;
    private void Loading()
    {
        m_callback(0, false);
    }
    private void Loading(float t)
    {
      
    }

    protected override void OnClose(Callback callback)
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           
        }
    }

}
