using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(SUIButton))]
public class Mingyang_ButtonEditor : Editor
{
    private bool isAnim;
    private string[] options = { "无动画", "脚本动画", "U3D1.5动画系统" };
    private string[] options1 = { "脚本动画001", "脚本动画002" };
    private RuntimeAnimatorController runtimeAnimatorController;

    private SUIButton m_SUIButton;


 
    private int idex;
    public override void OnInspectorGUI()
    {
        m_SUIButton = (SUIButton)target;
        m_SUIButton.m_EffectTime = EditorGUILayout.FloatField("特效播放时间： ", m_SUIButton.m_EffectTime);
        m_SUIButton.m_LoseMessage = EditorGUILayout.TextField("提示信息：", m_SUIButton.m_LoseMessage);



        m_SUIButton.m_MyUIevent = (MyUIevent)EditorGUILayout.ObjectField("hiahia", m_SUIButton.m_MyUIevent, typeof(MyUIevent));
        if (m_SUIButton.m_MyUIevent != null)
        {

            m_SUIButton.m_SceneID = EditorGUILayout.Popup("选择场景：", m_SUIButton.m_SceneID, m_SUIButton.m_MyUIevent.MagEvnet);
            switch (m_SUIButton.m_SceneID)
            {
                case 0:
                     m_SUIButton.msgID = EditorGUILayout.Popup("按钮事件：", m_SUIButton.msgID, m_SUIButton.m_MyUIevent.HomePageMagEvnet);
                    EditorGUILayout.HelpBox(m_SUIButton.m_MyUIevent.HomePageMagEvnet[m_SUIButton.msgID], MessageType.None);
                    m_SUIButton.m_ButtonMsg = m_SUIButton.m_MyUIevent.HomePageMagEvnet[m_SUIButton.msgID];
                    break;
                case 1:
                    m_SUIButton.msgID = EditorGUILayout.Popup("按钮事件：", m_SUIButton.msgID, m_SUIButton.m_MyUIevent.PhotoMagEvnet);
                    EditorGUILayout.HelpBox(m_SUIButton.m_MyUIevent.PhotoMagEvnet[m_SUIButton.msgID], MessageType.None);
                    m_SUIButton.m_ButtonMsg = m_SUIButton.m_MyUIevent.PhotoMagEvnet[m_SUIButton.msgID];
                    break;
                case 2:
                    m_SUIButton.msgID = EditorGUILayout.Popup("按钮事件：", m_SUIButton.msgID, m_SUIButton.m_MyUIevent.csMagEvnet);
                    EditorGUILayout.HelpBox(m_SUIButton.m_MyUIevent.csMagEvnet[m_SUIButton.msgID], MessageType.None);
                    m_SUIButton.m_ButtonMsg = m_SUIButton.m_MyUIevent.csMagEvnet[m_SUIButton.msgID];
                    break;
                
            }
        }
        EditorGUILayout.HelpBox("------------------以下控制动画----------------------",MessageType.None);

     // isAnim=EditorGUILayout.Toggle("无动画", isAnim);
        m_SUIButton.AnimationType = EditorGUILayout.Popup("选择动画：",m_SUIButton.AnimationType, options);
        EditorGUILayout.HelpBox(m_SUIButton.AnimationType.ToString(), MessageType.None);
        switch (m_SUIButton.AnimationType)
        {
            case 0:
                break;
            case 1:
                  m_SUIButton.direction = (SUIButton.Direction)EditorGUILayout.EnumPopup("所在方向", m_SUIButton.direction);
                  m_SUIButton.ScriptType = EditorGUILayout.Popup("选择动画：", m_SUIButton.ScriptType, options1);
                  EditorGUILayout.HelpBox(m_SUIButton.ScriptType.ToString(), MessageType.None);
                break;
               
            default:
                runtimeAnimatorController = (RuntimeAnimatorController)EditorGUILayout.ObjectField("动画控制器", runtimeAnimatorController, typeof(RuntimeAnimatorController));
                break;
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }


    }

	
}
