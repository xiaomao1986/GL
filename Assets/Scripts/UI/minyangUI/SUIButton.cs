using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SUIButton : UIAnimation
{


    private enum ButtonSort
    {
        State_default,
        State_press,
        State_lose,
    }
    public const string
        NAME_DEFAULT = "default",
        NAME_PRESS = "press",
        NAME_LOSE = "lose",
        NAME_EFFECT = "effect",
        NAME_HINT = "hint",
        NAME_TEXT = "Text";



    public float m_EffectTime;

    public string m_LoseMessage;

    public string m_ButtonMsg;

    public int m_SceneID;

    public MyUIevent m_MyUIevent;

    public int msgID;

    /// <summary>
    /// 是否打开Lose
    /// </summary>
    private bool m_isLose;
    /// <summary>
    /// Button 组件
    /// </summary>
    private Button m_button;

    /// <summary>
    /// 按钮状态
    /// </summary>
    private ButtonSort buttonState = ButtonSort.State_default;

    /// <summary>
    /// 子节点管理
    /// </summary>
    private Dictionary<string, GameObject> m_ButtonDictionary = new Dictionary<string, GameObject>();


   


    #region Mono 方法


    /// <summary>
    /// 查找 是否有 default 名字的子节点     将 子节点 添加到 字典中管理
    /// </summary>
 
    void Awake()
    {
        m_button = gameObject.GetComponent<Button>();
        if (transform.FindChild("default") == null)
        {
          // bool isDisplayDialog = UnityEditor.EditorUtility.DisplayDialog("错误", " Button 组件 必须有 default 节点（背景）", "ok");
        }

        foreach (Transform tmp in gameObject.transform)
        {
            if (!m_ButtonDictionary.ContainsKey(tmp.name))
            {
                tmp.gameObject.SetActive(false);
                if (tmp.gameObject.name == NAME_DEFAULT || tmp.gameObject.name == NAME_TEXT)
                {
                    tmp.gameObject.SetActive(true);
                }
                m_ButtonDictionary.Add(tmp.name,tmp.gameObject);
            }
            else
            {
               // bool isDisplayDialog = UnityEditor.EditorUtility.DisplayDialog("错误", " m_ButtonDictionary 重复Key", "ok");
            }
        }
    }
    /// <summary>
    /// 查找 是否有 Buton 组件   有 则 添加 事件
    /// </summary>
    void Start()
    {
        if (m_button == null)
        {
          //  bool isDisplayDialog = UnityEditor.EditorUtility.DisplayDialog("错误", "必须是 Button 组件 才可以使用本类", "ok");
        }
        else
        {
            EventTriggerListener.Get(gameObject).onClick = CallbackEvent;
            EventTriggerListener.Get(gameObject).onDown = PointerDowkEvent;
            EventTriggerListener.Get(gameObject).onExit = ExitEvent;
        }
     
    }

    private void ExitEvent(GameObject go)
    {
        buttonState = ButtonSort.State_default;
        foreach (var tmp in m_ButtonDictionary)
        {
            tmp.Value.SetActive(false);
            if (tmp.Key == NAME_DEFAULT || tmp.Key == NAME_TEXT)
            {
                tmp.Value.SetActive(true);
            }
        }
    }
    /// <summary>
    /// 当脚本挂 物体上 会检查 是否是 UGUI 按钮 不是 则提示 
    /// </summary>
    void Reset()
    {
        if (gameObject.GetComponent<Button>() == null)
        {
             
            Debug.LogError(" 必须是 Button 组件 才可以使用本类");
           // bool isDisplayDialog = UnityEditor.EditorUtility.DisplayDialog("错误", "必须是 Button 组件 才可以使用本类", "ok");

        }

    }
    #endregion


    #region 外部调用
    /// <summary>
    /// 创建 SUIButton
    /// </summary>
    /// <param name="res">  Resources文件夹路径 </param>
    /// <returns></returns>
    /// 

    public static SUIButton Create(string res)
    {
        GameObject tmp = Instantiate(Resources.Load(res)) as GameObject;
        return tmp.GetComponent<SUIButton>();
    }
    /// <summary>
    /// 创建 SUIButton
    /// </summary>
    /// <param name="obj">  GameObject </param>
    /// <returns></returns>

    public static SUIButton Create(GameObject obj)
    {
        if (obj.GetComponent<SUIButton>() != null)
        {
            return obj.GetComponent<SUIButton>();
        }
        Debug.LogError(" 没有找到 SUIButton  ");
        return null;
    }
    
   /// <summary>
    /// //设置按钮提示 
   /// </summary>
   /// <param name="Text"> 内容 </param>
  
    public void SetHint(string Text)
    {
        GameObject tmpobj = GetButtonComponent(NAME_HINT);
        if (tmpobj!=null)
        {
            tmpobj.transform.FindChild("Text").GetComponent<Text>().text = Text;
            tmpobj.SetActive(true);
        }
    }
    
    /// <summary>
    /// //特效播放时间 
    /// </summary>
    /// <param name="time"> 时间 </param>
    public void SetEffecTime(float time)
    {
        m_EffectTime = time;
    }
    /// <summary>
    /// 设置按钮 设置失效状态下点击按钮的提示信息
    /// </summary>
    /// <param name="message"> 提示信息 </param>
    public void SetLoseMessage(string message)
    {
        m_LoseMessage = message;

    }
    /// <summary>
    /// 外部设置 isLose  是否打开
    /// </summary>
    public bool isLose
    {
        get { return m_isLose; }
        set
        {
            m_isLose = value;
            SetLose(m_isLose);
        }
    }

    private bool IsDownEvent = false;

    public bool isDownEvent
    {
        get { return IsDownEvent; }
        set
        {
            IsDownEvent = value;
        }
    }
    public void Rotate(string name, Vector3 v, float speed, DG.Tweening.TweenCallback Callback=null)
    {
        RectTransform tmp = m_ButtonDictionary[name].GetComponent<RectTransform>();
        RectTransform tmp1 = m_ButtonDictionary["press"].GetComponent<RectTransform>();
        if (Callback==null)
            base.SetRotate(tmp,tmp1, v, speed);
        else
            base.SetRotate(tmp,tmp1, v, speed, Callback);
    }

    public void AnimPlay(bool isStart)
    {
        base.ButtonPlay(isStart);
    }

    #endregion




    #region 内部方法

   /// <summary>
    /// 关闭 或 打开 GameObject
   /// </summary>
    /// <param name="obj"> GameObject 类型 </param>
    /// <param name="show"> bool类型 是否打开  </param>
    private void isUishow(GameObject obj, bool show)
    {
        obj.SetActive(show);
    }
    /// <summary>
    ///  设置 是否显示 Lose 
    /// </summary>
    /// <param name="_isLose"> _isLose 由于  isLose 传递</param>
    public void SetLose(bool _isLose)
    {
        m_button = gameObject.GetComponent<Button>();
        if (_isLose)
        {
            m_button.enabled = false;
            GetComponent<EventTriggerListener>().enabled = false;
            buttonState=ButtonSort.State_lose;
            foreach (var tmp in m_ButtonDictionary)
            {
                tmp.Value.SetActive(false);
                if (tmp.Key==NAME_LOSE||tmp.Key==NAME_TEXT)
                {
                    tmp.Value.SetActive(true);
                }
            }
        }
        else
        {
            //显示按钮
            buttonState = ButtonSort.State_default;
            foreach (var tmp in m_ButtonDictionary)
            {
                tmp.Value.SetActive(false);
                if (tmp.Key == NAME_DEFAULT || tmp.Key == NAME_TEXT)
                {
                    tmp.Value.SetActive(true);
                }
            }
            //打开按钮事件
            m_button.enabled = true;
            GetComponent<EventTriggerListener>().enabled = true;
        }


    }
    
    /// <summary>
    /// //当按钮抬起触发事件
    /// </summary>
    /// <param name="eventData"> 传递 GameObject </param>
    private void CallbackEvent(GameObject eventData)
    {
        if (IsDownEvent)
        {
            return;
        }
        GameObject tmpPress = GetButtonComponent(NAME_PRESS);
        if (tmpPress != null && tmpPress.activeSelf)
        {
                buttonState = ButtonSort.State_default;
                tmpPress.SetActive(false);
                GetButtonComponent(NAME_DEFAULT).SetActive(true);
        }

        GameObject tmpHintObj = GetButtonComponent(NAME_HINT);
        if (tmpHintObj != null)
        {
            //关闭提醒
            isUishow(tmpHintObj, false);
        }
        //播放特效
        GameObject tmpEffect = GetButtonComponent(NAME_EFFECT);
        if (tmpEffect != null)
        {
            //打开特效
            isUishow(tmpEffect, true);
            Invoke("Click", m_EffectTime);
            return;
        }
        Click();
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    private void Click()
    {
   
        //播放特效
        GameObject tmpEffect = GetButtonComponent(NAME_EFFECT);
        if (tmpEffect != null)
        {
            //关闭特效
            isUishow(tmpEffect, false);
            //发送消息
            if (gameObject.tag == "HomePage")
            {
                MsgBase.SendMsg<string>(m_ButtonMsg, gameObject.name);
                return;
            }
            MsgBase.SendMsg(m_ButtonMsg);
            return;
        }
        if (gameObject.tag == "HomePage")
        {
            MsgBase.SendMsg<string>(m_ButtonMsg, gameObject.name);
            return;
        }
        MsgBase.SendMsg(m_ButtonMsg);
        return;
    }

    //当按钮按下触发事件
    /// <summary>
    /// 按钮 按下事件 
    /// </summary>
    /// <param name="eventData"> 传递 GameObject</param>
    private void PointerDowkEvent(GameObject eventData)
    {
        GameObject tmpPress = GetButtonComponent(NAME_PRESS);


        Debug.Log("dddddddddd");
        if (tmpPress!=null)
        {
            buttonState = ButtonSort.State_press;
            tmpPress.SetActive(true);
            GetButtonComponent(NAME_DEFAULT).SetActive(false);
            if (IsDownEvent)
            {
                ////播放特效
                //GameObject tmpEffect = GetButtonComponent(NAME_EFFECT);
                //if (tmpEffect != null)
                //{
                //    //关闭特效
                //    isUishow(tmpEffect, false);
                //    //发送消息
                //    MsgBase.SendMsg(m_ButtonMsg);
                //    return;
                //}
                if (gameObject.tag=="HomePage")
                {
                    MsgBase.SendMsg<string>(m_ButtonMsg, gameObject.name);
                }
                MsgBase.SendMsg(m_ButtonMsg);
                return;
            }
        }
    }
    /// <summary>
    /// /查找UI组件
    /// </summary>
    /// <param name="NameKey"> 传递 NAME </param>
    /// <returns></returns>
    private GameObject GetButtonComponent(string NameKey)
    {
        if (m_ButtonDictionary.ContainsKey(NameKey))
        {
            return m_ButtonDictionary[NameKey];
        }
        return null;
    }
    #endregion
}
