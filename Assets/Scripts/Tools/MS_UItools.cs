using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public enum UI_Anchor
{
    top_left = 1,
    top_center = 2,
    top_right = 3,
    top_stretch = 4,
    middle_left = 5,
    middle_center = 6,
    middle_right = 7,
    middle_stretch = 8,
    bottiom_left = 9,
    bottiom_center = 10,
    bottiom_right = 11,
    bottiom_stretch = 12,
    stretch_left = 13,
    stretch_center = 14,
    stretch_right = 15,
    stretch_stretch = 16,
}
public class MS_UItools  
{

	// Use this for initialization

    public static void SetRectTransform(GameObject g, Vector3 Pos, Vector2 sizeDelta)
    {
         if (g.GetComponent<RectTransform>()==null)
	     {
           //  MSDebug.LogError("1001" + "RectTransform错误！！！");
             return;
	     }
         g.GetComponent<RectTransform>().localPosition = Pos;
         g.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }
    /// <summary>
    ///      用于UI的 锚点设置  
    /// </summary>
    /// <param name="g"> GameObject </param>
    /// <param name="anchors"> 枚举 </param>
    public static void SetAnchor(GameObject g, UI_Anchor anchors)
    {


        if (g.GetComponent<RectTransform>() == null)
        {
           // MSDebug.LogError("1001" + "RectTransform错误！！！");
            return;
        }
        switch (anchors)
        {
                case UI_Anchor.top_left:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                 case UI_Anchor.top_center:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                case UI_Anchor.top_right:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                case UI_Anchor.top_stretch:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                case UI_Anchor.middle_left:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                case UI_Anchor.middle_center:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                case UI_Anchor.middle_right:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0.5f);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.5f);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                case UI_Anchor.middle_stretch:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0.5f);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0.5f);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                case UI_Anchor.bottiom_left:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 0f);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                case UI_Anchor.bottiom_center:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0f);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0f);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                case UI_Anchor.bottiom_right:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0f);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0f);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                case UI_Anchor.bottiom_stretch:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 0f);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                case UI_Anchor.stretch_left:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 1f);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                 case UI_Anchor.stretch_center:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0f);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                 case UI_Anchor.stretch_right:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(1f, 0f);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
                case UI_Anchor.stretch_stretch:
                {
                    g.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
                    g.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
                    g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                }
                break;
        }
    }
    /// <summary>
    ///       用于UI的 锚点设置 可输入 锚点
    /// </summary>
    /// <param name="g"></param>
    /// <param name="anchorMin"></param>
    /// <param name="anchorMax"></param>
    /// <param name="pivot"></param>
    public static void SetAnchor2(GameObject g, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
    {
        if (g.GetComponent<RectTransform>() == null)
        {
           // MSDebug.LogError("1001" + "RectTransform错误！！！");
            return;
        }
        g.GetComponent<RectTransform>().anchorMin = anchorMin;
        g.GetComponent<RectTransform>().anchorMax = anchorMax;
        g.GetComponent<RectTransform>().pivot = pivot;
    }
    /// <summary>
    /// 设置 Image
    /// </summary>
    /// <param name="g"></param>
    /// <param name="sprite"></param>
    /// <param name="color"></param>
    /// <param name="mat"></param>
    /// <param name="Aspect"></param>
    public static void SetImage(GameObject g, Sprite sprite, Color color, Material mat, bool Aspect)
    {
        if (g.GetComponent<Image>() == null)
        {
          //  MSDebug.LogError("1002" + "Image错误！！！");
            return;
        }
        g.GetComponent<Image>().sprite = sprite;
        g.GetComponent<Image>().color = color;
        g.GetComponent<Image>().material = mat;
        g.GetComponent<Image>().preserveAspect = Aspect;
    }
    /// <summary>
    ///  用于修改 ScrollRect 目前 只支持 
    /// </summary>
    /// <param name="g"></param>
    /// <param name="content"></param>
    /// <param name="Ishorizontal"></param>
    /// <param name="Isvertical"></param>
    public static void SetScrollRect(GameObject g, GameObject content, bool Ishorizontal, bool Isvertical)
    {
        if (g.GetComponent<ScrollRect>() == null)
        {
          //  MSDebug.LogError("1003" + "ScrollRect错误！！！");
            return;
        }
        g.GetComponent<ScrollRect>().content = content.GetComponent<RectTransform>();
        g.GetComponent<ScrollRect>().horizontal = Ishorizontal;
        g.GetComponent<ScrollRect>().vertical = Isvertical;
    }

    public static void SetGrid(GameObject g, Vector2 cellSize, Vector2 spacing)
    {
        if (g.GetComponent<GridLayoutGroup>() == null)
        {
           // MSDebug.LogError("1004" + "GridLayoutGroup错误！！！");
            return;
        }
        g.GetComponent<GridLayoutGroup>().cellSize = cellSize;
        g.GetComponent<GridLayoutGroup>().spacing = spacing;

    }
    public static void Create_UI_Scrillobj(GameObject g)
    {
        g.AddComponent<RectTransform>();
        g.AddComponent<Image>();
        g.AddComponent<ScrollRect>();
        g.AddComponent<Mask>();
    }
    public static void  Create_UI_Gridobj(GameObject g)
    {
        g.AddComponent<RectTransform>();
        //g.AddComponent<MSGiad>();
    }
    public static void Create_UI_Gridobj()
    {




    }
         
}
