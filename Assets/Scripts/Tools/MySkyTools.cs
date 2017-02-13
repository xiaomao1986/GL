
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System.Data;
//using System.Data.Odbc;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
//using UnityEditor;
using UnityEngine;

 public sealed class MySkyTools
{
	private MySkyTools()
    {

    }

    public static void saveFile(string name, string content)
    {
#if UNITY_WEBPLAYER || UNITY_FLASH || UNITY_METRO || UNITY_WP8
		return;
#else
	System.IO.File.WriteAllText(Application.persistentDataPath + "//" + name + ".txt", content.ToString());
#endif
        
    }

    public static void saveFileByte(string name, string content)
    {
        System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
        Byte[] encodedBytes = utf8.GetBytes(content);

        //System.IO.File.WriteAllBytes (Application.persistentDataPath + "//" + name + ".txt",content);
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + name + ".txt", FileMode.Create);
        //byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
        stream.Write(encodedBytes, 0, encodedBytes.Length);
        stream.Flush();
        stream.Close();
    }

    public static string loadFile(string name)
    {
        StreamReader sr2 = null;
        try
        {
            sr2 = File.OpenText(Application.persistentDataPath + "//" + name + ".txt");
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return null;
        }
        string line2 = "";
        StringBuilder sb = new StringBuilder();
        while ((line2 = sr2.ReadLine()) != null)
        {
            sb.Append(line2 + "\n");
        }
        sr2.Close();
        sr2.Dispose();
        string content = sb.ToString();
        return content;
    }

    public static string loadFileByte(string name)
    {
        //Debug.Log(Application.persistentDataPath + "/" + name + ".txt");
        FileInfo t = new FileInfo(Application.persistentDataPath + "/" + name + ".txt");
        if (t.Exists)
        {

            FileStream fs = new FileStream(Application.persistentDataPath + "/" + name + ".txt", FileMode.Open);
            int len = (int)fs.Length;
            byte[] data = new byte[len];
            fs.Read(data, 0, len);
            fs.Close();
            System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
            String decodedString = utf8.GetString(data);
            //Debug.Log("decodedString:"+decodedString);
            return decodedString;

        }
        else
        {
            return "";
        }

    }


    public static string GetMd5(string str)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        string a = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str)));
        a = a.Replace("-", "");
        return a;
    }

    public static String GetPrefabName(string game_object_name)
    {
        return game_object_name.Split(new string[] { "(Clone)" }, StringSplitOptions.RemoveEmptyEntries)[0];
    }

    public static Transform GetChildTransformFromSelf(Transform check, string name)
    {
        Transform[] ta = check.GetComponentsInChildren<Transform>();

        for (int i = 0, len = ta.Length; i < len; i++)
        {
            Transform t = ta[i];
            //Debug.Log("<" + t.name + ">");
            if (t.name == name)
            {
                //Debug.Log("find <" + t.name + ">");
                return t;
            }
        }
        return null;
    }


    public static void DestoryAllEffect(GameObject check)
    {
        Component[] ta = check.GetComponentsInChildren<Component>();

        for (int i = 0, len = ta.Length; i < len; i++)
        {
            Component t = ta[i];
            //Debug.Log("<" + t.name + ">");
            if (t.name.Contains("E_"))
            {
                GameObject.Destroy(t.gameObject);
            }
        }
    }

    public static Transform GetRootObjectFromChild(Transform childObject)
    {
        if (childObject.parent == null)
        {
            return childObject;
        }
        else
        {
            return GetRootObjectFromChild(childObject.parent);
        }
    }

    public static float Distance2D(Transform t1, Transform t2)
    {
        Vector2 v2_1 = new Vector2(t1.position.x, t1.position.z);
        Vector2 v2_2 = new Vector2(t2.position.x, t2.position.z);

        return Vector2.Distance(v2_1, v2_2);
    }

    public static float Distance2D(Vector3 t1, Vector3 t2)
    {
        Vector2 v2_1 = new Vector2(t1.x, t1.z);
        Vector2 v2_2 = new Vector2(t2.x, t2.z);

        return Vector2.Distance(v2_1, v2_2);
    }

    public static Vector3[] ComputeRoundPos(float radius, int count = 8)
    {
        Vector3[] poses = new Vector3[count];
        float radian = Mathf.PI * 2 / poses.Length;

        for (int i = 0, len = poses.Length; i < len; i++)
        {
            poses[i] = new Vector3(radius * Mathf.Cos(radian * i), 0, radius * Mathf.Sin(radian * i));
            //Debug.LogError("poses["+i+"]:"+poses[i]);
        }

        return poses;
    }

    public static bool PositionAndLayerHitTest(Vector3 Pos, LayerMask maskLayer)
    {
        RaycastHit hit;
        //Debug2.LogError(Pos, "ZZ");
        bool ret = Physics.Raycast(new Vector3(Pos.x, 100, Pos.z), Vector3.down, out hit, 200, maskLayer);

        return ret;
    }

    /**参数：z轴参照物的位置.*/
    public static Vector3 MousePositionToWorld(Vector3 pos_of_z_reference)
    {
        //物体的世界坐标转化成屏幕坐标.
        Vector3 screen_pos = Camera.main.WorldToScreenPoint(pos_of_z_reference);

        //鼠标的位置.
        Vector3 e = Input.mousePosition;

        //因为鼠标的屏幕Z坐标的默认值是0，所以需要一个z坐标.
        e.z = screen_pos.z;

        Vector3 world_pos = Camera.main.ScreenToWorldPoint(e);
        world_pos.y = 0;
        return world_pos;
    }

    public static RaycastHit MouseHitTest(Vector3 mousePos, LayerMask maskLayer)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        Physics.Raycast(ray, out hit, 100, maskLayer);

        return hit;
    }

    public static long StringToLong(string s)
    {
        long l = 0;
        long.TryParse(s, out l);
        return l;
    }

    public static float Abs(float f)
    {
        return Math.Abs(f);
    }

    public static bool FloatEquals(float fa, float fb)
    {
        return FloatEquals(fa, fb, 0.0001f);
    }

    public static bool FloatEquals(float fa, float fb, float precision)
    {
        return Mathf.Abs(fa - fb) < precision;
    }

    public static BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;


    public static void SimpleObjectToJson(System.Object o)
    {
        //MiniJSON.Json.Serialize(o);
    }


    public static string la2s(long[] la)
    {
        if (la == null || la.Length <= 0)
        {
            return "NULL";
        }
        string ret = "[";
        for (int i = 0, len = la.Length; i < len; i++)
        {
            ret += la[i].ToString();
            ret += ",";
        }
        ret = ret.Remove(ret.Length - 1);
        ret += "]";
        return ret;
    }
    public static string ia2s(int[] ia)
    {
        if (ia == null || ia.Length <= 0)
        {
            return "";
        }
        string ret = "[";
        for (int i = 0, len = ia.Length; i < len; i++)
        {
            ret += ia[i].ToString();
            ret += ",";
        }
        ret = ret.Remove(ret.Length - 1);
        ret += "]";
        return ret;
    }

	public static void LogByteArray(byte[] sa)
	{
		string s = "[";
		for (int i = 0, len = sa.Length; i < len; i++)
		{
			s += sa[i];
			if (i < len - 1)
			{
				s += ", ";
			}
		}
		s += "]";
		Debug.Log(s);
	}

    public static void LogStringArray(string[] sa)
    {
        string s = "[";
        for (int i = 0, len = sa.Length; i < len; i++)
        {
            s += sa[i];
            if (i < len - 1)
            {
                s += ", ";
            }
        }
        s += "]";
        Debug.Log(s);
    }

    public static bool HasChinese(string str)
    {
        return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
    }


    public static List<string> addStringList(List<string> list, string str)
    {
        if (!list.Contains(str))
        {
            list.Add(str);
        }
        return list;
    }
    public static List<string> removeStringList(List<string> list, string str)
    {
        if (list.Contains(str))
        {
            list.Remove(str);
        }
        return list;
    }


    #region time
    private static DateTime date_time_base_utc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private static DateTime date_time_base_local = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

    public static string DeltaTime(long ms)
    {
        int temM = (int)(ms / 60);

        if (temM < 60)
        {
            if (temM < 1)
            {
                temM = 1;
            }
            return temM + "分钟前";
        }
        else
        {
            int temH = temM / 60;
            if (temH < 24)
            {
                return temH + "小时前";
            }
            else
            {
                int temD = temH / 24;
                if (temD < 7)
                {
                    return temD + "天前";
                }
                else
                {
                    return "1周前";
                }
            }
        }

        //if (ms < 0)
        //{
        //    return "";
        //}
        //long deltaTime = (CurrentTimeMillis() - ms) / 1000;
        //if (deltaTime > 604800)
        //{
        //    return "1周前";
        //}
        //else if (deltaTime > 86400)
        //{
        //    return "1天前";
        //}
        //else if (deltaTime > 3600)
        //{
        //    return "1小时前";
        //}
        //else
        //{
        //    return "刚刚";
        //}
    }

    /** 同java->System.currentTimeMillis(). */
    public static long CurrentTimeMillis()
    {
        return (long)(DateTime.UtcNow - date_time_base_utc).TotalMilliseconds;
    }

    /** 毫秒转换为时间点. */
    public static string MillisecondsToTime(long ms, string arg = "yyyy/MM/dd HH:mm:ss")
    {
        DateTime date_time_visit = date_time_base_local.AddMilliseconds(ms);
        return date_time_visit.ToString(arg);
    }

    /** 把毫秒转换为时间差. */
    public static string MillisecondsToDifftime(long ms, string arg = "HH:mm:ss")
    {
        DateTime date_time_diff = date_time_base_utc.AddMilliseconds(ms);
        return date_time_diff.ToString(arg);
    }

    public static string formatTime(int time, bool use_ms = false)
    {
        //			if (time < 0)
        //			{
        //				time = 0;
        //			}
        //			DateTime data_time = new DateTime(time * 10000);
        //			if (use_ms)
        //			{
        //				return data_time.ToString("HH:mm:ss.ff");
        //			} else
        //			{
        //				return data_time.ToString("HH:mm:ss");
        //			}
        if (time <= 0)
        {
            return "00:00:00";
        }
        string hh, mm, ss;
        int release;
        hh = (time / 3600).ToString();
        if (hh.Length == 1)
        {
            hh = "0" + hh;
        }
        release = time % 3600;
        mm = (release / 60).ToString();
        if (mm.Length == 1)
        {
            mm = "0" + mm;
        }
        ss = (release % 60).ToString();
        if (ss.Length == 1)
        {
            ss = "0" + ss;
        }
        return hh + ":" + mm + ":" + ss;
    }
    #endregion


    #region color
    public static string ColorToHex(Color color)
    {
        string r = Convert.ToString((int)(color.r * 255), 16).PadLeft(2, '0');
        string g = Convert.ToString((int)(color.g * 255), 16).PadLeft(2, '0');
        string b = Convert.ToString((int)(color.b * 255), 16).PadLeft(2, '0');
        return "[" + r + g + b + "]";
    }
    public static string[] colorsRGB = { "eee5d5", "00f007", "107ddd", "9a1bf1", "cf590b" };
    public static Color Color_White = new Color(238f / 255, 229f / 255, 213f / 255);
    public static Color Color_Green = new Color(0f / 255, 240f / 255, 7f / 255);
    public static Color Color_Blue = new Color(16f / 255, 125f / 255, 221f / 255);
    public static Color Color_Purple = new Color(154f / 255, 27f / 255, 241f / 255);
    public static Color Color_Orange = new Color(207f / 255, 89f / 255, 11f / 255);
    public static Color Color_Red = new Color(1f, 0f, 0f);
    public static Color Color_Gray = new Color(140f / 255, 140f / 255, 140f / 255);

    public static Color Color_Outline = new Color(93f / 255, 57f / 255, 15f / 255);

    private static Color[] colors = { Color_White, Color_White, Color_Green, Color_Blue, Color_Purple, Color_Orange };

    public static Color getQualityColor(int quality)
    {
        try
        {
            return colors[quality];
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return colors[0];
        }
    }

    #endregion

    public static string FormatNum(string value){
        if (value.Length > 10)
        {
            return value.Substring(0, value.Length - 9);
        }
        else if (value.Length > 7)
        {
            return value.Substring(0, value.Length - 5);
        }
        else
        {
            return value;
        }
    }

    public static void ClearTagsChild(Transform tfTag)
    {
        if (tfTag.childCount > 0)
        {
            for (int i = 0, len = tfTag.childCount; i < len; i++)
            {
                GameObject.Destroy(tfTag.GetChild(i).gameObject);
            }
        }
    }

    public static bool IsExist(object obj)
    {
        return obj != null;
    }
    public static bool IsExist(object[] obj)
    {
        return obj != null && obj.Length > 0;
    }
    public static bool IsExist(List<object> obj)
    {
        return obj != null && obj.Count > 0;
    }

	public static string Hash_MD5_32(string word, bool toUpper = false)
	{
		try
		{
			System.Security.Cryptography.MD5CryptoServiceProvider MD5CSP
				= new System.Security.Cryptography.MD5CryptoServiceProvider();
			
			byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(word);
			byte[] bytHash = MD5CSP.ComputeHash(bytValue);
			MD5CSP.Clear();
			
			//根据计算得到的Hash码翻译为MD5码
			string sHash = "", sTemp = "";
			for (int counter = 0; counter < bytHash.Length; counter++)
			{
				long i = bytHash[counter] / 16;
				if (i > 9)
				{
					sTemp = ((char)(i - 10 + 0x41)).ToString();
				}
				else
				{
					sTemp = ((char)(i + 0x30)).ToString();
				}
				i = bytHash[counter] % 16;
				if (i > 9)
				{
					sTemp += ((char)(i - 10 + 0x41)).ToString();
				}
				else
				{
					sTemp += ((char)(i + 0x30)).ToString();
				}
				sHash += sTemp;
			}
			
			//根据大小写规则决定返回的字符串
			return toUpper ? sHash : sHash.ToLower();
		}
		catch (UnityException ex)
		{
			throw new UnityException(ex.Message);
		}
	}
}
