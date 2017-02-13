using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using UnityEditor;
public class My_UIEditorToos
{
    public static void CreateFile(string path, string name, JsonData jd)
    {
        try
        {
            //文件流信息
            StreamWriter sw;
            FileInfo t = new FileInfo(path + "/" + name + ".txt");
            if (!t.Exists)
            {
                //如果此文件不存在则创建
                sw = t.CreateText();
            }
            else
            {
                //如果此文件存在则打开
                FileStream fs = new FileStream(path + "/" + name + ".txt", FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);
            }
            sw.Flush();
            //以行的形式写入信息
            sw.Write(jd.ToJson());
            //关闭流
            sw.Close();
            //销毁流
            sw.Dispose();
           // My_UIEditorToos.Progress();
        }
        catch (System.Exception e)
        {

        }
    }

    public static void InitProgress(int k)
    {
        progress = 0;
        startVal = k;
    }


    static float secs = 1f;
    static int startVal = 0;
    static float progress = 0;
    public static void Progress()
    {
        if (progress < secs)
        {
            EditorUtility.DisplayProgressBar("Simple Progress Bar", "Shows a progress bar for the given seconds", progress);
        }
        else
        {
            EditorUtility.ClearProgressBar();
            bool isDisplayDialog = UnityEditor.EditorUtility.DisplayDialog("生成 配置文件和预设", "完成。。。。。", "ok");
        }
        float t = (float)1 / startVal;
        progress += t;

    }


}
