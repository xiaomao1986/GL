#region 备注
/*****************************************************************
 * 类名：SkyEventManager
 * 作者：全宏伟
 * 介绍：用于介绍
 * 范例：
 * 依赖：
 * **************************************************************/
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using com.imysky.camera;

/// <summary>
/// 输入对象
/// </summary>
public class MySkyInputEvent : MonoBehaviour
{
    /// <summary>
    /// 在屏幕上滑动
    /// </summary>
    /// <param name="GameObject _gameObject">  拖动的物体，有可能返回null</param>
    /// <param name="ClickState _state">点击的状态</param>
    /// <param name="Vector2 _screenPos">touch当前屏幕坐标位置</param>
    public const string EventClick = "EventClick";

    public const string EventDoubleClick = "EventDoubleClick";
    public const string EventLongClick = "EventLongClick";

    /// <summary>
    /// 在屏幕上滑动
    /// </summary>
    /// <param name="GameObject _gameObject">  拖动的物体，有可能返回null</param>
    /// <param name="DragState _state">拖动的状态</param>
    /// <param name="Vector2 _screenPos">touch当前屏幕坐标位置</param>
    /// <param name="Vector2 _deltaPos">拖动偏移（速度）</param>
    /// <param name="Vector3 _objPos">被拖动3D物体的坐标位置</param>
    public const string EventDrag = "EventDrag";

    /// <summary>
    /// 在屏幕上滑动
    /// </summary>
    /// <param name="GameObject">拖动的物体，有可能返回null</param>
    /// <param name="DragState">拖动的状态</param>
    /// <param name="Vector2">当前坐标位置</param>
    /// <param name="Vector2 speed">拖动的速度</param>
    public const string EventSlide = "EventSlide";

    /// <summary>
    /// 向某个方向滑动
    /// </summary>
    /// <param name="GameObject">拖动的物体，有可能返回null</param>
    /// <param name="Direction ">滑动的方向</param>
    /// 
    public const string EventSlideDirection = "EventSlideDirection";



    public const string EventDoubleDrag = "EventDoubleDrag";
    public const string EventAngleHit = "EventAngleHit";
    public const string EventAccelerationHit = "EventAccelerationHit";

    public enum SlideState
    {
        Start = 0,
        OnDrag,
        End
    }
    public enum Direction
    {
        up = 0,
        down,
        left,
        right
    }
    public enum DragState
    {
        Start = 0,
        OnDrag,
        End
    }
    public enum ClickState
    {
        Down = 0,
        Up
    }
    //启动滑动事件的距离
    public const float SLIDE_DIRECTION_DISTANCE = 200;
    
    
    //点击的最少抬起事件
    public const float CLICK_OUT_TIME = 0.5F;
    //点击的最小滑动距离（滑动出该距离则无法触发点击事件）
    public const float CLICK_OUT_DISTANCE = 10F;
    //点击的最小滑动角度（摄影机滑动出该角度则无法触发点击事件）
    public const float CLICK_OUT_ANGLE = 20F;

    //双击的最小滑动距离（滑动出该距离则无法触发点击事件）
    public const float DOUBLECLICK_OUT_DISTANCE = 30F;
    //双击的最小滑动角度（摄影机滑动出该角度则无法触发点击事件）
    public const float DOUBLECLICK_OUT_ANGLE = 20F;

    //长安超出多长时间
    public const float LONGCLICK_OUT_TIME = 2F;
    //长安超出多少距离后
    public const float LONGCLICK_OUT_DISTANCE = 50F;
    //砸锤子超出多长时间，可以再次触发
    public const float HIT_OUT_TIME = 2F;

