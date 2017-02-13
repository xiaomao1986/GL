using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



/// <summary>
/// Alert.   提示框
/// 开发者：全宏伟
/// 示例代码：
/// </summary>
 
public class SUIAlert {

    //提示框Object
    private GameObject m_gameObject;
    private Text m_title;
	private Text m_message;
    private Button m_button1;
    private Button m_button2;
    private Button m_button3;
	private Button m_button4;
    private Button m_button5;
    private Text m_button1text;
    private Text m_button2text;
    private Text m_button3text;

	private RectTransform m_image_BackGround;

	private Animation m_ani;

	private bool m_isClose = true;
	//动画是否播放
	private bool m_isAniPlay = false;

    private static int m_id = 0;

    private class AlertData
    {
		//提示标题
		public string title;
		//提示内容
        public string message;
		//确定按钮标题
        public string okName;
		//取消按钮标题
        public string cancelName;
		//确定事件
        public Callback okCallback;
		//返回事件
        public Callback cancelCallback;
        //是否显示退出按钮
        public bool isShowExite;

        //所在的UIRoot
        public SUIRoot uiRoot;

        //按钮个数 1个或2个
        public int buttonNumber;

        //当前状态 1表示正常显示 2表示隐藏  
        public int state = 1;

        //当前id
        public int id = -1;
    }

    private List<AlertData> m_alertStack = new List<AlertData>();

	/// <summary>
	/// 设置提示框的预设体
	/// </summary>
	/// <param name="g">The green component.</param>
    public void SetGameObject(GameObject g)
    {
        try
        {

        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            Debug.LogError("错误：SUIAlert 提示框的预设体错误。");
        }
        m_gameObject = g;
		m_title = m_gameObject.transform.FindChild("Title").GetComponent<Text>();
		m_message = m_gameObject.transform.FindChild("Message").GetComponent<Text>();
        m_button1 = m_gameObject.transform.FindChild("Button1").GetComponent<Button>();
        m_button2 = m_gameObject.transform.FindChild("Button2").GetComponent<Button>();
        m_button3 = m_gameObject.transform.FindChild("Button3").GetComponent<Button>();
		m_button4 = m_gameObject.transform.FindChild("Button4").GetComponent<Button>();
        m_button5 = m_gameObject.transform.FindChild("Button5").GetComponent<Button>();

		m_image_BackGround = m_gameObject.transform.FindChild("Image_BackGround").GetComponent<RectTransform>();

        m_button1text = m_button1.transform.FindChild("Text").GetComponent<Text>();
        m_button2text = m_button2.transform.FindChild("Text").GetComponent<Text>();
        m_button3text = m_button3.transform.FindChild("Text").GetComponent<Text>();



        m_button1.onClick.RemoveAllListeners();
        m_button1.onClick.AddListener(ButtonCallback1);

        m_button2.onClick.RemoveAllListeners();
        m_button2.onClick.AddListener(ButtonCallback2);

        m_button3.onClick.RemoveAllListeners();
        m_button3.onClick.AddListener(ButtonCallback1);

        m_button4.onClick.RemoveAllListeners();
        m_button4.onClick.AddListener(ButtonCallbackExit);

        m_button5.onClick.RemoveAllListeners();
        m_button5.onClick.AddListener(ButtonCallbackExit);

		m_ani = m_gameObject.GetComponent<Animation> ();

		SUIManager.instance.StartCoroutine (update ());
    }

	/// <summary>
	/// 创建一个Update用来检测动画播放
	/// </summary>
	private IEnumerator update()
	{
		while (true) {
			yield return new WaitForFixedUpdate ();

			if (m_isAniPlay&& m_isClose && m_gameObject.active) {
				m_ani.Play ("AlertOut");
				m_isAniPlay = false;
			}
			if (!m_isAniPlay && m_isClose && m_ani.isPlaying == false && m_gameObject.active) {
				m_gameObject.SetActive (false);
			}

		}
	}


    /// <summary>
    /// 提示框
    /// </summary>
    /// <param name="_uiroot">当前场景的Uiroot</param>
    /// <param name="title">标题</param>
    /// <param name="message">提示内容</param>
    /// <param name="but1Name">按钮1名称</param>
    /// <param name="but1CallBack">回调函数</param>
    /// <param name="but2Name">按钮2名称</param>
    /// <param name="but2CallBack">按钮2的回调函数</param>
    /// <param name="isShowExite">是否显示退出按钮</param>
    /// <returns>该提示框的ID号</returns>
    public int ShowAlert(SUIRoot _uiroot,string title, string message,
        string but1Name, Callback but1CallBack,
        string but2Name = null, Callback but2CallBack = null, bool isShowExite = false)
    {
        //关闭所有3D空间的操作
        //MySkyInputEvent.instance.Pause(true);

        AlertData data = new AlertData();
        data.uiRoot = _uiroot;
        data.title = title;
        data.message = message;
        data.okCallback = but1CallBack;
        data.okName = but1Name;
        data.cancelCallback = but2CallBack;
        data.cancelName = but2Name;
        data.id = m_id++;
        data.isShowExite = isShowExite;

        if (but2Name != null)
            data.buttonNumber = 2;
        else
            data.buttonNumber = 1;

        //添加提示框
        m_alertStack.Insert(0,data);
        //显示提示框
        ShowAlertUI();

        return data.id;
    }

