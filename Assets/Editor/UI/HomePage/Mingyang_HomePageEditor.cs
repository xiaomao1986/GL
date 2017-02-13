using UnityEngine;
using System.Collections;
using UnityEditor;
using LitJson;
[CustomEditor(typeof(HomePageMag))]
public class Mingyang_HomePageEditor : UISetConfig 
{
    private HomePageMag m_HomePageMag;

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        if (GUILayout.Button("生成 预设 与 Json", GUILayout.Width(255)))
        {
            m_HomePageMag = Selection.activeGameObject.GetComponent<HomePageMag>();
            CreateJson(Selection.activeGameObject, m_HomePageMag.jsonDataName, m_HomePageMag.ParentPathName);

        }
        if (GUILayout.Button("生成 Menu 样例", GUILayout.Width(255)))
        {
            m_HomePageMag = Selection.activeGameObject.GetComponent<HomePageMag>();
            Debug.Log("m_HomePageMag" + m_HomePageMag.name);
            TextAsset MenuJson = (TextAsset)Resources.Load(m_HomePageMag.jsonDataName);
            m_HomePageMag.LoadHomePageJsonConfig(MenuJson.text);
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
