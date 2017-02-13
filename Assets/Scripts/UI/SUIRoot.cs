using UnityEngine;
using System.Collections;

public class SUIRoot : object {
    private SUIView m_uiView = null;

#region 基础操作
    public SUIRoot()
    {

    }
    public void Hide()
    {
        if (m_uiView != null) m_uiView.gameObject.SetActive(false);
    }
    public void Show()
    {
        if (m_uiView != null) m_uiView.gameObject.SetActive(true);
    }
    public void onDestroy()
    {
        RemoveView();
    }
#endregion

#region View设置
    //设置当前View
    public void SetView(SUIView _view)
    {
        if (m_uiView != null) MonoBehaviour.Destroy(m_uiView.gameObject);
        m_uiView = _view;
        m_uiView.transform.SetParent(SUIManager.instance.uiViewManager.transform);
    }

    // 获取当前View  如果当前View没有被赋值，那么自动创建一个View
    public SUIView GetUIView()
    {
        if (m_uiView == null)
        {
            m_uiView = SUIView.Create();
            m_uiView.transform.SetParent(SUIManager.instance.uiViewManager.transform);
            m_uiView.transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
            m_uiView.transform.GetComponent<RectTransform>().localScale = Vector3.one;
            m_uiView.transform.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }
        return m_uiView;
    }

    public void RemoveView()
    {
        MonoBehaviour.Destroy(m_uiView.gameObject);
    }
#endregion

#region 警示提示内容

    /// <summary>
    /// 提示框
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="message">提示内容</param>
    /// <param name="but1Name">按钮1名称</param>
    /// <param name="but1CallBack">回调函数</param>
    /// <param name="but2Name">按钮2名称</param>
    /// <param name="but2CallBack">按钮2的回调函数</param>
    /// <param name="isShowExite">是否显示退出按钮</param>
    public int ShowAlert(string title, string message,
        string but1Name, Callback but1CallBack,
        string but2Name = null, Callback but2CallBack = null, bool isShowExite = false)
    {
        return SUIManager.instance.alert.ShowAlert(this, title, message, but1Name, but1CallBack, but2Name, but2CallBack, isShowExite);
    }

    public void CloseAlert(int _id)
    {
        SUIManager.instance.alert.Close(this, _id);
    }

    public void CloseAllAlert()
    {
        SUIManager.instance.alert.CloseAll();
    }

#endregion

#region 显示提示内容

    public void ShowReminder(string message, float showTime = 2)
    {
        SUIManager.instance.reminder.ShowReminder(message, showTime);
    }
    public void ColseReminder()
    {
        SUIManager.instance.reminder.ColseReminder();
    }

    public int ShowReminderKeep(string message)
    {
        return SUIManager.instance.reminder.ShowReminderKeep(message);
    }

    public void ColseReminderKeep(int id = -1)
    {
        SUIManager.instance.reminder.ColseReminderKeep(id);
    }





#endregion
}
