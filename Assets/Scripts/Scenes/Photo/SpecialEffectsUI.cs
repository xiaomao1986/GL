using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
public class SpecialEffectsUI : MonoBehaviour {

    private Tweener scale;

    private Callback callback=null;
	void Start () 
	{

	}
	public void Init()
    {
        gameObject.SetActive(false);
        GetComponent<RectTransform>().localPosition = Vector3.zero;
        Color p = GetComponent<Image>().color;
        GetComponent<Image>().color=new Color(p.r,p.g,p.b,0.1f);
        GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
    public void clickShow(Callback _callback)
    {
        GetComponent<RectTransform>().DOKill();
        callback = _callback;
        gameObject.SetActive(true);
        Color p = GetComponent<Image>().color;
        GetComponent<Image>().DOColor(new Color(p.r, p.g, p.b, 1f), 1f);
        scale = GetComponent<RectTransform>().DOScale(new Vector3(30, 30, 30), 0.6f);
        scale.OnComplete(ShowEnd);
    }
    public void clickHide(Callback _callback)
    {
       GetComponent<RectTransform>().DOKill();
       callback = _callback;
       Color p = GetComponent<Image>().color;
       GetComponent<Image>().DOColor(new Color(p.r, p.g, p.b,0.1f), 1f);
	   scale = GetComponent<RectTransform>().DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.6f);
       scale.OnComplete(HideEnd);
    }
    private void ShowEnd()
    {
        if (callback!=null)
        {
            callback();
        }
    }
    private void HideEnd()
    {
        gameObject.SetActive(false);
        if (callback!=null)
        {
            callback();
        }
    }
    public void UpShow()
    {
        gameObject.SetActive(true);
        GetComponent<RectTransform>().localScale = new Vector3(30, 30, 30);

 

        scale = GetComponent<RectTransform>().DOScale(new Vector3(0.1f, 0.1f, 0.1f), 1);
        scale.OnComplete(HideEnd);
    }
}
