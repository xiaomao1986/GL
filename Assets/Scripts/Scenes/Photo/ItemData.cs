using UnityEngine;
using System.Collections;
using LitJson;
using DG.Tweening;
public class ItemData  
{
    private GameObject SceneGameObj;
    public GameObject itemRootObj;
    public Vector3 RootObjPos=Vector3.zero;
    public bool isUp = false;
    public Item item=null;

    public string nid="";

    public JsonData itemJson=null;

    public double itemGpsN;
    public double itemGpsE;

    public int width;//宽
    public int height;//高
    public string ImageHttpsite;

    public int cardType;


    private string ResourcesItmeD = "photo/Down";
    private string ResourcesItmeL = "photo/Left";



    public ItemData(JsonData jd,GameObject rootObj)
    {
        SceneGameObj = rootObj;
        itemJson = jd;
        ParseJson2();
    }

    private void ParseJson2()
    {
        if (itemJson == null)
        {
            return;
        }
        cardType = int.Parse(itemJson["cardType"].ToString());
        Debug.Log("cardType----"+ cardType);
        nid = itemJson["cardId"].ToString();
        if (cardType == 1)
        {
            item = CreateItem("photo/Down2");
            return;
        }
        if (cardType == 4)
        {
            item = CreateItem("gamelisk/box2");
            return;
        }
        if (cardType == 5)
        {
            item = CreateItem("gamelisk/hongbao");
            return;
        }
        item = CreateItem(ResourcesItmeD);
    }

