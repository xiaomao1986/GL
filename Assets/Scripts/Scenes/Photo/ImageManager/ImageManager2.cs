//#define SKY_DEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ImageManager2 : MonoBehaviour
{
    #region 基础变量

    private static ImageManager2 instance;
    public static ImageManager2 Instance
    {
        get
        {
            return instance;
        }
    }

    //图片信息集合
    public class Image_info
    {
        public string key;
        public TextureSize_state state;
        public int[] stateIndex = new int[6];
        public Texture2D tex;
        public Callback<Texture2D> fun;
        public string isError = "null";
        public string site = "null";
    }

    private Dictionary<string, Image_info> imgList = new Dictionary<string, Image_info>();
    private List<Image_info> iList = new List<Image_info>();
    public List<Image_info> getIlist()
    {
        return iList;
    }

    void Awake()
    {
        instance = this;

        StartCoroutine(WaitToGCcollect());
    }

    IEnumerator WaitToGCcollect()
    {
        while (true)
        {
            yield return new WaitForSeconds(60);
            GC.Collect();
        }
    }

    #endregion

    #region 下载图片方法
    public void loadImage(string id, TextureSize_state size, Callback<Texture2D> fun, bool isFistLoad = false)
    {

        if (imgList.ContainsKey(id))
        {
            Image_info info = imgList[id];
            info.isError = "null";
			info.site = "null";
			info.fun=fun;
            //如果有小图，则删除小图，加载大图
            if ((int)imgList[id].state > (int)size)
            {
                info.state = size;
                info.stateIndex[(int)size]++;
                listAddImg(info, isFistLoad);
                listLoadImg();
            }
            //否则记数加1，然后返回图片
            else
            {
                info.stateIndex[(int)size]++;
				fun(imgList[id].tex);
            }
        }
        else
        {
            //请求本地是否有该图片
            Image_info info = new Image_info();
            info.isError = "null";
            info.site = "null";
            info.tex = new Texture2D(0, 0);
            info.key = id;
            info.state = size;
            info.stateIndex[(int)size] = 1;
            info.fun = fun;
            imgList.Add(id, info);
            listAddImg(info, isFistLoad);
            listLoadImg();
        }
    }
    private bool startLoadImg = false;
    private void listLoadImg()
    {
        if (startLoadImg) return;
        startLoadImg = true;
        if (iList.Count >= 1)
        {
            StartCoroutine(LoadLocalImage(iList[0]));
        }
    }
    /// <summary>
    /// 删除加载缓存图片
    /// </summary>
    /// <param name="info"></param>
    private void listRemoveImg(Image_info info)
    {
        iList.Remove(info);
        if (iList.Count >= 1)
        {
            StartCoroutine(LoadLocalImage(iList[0]));
        }
        else if (iList.Count <= 0)
        {
            startLoadImg = false;
        }
    }
    /// <summary>
    /// 添加一个元素到缓存中
    /// </summary>
    /// <param name="info">加载元素</param>
    /// <param name="isFistLoad">是否优先加载</param>
    private void listAddImg(Image_info info, bool isFistLoad)
    {
        if (iList.Contains(info)) iList.Remove(info);
        if (isFistLoad) iList.Insert(0, info);
        else iList.Add(info);
    }
				
    private void loadLocalImage(Image_info info)
    {
        string filePath = Application.persistentDataPath + "/Resources/" + info.key;

        if (System.IO.File.Exists(filePath))
        {
            try
            {
                Texture2D test = (Texture2D)Resources.Load(info.key, typeof(Texture2D));
                info.tex = test;
                info.fun(info.tex);
            }
            catch (System.Exception e)
            {
                Debug.Log("loadLocalImage 读取本地文件出错：" + e.ToString());
            }
        }
        else
        {
            StartCoroutine(DownloadImage(info));
        }
    }
    IEnumerator LoadLocalImage(Image_info info)
    {
        string filePath = getWWWImgUrl(info);
        WWW www = new WWW(filePath);
        yield return www;
        if (www.error == null)
        {
            info.tex.LoadImage(www.bytes);
            listRemoveImg(info);
            info.isError = "OK";
            info.site = "本地";
			info.fun(info.tex);
        }
        else
        {
            //TODO 加载网络数据
            StartCoroutine(DownloadImage(info));
        }
    }

    IEnumerator DownloadImage(Image_info info)
    {

        string filePath = getImgUrl(info);

        string url = idToUrl(info.key) + "@" + getSizeString(info.state);
        bool isOk = false;
        int times = 0;
        while (!isOk)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.error == null)
            {
                info.tex.LoadImage(www.bytes);
                info.isError = "OK";
                info.site = "网络";
                info.fun(info.tex);
                listRemoveImg(info);
                isOk = true;
                try
                {
                    File.WriteAllBytes(filePath, info.tex.EncodeToJPG());
                }
                catch (System.Exception e)
                {
                    string s = e.ToString();
                }

            }
            else
            {
                Debug.LogError("ImageManager2 网络请求图片出错：" + url + www.error.ToString());
                info.isError = "Error";
                if (times == 0) listRemoveImg(info);
                times++;
                if (times >= 10)
				{
					isOk = true;
					info.fun(null);
				}
            }
            if (!isOk)
            {
                yield return new WaitForSeconds(3f);
            }
        }
    }
    #endregion

    #region 删除没用的图片
    /// <summary>
    /// 删除图片（非强制删除）
    /// </summary>
    /// <param name="id"></param>
    /// <param name="size"></param>
    public void deleteImage(string id, TextureSize_state size)
    {
		//Debug.Log ("deleteImage   "+id);
        if (!imgList.ContainsKey(id))
        {

            Debug.LogError(imgList.Count + "====deleteImage ERROR" + id);
            return;
        }
        Image_info info = imgList[id];
        if (info == null)
        {
            Debug.LogError("deleteImage ERROR");
            return;
        }

        //如果缓存中有图片，则删除该图片不继续加载
        if (iList.Contains(info)) iList.Remove(info);

        info.stateIndex[(int)size]--;
        if (info.stateIndex[(int)size] < 0)
        {
            Debug.LogError("deleteImage ERROR 引用计数溢出");
        }

        //如果所有计数均为0，则删除该图片
        int max = 0;
        for (int i = 0; i < info.stateIndex.Length; i++)
        {
            max += info.stateIndex[i];
        }
        if (max <= 0)
        {
            imgList.Remove(id);
            Resources.UnloadUnusedAssets();
        }
    }


    /// <summary>
    /// 开机检测图片中超过一定天数的文件，删除
    /// 检测文件中图片数量，如果超出范围则删除多余的照片
    /// </summary>
    private void deleteImageFile()
    {
        try
        {
            deleteImageFile2(FileConfig.img_file_raw);
            deleteImageFile2(FileConfig.img_file_big);
            deleteImageFile2(FileConfig.img_file_centre);
            deleteImageFile2(FileConfig.img_file_little);
            deleteImageFile2(FileConfig.img_file_tiny);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
    public void deleteImageFile2(string url)
    {
        DirectoryInfo theFolder = new DirectoryInfo(url);
        foreach (FileInfo nextFile in theFolder.GetFiles())
        {
            DateTime now = DateTime.Now;
            DateTime time = nextFile.CreationTime;
            TimeSpan t = now - time;
            if (t.TotalDays > 7f)
            {
                File.Delete(nextFile.FullName);
            }
        }
    }
    #endregion

    #region 公用方法
    public static string urlToId(string url)
    {
		return url.Replace('/','|');
    }

    private static string idToUrl(string id)
    {
		return id.Replace('|','/');
    }

    private static string getWWWImgUrl(Image_info info)
    {
        return "file:///" + getImgUrl(info);
    }
    private static string getImgUrl(Image_info info)
    {
        switch (info.state)
        {
            case TextureSize_state.Texture_raw:
                return FileConfig.img_file_raw + info.key + ".sky";
            case TextureSize_state.Texture_big:
                return FileConfig.img_file_big + info.key + ".sky";
            case TextureSize_state.Texture_centre:
                return FileConfig.img_file_centre + info.key + ".sky";
            case TextureSize_state.Texture_little:
                return FileConfig.img_file_little + info.key + ".sky";
            case TextureSize_state.Texture_tiny:
                return FileConfig.img_file_tiny + info.key + ".sky";
            default:
                return null;
        }
    }
    private static string getSizeString(TextureSize_state size)
    {
        switch (size)
        {
            case TextureSize_state.Texture_raw:
                return "";
            case TextureSize_state.Texture_big:
                return "!imysky-style-adaptiveh-360w-90q";
            case TextureSize_state.Texture_centre:
                return "!imysky-style-adaptiveh-180w-90q";
            case TextureSize_state.Texture_little:
                return "imysky-style-adaptiveh-100w-90q";
            case TextureSize_state.Texture_tiny:
                return "!imysky-style-adaptiveh-40w-90q";
            default:
                return null;
        }
    }
    #endregion

#if SKY_DEBUG
    private Vector2 scrollPosition = Vector2.zero;
    private bool isShowImages = true;
    private void OnGUI()
    {
        GUIStyle bb = new GUIStyle();
        bb.normal.background = null;    //这是设置背景填充的
        bb.normal.textColor = new Color(1, 1, 1);   //设置字体颜色的
        bb.fontSize = 19;       //当然，这是字体颜色

        scrollPosition.y = 0;
        GUI.Label(new Rect(0, scrollPosition.y, 1080, 20), "图片缓存数量：" + imgList.Count, bb);
        scrollPosition.y += 20;
        GUI.Label(new Rect(0, scrollPosition.y, 1080, 20), "图片缓存数量：" + iList.Count, bb);
        scrollPosition.y += 20;
        if (GUI.Button(new Rect(0, scrollPosition.y, 500, 30), "开启、关闭", bb))
        {
            isShowImages = !isShowImages;
        }
        scrollPosition.y += 30;
        if (isShowImages)
        {
            foreach (KeyValuePair<string, Image_info> item in imgList)
            {
                GUI.Label(new Rect(0, scrollPosition.y, 1080, 20), "[" + item.Key + "] state: " + item.Value.state + "|加载位置:" + item.Value.site + "|isError:" + item.Value.isError, bb);
                scrollPosition.y += 20;
            }
        }
    }
#endif
}
