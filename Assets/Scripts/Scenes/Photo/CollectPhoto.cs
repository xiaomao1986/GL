using UnityEngine;
using System.Collections;
using com.imysky.camera;
using DG.Tweening;
public class CollectPhoto
{
    private GameObject CollectIn = null;
    private GameObject CollectOut = null;
    public SpecialEffectsUI specialEffectsUI;

    private Color[] OutColors=new Color[4];
    private int i = 1;
    public CollectPhoto(SpecialEffectsUI _SpecialEffectsUI, GameObject PhotoCameraObj)
    {
        specialEffectsUI = _SpecialEffectsUI;
        CollectIn =MonoBehaviour.Instantiate(Resources.Load("photo/CollectIn")) as GameObject;
        CollectOut = MonoBehaviour.Instantiate(Resources.Load("photo/CollectOut")) as GameObject;
        CollectIn.SetActive(false);
        CollectOut.SetActive(false);
        CollectIn.name = "CollectIn";
        CollectOut.name = "CollectOut";
        CollectIn.transform.SetParent(PhotoCameraObj.transform);
        CollectOut.transform.SetParent(PhotoCameraObj.transform);
        CollectIn.transform.localPosition = new Vector3(0, 0, 8);
        CollectOut.transform.localPosition = new Vector3(0, 0, 8);
        OutColors[0] = CollectOut.GetComponent<ParticleSystem>().startColor;
        foreach (Transform item in CollectOut.transform)
        {
            OutColors[i] = item.GetComponent<ParticleSystem>().startColor;
            i++;
        }
    }
    public void Open()
    {
        MsgBase.QuitUI();
        // CollectIn.SetActive(true);

         PhotoScene.Instance.PhotoInvoke(1, Show);
    }
    public void Off()
    {
      //  CollectOut.SetActive(true);
        Debug.Log(" OutColors[0]" + OutColors[0].a.ToString());
        CollectOut.GetComponent<ParticleSystem>().startColor = OutColors[0];
        int p = 1;
        foreach (Transform item in CollectOut.transform)
        {

            item.GetComponent<ParticleSystem>().startColor = OutColors[p];
            p++;
        }
        specialEffectsUI.clickHide(Hide);
    }
    private IEnumerator SetCollectOutobjColor()
    {
        float t = CollectOut.GetComponent<ParticleSystem>().startColor.a;
        while (t>0)
        {
            t = t - 0.1f;
            Color r = CollectOut.GetComponent<ParticleSystem>().startColor;
            CollectOut.GetComponent<ParticleSystem>().startColor = new Color(r.r, r.g, r.b,t);
            foreach (Transform item in CollectOut.transform)
            {
                Color r1 = item.GetComponent<ParticleSystem>().startColor;
                item.GetComponent<ParticleSystem>().startColor = new Color(r1.r, r1.g, r1.b, t);
            }
            yield return new WaitForSeconds(0.1f);
        }

    }


    private void Show()
    {
        
        specialEffectsUI.clickShow(SendNaitveMsg);
    }
    private void Hide()
    {
        PhotoScene.Instance.PhotoInvoke(0.01f, SendPhotoMsg);
    }

    private void SendNaitveMsg()
    {
        CollectIn.SetActive(false);
        MsgBase.SendMsg<Callback<bool, string>>("U2N_U_OpenNearby",null);
    }
    private void SendPhotoMsg()
    {
        Debug.Log("   SendPhotoMsg");
       PhotoScene.Instance.StartCoroutine(SetCollectOutobjColor());
        //CollectOut.SetActive(false);
        MsgBase.ShowUI();
    }
    public void NearbyBack()
    {
        Debug.Log("  NearbyBack");
        Off();
    }

    public void Delete()
    {
       MonoBehaviour.Destroy(CollectIn);
       MonoBehaviour.Destroy(CollectOut);
       specialEffectsUI = null;
    }

}
