using UnityEngine;
using UnityEditor;
using System.Collections;
using LitJson;

[CustomEditor(typeof(SUIMenu))]
public class Mingyang_MenuEditor : Editor
{
    private SUIMenu m_SUIMenu;
    private string[] options = { "无动画", "脚本动画", "U3D1.5动画系统" };
    private string[] options1 = { "脚本动画001", "脚本动画002" };
    private RuntimeAnimatorController runtimeAnimatorController;
    public override void OnInspectorGUI()
    {
       base.OnInspectorGUI();

        m_SUIMenu = (SUIMenu)target;
        if (GUILayout.Button("生成 预设 与 Json", GUILayout.Width(255)))
        {
            bool isDisplayDialog = UnityEditor.EditorUtility.DisplayDialog("生成 配置文件和预设", "生成 不要操作", "ok");
            m_SUIMenu = Selection.activeGameObject.GetComponent<SUIMenu>();
            CreateJson(Selection.activeGameObject,m_SUIMenu.jsonDataName, m_SUIMenu.ParentPathName);

        }
        if (GUILayout.Button("生成 Menu 样例", GUILayout.Width(255)))
        {
            m_SUIMenu = Selection.activeGameObject.GetComponent<SUIMenu>();
            TextAsset MenuJson = (TextAsset)Resources.Load("SUIMenuData");
            m_SUIMenu.LoadMenuJsonConfig(MenuJson.text);
        }

        EditorGUILayout.HelpBox("------------------以下控制动画----------------------", MessageType.None);

        // isAnim=EditorGUILayout.Toggle("无动画", isAnim);
        m_SUIMenu.AnimationType = EditorGUILayout.Popup("选择动画：", m_SUIMenu.AnimationType, options);
        EditorGUILayout.HelpBox(m_SUIMenu.AnimationType.ToString(), MessageType.None);
        switch (m_SUIMenu.AnimationType)
        {
            case 0:
                break;
            case 1:
                m_SUIMenu.direction = (SUIButton.Direction)EditorGUILayout.EnumPopup("所在方向", m_SUIMenu.direction);
                m_SUIMenu.ScriptType = EditorGUILayout.Popup("选择动画：", m_SUIMenu.ScriptType, options1);
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

    public static string CreateJson(GameObject obj,string jsonDataName,string ParentPath)
    {
        
      //  My_UIEditorToos.InitProgress(obj.transform.GetChildCount());

        JsonData jd = new JsonData();
        JsonData RootpositionJson = new JsonData();
        JsonData RootsizeDeltaJson = new JsonData();
        JsonData jd2 = new JsonData();
        int i = 0;
        foreach (Transform item in obj.transform)
        {
            JsonData tmp = new JsonData();
            JsonData TransformJson = new JsonData();
            JsonData positionJson = new JsonData();
            JsonData sizeDeltaJson = new JsonData();
            i++;
            tmp["PrefabName"] = item.name;
            positionJson["x"] = item.GetComponent<RectTransform>().localPosition.x;
            positionJson["y"] = item.GetComponent<RectTransform>().localPosition.y;
            positionJson["z"] = item.GetComponent<RectTransform>().localPosition.z;
            sizeDeltaJson["w"] = item.GetComponent<RectTransform>().sizeDelta.x;
            sizeDeltaJson["h"] = item.GetComponent<RectTransform>().sizeDelta.y;
            tmp["position"] = positionJson;
            tmp["sizeDelta"] = sizeDeltaJson;
            if ( item.GetComponent<UIAnimation>()!=null)
            {
                tmp["AnimationType"] = item.GetComponent<UIAnimation>().AnimationType;
                tmp["ScriptType"] = item.GetComponent<UIAnimation>().ScriptType;
                tmp["Direction"] = item.GetComponent<UIAnimation>().direction.ToString();
            }
            if ( item.GetComponent<SUIButton>()!=null)
            {
                string s = item.GetComponent<SUIButton>().m_ButtonMsg.ToString();
                tmp["ButtonEvent"] = ""+s;
            }else
                 tmp["ButtonEvent"] = "";
            jd2.Add(tmp);
          //  My_UIEditorToos.Progress();
            MyParent.make(item.gameObject, "Assets/Resources/UI/" + ParentPath + "/" + item.name + ".prefab");
        }
        RootpositionJson["x"] = obj.GetComponent<RectTransform>().localPosition.x;
        RootpositionJson["y"] = obj.GetComponent<RectTransform>().localPosition.y;
        RootpositionJson["z"] = obj.GetComponent<RectTransform>().localPosition.z;
        RootsizeDeltaJson["w"] = obj.GetComponent<RectTransform>().sizeDelta.x;
        RootsizeDeltaJson["h"] = obj.GetComponent<RectTransform>().sizeDelta.y;
        jd["parentURL"] = ParentPath;
        jd["Name"] = obj.name;
        jd["position"] = RootpositionJson;
        jd["sizeDelta"] = RootsizeDeltaJson;
        jd["Child"] = jd2;
        Debug.Log(jd.ToJson());
        string filepath = Application.dataPath + "/Resources";

        My_UIEditorToos.CreateFile(filepath, jsonDataName, jd);
        return filepath;
    }


   


}
