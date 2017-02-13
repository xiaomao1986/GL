using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
public class SpecialEffectsUI2 : MonoBehaviour {


    private Tweener Scale;
    private Callback callback = null;
    private GameObject SpreadOut02 = null;

    private GameObject ImageObj = null;
    public void Init()
    {
        
        SpreadOut02 = gameObject.transform.FindChild("SpreadOut02").gameObject;
        ImageObj = gameObject.transform.FindChild("ImageObj").gameObject;
        SpreadOut02.SetActive(false);
        ImageObj.SetActive(false);
        Color p = ImageObj.GetComponent<Image>().color;
        ImageObj.GetComponent<Image>().color = new Color(p.r, p.g, p.b, 0.1f);
        ImageObj.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //ImageObj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }
    public void clickShow(Callback _callback)
    {
        callback = _callback;
        SpreadOut02.SetActive(true);
        gameObject.SetActive(true);
        ImageObj.SetActive(true);
        Color p = ImageObj.GetComponent<Image>().color;
        ImageObj.GetComponent<Image>().color = new Color(p.r, p.g, p.b, 0.5f);
        ImageObj.GetComponent<RectTransform>().localScale = new Vector3(1.5f,1.5f, 1.5f);
        SpreadOut02.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 0.1f);


        SpreadOut02.gameObject.GetComponent<RectTransform>().DOScale(new Vector3(1.5f, 1.5f, 0), 0.3f);
        Scale = ImageObj.gameObject.GetComponent<RectTransform>().DOScale(new Vector3(0.1f, 0.1f, 0), 0.3f);
        Scale.OnComplete(Move);
    }
    private void Move()
    {
        SpreadOut02.SetActive(false);

        ImageObj.SetActive(true);
        Color p = ImageObj.GetComponent<Image>().color;
        ImageObj.GetComponent<Image>().DOColor(new Color(p.r, p.g, p.b, 0.8f), 1f);
        ImageObj.gameObject.GetComponent<RectTransform>().DOScale(new Vector3(3f, 3f, 0), 0.5f);

        Scale = gameObject.GetComponent<RectTransform>().DOLocalMove(new Vector3(540, -960, 0), 0.5f);
        Scale.OnComplete(ShowSetScale);
    }
    private void ShowSetScale()
    {
        Color p = ImageObj.GetComponent<Image>().color;
        ImageObj.GetComponent<Image>().DOColor(new Color(p.r, p.g, p.b, 1f), 1f);
        Scale = ImageObj.GetComponent<RectTransform>().DOScale(new Vector3(30, 30, 30), 0.6f);
        Scale.OnComplete(ShowEnd);
    }
    private void ShowEnd()
    {
        if (callback != null)
        {
            callback();
        }
    }
    public void clickHide(Callback _callback)
    {
        callback = _callback;
        Color p = ImageObj.GetComponent<Image>().color;
        ImageObj.GetComponent<Image>().DOColor(new Color(p.r, p.g, p.b, 0.1f), 1f);
        Scale = ImageObj.GetComponent<RectTransform>().DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.6f);
        Scale.OnComplete(HideEnd);
    }
    private void HideEnd()
    {
        gameObject.SetActive(false);
        if (callback != null)
        {
            callback();
        }
    }


}
