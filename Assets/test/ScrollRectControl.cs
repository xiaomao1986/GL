using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ScrollRectControl : MonoBehaviour, IEndDragHandler
{
    public Scrollbar bar;
    private ScrollRect scrollRect;
    private GameObject item;
    private GameObject grid;
    void Start()
    {
        scrollRect = transform.GetComponent<ScrollRect>();
        item = transform.FindChild("Item").gameObject;
        grid = transform.FindChild("grid").gameObject;
        addItem();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (bar.value <= 0)
        {
            addItem();
        }
    }
    void addItem()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject newItem = addChild(item, grid);
            newItem.SetActive(true);
            string str = string.Format("第{0}项时间为{1}", i, System.DateTime.Now);
            newItem.GetComponent<ItemControl>().setItem(str);
        }
    }
    public static GameObject addChild(GameObject o, GameObject parent)
    {
        if (o == null || parent == null)
        {
            return null;
        }
        GameObject inst = GameObject.Instantiate(o) as GameObject;
        inst.transform.SetParent(parent.transform, false);
        inst.transform.localPosition = Vector3.zero;
        inst.transform.localScale = Vector3.one;
        return inst;
    }
}