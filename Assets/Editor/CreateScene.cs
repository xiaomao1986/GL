using UnityEngine;
using System.Collections;
using UnityEditor;


public class CreateScene : MonoBehaviour
{
    [MenuItem("Custom Editor/CreateScene")]
    static void CreateSceneALL()
    {
        //清空一下缓存
        Caching.CleanCache();
        string Path = Application.dataPath + "/MyScene.unity3d";
        string[] levels = { "Assets/UI.unity", "Assets/Photo.unity" };
        //打包场景
        BuildPipeline.BuildPlayer(levels, Path, BuildTarget.WebPlayer, BuildOptions.BuildAdditionalStreamedScenes);
        AssetDatabase.Refresh();
    }


}
