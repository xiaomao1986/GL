using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SUIMenuhelpScriptStyle01
{
    private GameObject obj = null;
    public SUIMenuhelpScriptStyle01(GameObject _obj)
    {
        obj = _obj;
    }
    public void Show()
    {
        obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        obj.SetActive(true);
        //obj.GetComponent<RectTransform>().DOLocalMoveY(0, 0.5f);
        
    }
    public void Quit()
    {
        obj.SetActive(false);
        Debug.Log("help04Evnet--------Quit-------");
       // obj.GetComponent<RectTransform>().DOLocalMoveY(1926, 0.5f);
    }
}
