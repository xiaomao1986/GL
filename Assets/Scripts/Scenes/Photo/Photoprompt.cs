using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
public class Photoprompt 
{
    private GameObject UItishi = null;
    private GameObject UItishi2=null;

    private GameObject UItishi3 = null;
    private GameObject UItishi4 = null;
    private ItemManager itemManager;

    private Color[] colors3=new Color[6];
    private Color[] colors4 = new Color[6];
    public void Delete()
    {
        MonoBehaviour.Destroy(UItishi);
        MonoBehaviour.Destroy(UItishi2);
        MonoBehaviour.Destroy(UItishi3);
        MonoBehaviour.Destroy(UItishi4);
    }


    public Photoprompt(ItemManager _itemManager)
    {
        itemManager = _itemManager;
        UItishi = MonoBehaviour.Instantiate(Resources.Load("UI/Photo/tishi")) as GameObject;
        UItishi.transform.SetParent(SUIManager.instance.g.transform);
        UItishi.GetComponent<RectTransform>().localScale = Vector3.one;
        UItishi.GetComponent<RectTransform>().localPosition = Vector3.zero;
        UItishi.transform.SetSiblingIndex(1);
        UItishi.SetActive(false);

        UItishi2 = MonoBehaviour.Instantiate(Resources.Load("UI/Photo/zuoyoutishi")) as GameObject;
        UItishi2.transform.SetParent(SUIManager.instance.g.transform);
        UItishi2.GetComponent<RectTransform>().localScale = Vector3.one;
        UItishi2.GetComponent<RectTransform>().localPosition = Vector3.zero;
        UItishi2.transform.SetSiblingIndex(2);

        UItishi3 = MonoBehaviour.Instantiate(Resources.Load("UI/Photo/huadongtishi")) as GameObject;
        UItishi3.transform.SetParent(SUIManager.instance.g.transform);
        UItishi3.GetComponent<RectTransform>().localScale = Vector3.one;
        UItishi3.GetComponent<RectTransform>().localPosition = Vector3.zero;
        UItishi3.SetActive(false);

        EventTriggerListener.Get(UItishi3.transform.FindChild("Button").gameObject).onClick = UItishi3Event;
        SetUItishi3Color(0,0,0.1f);

        UItishi4 = MonoBehaviour.Instantiate(Resources.Load("UI/Photo/tuodongtishi")) as GameObject;
        UItishi4.transform.SetParent(SUIManager.instance.g.transform);
        UItishi4.GetComponent<RectTransform>().localScale = Vector3.one;
        UItishi4.GetComponent<RectTransform>().localPosition = Vector3.zero;
        UItishi4.SetActive(false);
        EventTriggerListener.Get(UItishi4.transform.FindChild("Button").gameObject).onClick = UItishi4Event;
        SetUItishi4Color(0,0, 0.1f);

    }

    private void UItishi4Event(GameObject go)
    {
        SetUItishi4Color(0f, 0,0.5f);
        PhotoScene.Instance.jiaocheng = 0;
        PhotoScene.Instance.IsClickPhtoto = false;
        UItishi3.transform.SetSiblingIndex(2);
        UItishi4.transform.SetSiblingIndex(2);
        PlayerPrefs.SetInt("3DStart", 3);
        MsgBase.SendMsg<bool>("ISslide", false);
        UItishi4.SetActive(false);
    }
    private void UItishi3Event(GameObject go)
    {
        PhotoScene.Instance.jiaocheng = 2;
        SetUItishi4Color(0.6f,1, 0.5f);
        UItishi4.SetActive(true);
        ShowUItishi3(false);
        UItishi3.SetActive(false);
    }
    public IEnumerator SetPrompt()
    {
        while (true)
        {
            if (itemManager.IsSvreenPhoto() == false && PhotoScene.Instance.IsUserAngle() && itemManager.GetItmeDataObjectCount()!=0)
            {
                UItishi2.SetActive(true);
                if (itemManager.Findphotos() == 1)
                {
                    Color re = UItishi2.transform.FindChild("Image1").GetComponent<Image>().color;
                    UItishi2.transform.FindChild("Image1").GetComponent<Image>().DOColor(new Color(re.r, re.g, re.b, 1), 0.5f);
                    Color re1 = UItishi2.transform.FindChild("Image2").GetComponent<Image>().color;
                    UItishi2.transform.FindChild("Image2").GetComponent<Image>().DOColor(new Color(re.r, re.g, re.b, 0), 0.5f);
                }
                else
                {
                    Color re = UItishi2.transform.FindChild("Image1").GetComponent<Image>().color;
                    UItishi2.transform.FindChild("Image1").GetComponent<Image>().DOColor(new Color(re.r, re.g, re.b, 0), 0.5f);
                    Color re1 = UItishi2.transform.FindChild("Image2").GetComponent<Image>().color;
                    UItishi2.transform.FindChild("Image2").GetComponent<Image>().DOColor(new Color(re.r, re.g, re.b, 1), 0.5f);
                }
            }
            else
            {
                Color re = UItishi2.transform.FindChild("Image1").GetComponent<Image>().color;
                UItishi2.transform.FindChild("Image1").GetComponent<Image>().DOColor(new Color(re.r, re.g, re.b, 0), 0.5f);
                Color re1 = UItishi2.transform.FindChild("Image2").GetComponent<Image>().color;
                UItishi2.transform.FindChild("Image2").GetComponent<Image>().DOColor(new Color(re.r, re.g, re.b, 0), 0.5f);
            }
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(1);
        }
    }
    public void Show(bool isUI)
    {
        Color rgbImage = UItishi.transform.FindChild("Image").GetComponent<Image>().color;
        Color rgbText = UItishi.transform.FindChild("Text").GetComponent<Text>().color;
        if (!isUI)
        {
            UItishi.SetActive(true);
            MsgBase.SendMsg<bool,float>("ShowBlur",false,10);
            UItishi.gameObject.GetComponent<Animator>().enabled = true;
            UItishi.transform.FindChild("Image").GetComponent<Image>().DOColor(new Color(rgbImage.r, rgbImage.g, rgbImage.b,1f), 0.5f);
            //UItishi.transform.FindChild("Text").GetComponent<Text>().DOColor(new Color(rgbText.r, rgbText.g, rgbText.b, 1f), 0.5f);
        }
        else
        {
            MsgBase.SendMsg<bool, float>("ShowBlur", true, 10);
            UItishi.gameObject.GetComponent<Animator>().enabled = false;
           UItishi.transform.FindChild("Image").GetComponent<Image>().DOColor(new Color(rgbImage.r, rgbImage.g, rgbImage.b, 0f), 0.5f);
           //UItishi.transform.FindChild("Text").GetComponent<Text>().DOColor(new Color(rgbText.r, rgbText.g, rgbText.b, 0f), 0.5f);
        }
    }

