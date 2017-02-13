using UnityEngine;
using System.Collections;
using System.IO;

public class FileConfig
{

    public static string GetPath(string Path)
    {
        string imagePath = "";
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			imagePath = Application.persistentDataPath + Path;
		else if (Application.platform == RuntimePlatform.WindowsPlayer)
			imagePath = Application.dataPath + Path;
		else if (Application.platform == RuntimePlatform.WindowsEditor) {
			imagePath = Application.dataPath + Path;
			imagePath = imagePath.Replace ("/Assets", null);
		} else {
			imagePath = Application.persistentDataPath + Path;
		}
        return imagePath;
    }


    //文件保存地址
    public static readonly string PathURL =
#if UNITY_ANDROID   //安卓
        "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE  //iPhone
        Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台
        "file://" + Application.dataPath + "/StreamingAssets/";
#else
 string.Empty;
#endif

    public static string GetPath()
    {
        string path = "";
        if (Application.platform == RuntimePlatform.Android ||
		    Application.platform == RuntimePlatform.IPhonePlayer) {
			path = Application.persistentDataPath;
		} else if (Application.platform == RuntimePlatform.WindowsPlayer) {
			path = Application.persistentDataPath;
		} else if (Application.platform == RuntimePlatform.WindowsEditor) {
			path = Application.dataPath;
		} else {
			path = Application.persistentDataPath;
		}
        return path;
    }

    public static readonly string img_file_raw = Application.persistentDataPath + "/Resources/raw/";
    public static readonly string img_file_big = Application.persistentDataPath + "/Resources/big/";
    public static readonly string img_file_centre = Application.persistentDataPath + "/Resources/centre/";
    public static readonly string img_file_little = Application.persistentDataPath + "/Resources/little/";
    public static readonly string img_file_tiny = Application.persistentDataPath + "/Resources/tiny/";

    public static void createGameFile()
    {
        try
        {
            if (!Directory.Exists(img_file_raw)) Directory.CreateDirectory(img_file_raw);
            if (!Directory.Exists(img_file_big)) Directory.CreateDirectory(img_file_big);
            if (!Directory.Exists(img_file_centre)) Directory.CreateDirectory(img_file_centre);
            if (!Directory.Exists(img_file_little)) Directory.CreateDirectory(img_file_little);
            if (!Directory.Exists(img_file_tiny)) Directory.CreateDirectory(img_file_tiny);
        }
        catch (System.Exception e)
        {
            Debug.LogError("createGameFile:" + e.ToString() + "###EEEEEEE###  ");
        }
    }
}
