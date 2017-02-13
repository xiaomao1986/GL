using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class SUISelfshowView : EventTrigger // MonoBehaviour//
{

    [SerializeField]  
    private List<GameObject> m_items = new List<GameObject>();
    [SerializeField]
    private List<GameObject> m_pointss = new List<GameObject>();
    [SerializeField]
    private List<GameObject> m_messages = new List<GameObject>();

    [SerializeField]  
    private GameObject m_viewport;

    [SerializeField]
    private GameObject m_content;

    [SerializeField]
    private GameObject m_pointsCurrent;
    [SerializeField]
    private GameObject m_pointsOther;

    [SerializeField]
    private float m_cutNextTime = 5;
    private float m_oldTime = 0;

    private int m_current_obj_index = 0;

    private int Current_obj_index
    {
        get { return m_current_obj_index; }
        set {
            if (m_current_obj_index != value)
            {
                for(int i = 0 ; i < m_pointss.Count ; i ++)
                {
                    bool isShow = i != value;
                    m_pointss[i].transform.FindChild("Current").gameObject.SetActive(isShow);
                    m_pointss[i].transform.FindChild("Other").gameObject.SetActive(!isShow);
                    m_messages[i].SetActive(!isShow);
                }
            }
            m_current_obj_index = value; 
        }
    }

    private int m_dragState = 2;

    private void Awake()
    {
        UpdateUI();
    }
    private void Start()
    {
        m_oldTime = Time.time;
    }
	
	// Update is called once per frame
    private void Update()
    {
        if (m_dragState != 2) return;

        RectTransform thisRect = GetComponent<RectTransform>();
        RectTransform conT = m_content.GetComponent<RectTransform>();
        float target = -Current_obj_index * thisRect.rect.width;
        target = conT.anchoredPosition.x+(target - conT.anchoredPosition.x) * 0.05f;
        conT.anchoredPosition = new Vector2(target, conT.anchoredPosition.y);

        //判断如果到一定的时间则开始跳转下一个标签
        if (Time.time - m_oldTime >= m_cutNextTime)
        {
            m_oldTime = Time.time;
            Current_obj_index++;
            if (Current_obj_index >= m_items.Count) Current_obj_index = 0;
        }
        

	}

    private void UpdateUI()
    {
        Transform obj = null;
        //判断是否为空
        if (m_items.Count == 0)
        {
            obj = transform.FindChild("Items");
            if (obj != null)
            {
                for (int i = 0; i < obj.GetChildCount(); i++)
                {
                    m_items.Add(obj.GetChild(i).gameObject);
                }
            }else
            {
                Debug.LogError("错误： SUISelfshowView 对不起，您没有配置滚动项 \n" + m_structureErrorString);
            }
        }

        RectTransform thisRect = GetComponent<RectTransform>();

        m_viewport = new GameObject();
        m_viewport.AddComponent<Mask>();
        Image img = m_viewport.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 1f/255f);
        RectTransform viewportRect = m_viewport.GetComponent<RectTransform>();
        viewportRect.name = "Viewport";
        viewportRect.SetParent(transform);
        viewportRect.localScale = Vector3.one;
        viewportRect.pivot = new Vector2(0, 0);
        viewportRect.anchorMax = new Vector2(1, 1);
        viewportRect.anchorMin = new Vector2(0, 0);
        viewportRect.offsetMax = new Vector2(0, 0);
        viewportRect.offsetMin = new Vector2(0, 0);
        viewportRect.anchoredPosition = Vector2.zero;

        m_content = new GameObject();
        RectTransform contentRect = m_content.AddComponent<RectTransform>();
        contentRect.name = "Content";
        contentRect.SetParent(viewportRect);
        contentRect.localScale = Vector3.one;
        contentRect.pivot = new Vector2(0, 0);
        contentRect.anchorMax = new Vector2(0, 1);
        contentRect.anchorMin = new Vector2(0, 0);
        contentRect.sizeDelta = new Vector2((m_items.Count > 1 ? m_items.Count : 1) * viewportRect.rect.width, 0);
        contentRect.anchoredPosition = new Vector2(0, 0);

        //获取点儿
        Transform pointsT = transform.FindChild("Points");

        for (int i = 0; i < m_items.Count ; i++ )
        {
            GameObject g = m_items[i];
            g.transform.SetParent(m_content.transform);
            RectTransform gRect = g.GetComponent<RectTransform>();
            gRect.pivot = new Vector2(0, 1);
            gRect.anchorMax = new Vector2(0, 1);
            gRect.anchorMin = new Vector2(0, 1);
            gRect.localScale = Vector2.one;
            gRect.anchoredPosition = new Vector2(i * viewportRect.rect.width, 0);
            gRect.sizeDelta = new Vector2(viewportRect.rect.width, viewportRect.rect.height);

            //设置小点的位置
            GameObject gp = (GameObject)MonoBehaviour.Instantiate(pointsT.gameObject);
            RectTransform baseRect = pointsT.GetComponent<RectTransform>();
            RectTransform gpRect = gp.GetComponent<RectTransform>();
            gpRect.SetParent(transform);
            gpRect.anchoredPosition = baseRect.anchoredPosition;
            gpRect.sizeDelta = baseRect.sizeDelta;
            float x = baseRect.rect.x - baseRect.rect.width * (((float)m_items.Count) / 2f - i - 1);
            gpRect.anchoredPosition = new Vector2(x, gpRect.anchoredPosition.y);
            gpRect.SetAsLastSibling();
            gpRect.localScale = Vector2.one;
            m_pointss.Add(gp);
        }
        try
        {
            for (int i = 0; i < m_pointss.Count; i++)
            {
                bool isShow = i != Current_obj_index;
                m_pointss[i].transform.FindChild("Current").gameObject.SetActive(isShow);
                m_pointss[i].transform.FindChild("Other").gameObject.SetActive(!isShow);
            }
            
        }
        catch (System.Exception e)
        {
            Debug.LogError(e + "\n" + m_structureErrorString);
        }
        

        //获取信息
        Transform message = transform.FindChild("Message");
        if (message == null)
        {
            Debug.LogError("错误：SUISelfshowView 必须包含 Message 的子物体 \n" + m_structureErrorString);
        }
        for(int i = 0 ; i < message.GetChildCount() ; i++ )
        {
            bool isShow = i == Current_obj_index;
            GameObject g = message.GetChild(i).gameObject;
            g.SetActive(isShow);
            m_messages.Add(g);
        }
        if(m_messages.Count<m_items.Count )
        {
            Debug.LogError("错误：SUISelfshowView中 Message 子物体必须与 Items子物体数目相同\n" + m_structureErrorString);
        }
        

        if (obj != null) GameObject.Destroy(obj.gameObject);
        if (pointsT != null) GameObject.Destroy(pointsT.gameObject);
        
    }

    public override void OnBeginDrag(PointerEventData data)
    {
        m_dragState = 1;
    }
    public override void OnEndDrag(PointerEventData data)
    {
        m_oldTime = Time.time;
        m_dragState = 2;
    }

    public override void OnDrag(PointerEventData data)
    {
        m_dragState = 3;
        RectTransform conT = m_content.GetComponent<RectTransform>();
        conT.position = new Vector3(data.delta.x * 2 + conT.position.x, conT.position.y, conT.position.z);
        RectTransform thisRect = GetComponent<RectTransform>();
        Current_obj_index = -(int)((conT.anchoredPosition.x - thisRect.rect.width / 2) / thisRect.rect.width);
        if (Current_obj_index <= 0) Current_obj_index = 0;
        if (Current_obj_index >= m_items.Count) Current_obj_index = m_items.Count - 1;
    }

    private string m_structureErrorString = 
        "对不起您的组件必须符合如下结构：  \n" +
        "- SUISelfshowView  \n" +
        "   - Items（子物体为每个显示的内容）  \n" +
        "       - 物体1  \n" +
        "       - 物体2  \n" +
        "       - .....  \n" +
        "   - Message（子物体的个数必须和Items相同）  \n" +
        "   - Points \n" +
        "       - Current  \n" +
        "       - Other  \n";
}