    public void Off()
    {
        UItishi.SetActive(false);
        UItishi2.SetActive(false);
    }


    public void ShowUItishi3(bool isShow)
    {
        if (PlayerPrefs.GetInt("3DStart")!=2)
        {
            SetUItishi4Color(0f, 0, 0.1f);
            return;
        }
        if (isShow)
        {
            UItishi3.SetActive(true);
 
            SetUItishi3Color(0.6f,1, 0.5f);
        }else
        {
            SetUItishi3Color(0,0, 0.5f);
        }
    }
	private void SetUItishi3Color(float v,float texta,float speed)
    {
        colors3[0] = UItishi3.transform.FindChild("Image").GetComponent<Image>().color;
        UItishi3.transform.FindChild("Image").GetComponent<Image>().DOColor(new Color(colors3[0].r, colors3[0].g, colors3[0].b, v), speed);

        colors3[1] = UItishi3.transform.FindChild("Button").transform.FindChild("Image").GetComponent<Image>().color;
        UItishi3.transform.FindChild("Button").transform.FindChild("Image").GetComponent<Image>().DOColor(new Color(colors3[1].r, colors3[1].g, colors3[1].b, texta), speed);

        colors3[5] = UItishi3.transform.FindChild("Button").transform.FindChild("Text").GetComponent<Text>().color;
		UItishi3.transform.FindChild("Button").transform.FindChild("Text").GetComponent<Text>().DOColor(new Color(colors3[5].r, colors3[5].g, colors3[5].b, texta), speed);

        colors3[2] = UItishi3.transform.FindChild("Text").GetComponent<Image>().color;
		UItishi3.transform.FindChild("Text").GetComponent<Image>().DOColor(new Color(colors3[2].r, colors3[2].g, colors3[2].b, texta), speed);

        colors3[3] = UItishi3.transform.FindChild("zuo").GetComponent<Image>().color;
		UItishi3.transform.FindChild("zuo").GetComponent<Image>().DOColor(new Color(colors3[3].r, colors3[3].g, colors3[3].b, texta), speed);

    }
	private void SetUItishi4Color(float v,float texta,float speed)
    {
		colors4[0] = UItishi4.transform.FindChild("Image").GetComponent<Image>().color;
        UItishi4.transform.FindChild("Image").GetComponent<Image>().DOColor(new Color(colors4[0].r, colors4[0].g, colors4[0].b, v), speed);

        colors4[1] = UItishi4.transform.FindChild("Button").transform.FindChild("Image").GetComponent<Image>().color;
		UItishi4.transform.FindChild("Button").transform.FindChild("Image").GetComponent<Image>().DOColor(new Color(colors4[1].r, colors4[1].g, colors4[1].b, texta), speed);

		colors4[5] = UItishi4.transform.FindChild("Button").transform.FindChild("Text").GetComponent<Text>().color;
		UItishi4.transform.FindChild("Button").transform.FindChild("Text").GetComponent<Text>().DOColor(new Color(colors4[5].r, colors4[5].g, colors4[5].b, texta), speed);

		colors4[2] = UItishi4.transform.FindChild("Text").GetComponent<Image>().color;
		UItishi4.transform.FindChild("Text").GetComponent<Image>().DOColor(new Color(colors4[2].r, colors4[2].g, colors4[2].b, texta), speed);

		colors4[3] = UItishi4.transform.FindChild("shangxia").GetComponent<Image>().color;
		UItishi4.transform.FindChild("shangxia").GetComponent<Image>().DOColor(new Color(colors4[2].r, colors4[2].g, colors4[2].b, texta), speed);
		     
    }
}
