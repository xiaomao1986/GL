using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
public class TutorialProcess 
{


    private GameObject[] TutorialUIObj;
    private GameObject[] TutorialUIObj2;
    private bool IsTutorialUIObj_0=false;

    private bool IsTutorialUIObj_1 = false;


    private float Speed = 0.5f;
    public TutorialProcess(GameObject[] obj,GameObject[] obj2)
    {
        TutorialUIObj=obj;
        TutorialUIObj2 = obj2;
        EventTriggerListener.Get(TutorialUIObj[1].transform.FindChild("Button").gameObject).onClick = Tutorial_1_button;
        EventTriggerListener.Get(TutorialUIObj[2].transform.FindChild("Button").gameObject).onClick = Tutorial_2_button;
        EventTriggerListener.Get(TutorialUIObj[3].transform.FindChild("Button").gameObject).onClick = Tutorial_3_button;
        EventTriggerListener.Get(TutorialUIObj[4].transform.FindChild("Button").gameObject).onClick = Tutorial_4_button;
        EventTriggerListener.Get(TutorialUIObj[5].transform.FindChild("Button").gameObject).onClick = Tutorial_5_button;
        TutorialUIObj[0].GetComponent<RectTransform>().localScale = new Vector3(0,0,0);
        TutorialUIObj[1].GetComponent<RectTransform>().localScale = new Vector3(0,0,0);
        TutorialUIObj[2].GetComponent<RectTransform>().localScale = new Vector3(0,0,0);
        TutorialUIObj[3].GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        TutorialUIObj[4].GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        TutorialUIObj[5].GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    }
    private Tweener pos;
    public void Tutorial1(bool isActive)
    {
        if (IsTutorialUIObj_1)
        {
            return;
        }
        if (isActive)
        {
            if (IsTutorialUIObj_0 == false)
            {
                TutorialUIObj[1].SetActive(true);
                TutorialUIObj[1].GetComponent<RectTransform>().DOScale(Vector3.one, Speed);
            }
            else
            {
                pos = TutorialUIObj[0].GetComponent<RectTransform>().DOScale(Vector3.zero, Speed);
                pos.OnComplete(Tutorial_0End);
            }
        }
        else
        {
            if (IsTutorialUIObj_0 == false)
            {
                TutorialUIObj[1].SetActive(true);
                pos = TutorialUIObj[1].GetComponent<RectTransform>().DOScale(Vector3.zero, Speed);
                pos.OnComplete(Tutorial_1End);
            }
            else
            {
                TutorialUIObj[0].SetActive(true);
                TutorialUIObj[0].GetComponent<RectTransform>().DOScale(Vector3.one, Speed);
            }
        }
    }
    private void Tutorial_0End()
    {
        IsTutorialUIObj_0 = false;
    }
    private void Tutorial_1End()
    {
        IsTutorialUIObj_0 = true;
    }

    private void Tutorial_1_button(GameObject eventData)
    {
        Debug.Log("Tutorial_1_button");
        TutorialScene.Instance.isOnClickPhoto = true;
        IsTutorialUIObj_1 = true;
        pos = TutorialUIObj[1].GetComponent<RectTransform>().DOScale(Vector3.zero, Speed);
        pos.OnComplete(Tutorial_2);
    }
    private void Tutorial_2()
    {
        for (int i = 0; i < TutorialUIObj2.Length; i++)
        {
            TutorialUIObj2[i].transform.FindChild("Image1").gameObject.SetActive(false);
            TutorialUIObj2[i].transform.FindChild("Image2").gameObject.SetActive(true);
        }


        Color r= TutorialUIObj[6].GetComponent<Image>().color;
        TutorialUIObj[6].GetComponent<Image>().DOColor(new Color(r.r, r.g, r.b, 0.86f), 0.5f);

        TutorialUIObj[7].SetActive(true);
        SetTutorialui2("shagnchuang");
        TutorialUIObj[2].SetActive(true);
        TutorialUIObj[2].GetComponent<RectTransform>().DOScale(Vector3.one, Speed);
    }
    private void Tutorial_2_button(GameObject eventData)
    {
        //TutorialUIObj[2].SetActive(true);
        pos = TutorialUIObj[2].GetComponent<RectTransform>().DOScale(Vector3.zero, Speed);
        pos.OnComplete(Tutorial_2End);
    }
    private void Tutorial_2End()
    {
        SetTutorialui2("Map");
        TutorialUIObj[3].SetActive(true);
        TutorialUIObj[3].GetComponent<RectTransform>().DOScale(Vector3.one, Speed);
    }
    private void Tutorial_3_button(GameObject eventData)
    {
        //TutorialUIObj[2].SetActive(true);
        pos = TutorialUIObj[3].GetComponent<RectTransform>().DOScale(Vector3.zero, Speed);
        pos.OnComplete(Tutorial_3End);
    }
    private void Tutorial_3End()
    {
        SetTutorialui2("mine");
        TutorialUIObj[4].SetActive(true);
        TutorialUIObj[4].GetComponent<RectTransform>().DOScale(Vector3.one, Speed);
    }
    private void Tutorial_4_button(GameObject eventData)
    {
        //TutorialUIObj[2].SetActive(true);
        pos = TutorialUIObj[4].GetComponent<RectTransform>().DOScale(Vector3.zero, Speed);
        pos.OnComplete(Tutorial_4End);
    }
    private void Tutorial_4End()
    {
        TutorialUIObj[7].SetActive(false);
        TutorialUIObj[5].SetActive(true);
        TutorialUIObj[5].GetComponent<RectTransform>().DOScale(Vector3.one, Speed);
    }
    private void Tutorial_5_button(GameObject go)
    {
        pos = TutorialUIObj[5].GetComponent<RectTransform>().DOScale(Vector3.zero, Speed);
        pos.OnComplete(Tutorial_5End);
    }
    private void Tutorial_5End()
    {
        MsgBase.SendMsg("RemoveScene", "Tutorial");
    }

    private void SetTutorialui2(string  name)
    {
        for (int i = 0; i < TutorialUIObj2.Length; i++)
        {
            TutorialUIObj2[i].transform.FindChild("Image1").gameObject.SetActive(false);
            TutorialUIObj2[i].transform.FindChild("Image2").gameObject.SetActive(true);
            if (TutorialUIObj2[i].name==name)
            {
                TutorialUIObj2[i].transform.FindChild("Image1").gameObject.SetActive(true);
                TutorialUIObj2[i].transform.FindChild("Image2").gameObject.SetActive(false);
            }
        
        }
    }
}