    /// <summary>
    /// 是否开启屏蔽UI事件
    /// </summary>
    public bool isShieldUGUI
    {
        get { return m_isShieldUGUI; }
        set { m_isShieldUGUI = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public int SetCamera(Camera c)
    {
        if (c == null)
            Debug.LogError("对不起摄影机不能为空");
        m_camera = c;
        return 1;
    }

    public int SetUICamera(Camera c)
    {
        if (c == null)
            Debug.LogError("对不起UI摄影机不能为空");
        ui_camera = c;
        return 1;
    }


    private enum INPUT_STATE
    {
        NORMAL,//正常状态
        DRAG,//拖动状态
        DOUBLE_DARG,//双手拖动状态
    }
    private INPUT_STATE m_state = INPUT_STATE.NORMAL;


    #region 点击事件
    /// <summary>
    /// 设置是否响应点击事件
    /// </summary>
    /// <param name="sw">true为响应 false为不响应</param>
    public void SetIsEventClick(bool sw)
    {
        isEventClick = sw;
    }
    /// <summary>
    /// 设置是否响应双击事件
    /// </summary>
    /// <param name="sw">true为响应 false为不响应</param>
    public void SetIsEventDoubleClick(bool sw)
    {
        isEventDoubleClick = sw;
    }
    /// <summary>
    /// 设置是否响应长安事件
    /// </summary>
    /// <param name="sw">true为响应 false为不响应</param>
    public void SetIsEventLongClick(bool sw)
    {
        isEventLongClick = sw;
    }
    /// <summary>
    /// 设置是否响应拖动事件
    /// </summary>
    /// <param name="sw">true为响应 false为不响应</param>
    public void SetIsEventDrag(bool sw)
    {
        isEventDrag = sw;
    }
    /// <summary>
    /// 设置是否响应滑动事件
    /// </summary>
    /// <param name="sw">true为响应 false为不响应</param>
    public void SetIsEventSlide(bool sw)
    {
        isEventSlide = sw;
    }
    /// <summary>
    /// 设置是否响应双手指拖动事件
    /// </summary>
    /// <param name="sw">true为响应 false为不响应</param>
    public void SetIsEventDoubleDrag(bool sw)
    {
        isEventDoubleDrag = sw;
    }
    /// <summary>
    /// 设置是否响应类似砸锤子事件，通过陀螺仪判断角度变化
    /// </summary>
    /// <param name="sw">true为响应 false为不响应</param>
    public void SetIsEventAngleHit(bool sw)
    {
        isEventAngleHit = sw;
    }
    /// <summary>
    /// 设置是否响应类似砸锤子事件，通过加速器判断用户拿手机向前快速移动
    /// </summary>
    /// <param name="sw">true为响应 false为不响应</param>
    public void SetIsEventAccelerationHit(bool sw)
    {
        isEventAccelerationHit = sw;
    }
    #endregion

    /*******************************************************非公有方法和变量*******************************************************/


    public static MySkyInputEvent instance = null;
    public string text = "";
    void Awake()
    {
        instance = this;
       
    }

    private bool isEventClick = true;
    private bool isEventDoubleClick = true;
    private bool isEventLongClick = true;
    private bool isEventSlide = true;
    private bool isEventDrag = true;
    private bool isEventDoubleDrag = true;
    private bool isEventAngleHit = true;
    private bool isEventAccelerationHit = false;

    //探测到的物体
    private GameObject currentGameObject = null;
    //当UGUI事件触发时，是否响应事件。
    private bool m_isShieldUGUI = true;
    //主摄影机
    public Camera m_camera = null;
    //UI摄像机
    public Camera ui_camera = null;


    private bool pause;
    public void Pause(bool pause)
    {
        //NativeConnection.instance.Send_log ("MySkyInputEvent Pause:"+pause);
        this.pause = pause;
        //		if (pause == false) {
        //			Reset();
        //		}
    }

    public void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            //暂停.
            //isEventAngleHit = false;
        }
        else
        {
            //NativeConnection.instance.Send_log ("MySkyInputEvent OnApplicationPause false");
            //恢复.
            //Reset();
            //isEventAngleHit = true;
        }
    }

    private void Reset()
    {
        lastRotation = Vector3.zero;
        AngleHitTime = Time.time;
    }

    void Update()
    {
 
        if (pause)
            return;

        testClickGameObject(this.ui_camera);
        if (currentGameObject == null)
            testClickGameObject(this.m_camera);

        if (isEventClick) testClick();
        if (isEventDoubleClick) testDoubleClick();
        if (isEventLongClick) testLongClick();
        if (isEventSlide) testSlide();

        if (isEventAngleHit) testAngleHit();

        if (isEventAccelerationHit) testAccelerationHit();

        if (isEventDoubleDrag) if (testDoubleDrag()) return;
        if (isEventDrag) testDrag();

    }

    //private string m_testString_Drag_Start;
    //private string m_testString_Drag_Update;
    //private string m_testString_Drag_End;
    ////private string m_testString_Drag_Start;