	/// <summary>
	/// 关闭所有提示框
	/// </summary>
	public void CloseAll()
	{
		CloseObjcet ();
		m_alertStack.Clear ();
	}

    public void Close(SUIRoot _uiRoot,int _id)
    {
        AlertData data1 = GetFirstUsed();
        AlertData data2 = null;

        for (int i = 0; i < m_alertStack.Count; i++)
        {
            if (_id == m_alertStack[i].id)
            {
                if(m_alertStack[i].uiRoot != _uiRoot )
                {
                    Debug.LogError(" 关闭出错，该id不可被该场景控制。");
                    return;
                }
                data2 = m_alertStack[i];
                m_alertStack.RemoveAt(i);
                break;
            }
        }

        if(data1 == data2)
        {
            //刷新显示
            ShowAlertUI();
        }
    }

    /// <summary>
    /// 刷新提示框
    /// </summary>
    /// <param name="_uiroot">当前场景的UIRoot</param>
    /// <param name="_state">1: 开启   2：隐藏   3：关闭  </param>
    public void Refresh(SUIRoot _uiroot, int _state)
    {
        switch(_state)
        {
            case 1:
            case 2:
                foreach (AlertData data in m_alertStack)
                {
                    if (data.uiRoot == _uiroot) data.state = _state;
                }
                break;
            case 3:
                for (int i = m_alertStack.Count-1;i>=0;i--)
                {
                    if (m_alertStack[i].uiRoot == _uiroot) m_alertStack.RemoveAt(i);
                }
                break;
        }
        //刷新界面中的对话框
        ShowAlertUI();
    }

    

    private void ButtonCallback1()
    {
        //获取第一个提示框内容
        AlertData data = GetFirstUsed();
        if (data == null)
        {
            Debug.LogError("Alert ButtonCallback3 错误，队列中没有可用的数据");
            return;
        }
        m_alertStack.Remove(data);
        if (data.okCallback != null) data.okCallback();

        ShowAlertUI();
    }
    private void ButtonCallback2()
    {
        //获取第一个提示框内容
        AlertData data = GetFirstUsed();
        if (data == null)
        {
            Debug.LogError("Alert ButtonCallback2 错误，队列中没有可用的数据");
            return;
        }
        m_alertStack.Remove(data);

        if (data.cancelCallback != null) data.cancelCallback();

        ShowAlertUI();
    }

    private void ButtonCallbackExit()
    {
        //获取第一个提示框内容
        AlertData data = GetFirstUsed();
        if (data == null)
        {
            Debug.LogError("Alert ButtonCallback2 错误，队列中没有可用的数据");
            return;
        }
        m_alertStack.Remove(data);

        ShowAlertUI();
    }

    private AlertData GetFirstUsed()
    {
        //获取第一个可以显示的对话框
        for(int i = 0 ; i < m_alertStack.Count ; i ++)
        {
            if (m_alertStack[i].state == 1) return m_alertStack[i];
        }
        return null;
    }

    //隐藏UI内容
    private void CloseObjcet()
    {
        //MySkyInputEvent.instance.Pause(false);
        m_isClose = true;
        m_isAniPlay = true;
    }

    /// <summary>
    /// 显示提示UI
    /// </summary>
    /// <param name="_alert">要显示的内容</param>
    private void ShowAlertUI()
    {
        AlertData _alert = GetFirstUsed();
        if (_alert == null)
        {
            CloseObjcet();
            return;
        }
        m_gameObject.SetActive(true);
        m_message.text = _alert.message;

        //设置图片的样式
        if (_alert.title == null)
        {
            m_title.gameObject.SetActive(false);
            m_title.text = "";
            //m_image_BackGround.anchoredPosition = new Vector2(0, 28f);
            //m_image_BackGround.sizeDelta = new Vector2(605f, 243f);
        }
        else
        {
            m_title.gameObject.SetActive(true);
            m_title.text = _alert.title;
            //m_image_BackGround.anchoredPosition = new Vector2(0, 77.77495f);
            //m_image_BackGround.sizeDelta = new Vector2(605f, 343.55f);
        }

        if (_alert.buttonNumber == 0)
        {
            m_button1.gameObject.SetActive(false);
            m_button2.gameObject.SetActive(false);
            m_button3.gameObject.SetActive(false);
        }
        else if (_alert.buttonNumber == 1)
        {
            m_button1.gameObject.SetActive(false);
            m_button2.gameObject.SetActive(false);
            m_button3.gameObject.SetActive(true);
            m_button3text.text = _alert.okName;
        }
        else if (_alert.buttonNumber == 2)
        {
            m_button1.gameObject.SetActive(true);
            m_button2.gameObject.SetActive(true);
            m_button3.gameObject.SetActive(false);
            m_button1text.text = _alert.okName;
            m_button2text.text = _alert.cancelName;
        }

        m_button4.gameObject.SetActive(_alert.isShowExite);
        m_button5.gameObject.SetActive(_alert.isShowExite);

        m_ani.Play("AlertIn");
        m_isClose = false;
    }
}
