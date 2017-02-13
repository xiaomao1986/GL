using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using UnityEditor;
public class MyParent : MonoBehaviour {

    public static void make(GameObject obj, string path)
    {
        string PrefabName=obj.name;
       // string path = "Assets/Resources/UI/SuiMenu/" + PrefabName + ".prefab";
        makePrefab(path, obj);
    }
    public static void makePrefab(string path, GameObject obj)
    {
        makeParentDirExist(path);
        Object prefab = PrefabUtility.CreateEmptyPrefab(path);
        PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }
    public static void makeParentDirExist(string path)
    {
        string pDir = getParentPath(path);

        if (!AssetDatabase.IsValidFolder(pDir))
        {
            makeParentDirExist(pDir);
            AssetDatabase.CreateFolder(getParentPath(pDir), getParentName(path));
            AssetDatabase.Refresh();
        }
        else
        {
            return;
        }
    }
    public static string getParentName(string path, char splitFlag = '/')
    {
        string[] dirs = path.Split(splitFlag);
        if (dirs.Length < 2)
        {
            return "";
        }
        else
        {
            return dirs[dirs.Length - 2];
        }
    }
    public static string getParentPath(string path, char splitFlag = '/')
    {
        string[] dirs = path.Split(splitFlag);
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < dirs.Length - 1; ++i)
        {
            str.Append(dirs[i]).Append(splitFlag);
        }
        //cut down the last splitFlag  
        string result = str.ToString();
        if (result.EndsWith(splitFlag.ToString()))
        {
            result = result.Substring(0, result.Length - 1);
        }
        return result;
    }
}
