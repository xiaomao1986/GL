//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class UIViewManager : MonoBehaviour
//{
//    private Stack<SUIView> m_mainStack = new Stack<SUIView>();

//    // Use this for initialization
//    void Start () {
//        //初始化管理器对象的位置信息
//        RectTransform rect = this.gameObject.AddComponent<RectTransform>();
//        rect.anchoredPosition = Vector2.zero;
//        rect.sizeDelta = Vector2.zero;
//        rect.anchorMax = Vector2.one;
//        rect.anchorMin = Vector2.zero;
//        rect.pivot = new Vector2(0,1);
//        rect.localScale = Vector3.one;
//    }
	
//    // Update is called once per frame
//    void Update () {
	    
//    }

//    /// <summary>
//    /// 关闭所有View对象
//    /// </summary>
//    public void CloseAll()
//    {

//    }

//    /// <summary>
//    /// 添加UIView
//    /// </summary>
//    /// <typeparam name="T"> 添加的UIView面类型 </typeparam>
//    /// <returns></returns>
//    public T PushView<T>() where T : SUIView 
//    {
//        if (m_mainStack.Count >= 1) m_mainStack.Peek().gameObject.SetActive(false);
//        GameObject g = new GameObject();
//        g.transform.SetParent(this.gameObject.transform);
//        T t = g.AddComponent<T>();
//        m_mainStack.Push(t);
//        return t;
//    }


//    public void PopView()
//    {
//        if (m_mainStack.Count <= 0)
//        {
//            Debug.LogError("对不起，已经没有任何显示的View了");
//            return;
//        }

//        //=================处理出栈的UI的逻辑================================
//        SUIView view = m_mainStack.Pop();
//        MonoBehaviour.Destroy(view.gameObject);


//        //=================处理当前要显示的UI的逻辑================================
//        if (m_mainStack.Count <= 0) return;
//        SUIView showView = m_mainStack.Peek();
//        showView.gameObject.SetActive(true);
//    }
//    public void PopToView(string name)
//    {

//    }
//}
