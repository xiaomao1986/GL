using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhotoNotReachable 
{
    private GameObject UItishi = null;
    private Callback m_Callback = null;

    public PhotoNotReachable(Callback _Callback)
    {
        m_Callback = _Callback;
        UItishi = MonoBehaviour.Instantiate(Resources.Load("UI/Photo/NotReachable")) as GameObject;
        UItishi.transform.SetParent(SUIManager.instance.g.transform);
        UItishi.transform.localPosition=Vector3.zero;
        UItishi.GetComponent<Button>().onClick.AddListener(delegate() { m_Callback(); });
        UItishi.SetActive(false);
    }

    public void Show()
    {
         UItishi.SetActive(true);
         MsgBase.SendMsg<bool,float>("ShowBlur",false,10);
    }
    public void Hide()
    {
        Debug.Log("===========Hide==============");
         UItishi.SetActive(false);
        // MsgBase.SendMsg<bool,float>("ShowBlur",true,10);
    }
     public void Delete()
     {
         m_Callback = null;
         MonoBehaviour.Destroy(UItishi);
     }
}
