using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
public class UIAnimation : MonoBehaviour
{
    public enum Direction
    {
        upon=0,
        under=1,
        left=2,
        right=3,
        Up = 4,
    }



    public int AnimationType=0;

    public int ScriptType= 0;

    public Direction direction;

    protected Tweener Rot;
    public void SetRotate(RectTransform RectT, RectTransform RectT2, Vector3 v, float speed, TweenCallback CallBack = null)
    {
            // Vector3 v = new Vector3(0, 0, 90);
            Rot = RectT.DORotate(v, speed, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
                  RectT2.DORotate(v, speed, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
             if (CallBack!=null)
             {
                 Rot.OnComplete(CallBack);
             }
    }

    private RectTransform m_RectTransform = null;
    private HomePageScriptStyle01 tmpeScriptStyle = null;
    public void ButtonPlay(bool isStart)
    {
        if (AnimationType==0)
        {
            return;
        }
        if (isStart)
        {
            if (tmpeScriptStyle == null)
            {
                tmpeScriptStyle = new HomePageScriptStyle01((int)direction, gameObject, AnimationType, ScriptType);
            }
            else
            {
                tmpeScriptStyle.ScriptSCC01();
            }
        }
        else
        {
            tmpeScriptStyle.ScriptSCC02();
        }
    }
    private SUIMenuScriptStyle01 tmpSuiMenu = null;
    public void MenuPlay(Dictionary<string, GameObject> _MenuDictionary, bool isBase,DG.Tweening.TweenCallback callback = null)
   {
          tmpSuiMenu = new SUIMenuScriptStyle01((int)direction, _MenuDictionary, AnimationType, ScriptType, isBase, callback);
   }



  

}
