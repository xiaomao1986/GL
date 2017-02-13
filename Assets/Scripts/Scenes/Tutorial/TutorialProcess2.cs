using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
public class TutorialProcess2
{

    public GameObject[] TutorialUIObj;
    public GameObject[] TutorialUIObj2;
    private float Speed = 0.5f;
    public TutorialProcess2(GameObject[] _TutorialUIObj, GameObject[] _TutorialUIObj2)
    {
        TutorialUIObj = _TutorialUIObj;
        TutorialUIObj2 = _TutorialUIObj2;
        TutorialUIObj[0].SetActive(true);
        EventTriggerListener.Get(TutorialUIObj[0].transform.FindChild("Button").gameObject).onClick = Tutorial_0_button;
        EventTriggerListener.Get(TutorialUIObj[1].transform.FindChild("Button").gameObject).onClick = Tutorial_1_button;
        EventTriggerListener.Get(TutorialUIObj[2].transform.FindChild("Button").gameObject).onClick = Tutorial_2_button;
        EventTriggerListener.Get(TutorialUIObj[3].transform.FindChild("Button").gameObject).onClick = Tutorial_3_button;
        EventTriggerListener.Get(TutorialUIObj[4].transform.FindChild("Button").gameObject).onClick = Tutorial_4_button;

        EventTriggerListener.Get(TutorialUIObj[6].transform.FindChild("Button").gameObject).onClick = Tutorial_6_button;
        EventTriggerListener.Get(TutorialUIObj[7].transform.FindChild("Button").gameObject).onClick = Tutorial_7_button;

        TutorialUIObj[1].GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        TutorialUIObj[2].GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        TutorialUIObj[3].GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        TutorialUIObj[6].GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
        TutorialUIObj[7].GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    }

    private void Tutorial_7_button(GameObject go)
    {
         SetTutorialui2("shagnchuang");
         TutorialUIObj[7].SetActive(false);
         SetTutorialUIObj(1);
    }

    private void Tutorial_6_button(GameObject go)
    {
        TutorialUIObj[6].SetActive(false);
        SetTutorialUIObj(7);
    }

    private void Tutorial_4_button(GameObject go)
    {
        MsgBase.SendMsg("RemoveScene", "Tutorial");
    }

    private void Tutorial_3_button(GameObject go)
    {

       // TutorialUIObj[5].SetActive(false);
        TutorialUIObj[3].SetActive(false);
        SetTutorialui2("2");
        SetTutorialUIObj(4);
    }

    private void Tutorial_2_button(GameObject go)
    {
        SetTutorialui2("mine");
        TutorialUIObj[2].SetActive(false);
        SetTutorialUIObj(3);
     }
    private void Tutorial_1_button(GameObject go)
    {
        SetTutorialui2("Map");
        TutorialUIObj[1].SetActive(false);

        SetTutorialUIObj(2);

    }
    private void Tutorial_0_button(GameObject go)
    {
        for (int i = 0; i < TutorialUIObj2.Length; i++)
        {
            TutorialUIObj2[i].transform.FindChild("Image1").gameObject.SetActive(false);
            TutorialUIObj2[i].transform.FindChild("Image2").gameObject.SetActive(true);
        }
        SetTutorialui2("2");
       // SetTutorialui2("shagnchuang");
        TutorialUIObj[0].GetComponent<Animator>().Play("Start");
        TutorialUIObj[0].SetActive(false);


        SetTutorialUIObj(6);

        TutorialUIObj[5].SetActive(true);

    }

    public bool isTutorial1 = false;
    public bool isTutorial12 = false;
    public void Tutorial1(bool isActive)
    {
        if (isTutorial1 && isActive)
        {
            TutorialUIObj[0].GetComponent<Animator>().Play("Tutorial2Return");
            return;
        }
            if (isActive)
            {
                TutorialUIObj[0].GetComponent<Animator>().Play("Tutorial2Return");
                if (isTutorial12)
                {
                   isTutorial1 = true;
                }
               isTutorial12 = true;
            }
            else
            {
                TutorialUIObj[0].GetComponent<Animator>().Play("Tutorial2");
            }

    }

    private void SetTutorialui2(string name)
    {
        for (int i = 0; i < TutorialUIObj2.Length; i++)
        {
            TutorialUIObj2[i].transform.FindChild("Image1").gameObject.SetActive(false);
            TutorialUIObj2[i].transform.FindChild("Image2").gameObject.SetActive(true);
            if (TutorialUIObj2[i].name == name)
            {
                TutorialUIObj2[i].transform.FindChild("Image1").gameObject.SetActive(true);
                TutorialUIObj2[i].transform.FindChild("Image2").gameObject.SetActive(false);
            }

        }
    }

    private void SetTutorialUIObj(int i)
    {
        TutorialUIObj[i].SetActive(true);
        TutorialUIObj[i].GetComponent<RectTransform>().DOScale(Vector3.one, Speed);
        Color tmpColor = TutorialUIObj[i].transform.FindChild("Image").GetComponent<Image>().color;
        TutorialUIObj[i].transform.FindChild("Image").GetComponent<Image>().DOColor(new Color(tmpColor.r, tmpColor.g, tmpColor.b, 1), Speed);
        if (i!=4)
        {
            Color tmpColor2 = TutorialUIObj[i].transform.FindChild("Image2").GetComponent<Image>().color;
            TutorialUIObj[i].transform.FindChild("Image2").GetComponent<Image>().DOColor(new Color(tmpColor.r, tmpColor.g, tmpColor.b, 1), Speed);
        }

        Color tmpColor3 = TutorialUIObj[i].transform.FindChild("Button").transform.FindChild("Image").GetComponent<Image>().color;
        TutorialUIObj[i].transform.FindChild("Button").transform.FindChild("Image").GetComponent<Image>().DOColor(new Color(tmpColor.r, tmpColor.g, tmpColor.b, 1), Speed);
    }

}
