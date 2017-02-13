using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
public class SUIMenuScriptStyle01 : ScriptStyle 
{

    private TweenCallback callback=null;
    private bool isBase;
    public SUIMenuScriptStyle01(int _direction, Dictionary<string, GameObject> _objDictionary, int _AnimationType, int _ScriptType, bool _isBase, DG.Tweening.TweenCallback _callback)
        : base(_direction, _objDictionary, _AnimationType, _ScriptType)
    {
        isBase = _isBase;
        callback = _callback;
        vpos = new Vector3[objDictionary.Count];
        ScriptSCC01(ScriptType);
    }

    private Vector3[] vpos;
    private int i=0;
    private Tweener pos;
    private void ScriptSCC01(int i)
    {

        if (isBase)
        {

            foreach (var item in objDictionary)
            {
                if (item.Key != "Backround")
                {
                    item.Value.GetComponent<RectTransform>().SetParent(objDictionary["Backround"].transform);
                    item.Value.GetComponent<RectTransform>().localScale = Vector3.one;
                    
                    vpos[i] = item.Value.GetComponent<RectTransform>().localPosition;
                    i++;
                    item.Value.GetComponent<RectTransform>().DOLocalMoveX(200,0.1f);
                }
            }
            i = 0;
            objDictionary["Backround"].AddComponent<Mask>();
            objDictionary["Backround"].GetComponent<RectTransform>().localPosition = new Vector3(0, -760, 0);
            objDictionary["Backround"].SetActive(true);
            pos = objDictionary["Backround"].GetComponent<RectTransform>().DOLocalMoveY(0, 0.2f);
            pos.OnComplete(Start);

        }else
        {
            pos= objDictionary["Backround"].GetComponent<RectTransform>().DOLocalMoveY(-760, 0.2f);
            pos.OnComplete(BackroundBack);
        }
    
    }
    private void BackroundBack()
    {
        foreach (var item in objDictionary)
        {
            if (item.Key != "Backround")
            {
                item.Value.SetActive(false);
            }
        }
        callback();
    }

    private void Start()
    {
       // objDictionary["Backround"].GetComponent<RectTransform>().DOPunchAnchorPos(new Vector2(0,2), 1);
        SkySceneManager.Instance.StartCoroutine(ButtonMove());
    }

   IEnumerator ButtonMove()
    {
        foreach (var item in objDictionary)
        {
            if (item.Key != "Backround")
            {
                i++;
               
                item.Value.SetActive(true);
                if (i==(vpos.Length-1))
                {
                   pos = item.Value.GetComponent<RectTransform>().DOLocalMoveX(vpos[i].x, 0.2f);
                   pos.OnComplete(PunchAnchorPos);
                }else
                {
                    item.Value.GetComponent<RectTransform>().DOLocalMoveX(vpos[i].x, 0.2f);
                }
                
            }
            yield return new WaitForSeconds(0.1f);
        }

        
    }

   private void PunchAnchorPos()
   {
       foreach (var item in objDictionary)
       {
           if (item.Key != "Backround")
           {
               item.Value.GetComponent<RectTransform>().DOPunchAnchorPos(new Vector2(1, 0), 0.5f);
           }
          
       }
   }

}
