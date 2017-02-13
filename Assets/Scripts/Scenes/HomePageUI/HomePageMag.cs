using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using DG.Tweening;
using com.imysky.camera;
using System.IO;
using System;
public class HomePageMag : MyUIBaes
{
    public static HomePageMag Initacne;
    private SUIMenu sUImenu = null;
    private SUIMenu sUImenuhelp = null;

    public GameObject ARxunbaoUIobj;


    private HomeButtonEvnetEnum M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.END;

    private enum HomeButtonEvnetEnum
    {
        MINE = 0,
        MAP=1,
        END=2,
        UpPhoto=3,
        Collectphoto=4,
        movableUI=5,
    }
    void Awake()
    {
        Initacne = this;
        //打开 或关闭 UI 
        MsgBase.MsgAdd("HomePageMagShowUI", base.Play);
        MsgBase.MsgAdd("HomePageMagQuitUI", base.Quit);
        //主菜单功能 按钮事件
        MsgBase.MsgAdd<string>("ButtonEvnetCallback", ButtonEvnetCallback);
        //收回 Menu 菜单
        MsgBase.MsgAdd("BaseButtonEvnet2", BaseButtonEvnet2);
        //帮助 事件
        MsgBase.MsgAdd("help", helpEvnet);
        //帮助页内  返回按钮事件
        MsgBase.MsgAdd("help04", help04Evnet);

        //滑动事件 
        MsgBase.MsgAdd<GameObject, MySkyInputEvent.Direction>("EventSlideDirection", Slide_MineButtonEvnet);
        //开启或 关闭 滑动事件
        MsgBase.MsgAdd<bool>("ISslide", isSlide);
        //宠物 按钮
        MsgBase.MsgAdd("pet", pet);
        //截屏 事件
        MsgBase.MsgAdd("Screenshot", Photograph);
        //打开刷新按钮
        MsgBase.MsgAdd<bool>("isRefresh", isRef);

        //附近返回
        MsgBase.MsgAdd("U2N_N_CloseNearby", NearbyBack);

        //单张点击返回
        MsgBase.MsgAdd("U2N_N_CloseStory", StoryCallback);
        //拍照返回
        MsgBase.MsgAdd<bool, JsonData>("U2N_N_CloseSendStory", UpPhotoBack);
        //重复教程
        MsgBase.MsgAdd("help01", help01Evnet);


        //按钮不可用状态

        MsgBase.MsgAdd<string, bool>("SetButtonLose", SetButtonLose);

        MsgBase.MsgAdd("U2N_N_CloseMainMenu", U2N_N_CloseMainMenu);
        MsgBase.MsgAdd("U2N_N_CloseMap", CloseMap);
        MsgBase.MsgAdd("U2N_N_OpenMainScene", U2N_N_OpenMainScene);
        MsgBase.MsgAdd("U2N_N_CloseMainScene", U2N_N_CloseMainScene);
        MsgBase.MsgAdd("U2N_N_CheeseBack", U2N_N_CheeseBack);
        MsgBase.MsgAdd("U2N_N_CloseWorld", U2N_N_CloseWorld);

    }

    private void SetButtonLose(string arg1, bool arg2)
    {
        this.ButtonLose(arg1, arg2);
    }

    private Sprite m_Sprite = null;
    //重复教程
    private int pk = 0;
    private void help01Evnet()
    {
        sUImenuhelp.MenuDictionary["Backround"].gameObject.SetActive(false);
        isHelp = false;
        MsgBase.SendMsg<string>("HideScene", "PhotoScene");
        MsgBase.SendMsg<string>("OnOpenScene", "Tutorial");
    }
    private void NearbyBack()
    {
        M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.END;
        
        PhotoScene.Instance.collectPhoto.NearbyBack();
    }

    private void UpPhotoBack(bool arg1, JsonData arg2)
    {
        M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.END;
        PhotoScene.Instance.itemManager.UpPhoto(arg1, arg2);
    }

