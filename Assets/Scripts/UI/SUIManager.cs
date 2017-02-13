using UnityEngine;
using System.Collections;


public class SUIManager : MonoBehaviour {

    //单例对象
    private static SUIManager m_instance;
    public static SUIManager instance
    {
        get { return m_instance; }
    }

    //提示信息
    private SUIReminder m_reminder = new SUIReminder();
    public SUIReminder reminder
    {
        get { return m_reminder; }
    }

    //确定提示
    private SUIAlert m_alert = new SUIAlert();

    public SUIAlert alert
    {
        get { return m_alert; }
        set { m_alert = value; }
    }


    public GameObject g; 

    //2D UIView 管理器对象
    private GameObject m_uiViewManager;

    public GameObject uiViewManager
    {
        get { return m_uiViewManager; }
    }


    void Awake()
    {
        m_instance = this;

        //从预设体中加载UI资源
        g = (GameObject)MonoBehaviour.Instantiate(Resources.Load(MySkyConfig.UI_MANAGER_RESOURCE_MAINCANVAS));
        if (g == null) Debug.LogError(MySkyConfig.UI_MANAGER_RESOURCE_MAINCANVAS+"找不到该对象");

        //加载提示信息资源
        GameObject g_reminder = g.transform.FindChild(MySkyConfig.UI_MANAGER_RESOURCE_REMINDER).gameObject;
        if (g_reminder == null) Debug.LogError(MySkyConfig.UI_MANAGER_RESOURCE_MAINCANVAS+"/"+MySkyConfig.UI_MANAGER_RESOURCE_REMINDER + "找不到该对象");

        //加载警告信息资源
        GameObject g_reminderKeep = g.transform.FindChild(MySkyConfig.UI_MANAGER_RESOURCE_REMINDERKEEP).gameObject;
        if (g_reminderKeep == null) Debug.LogError(MySkyConfig.UI_MANAGER_RESOURCE_MAINCANVAS + "/" + MySkyConfig.UI_MANAGER_RESOURCE_REMINDERKEEP + "找不到该对象");

        //创建UIViewManager对象物体
        m_uiViewManager = new GameObject();
        m_uiViewManager.AddComponent<RectTransform>();
        m_uiViewManager.name = MySkyConfig.UI_MANAGER_VIEWMANAGER_NAME;
        m_uiViewManager.transform.SetParent(g.transform);
        RectTransform viewManagerRect = m_uiViewManager.GetComponent<RectTransform>();
        viewManagerRect.localPosition = Vector3.zero;
        viewManagerRect.localScale = Vector3.one;
        viewManagerRect.sizeDelta = Vector2.zero;
        viewManagerRect.anchorMin = Vector2.zero;
        viewManagerRect.anchorMax = Vector2.one;
        m_reminder.SetGameObject(g_reminder,g_reminderKeep);

        //创建提示物体
        GameObject g_alert = g.transform.FindChild(MySkyConfig.UI_MANAGER_RESOURCE_ALERT).gameObject;
        if (g_alert == null) Debug.LogError(MySkyConfig.UI_MANAGER_RESOURCE_MAINCANVAS + "/" + MySkyConfig.UI_MANAGER_RESOURCE_ALERT + "找不到该对象");
        g_alert.SetActive(false);
        m_alert.SetGameObject(g_alert);

    }

    private IEnumerator TimerDelegate(float time)
    {
        yield return new WaitForSeconds(time);
    }

    //private void OnGUI()
    //{
    //    if(GUI.Button(new Rect(0,0,100,80),""))
    //    {
    //        PhotoScene.Instance.uiRoot.ShowAlert("1", "asdfasdfasdfasdfadsfasdf", "3", delegate() { }, null, null);
    //    }
    //}
}
