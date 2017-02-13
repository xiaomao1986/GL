using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
public class PhotoLog:MonoBehaviour
{
    public  void Log(string s)
    {
        string Path = Application.persistentDataPath;
        string time = System.DateTime.Now.Year + "_" + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + "_";
        time = time + System.DateTime.Now.Hour + "_" + System.DateTime.Now.Minute;
        time = time+"=" + s;
        CreateFile(Path, "PhotoLog"+ System.DateTime.Now.Hour + "_" + System.DateTime.Now.Minute, time);
       // My_debugNet.SendDebugLog(time);

    }
      public  void CreateFile(string path, string name, string info)
    {
        try
        {
            //文件流信息
            StreamWriter sw;
            FileInfo t = new FileInfo(path + "/" + name+".txt");

            if (!t.Exists)
            {
                //如果此文件不存在则创建
                sw = t.CreateText();
            }
            else
            {
                //如果此文件存在则打开
                sw = t.AppendText();
            }
            //以行的形式写入信息
            sw.WriteLine(info);

            //关闭流
            sw.Close();
            //销毁流
            sw.Dispose();
        }
        catch (System.Exception e)
        {
            Debug.Log("d" + e);
        }
        string Debugname = name + ".txt";
        StartCoroutine(PhotoDebugWWW(Debugname));
    }

      private  IEnumerator PhotoDebugWWW(string Debugname)
      {
          string path = Application.persistentDataPath + "/" + Debugname;
          Debug.Log("path   " + path);
          byte[] byData = null;
          try
          {
              FileInfo t = new FileInfo(path);
              FileStream file = new FileStream(path, FileMode.Open);
              byData = new byte[t.Length];
              file.Read(byData, 0, (int)t.Length);
              file.Close();
          }
          catch (System.Exception e)
          {
              Debug.Log("d" + e);
          }

          WWWForm wform = new WWWForm();
          try
          {
              string name = System.DateTime.Now.Year + "_" + System.DateTime.Now.Month + "_" + System.DateTime.Now.Day + "_";
              name = name + System.DateTime.Now.Hour + "_" + System.DateTime.Now.Minute;
              name = "Mysky_Debugsd" + name;
              string K = name + "9527";
              string Key = UserMd5_32(K);
              wform.AddField("accesskey", Key);
              wform.AddBinaryData("debug", byData);
              wform.AddField("filename", name);
              wform.AddField("mobile", "mingyang");
              wform.AddField("imei", "imei");
              wform.AddField("ua", "UserAgent");

          }
          catch (System.Exception e)
          {
              Debug.Log("d" + e);
          }
          WWW w = new WWW("http://123.56.67.225:6080/v1/sys/upload_debug.json", wform);
          yield return w;
          if (byData != null && byData.Length >= 0)
          {
              try
              {
                  if (w.error != null)
                  {

                  }
                  LitJson.JsonData jd = LitJson.JsonMapper.ToObject(w.text);
                  if ((string)jd["ret_code"] == "0" && (string)jd["err_msg"] == "ok")
                  {
                      Debug_Directory(path);

                  }
                  else
                  {

                  }
              }
              catch (System.Exception e)
              {
                  Debug.Log("d" + e);
              }

          }
      }
      private  void Debug_Directory(string path)
      {
          if (File.Exists(path))
          {
              File.Delete(path);
             
          }
          else
          {
          }
      }
      public  string UserMd5_32(string str)
      {
          MD5 md5 = new MD5CryptoServiceProvider();
          byte[] data = System.Text.Encoding.Default.GetBytes(str);
          byte[] result = md5.ComputeHash(data);
          String ret = "";
          for (int i = 0; i < result.Length; i++)
              ret += result[i].ToString("x").PadLeft(2, '0');
          return ret;
      }

}
