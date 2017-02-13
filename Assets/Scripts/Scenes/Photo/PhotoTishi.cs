using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class PhotoTishi 
{

    private GameObject tishiUI = null;
    public PhotoTishi(GameObject obj)
    {
        tishiUI = obj;
    }

    public void Show(bool isUI)
    {
        Color rgbImage = tishiUI.transform.FindChild("Image").GetComponent<Image>().color;
        Color rgbText = tishiUI.transform.FindChild("Text").GetComponent<Text>().color;
        if (!isUI)
	    {
            tishiUI.SetActive(true);
            tishiUI.transform.FindChild("Image").GetComponent<Image>().DOColor(new Color(rgbImage.r, rgbImage.g, rgbImage.b, 0.8f), 0.5f);
            tishiUI.transform.FindChild("Text").GetComponent<Text>().DOColor(new Color(rgbText.r, rgbText.g, rgbText.b, 1f), 0.5f);
	    }else
        {
            tishiUI.transform.FindChild("Image").GetComponent<Image>().DOColor(new Color(rgbImage.r, rgbImage.g, rgbImage.b,0f), 0.5f);
            tishiUI.transform.FindChild("Text").GetComponent<Text>().DOColor(new Color(rgbText.r, rgbText.g, rgbText.b, 0f), 0.5f);
        }
        
    }

    
  
}