    //private string m_testString_DoubleDrag_Start;
    //private string m_testString_DoubleDrag_Update;
    //private string m_testString_Double_Drag_End;
    //	void OnGUI()
    //	{
    //		string s = "";
    //		s += "\nDrag_Start:"+m_testString_Drag_Start;
    //		s += "\nDrag_Update:"+m_testString_Drag_Update;
    //		s += "\nDrag_End:"+m_testString_Drag_End;
    //		s += "\nDoubleDrag_Start:"+m_testString_DoubleDrag_Start;
    //		s += "\nDoubleDrag_Update:"+m_testString_DoubleDrag_Update;
    //		s += "\nDouble_Drag_End:"+m_testString_Double_Drag_End;
    //
    //		GUIStyle style = new GUIStyle();
    //		style.fontSize = 40;
    //		style.alignment = TextAnchor.UpperCenter; 
    //		//style.font.material.color = Color.red;
    //		GUI.Label (new Rect (0, 0, Screen.width, Screen.height), s, style);
    //	}

    #region 探测点击的物体

    private Ray m_Mray;

    private RaycastHit m_Mhit;
    private void testClickGameObject(Camera camera)
    {
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {

            //判断点击的3D物体
            Vector3 Mps = Input.mousePosition;
            m_Mray = camera.ScreenPointToRay(Mps);


            if (Physics.Raycast(m_Mray, out m_Mhit))
            {
                if (camera.name == "Camera2D")
                {
                    if (m_Mhit.transform.gameObject.layer == 5)
                    {
                        currentGameObject = m_Mhit.transform.gameObject;
                        return;
                    }
                    else
                    {
                        currentGameObject = null;
                        return;
                    }
                }
                else
                {

                    currentGameObject = m_Mhit.transform.gameObject;
                    return;
                }
            }
            else
            {
                currentGameObject = null;
            }
        }

        if ((Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) &&
            (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)))
        {
            m_Mray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(m_Mray, out m_Mhit))
            {
                if (camera.name == "Camera2D")
                {
                    if (m_Mhit.transform.gameObject.layer == 5)
                    {
                        currentGameObject = m_Mhit.transform.gameObject;
                        return;
                    }
                    else
                    {
                        currentGameObject = null;
                        return;
                    }
                }
                else
                {
                    currentGameObject = m_Mhit.transform.gameObject;
                    return;
                }
            }
            else
            {
                currentGameObject = null;
            }
        }
    }
    #endregion

    #region 点击事件的探测
    private float clickTime = 0;
    private Vector2 clickPoint;
    /// <summary>
    /// 检测Click事件
    /// </summary>
    private void testClick()
    {

        //开始touch
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            // 点击2DUI则退出
            if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;
            clickTime = Time.time;
            clickPoint = Input.touches[0].position;
            //产生点击事件
            Messenger.Broadcast<GameObject, ClickState, Vector2>
                (MySkyInputEvent.EventClick, currentGameObject, ClickState.Down, Input.touches[0].position);

        }
        //结束拖动
        else if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Ended)
        {
            // 点击2DUI则退出
            if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;

            // 判断时间超时则退出
            if (Time.time - clickTime >= CLICK_OUT_TIME) return;

            // 判断位置超出则退出
            if (Vector2.Distance(clickPoint, Input.touches[0].position) >= CLICK_OUT_DISTANCE) return;


            //产生点击事件
            Messenger.Broadcast<GameObject, ClickState, Vector2>
                (MySkyInputEvent.EventClick, currentGameObject, ClickState.Up, Input.touches[0].position);
        }

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 点击2DUI则退出
                if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;
                clickTime = Time.time;
                clickPoint = Input.mousePosition;
                Messenger.Broadcast<GameObject, ClickState, Vector2>
                    (MySkyInputEvent.EventClick, currentGameObject, ClickState.Down, Input.mousePosition);
            }
            if (Input.GetMouseButtonUp(0))
            {

                if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
                    return;
                // 判断时间超时则退出
                if (Time.time - clickTime >= CLICK_OUT_TIME)
                    return;
                // 判断位置超出则退出
                if (Vector2.Distance(clickPoint, Input.mousePosition) >= CLICK_OUT_DISTANCE)
                    return;

                Messenger.Broadcast<GameObject, ClickState, Vector2>
                    (MySkyInputEvent.EventClick, currentGameObject, ClickState.Up, Input.mousePosition);
            }
        }
    }
    #endregion

    #region 双击事件的探测
    private float doubleClickTime = 0f;
    private bool doubleClickTest = false;
    private Vector2 doubleClickPoint;
    private Quaternion doubleClickAngle;
    private void testDoubleClick()
    {
        //开始touch
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            // 点击2DUI则退出
            if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;
            if (doubleClickTest)
            {
                // 判断位置超出则退出
                if (Vector2.Distance(doubleClickPoint, Input.touches[0].position) >= DOUBLECLICK_OUT_DISTANCE) return;

                // 判断摄影机移动太大则退出
                if (Quaternion.Angle(doubleClickAngle, m_camera.transform.rotation) >= DOUBLECLICK_OUT_ANGLE) return;
                //产生点击事件
                Messenger.Broadcast<GameObject, Vector2>(MySkyInputEvent.EventDoubleClick, currentGameObject, Input.touches[0].position);
            }
            doubleClickTime = Time.time;
            doubleClickTest = true;
            doubleClickPoint = Input.touches[0].position;
            doubleClickAngle = m_camera.transform.rotation;
        }
        else if (Time.time - doubleClickTime > 1f)
        {
            doubleClickTest = false;
        }
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 点击2DUI则退出
                if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;
                if (doubleClickTest)
                {
                    // 判断位置超出则退出
                    if (Vector2.Distance(doubleClickPoint, Input.mousePosition) >= DOUBLECLICK_OUT_DISTANCE) return;

                    // 判断摄影机移动太大则退出
                    if (Quaternion.Angle(doubleClickAngle, m_camera.transform.rotation) >= DOUBLECLICK_OUT_ANGLE) return;
                    //产生点击事件
                    Messenger.Broadcast<GameObject, Vector2>(MySkyInputEvent.EventDoubleClick, currentGameObject, Input.mousePosition);
                }
                doubleClickTime = Time.time;
                doubleClickTest = true;
                doubleClickPoint = Input.mousePosition;
                doubleClickAngle = m_camera.transform.rotation;
            }
            else if (Time.time - doubleClickTime > 1f)
            {
                doubleClickTest = false;
            }
        }
    }
    #endregion

    #region 长安事件
    private float longClickTime = 0f;
    private bool longClickTest = false;
    private Vector2 longClickPoint;
    private void testLongClick()
    {
        //开始touch
        text = "testLongClick";
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            // 点击2DUI则退出
            text = "Input.touchCount" + Input.touchCount;
            if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;
            longClickTest = true;
            longClickTime = Time.time;
            longClickPoint = Input.touches[0].position;
        }
        else if (longClickTest && Input.touchCount == 1)
        {
            text = "longClickTest===" + longClickTest;
            //如果按住的过程产生了滑动，则停止长按事件
            if (Vector2.Distance(longClickPoint, Input.touches[0].position) >= LONGCLICK_OUT_DISTANCE)
            {
                text = "Vector2.Distance===";
                longClickTest = false;
                return;
            }
            //时间没有到
            if (Time.time - longClickTime <= LONGCLICK_OUT_TIME) return;

            longClickTest = false;
            //产生点击事件
            //产生点击事件
            //if(currentGameObject!=null)
            text = "Broadcast===";
            Messenger.Broadcast<GameObject, Vector2>(MySkyInputEvent.EventLongClick, currentGameObject, Input.touches[0].position);
        }

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 点击2DUI则退出
                if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;
                longClickTest = true;
                longClickTime = Time.time;
                longClickPoint = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0) && longClickTest)
            {

                //如果按住的过程产生了滑动，则停止长按事件
                if (Vector2.Distance(longClickPoint, Input.mousePosition) >= LONGCLICK_OUT_DISTANCE)
                {
                    longClickTest = false;
                    return;
                }
                //时间没有到
                if (Time.time - longClickTime <= LONGCLICK_OUT_TIME) return;

                longClickTest = false;
                //if (currentGameObject != null)
                Messenger.Broadcast<GameObject, Vector2>(MySkyInputEvent.EventLongClick, currentGameObject, Input.mousePosition);
            }
        }
    }
    #endregion

    #region 拖动事件

    private Vector2 lastPoint;
    private Vector3 lastObjPoint;
    private bool m_isDrage = false;
    private int m_fingerId = 0;
    private void testDrag()
    {
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            //检测是否选中按钮等信息判断
            if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;

            m_isDrage = false;
            m_fingerId = Input.touches[0].fingerId;
            lastPoint = Input.touches[0].position;
            lastObjPoint = m_camera.ScreenToWorldPoint(new Vector3(lastPoint.x, lastPoint.y, m_Mhit.distance));
        }
        else if (Input.touchCount >= 1 && m_fingerId == Input.touches[0].fingerId &&
            (Input.touches[0].phase == TouchPhase.Moved ||
           Input.touches[0].phase == TouchPhase.Stationary))
        {
            if (m_isShieldUGUI && EventSystem.current != null &&
                EventSystem.current.currentSelectedGameObject != null) return;

            Vector2 temp = Input.touches[0].position - lastPoint;

            //滑动被启动
            if (!m_isDrage && Vector2.Distance(Input.touches[0].position, lastPoint) > 5)
            {
                m_isDrage = true;
                //产生点击事件
                Messenger.Broadcast<GameObject, DragState, Vector2, Vector2, Vector3>
                    (MySkyInputEvent.EventDrag, currentGameObject, DragState.Start, Input.touches[0].position, Vector2.zero, Vector3.zero);
                m_state = INPUT_STATE.DRAG;
                return;
            }

            if (!m_isDrage) return;

            //分析3D位移
            Vector3 screenPos = Vector3.one;
            Vector3 newObjPoint = m_camera.ScreenToWorldPoint(new Vector3(lastPoint.x, lastPoint.y, m_Mhit.distance));
            if (currentGameObject != null) screenPos = currentGameObject.transform.position;
            screenPos = screenPos + newObjPoint - lastObjPoint;

            Messenger.Broadcast<GameObject, DragState, Vector2, Vector2, Vector3>
                    (MySkyInputEvent.EventDrag, currentGameObject, DragState.OnDrag,
                    Input.touches[0].position, Input.touches[0].deltaPosition, screenPos);

            lastPoint = Input.touches[0].position;
            lastObjPoint = newObjPoint;
            m_state = INPUT_STATE.DRAG;
        }
        //当用户抬起时结束抬起事件。
        else if ((Input.touchCount >= 2 && 
            m_fingerId == Input.touches[0].fingerId &&
            Input.touches[0].phase == TouchPhase.Ended) ||
            (Input.touchCount == 1 &&
            Input.touches[0].phase == TouchPhase.Ended)
            )
        {
            Messenger.Broadcast<GameObject, DragState, Vector2, Vector2, Vector3>
                (MySkyInputEvent.EventDrag, currentGameObject, DragState.End, Input.touches[0].position,
                Input.touches[0].deltaPosition, Vector3.zero);
            m_state = INPUT_STATE.NORMAL;
        }

        //鼠标模拟
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (Input.GetMouseButtonDown(0))
            {
                //产生点击事件
                lastPoint = Input.mousePosition;
                lastObjPoint = m_camera.ScreenToWorldPoint(new Vector3(lastPoint.x, lastPoint.y, m_Mhit.distance));

            }
            else if (Input.GetMouseButton(0))
            {
                Vector2 temp = mousePosition - lastPoint;

                //滑动被启动
                if (!m_isDrage && Vector2.Distance(mousePosition, lastPoint) > 5)
                {
                    m_isDrage = true;
                    //产生点击事件
                    Messenger.Broadcast<GameObject, DragState, Vector2, Vector2, Vector3>
                        (MySkyInputEvent.EventDrag, currentGameObject, DragState.Start,
                        Input.mousePosition, Vector2.zero, Vector3.zero);
                    m_state = INPUT_STATE.DRAG;
                    return;
                }
                if (!m_isDrage) return;

                Vector3 screenPos = Vector3.one;
                Vector3 newObjPoint = m_camera.ScreenToWorldPoint(new Vector3(lastPoint.x, lastPoint.y, m_Mhit.distance));
                if (currentGameObject != null) screenPos = currentGameObject.transform.position;
                screenPos = screenPos + newObjPoint - lastObjPoint;

                Messenger.Broadcast<GameObject, DragState, Vector2, Vector2, Vector3>
                    (MySkyInputEvent.EventDrag, currentGameObject, DragState.OnDrag, mousePosition, temp, screenPos);

                lastPoint = Input.mousePosition;
                lastObjPoint = newObjPoint;
            }
            //当用户抬起时结束抬起事件。
            else if (Input.GetMouseButtonUp(0))
            {
                Vector2 temp = mousePosition - lastPoint;
                Messenger.Broadcast<GameObject, DragState, Vector2, Vector2, Vector3>
                    (MySkyInputEvent.EventDrag, currentGameObject, DragState.End,
                    mousePosition, temp, Vector3.zero);
                m_isDrage = false;
            }
        }

    }
    #endregion
    #region 滑动事件

    private Vector2 m_oldMousePosition;
    private bool m_isSlideDirection = false;
    private Vector2 m_SlideDirection_StartPosition;
    private GameObject m_SlideDirection_StartGameObject;
    private void testSlide()
    {

        if (Input.touchCount > 1)
        {
            Messenger.Broadcast<GameObject, SlideState, Vector2, Vector2>
                (MySkyInputEvent.EventSlide, currentGameObject, SlideState.End, Input.touches[0].position, Input.touches[0].position - m_oldMousePosition);
            return;
        }
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;
            //产生点击事件
            Messenger.Broadcast<GameObject, SlideState, Vector2, Vector2>
                (MySkyInputEvent.EventSlide, currentGameObject, SlideState.Start, Input.touches[0].position, Input.touches[0].deltaPosition);
            m_oldMousePosition = Input.touches[0].position;
            m_isSlideDirection = true;
            m_SlideDirection_StartPosition = Input.touches[0].position;
            m_SlideDirection_StartGameObject = currentGameObject;
        }
        else if (Input.touchCount == 1 && (Input.touches[0].phase == TouchPhase.Moved ||
            Input.touches[0].phase == TouchPhase.Stationary))
        {
            if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;

            //判断方向
            if (m_isSlideDirection && Vector2.Distance(m_SlideDirection_StartPosition, Input.touches[0].position) >= SLIDE_DIRECTION_DISTANCE)
            {
                Vector2 v = Input.touches[0].position - m_SlideDirection_StartPosition;
                Direction d;
                if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
                    d = v.x > 0 ? Direction.right : Direction.left;
                else
                    d = v.y > 0 ? Direction.up : Direction.down;
                Messenger.Broadcast<GameObject, Direction>(MySkyInputEvent.EventSlideDirection, m_SlideDirection_StartGameObject, d);
                m_isSlideDirection = false;
            }

            Messenger.Broadcast<GameObject, SlideState, Vector2, Vector2>
                (MySkyInputEvent.EventSlide, currentGameObject, SlideState.OnDrag, Input.touches[0].position, Input.touches[0].deltaPosition);
            m_oldMousePosition = Input.touches[0].position;
        }
        //当用户抬起时结束抬起事件。
        else if (Input.touchCount >= 1 && Input.touches[0].phase == TouchPhase.Ended)
        {
            Messenger.Broadcast<GameObject, SlideState, Vector2, Vector2>
                (MySkyInputEvent.EventSlide, currentGameObject, SlideState.End, Input.touches[0].position, Input.touches[0].position - m_oldMousePosition);
            m_oldMousePosition = Input.touches[0].position;
        }



        //鼠标模拟
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (Input.GetMouseButtonDown(0))
            {
                //产生点击事件
                lastPoint = Input.mousePosition;
                //if (currentGameObject != null)
                Messenger.Broadcast<GameObject, SlideState, Vector2, Vector2>
                    (MySkyInputEvent.EventSlide, currentGameObject, SlideState.Start, Input.mousePosition, Vector2.zero);
                m_oldMousePosition = mousePosition;
                m_isSlideDirection = true;
                m_SlideDirection_StartGameObject = currentGameObject;
                m_SlideDirection_StartPosition = mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                if (m_isShieldUGUI && EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null) return;

                if (Vector2.Distance(Input.mousePosition, lastPoint) <= 0) return;

                //判断方向
                if (m_isSlideDirection && Vector2.Distance(m_SlideDirection_StartPosition, m_oldMousePosition) >= SLIDE_DIRECTION_DISTANCE)
                {
                    Vector2 v = m_oldMousePosition - m_SlideDirection_StartPosition;
                    Direction d;
                    if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
                        d = v.x > 0 ? Direction.right : Direction.left;
                    else
                        d = v.y > 0 ? Direction.up : Direction.down;
                    Messenger.Broadcast<GameObject, Direction>(MySkyInputEvent.EventSlideDirection, m_SlideDirection_StartGameObject, d);
                    m_isSlideDirection = false;
                }
                Messenger.Broadcast<GameObject, SlideState, Vector2, Vector2>
                    (MySkyInputEvent.EventSlide, currentGameObject, SlideState.OnDrag, mousePosition, mousePosition - m_oldMousePosition);
            }
            //当用户抬起时结束抬起事件。
            else if (Input.GetMouseButtonUp(0))
            {
                Messenger.Broadcast<GameObject, SlideState, Vector2, Vector2>
                    (MySkyInputEvent.EventSlide, currentGameObject, SlideState.End, mousePosition, mousePosition - m_oldMousePosition);
            }
            m_oldMousePosition = mousePosition;
        }
    }
    #endregion

    #region 两个手指拖动事件

    private bool doubleDragTest = false;
    private Vector2 doubleDragOldCenter;
    private Vector2[] doubleDragPoint = new Vector2[2];
    private float doubleDragDistance;


    /// <summary>
    /// 检测双手拖动状态
    /// </summary>
    /// <returns>该事件是否发生过</returns>
    private bool testDoubleDrag()
    {
        //如果是非正常装填则跳出检测
        if (m_state != INPUT_STATE.NORMAL
            && m_state != INPUT_STATE.DOUBLE_DARG
            && m_state != INPUT_STATE.DRAG
        ) return false;


        if (Input.touchCount == 2)
        {
            m_state = INPUT_STATE.DOUBLE_DARG;
            //当前触摸点位置
            Vector2[] dp = new Vector2[2];
            dp[0] = Input.touches[0].position;
            dp[1] = Input.touches[1].position;
            Vector2 center = Input.touches[1].position + ((Input.touches[0].position - Input.touches[1].position) * 0.5f);
            float distance = Vector2.Distance(dp[0], dp[1]);

            if (!doubleDragTest)
            {
                //开始拖动
                //if(currentGameObject!=null)
                Messenger.Broadcast<GameObject, DragState, Vector2[], Vector2, float, float>(MySkyInputEvent.EventDoubleDrag, currentGameObject, DragState.Start, null, Vector2.zero, 0, 0);
                doubleDragTest = true;
            }
            //拖动过程中
            else
            {
                //检测上一次中心点的位置。
                //if(currentGameObject!=null)
                Messenger.Broadcast<GameObject, DragState, Vector2[], Vector2, float, float>(MySkyInputEvent.EventDoubleDrag, currentGameObject, DragState.OnDrag, dp, center - doubleDragOldCenter, distance - doubleDragDistance, Vector2.Angle(doubleDragPoint[0] - doubleDragPoint[1], Input.touches[0].position - Input.touches[1].position));
            }
            doubleDragPoint[0] = Input.touches[0].position;
            doubleDragPoint[1] = Input.touches[1].position;
            doubleDragOldCenter = center;
            doubleDragDistance = distance;
            return true;
        }
        //抬起事件
        else if (doubleDragTest)
        {
            //if(currentGameObject!=null)
            Messenger.Broadcast<GameObject, DragState, Vector2[], Vector2, float, float>(MySkyInputEvent.EventDoubleDrag, currentGameObject, DragState.End, null, Vector2.zero, 0, 0);
            doubleDragTest = false;
            m_state = INPUT_STATE.NORMAL;
            currentGameObject = null;
            return true;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (currentGameObject != null)
                    Messenger.Broadcast<GameObject, DragState, Vector2[], Vector2, float, float>(MySkyInputEvent.EventDoubleDrag, currentGameObject, DragState.Start, null, Vector2.zero, 0, 0);

            }
            else if (Input.GetMouseButton(1))
            {
                float scale = Input.GetAxis("Mouse ScrollWheel");
                Vector2[] dp = new Vector2[2];
                dp[0] = Input.mousePosition;
                dp[1] = Input.mousePosition;
                if (currentGameObject != null)
                    Messenger.Broadcast<GameObject, DragState, Vector2[], Vector2, float, float>(MySkyInputEvent.EventDoubleDrag, currentGameObject, DragState.OnDrag, dp, Vector2.zero, scale, 0);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                if (currentGameObject != null)
                    Messenger.Broadcast<GameObject, DragState, Vector2[], Vector2, float, float>(MySkyInputEvent.EventDoubleDrag, currentGameObject, DragState.End, null, Vector2.zero, 0, 0);

            }
        }
        return false;
    }
    #endregion


    #region 用陀螺仪判断角度变化，触发类似砸锤子事件，在电脑上用鼠标右键实现
    private Vector3 lastRotation = Vector3.zero;
    private float AngleHitTime = 0;
    public List<float> angles = new List<float>();
    private void testAngleHit()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Messenger.Broadcast(MySkyInputEvent.EventAngleHit);
            }
            return;
        }

        //TODO 判断陀螺仪是否生效
        //if ( MySkyGyroController.instance == null || !MySkyGyroController.instance.gyroEnabled)
        //	return;
        if (lastRotation == Vector3.zero)
            lastRotation = m_camera.gameObject.transform.rotation.eulerAngles;
        Vector3 curRotation = m_camera.gameObject.transform.rotation.eulerAngles;
        if (AngleHitTime != 0 && Time.time - AngleHitTime <= HIT_OUT_TIME)
        {
            lastRotation = curRotation;
            return;
        }
        else
            AngleHitTime = 0;

        float angle = Quaternion.Angle(Quaternion.Euler(new Vector3(curRotation.x, 0, 0)),
                          Quaternion.Euler(new Vector3(lastRotation.x, 0, 0))
                      );

        Vector3 acceleration = Input.acceleration - Input.gyro.gravity;

        if ((curRotation.x > 30f && curRotation.x < 90f) &&
            (angle > 75 * Time.deltaTime || Mathf.Abs(acceleration.z) >= 2.0f)
        )
        {
            Messenger.Broadcast(MySkyInputEvent.EventAngleHit);
            AngleHitTime = Time.time;
        }
        lastRotation = curRotation;
    }
    #endregion

    //	private float old_acceleration_x = 0;
    //	private float old_acceleration_y = 0;
    //	private float old_acceleration_z = 0;
    //	public void OnGUI()
    //	{
    //		Vector3 acceleration = Input.acceleration - Input.gyro.gravity;
    //		if (old_acceleration_x <= Mathf.Abs(acceleration.x))
    //			old_acceleration_x = Mathf.Abs(acceleration.x);
    //		if (old_acceleration_y <= Mathf.Abs(acceleration.y))
    //			old_acceleration_y = Mathf.Abs(acceleration.y);
    //		if (old_acceleration_z <= Mathf.Abs(acceleration.z))
    //			old_acceleration_z = Mathf.Abs(acceleration.z);
    //
    //		GUIStyle st = new GUIStyle ();
    //		st.fontSize = 40;
    //		if(GUI.Button(new Rect(0,0,200,100),"重置",st))
    //		{
    //			old_acceleration_x = 0;
    //			old_acceleration_y = 0;
    //			old_acceleration_z = 0;
    //		}
    //		Vector3 r = MySkyManager.instance.cameraManager.m_transform.rotation.eulerAngles;
    //		GUI.Label (new Rect (0, 100, Screen.width, Screen.height),
    //			"x:" + old_acceleration_x
    //			+"y:" + old_acceleration_y +
    //			"z:" +old_acceleration_z+
    //			"\n r x:"+r.x+"y:"+r.y+"z:"+r.z,st);
    //	}

    #region 用加速器判断向前力度变化，触发类似砸锤子事件，在电脑上用鼠标滚轮实现
    private float AccelerationHitTime = 0;
    private void testAccelerationHit()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (AccelerationHitTime != 0 && Time.time - AccelerationHitTime <= HIT_OUT_TIME)
                return;
            else
                AccelerationHitTime = 0;
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Messenger.Broadcast(MySkyInputEvent.EventAccelerationHit);
                AccelerationHitTime = Time.time;
            }
            return;
        }

        if (AccelerationHitTime != 0 && Time.time - AccelerationHitTime <= HIT_OUT_TIME)
            return;
        else
            AccelerationHitTime = 0;

        for (int j = 0; j < Input.accelerationEventCount; j++)
        {
            Vector3 a = Input.accelerationEvents[j].acceleration;
            if (a.z > 1)
            {
                Messenger.Broadcast(MySkyInputEvent.EventAccelerationHit);
                AccelerationHitTime = Time.time;
                break;
            }
        }
    }
    #endregion
}
