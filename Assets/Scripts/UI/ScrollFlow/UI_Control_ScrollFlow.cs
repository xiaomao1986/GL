using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UI_Control_ScrollFlow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{


    public Text t;
    public RectTransform Rect;
    public List<UI_Control_ScrollFlow_Item> Items;

    public float Width = 500;
    public float MaxScale;
    /// <summary>
    /// 开始坐标值，间隔坐标值，小于vmian 达到最左，大于vmax达到最右
    /// </summary>
    private float StartValue = 0.5f, AddValue = 0.2f;
    public float VMin = 0.1f, VMax = 0.9f;
    /// <summary>
    /// 坐标曲线
    /// </summary>
    public AnimationCurve PositionCurve;
    /// <summary>
    /// 大小曲线
    /// </summary>
    public AnimationCurve ScaleCurve;
    /// <summary>
    /// 透明曲线
    /// </summary>
    public AnimationCurve ApaCurve;

    private float v = 0;
    private List<UI_Control_ScrollFlow_Item> GotoFirstItems = new List<UI_Control_ScrollFlow_Item>(), GotoLaserItems = new List<UI_Control_ScrollFlow_Item>();
    /// <summary>
    /// 计算值
    /// </summary>
    private Vector2 start_point, add_vect;
    public event CallBack<UI_Control_ScrollFlow_Item> MoveEnd;
    public void Refresh()
    {
        Debug.Log("===================Refresh=================");
        for (int i = 0; i < Rect.childCount; i++)
        {
            Transform tran = Rect.GetChild(i);
            if (ItemsOld.Contains(tran)) { continue; }
            if (!tran.gameObject.activeInHierarchy) { continue; }

            UI_Control_ScrollFlow_Item item = tran.GetComponent<UI_Control_ScrollFlow_Item>();
            if (item != null)
            {
                Items.Add(item);
                item.Init(this);
                item.Drag(StartValue + (Items.Count - 1) * AddValue);
                if (item.v - 0.5 < 0.05f)
                {
                    Current = item;
                }
            }
        }
        if (Items.Count < 5)
        {
            switch (Items.Count)
            {
                case 1:
                case 2:
                    VMax = 0.7f;
                    VMin = 0.3f;
                    break;
                case 3:
                    VMax = 0.9f;
                    VMin = 0.1f;
                    break;
                case 4:
                    VMax = 1.1f;
                    VMin = -0.1f;
                    break;
            }
        }
        else
        {
            VMax = 0.9f + (Items.Count - 5) * AddValue;
            VMin = 0.1f;
        }
        if (MoveEnd != null)
        {
            MoveEnd(Current);
        }
        Check(1);
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].Refresh();
        }
        ItemsOld.Clear();
    }


    public List<Transform> ItemsOld;
    public void Clear()
    {
        ItemsOld = new List<Transform>();
        for (int i = 0; i < Items.Count; i++)
        {
            ItemsOld.Add(Items[i].transform);
            Destroy(Items[i].gameObject);
        }
        Items.Clear();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        start_point = eventData.position;
        add_vect = Vector3.zero;
        _anim = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        add_vect = eventData.position - start_point;
        v = eventData.delta.x * 1.00f / Width;
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].Drag(v);
        }

        Check(v);
    }


    public void Check(float _v)
    {
        if (Items.Count < 5) { return; }
        if (_v < 0)
        {//向左运动
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].v < (VMin - AddValue / 2))
                {
                    GotoLaserItems.Add(Items[i]);
                }
            }
            if (GotoLaserItems.Count > 0)
            {
                for (int i = 0; i < GotoLaserItems.Count; i++)
                {
                    GotoLaserItems[i].v = Items[Items.Count - 1].v + AddValue;
                    Items.Remove(GotoLaserItems[i]);
                    Items.Add(GotoLaserItems[i]);
                }
                GotoLaserItems.Clear();
            }
        }
        else if (_v > 0)
        {//向右运动，需要把右边的放到前面来

            for (int i = Items.Count - 1; i > 0; i--)
            {
                if (Items[i].v > VMax)
                {
                    GotoFirstItems.Add(Items[i]);
                }
            }
            if (GotoFirstItems.Count > 0)
            {
                for (int i = 0; i < GotoFirstItems.Count; i++)
                {
                    GotoFirstItems[i].v = Items[0].v - AddValue;
                    Items.Remove(GotoFirstItems[i]);
                    Items.Insert(0, GotoFirstItems[i]);
                }
                GotoFirstItems.Clear();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag:" + eventData.pointerPressRaycast.gameObject);
        if (Items.Count < 5)
        {
            if (Items[Items.Count - 1].v > VMax)
            {
                AnimToEnd(VMax - Items[Items.Count - 1].v);
                return;
            }
            else if (Items[0].v < VMin)
            {
                AnimToEnd(VMin - Items[0].v);
                return;
            }
        }
        float k = 0, v1;
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].v >= VMin)
            {
                v1 = (Items[i].v - VMin) % AddValue;
                //Debug.Log(v1 + "--" + NextAddValue);
                if (add_vect.x >= 0)
                {
                    k = AddValue - v1;
                }
                else
                {
                    k = v1 * -1;
                }
                break;
            }
        }
        add_vect = Vector3.zero;
        AnimToEnd(k);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       // My_debugNet.SendDebugLog("OnPointerClick-----" +eventData.pointerPressRaycast.gameObject+"==="+Items.Count);
         Debug.Log("OnPointerClick:" + eventData.pointerPressRaycast.gameObject);
      

        if (add_vect.sqrMagnitude <= 1)
        {
           // Debug.Log("============OnPointerClickOK============");
            UI_Control_ScrollFlow_Item script = eventData.pointerPressRaycast.gameObject.GetComponent<UI_Control_ScrollFlow_Item>();
            if (script != null)
            {
                float k = script.v;
                k = 0.5f - k;
                AnimToEnd(k);
            }

        }
    }


    public float GetApa(float v)
    {
        return ApaCurve.Evaluate(v);
    }
    public float GetPosition(float v)
    {
        return PositionCurve.Evaluate(v) * Width;
    }
    public float GetScale(float v)
    {
        return ScaleCurve.Evaluate(v) * MaxScale;
    }


    private List<UI_Control_ScrollFlow_Item> SortValues = new List<UI_Control_ScrollFlow_Item>();
    private int index = 0;
    public void LateUpdate()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].v >= 0.1f && Items[i].v <= 0.9f)
            {
                index = 0;
                for (int j = 0; j < SortValues.Count; j++)
                {
                    if (Items[i].sv >= SortValues[j].sv)
                    {
                        index = j + 1;
                    }
                }

                SortValues.Insert(index, Items[i]);
            }
        }

        for (int k = 0; k < SortValues.Count; k++)
        {
            SortValues[k].rect.SetSiblingIndex(k);
        }
        SortValues.Clear();
    }

    public void ToLaster(UI_Control_ScrollFlow_Item item)
    {
        item.v = Items[Items.Count - 1].v + AddValue;
        Items.Remove(item);
        Items.Add(item);
    }

    /// <summary>
    /// 是否开启动画
    /// </summary>
    public bool _anim = false;
    private float AddV = 0, Vk = 0, CurrentV = 0, Vtotal = 0, VT = 0;
    private float _v1 = 0, _v2 = 0;
    /// <summary>
    /// 动画速度
    /// </summary>
    public float _anim_speed = 1f;
    // private float start_time = 0, running_time = 0;

    public UI_Control_ScrollFlow_Item Current;



    public void AnimToEnd(float k)
    {
        AddV = k;
        if (AddV > 0) { Vk = 1; }
        else if (AddV < 0) { Vk = -1; }
        else
        {
            return;
        }
        Vtotal = 0;
        _anim = true;

    }

    void Update()
    {
        if (Current!=null)
        {
            UIMovableData tmpData = Current.gameObject.GetComponent<UIMovableData>();
            t.text = tmpData.UIdata.activity_theme + "\n" +"宝贝数量："+ tmpData.UIdata.amount+"\n"+"活动地点："+ tmpData.UIdata.activity_site + "\n"+"活动时间：" + tmpData.UIdata.begin_time;
        }
        if (_anim)
        {
            CurrentV = Time.deltaTime * _anim_speed * Vk;
            VT = Vtotal + CurrentV;
            if (Vk > 0 && VT >= AddV) { _anim = false; CurrentV = AddV - Vtotal; }
            if (Vk < 0 && VT <= AddV) { _anim = false; CurrentV = AddV - Vtotal; }
            //==============
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Drag(CurrentV);
                if (Items[i].v - 0.5 < 0.05f)
                {
                    Current = Items[i];
                }
            }
            Check(CurrentV);
            Vtotal = VT;


            if (!_anim)
            {
                if (MoveEnd != null) { MoveEnd(Current); }
            }
        }
    }


    public void ToNext()
    {
        if (Items.Count < 5)
        {
            if (Items[0].v - VMin < 0.05)
            {
                //AnimToEnd(VMin - Items[0].v);
                return;
            }
        }

        float k = 0.5f - AddValue - Current.v;
        AnimToEnd(k);
    }
    public void ToBefore()
    {
        if (Items.Count < 5)
        {
            if (Items[Items.Count - 1].v - VMax > -0.05f)
            {
                return;
            }
        }

        float k = 0.5f + AddValue - Current.v;
        AnimToEnd(k);
    }

}