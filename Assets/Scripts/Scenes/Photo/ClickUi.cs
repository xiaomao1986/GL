using UnityEngine;
using System.Collections;

public class ClickUi
{

    public SpecialEffectsUI2 specialEffectsUI2 = null;
    public GameObject ClickUiObj = null;

    public ClickUi(GameObject photoUIobj)
    {
        ClickUiObj = MonoBehaviour.Instantiate(Resources.Load("UI/ClickUI")) as GameObject;
        ClickUiObj.SetActive(false);
        ClickUiObj.transform.SetParent(photoUIobj.transform);
        ClickUiObj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
       // ClickUiObj.GetComponent<RectTransform>().localPosition = Vector3.zero;
        specialEffectsUI2 = ClickUiObj.AddComponent<SpecialEffectsUI2>();
        specialEffectsUI2.Init();
    }
    public void Delete()
    {
        specialEffectsUI2 = null;
        MonoBehaviour.Destroy(ClickUiObj);

    }
}
