using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(ListTester))]
public class ListTesterInspector : Editor
{
    public int notAList;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

      //  EditorList.Show(serializedObject.FindProperty("integers"), EditorListOption.Buttons);
  
        EditorList.Show(serializedObject.FindProperty("vectors"));
        EditorList.Show(
            serializedObject.FindProperty("objects"),
            EditorListOption.ListLabel | EditorListOption.Buttons);
        serializedObject.ApplyModifiedProperties();
    }

}