    private void StoryCallback()
    {
        M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.END;
        PhotoScene.Instance.itemManager.StoryCallback();
    }
    private string userhead = "";
    void Start()
    {
        userhead =  PlayerPrefs.GetString("userhead");
        //userhead = "http://imysky-user-img01.img-cn-beijing.aliyuncs.com/584c0a7570bedba83babec38.jpg";
        sUImenu = HomePageChildDictionary["SUIMenu"].GetComponent<SUIMenu>();
        sUImenuhelp = HomePageChildDictionary["SUIMenuhelp"].GetComponent<SUIMenu>();
        sUImenuhelp.gameObject.AddComponent<SUIMenuhelpEvent>();
        if (m_Sprite==null)
        {
            m_Sprite = HomePageChildDictionary["mine"].gameObject.GetComponent<MineTexture>().mineSprite;
        }
        if (userhead!="")
        {
            StartCoroutine(LoadUserhead(userhead));
        }
    }
    private IEnumerator LoadUserhead(string _userhead)
    {
        WWW www = new WWW(_userhead+"@!imysky-style-adaptiveh-100w-90q");
        yield return www;
        if (www.error==null)
        {
            Texture2D t = www.texture;
            foreach (Transform item in HomePageChildDictionary["mine"].gameObject.transform)
	        {
                if (item.name=="default")
                {
                    item.transform.FindChild("userhead").transform.FindChild("Image").GetComponent<Image>().sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(1, 1));
                }
                if (item.name == "press")
                {
                    item.transform.FindChild("userhead").transform.FindChild("Image").GetComponent<Image>().sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(1, 1));
                }
	        }
        }else
        {
           
        }
    }
    private void Setsprite()
    {
        foreach (Transform item in HomePageChildDictionary["mine"].gameObject.transform)
        {
            if (item.name == "default")
            {
                item.transform.FindChild("userhead").transform.FindChild("Image").GetComponent<Image>().sprite = m_Sprite;
            }
            if (item.name == "press")
            {
                item.transform.FindChild("userhead").transform.FindChild("Image").GetComponent<Image>().sprite = m_Sprite;
            }
        }
    }


    private float EventTime = 0;
    void Update()
    {
        if (EventTime>0)
        {
            EventTime -= Time.deltaTime;
        }
        if (userhead != PlayerPrefs.GetString("userhead"))
        {
            userhead = PlayerPrefs.GetString("userhead");
            StartCoroutine(LoadUserhead(userhead));
        }
        if (userhead == "" && m_Sprite!=null)
        {
            Setsprite();
        }
            
    }

    public void Delelte()
    {
        MsgBase.MsgRemove("HomePageMagShowUI", base.Play);
        MsgBase.MsgRemove("HomePageMagQuitUI", base.Quit);
        MsgBase.MsgRemove<string>("ButtonEvnetCallback", ButtonEvnetCallback);
        Destroy(gameObject);
    }

    private void U2N_N_CloseWorld()
    {
        M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.END;
        PhotoScene.Instance.RequestAnUpdate();
    }
    private void U2N_N_CheeseBack()
    {
        if (SkySceneManager.Instance.GetScene() == false)
        {
            return;
        }
        gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(true);
        IsPhotograph = false;
        MsgBase.ShowUI();
    }


    private void U2N_N_CloseMainScene()
    {
        M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.END;
        PhotoScene.Instance.RequestAnUpdate();
    }
    private void U2N_N_OpenMainScene()
    {
        M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.END;
        PhotoScene.Instance.RequestAnUpdate();
  
    }
    //地图页面返回
    private void CloseMap()
    {
        PhotoScene.Instance.specialEffectsUI.clickHide(delegate() { M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.END; MsgBase.ShowUI(); return; });
    }
    //我的页面返回
    private void U2N_N_CloseMainMenu()
    {
        M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.END;
        PhotoScene.Instance.isNotReachable(true);
        if (PhotoScene.Instance.movableUI)
        {
          //  HomePageChildDictionary["movableUI"].SetActive(true);
        }
        MsgBase.SendMsg<Callback, bool>("SetIsAngle02", null, false);
        MsgBase.ShowUI();
    }

    //刷新
    private bool isRefresh = false;
    private void isRef(bool arg1)
    {
        if (sUImenu != null)
        {
            sUImenu.MenuDictionary["Backround"].gameObject.transform.FindChild("Refresh").GetComponent<SUIButton>().isLose = arg1;
        }
    }

    /// <summary>
    /// 截屏  事件 处理 
    /// </summary>
    private bool IsPhotograph = false;
    private void Photograph()
    {
        if (IsPhotograph == false)
        {
            //MsgBase.QuitUI();
            BaseButtonEvnet2();
            gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
            IsPhotograph = true;
            SkySceneManager.Instance.StartCoroutine(PhotographFile());
        }
    }
    private IEnumerator PhotographFile()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.5f);
        SCameraManager.currentCamera.PhotographFile(
            delegate(bool isok, string filename) 
            {
                if (isok)
                {
                    //消息通知 安卓 发送地址
                    MsgBase.SendMsg<string, Callback<bool, string>>("U2N_U_Cheese", filename, null);

                }
                else
                {
                    //出错提示
                    MsgBase.ShowUI();
                    IsPhotograph = false;
                }
            });
    }
    //开启宠物 提示页面
    private void pet()
    {
        // MsgBase.SendMsg("ShowpetUI");
        ButtonEvnetCallback("movableUI");
    }
    //是否开启 滑动事件
    private bool ISslide = false;
    private void isSlide(bool arg1)
    {
        ISslide = arg1;
    }
    //滑动事件
    private void Slide_MineButtonEvnet(GameObject arg1, MySkyInputEvent.Direction arg2)
    {
        if (ISslide|| M_HomeButtonEvnetEnum != HomeButtonEvnetEnum.END)
        {
            return;
        }
        if (arg1 == null && arg2 == MySkyInputEvent.Direction.down && isHelp == false && SkySceneManager.Instance.GetScene() == true)
        {
            ButtonEvnetCallback("mine");
        }
    }
    //当前是否是帮助页面
    private bool isHelp = false;
    /// <summary>
    /// 帮助 按钮事件
    /// </summary>
    private void helpEvnet()
    {
        //关闭列表菜单   注：当列表菜单打开  点击其他事件 关闭列表
        MsgBase.QuitUI();
        PhotoScene.Instance.IsClickPhtoto = true;
        //HomePageChildDictionary["movableUI"].SetActive(false);
        sUImenuhelp.MenuDictionary["Backround"].gameObject.SetActive(true);
        //MsgBase.SendMsg<bool>("isheip", true);
        isHelp = true;

    }
    private void help04Evnet()
    {
        PhotoScene.Instance.IsClickPhtoto = false;
        if (PhotoScene.Instance.movableUI)
        {
            //HomePageChildDictionary["movableUI"].SetActive(true);
        }
        sUImenuhelp.MenuDictionary["Backround"].gameObject.SetActive(false);
       // MsgBase.SendMsg<bool>("isheip", false);
        MsgBase.ShowUI();
        isHelp = false;
    }
    //关闭 列表
    private void BaseButtonEvnet2()
    {
        if (isBase == false)
        {
            if (isBaseStatr)
            {
                Debug.Log("isBaseStatr");
                isBaseStatr = false;
                Vector3 v = Vector3.zero;
                SUIButton tmpSUIButton = HomePageChildDictionary["BaseButton"].GetComponent<SUIButton>();
                if (isBase)
                    v = new Vector3(0, 0, 90);
                else
                    v = new Vector3(0, 0, -90);
                tmpSUIButton.Rotate("default", v, 0.5f, EndBase);
                sUImenu.AnimPlay(isBase);
                isBase = !isBase;
            }
        }
    }
    private bool isBase = true;
    private bool isBaseStatr = true;
    public void ButtonEvnetCallback(string eventGameObjName)
    {

        PhotoScene.Instance.uiRoot.CloseAllAlert();
        PhotoScene.Instance.uiRoot.ColseReminder();
        if (M_HomeButtonEvnetEnum != HomeButtonEvnetEnum.END || EventTime>0)
        {
            return;
        }
        if (eventGameObjName == "ar")
        {
            EventTime = 1;
            MsgBase.QuitUI();
           
            MsgBase.SendMsg("HideScene", "PhotoScene");
            MsgBase.SendMsg<bool, float>("ShowBlur", false, 10);
            MsgBase.SendMsg<string>("OnOpenScene", "ImageIdentificationScene");
            //关闭列表菜单   注：当列表菜单打开  点击其他事件 关闭列表
            BaseButtonEvnet2();
            return;
        }
        if (eventGameObjName == "mine")
        {
            EventTime = 1;
            MsgBase.QuitUI();
           // HomePageChildDictionary["movableUI"].SetActive(false);
            PhotoScene.Instance.photoprompt.Off();
            PhotoScene.Instance.isNotReachable(false);
            M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.MINE;
            MsgBase.SendMsg<Callback<bool, string>>("U2N_U_OpenMainMenu", null);
            //关闭列表菜单   注：当列表菜单打开  点击其他事件 关闭列表
            MsgBase.SendMsg<Callback, bool>("SetIsAngle02", delegate() { MsgBase.SendMsg<bool, float>("ShowBlur", false, 30); }, true);
       
            BaseButtonEvnet2();
            return;
        }
        if (eventGameObjName == "map")
        {
            EventTime = 1;
            M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.MAP;
            BaseButtonEvnet2();
            MsgBase.QuitUI();
            PhotoScene.Instance.specialEffectsUI.clickShow(delegate()
            {
                MsgBase.SendMsg<Callback<bool, string>>("U2N_U_OpenMap", null);
            });
            return;
        }
        if (eventGameObjName == "BaseButton")
        {
            if (isBaseStatr)
            {
                Debug.Log("isBaseStatr");
                isBaseStatr = false;
                Vector3 v = Vector3.zero;
                SUIButton tmpSUIButton = HomePageChildDictionary["BaseButton"].GetComponent<SUIButton>();
                if (isBase)
                    v = new Vector3(0, 0, 90);
                else
                    v = new Vector3(0, 0, -90);
                tmpSUIButton.Rotate("default", v, 0.5f, EndBase);
                sUImenu.AnimPlay(isBase);
                isBase = !isBase;
            }
            return;
        }
        
        if (eventGameObjName == "UPphoto")
        {
            EventTime = 1;
            M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.UpPhoto;
            MsgBase.QuitUI();
            PhotoScene.Instance.UpPhoto();
            return;
        }
        if (eventGameObjName == "CollePhoto")
        {
            EventTime = 1;
            M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.Collectphoto;
            PhotoScene.Instance.Collectphoto();
            return;
        }
        if (eventGameObjName == "movableUI")
        {
            if (PhotoScene.Instance.IsLoadingData)
            {
                return;
            }
            Debug.Log("==eventGameObjName==" + PlayerPrefs.GetString("uid").ToString());
            if (PlayerPrefs.GetString("uid").Equals("") || PlayerPrefs.GetString("uid") == null)
            {
                MsgBase.SendMsg<Callback<bool, string>>("U2N_U_OpenLogin", null);
                return;
            }
            EventTime = 1;
            M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.movableUI;
            MsgBase.QuitUI();
            MsgBase.SendMsg("HideScene", "PhotoScene");
            //MsgBase.SendMsg<bool, float>("ShowBlur", false, 10);
            MsgBase.SendMsg<string>("OnOpenScene", "MovableScene");
           // HomePageChildDictionary["movableUI"].SetActive(false);
            //关闭列表菜单   注：当列表菜单打开  点击其他事件 关闭列表
            BaseButtonEvnet2();
            return;
        }
    }
    private void EndBase()
    {
        isBaseStatr = true;
    }
    public void SetHomeButtonEvnetEnumEnd()
    {
        M_HomeButtonEvnetEnum = HomeButtonEvnetEnum.END;

    }

}