    private void ParseJson()
    {
        if (itemJson==null)
        {
            return;
        }
        nid = itemJson["nid"].ToString();
        JsonData geo = itemJson["geo"];
        itemGpsN = double.Parse(geo["latitude"].ToString());
        itemGpsE = double.Parse(geo["longitude"].ToString());

        JsonData pic = itemJson["pic_ids"][0];
        ImageHttpsite = pic["uri"].ToString();
        width = int.Parse(pic["width"].ToString());
        height = int.Parse(pic["height"].ToString());
        SetShowObject(width,height);
    }
    public void SetShowObject(int width, int height)
    {
        bool isWidth = width / height >= 1f / 1f;
        if (isWidth)
        {
            item = CreateItem(ResourcesItmeL);
        }
        else
        {
            item = CreateItem(ResourcesItmeD);
        }
    }
    private Item CreateItem(string path)
    {
        GameObject tmp = MonoBehaviour.Instantiate(Resources.Load(path)) as GameObject;
        tmp.SetActive(false);
        itemRootObj = new GameObject();
        itemRootObj.name = "Photo";
        itemRootObj.transform.SetParent(SceneGameObj.transform);
        if (tmp.GetComponent<Item>() == null)
        {
            Debug.LogError("  Item 未创建 ");
            MonoBehaviour.Destroy(tmp);
            return null;
        }
        tmp.gameObject.transform.SetParent(itemRootObj.transform);
        tmp.transform.localPosition = Vector3.zero;
        tmp.transform.localRotation = Quaternion.Euler(Vector3.zero);
        return tmp.GetComponent<Item>();
    }
    public void SetItemAngle()
    {
        float ry;
        GameObject tmp = new GameObject();
        tmp.transform.localPosition = Vector3.zero;
        tmp.transform.localRotation = Quaternion.Euler(Vector3.zero);
        itemRootObj.gameObject.transform.LookAt(tmp.transform);
        ry = itemRootObj.gameObject.transform.localEulerAngles.y;
        itemRootObj.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, ry, 0));
        MonoBehaviour.Destroy(tmp);
    }
    public void OnDestroy()
    {
        MonoBehaviour.Destroy(item.gameObject.transform.parent.gameObject);
    }
    private void SetClip()
    {
      int i =  Random.Range(0, item.Textures.Length);

      item.Clip.GetComponent<Renderer>().material.mainTexture=item.Textures[i];
      int k= Random.Range(5, 15);
      if (PhotoScene.Instance.itmeClipID==0)
      {
          PhotoScene.Instance.itmeClipID = 1;
          item.Clip.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, k));
      }else
      {
          PhotoScene.Instance.itmeClipID = 0;
          item.Clip.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, (360-k)));
      }
     
      LoadImage();
    }
    public void LoadImage()
    {
        // SetRendererScale();
        //item.isOpenLoad(true);
        // string ImageID=ImageManager2.urlToId(ImageHttpsite);
        //ImageManager2.Instance.loadImage(ImageID, TextureSize_state.Texture_big, loadImageOK);


        
        if (cardType == 1)
        {
            string ImageURL1 = itemJson["cardData"]["playingGame"]["iconUrl"].ToString();
            Debug.Log("--cardType----" + itemJson.ToJson());
            PhotoScene.Instance.StartCoroutine(LoadImage2D_1(ImageURL1));
        }
        if (cardType >= 4)
        {
            return;
        }
        string ImageURL = itemJson["cardImg"].ToString();
        PhotoScene.Instance.StartCoroutine(LoadImage2D(ImageURL));
    }

    private IEnumerator LoadImage2D(string url)
    {
        WWW www = new WWW(url);

        yield return www;
        if (www.error==null)
        {

            item.Photo.GetComponent<Renderer>().material.mainTexture = www.texture;
        }
    }
    private IEnumerator LoadImage2D_1(string url)
    {
        WWW www = new WWW(url);

        yield return www;
        if (www.error == null)
        {
            item.PhotoFrame2.SetActive(true);
            item.PhotoFrame2.GetComponent<Renderer>().material.mainTexture = www.texture;
        }
    }




    public void loadImageOK(Texture2D img)
    {
        item.isOpenLoad(false);
        item.Photo.GetComponent<Renderer>().material.mainTexture = img;
    }
    public void SetRendererScale()
    {
        //设置裁切宽高
        float tw = (float)width / (float)height;
        if (tw > 4f / 3f)
        {
            float n = (4f / 3f) / tw;
            item.Photo.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(n, 1));
            item.Photo.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2((1f - n) * .5f, 0));
        }
        else if (tw <= 4f / 3f && tw >= 1f)
        {

            float n = (3f / 4f) * tw;
            item.Photo.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, n));
            item.Photo.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0f, (1f - n) * .5f));
        }
        else if (tw < 1f && tw >= 3f / 4f)
        {
            float n = (3f / 4f) / tw;
            item.Photo.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(n, 1));
            item.Photo.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2((1f - n) * .5f, 0));

        }
        else
        {
            float n = (4f / 3f) * tw;
            item.Photo.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1, n));
            item.Photo.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, (1f - n) * .5f));
        }
    }
    private int AnimInt = 0;

    public void AnimatorPlay2()
    {
        item.GetComponent<Animator>().speed = 1;
        item.GetComponent<Animator>().Play("PhotoRotation0" + AnimInt.ToString());
    }

    public void AnimatorPlay()
    {
        int k=Random.Range(1, 5);
        if (AnimInt==0)
        {
            AnimInt = k;
            item.GetComponent<Animator>().speed = 1;
            item.GetComponent<Animator>().Play("PhotoRotation0" + k.ToString());
        }else
        {
            item.GetComponent<Animator>().speed = 1;
        }
    }
    public void AnimatorStop()
    {
        item.GetComponent<Animator>().speed = 0;
    }
    public void SetPos(Vector3 pos)
    {
        RootObjPos = pos;
        pos.y = pos.y;
        itemRootObj.gameObject.transform.localPosition = pos;
        //  SetClip();
        LoadImage();
        SetItemAngle();
    }

    private Tweener pos;
    public void Appearanc()
    {
        if (cardType>3)
        {
            item.gameObject.SetActive(false);
        }
        else
        {
            item.gameObject.SetActive(true);
        }
      // item.transform.localPosition = new Vector3(0, 20, 0);
       //pos = item.transform.DOLocalMoveY(-2,1);
      // pos.OnComplete(EndAppearanc);
    }
    private void EndAppearanc()
    {
       // AnimatorPlay();
    }

    private Tweener Automaticpot;
    public void ItemRotate()
    {
        Debug.Log("---ItemRotate----");
        Vector3 v = new Vector3(0, 360, 0);
       Automaticpot= item.transform.parent.gameObject.transform.DORotate(v, 2f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
       Automaticpot.OnComplete(AnimatorPlay);
    }
}
