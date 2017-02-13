using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// SUIReminder 用于开发
/// </summary>
public class SUIReminder {

    //提示对象
    private GameObject m_reminderObject;
    private Text m_reminderText;
    private GameObject m_reminderKeepObject;
    private Text m_reminderKeepText;

    private Animation m_aniKeep;
    private Animation m_aniNothing;

	//动画是否播放
	private bool m_isAniKeepPlay = false;
	private bool m_isShowReminderKeep = false;
    private bool m_isShowReminder = false;

    //保持的信息显示字典
    private Dictionary<int, string> m_reminderItemList = new Dictionary<int, string>();

    //id自增长值
    private int reminderItemID;

    public SUIReminder()
    {
        reminderItemID = 0;
    }

    /// <summary>
    /// 设置GameObject  主要供UImanager调用
    /// </summary>
    /// <param name="g"></param>
    public void SetGameObject(GameObject reminderObject, GameObject reminderKeepObject)
    {
        m_reminderObject = reminderObject;
        m_reminderKeepObject = reminderKeepObject;

        m_reminderObject.SetActive(false);
        m_reminderKeepObject.SetActive(false);

        m_reminderText = m_reminderObject.transform.FindChild("Text").gameObject.GetComponent<Text>();
        m_reminderKeepText = m_reminderKeepObject.transform.FindChild("Text").gameObject.GetComponent<Text>();

        m_aniKeep = m_reminderKeepObject.GetComponent<Animation>();
        m_aniNothing = m_reminderObject.GetComponent<Animation>();

        //启动update
        SUIManager.instance.StartCoroutine(update());
    }


#region 提示框显示

    /// <summary>
    /// 显示提示信息
    /// </summary>
    /// <param name="message">提示信息的内容</param>
    /// <param name="showTime">显示多久后被关闭</param>
    /// <returns>该提示信息的id</returns>
    public void ShowReminder(string message, float showTime = 2)
    {
        SetReminderText(message);
        m_showReminderColseTime = Time.time+showTime;
    }

    private float m_showReminderColseTime = 0f;

    private IEnumerator update()
    {
        while(true)
        {
            yield return new WaitForFixedUpdate();
            //当显示超时则关闭显示内容
			if(Time.time >= m_showReminderColseTime && m_isShowReminder)
            {
                m_isShowReminder = false;
				m_aniNothing.Play("ReminderOut2");
            }

            if (!m_isShowReminder && m_aniNothing.isPlaying == false)
			{
                m_reminderText.text = "";
                m_reminderObject.SetActive(false);
			}
            //保持提示关闭状态检测
			if(m_isAniKeepPlay && !m_isShowReminderKeep)
			{
                m_aniKeep.Play("ReminderOut");
				m_isAniKeepPlay = false;
			}
			//当动画播放完毕则关闭提示框
            if (!m_isShowReminderKeep && m_aniKeep.isPlaying == false)
			{
				m_reminderKeepText.text = "";
				m_reminderKeepObject.SetActive (false);
			}
        }
    }

    private void SetReminderText(string message)
    {
        m_reminderObject.SetActive(true);
        m_reminderText.text = message;
        m_aniNothing.Play("ReminderIn2");
        m_isShowReminder = true;
    }


#endregion

#region 保持性提示框

    /// <summary>
    /// 显示提示信息   只有手动关闭
    /// </summary>
    /// <param name="message">提示信息的内容</param>
    /// <returns>该提示信息的id</returns>
    public int ShowReminderKeep(string message)
    {
		m_isShowReminderKeep = true;
        int id = reminderItemID++;
        SetReminderKeepText(message);
        return id;
    }
    public void ColseReminderKeep(int id = -1)
    {
		if (m_isShowReminderKeep) {
			m_isAniKeepPlay = true;
		}
		m_isShowReminderKeep = false;
    }
    public void ColseReminder()
    {
        m_showReminderColseTime = Time.time;
        m_isShowReminder = false;
    }



    private void SetReminderKeepText(string message)
    {
        m_reminderKeepObject.SetActive(true);
        m_reminderKeepText.text = message;
        m_aniKeep.Play("ReminderIn");
    }

#endregion

}
