using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class SUIView : MonoBehaviour {

	void Start () {
        RectTransform rect = null;
        rect = this.gameObject.GetComponent<RectTransform>();
        if(rect == null) this.gameObject.AddComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.anchorMin = Vector2.zero;
        rect.pivot = new Vector2(0, 1);
        rect.localScale = Vector3.one;
	}

    /// <summary>
    /// 检测所有子物体中包含Button组件对象的回调函数
    /// </summary>
    /// <param name="callback"> 当Button按下回调的函数 </param>
    public void TestButtonCallBack(Callback<string> callback)
    {
        Button[] bs = gameObject.GetComponentsInChildren<Button>();
        
        foreach(Button b in bs)
        {
            b.onClick.RemoveAllListeners();
            string b_name = b.name;
            b.onClick.AddListener(
                delegate()
                {
                    callback(b_name);
                }
            );
        }
    }

    /// <summary>
    /// 自动创建一个UI View
    /// </summary>
    /// <returns></returns>
    public static SUIView Create()
    {
        GameObject g = new  GameObject();
        RectTransform t = g.AddComponent<RectTransform>();
        SUIView s = g.AddComponent<SUIView>();
        return s;
    }

    /// <summary>
    /// 加载资源对象
    /// </summary>
    /// <param name="url">资源对象地址</param>
    public GameObject AddResources(string url)
    {
        GameObject g = (GameObject)MonoBehaviour.Instantiate(Resources.Load(url));
        
        RectTransform rect = g.GetComponent<RectTransform>();
        rect.SetParent(gameObject.transform);
        rect.localScale = Vector3.one;
        rect.anchoredPosition = Vector2.zero;

        return g;
    }
}
