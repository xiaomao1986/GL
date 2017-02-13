using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SUILoading : MonoBehaviour {

    public Text LoadingText;

    public static SUILoading CreateLoading()
     {
        GameObject UiLoading = Instantiate(Resources.Load("UI/Loading/loading")) as GameObject;
        UiLoading.transform.SetParent(SUIManager.instance.g.transform);
        UiLoading.GetComponent<RectTransform>().localScale = Vector3.one;
        UiLoading.GetComponent<RectTransform>().localPosition = Vector3.zero;
        UiLoading.transform.SetSiblingIndex(0);
        UiLoading.SetActive(false);
        return UiLoading.GetComponent<SUILoading>();
     }
    public void SetLoadingText(string num)
    {
        gameObject.SetActive(true);
        string s= num+"%";
        LoadingText.text= s;
        if(float.Parse(num)>99f)
        {
            gameObject.SetActive(false);
        }

    }
}
