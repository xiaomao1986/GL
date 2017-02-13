using UnityEngine;
using System.Collections;

public class TutorialItem
{
  

    private GameObject SceneGameObj;
     public GameObject itemRootObj;
     private string ResourcesItmeD = "photo/Down";
     private string ResourcesItmeL = "photo/Left";


      public TutorialItem(GameObject obj)
     {
         SceneGameObj = obj;
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
     private int p = 0;
     private int itmeClipID = 0;
    public void Init(int ItemSum)
    {
        for (int i = 0; i < ItemSum; i++)
        {
            Item tmpItem = null;
            if (p==0)
            {
               tmpItem = CreateItem(ResourcesItmeD);
               SetRendererScale(tmpItem, 400, 512);
               p = 1;
            }
            else
            {
                tmpItem = CreateItem(ResourcesItmeL);
                SetRendererScale(tmpItem, 512, 512);
                p =0;
            }

            Vector3 v = TutorialScene.Instance.m_PhotoConfigArray.GetPos(i);
            tmpItem.gameObject.transform.parent.gameObject.transform.localPosition = v;
            tmpItem.gameObject.SetActive(true);

            float ry;
            GameObject tmp = new GameObject();
            tmp.transform.localPosition = Vector3.zero;
            tmp.transform.localRotation = Quaternion.Euler(Vector3.zero);
            tmpItem.gameObject.transform.parent.gameObject.transform.LookAt(tmp.transform);
            ry = tmpItem.gameObject.transform.parent.gameObject.transform.localEulerAngles.y;
            tmpItem.gameObject.transform.parent.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, ry, 0));
            MonoBehaviour.Destroy(tmp);

            int k=Random.Range(0,TutorialScene.Instance.Textures.Length);
            int h = Random.Range(0, TutorialScene.Instance.Textures2.Length);
            if (p == 0)
            {
                tmpItem.Photo.GetComponent<Renderer>().material.mainTexture = TutorialScene.Instance.Textures2[k];
            }else
            {
                tmpItem.Photo.GetComponent<Renderer>().material.mainTexture = TutorialScene.Instance.Textures[h];
            }
        
            tmpItem.Load.SetActive(false);
            tmpItem.PhotoFrame.SetActive(true);
            int k1=Random.Range(1, 5);
            tmpItem.GetComponent<Animator>().Play("PhotoRotation0" + k1.ToString());
            tmpItem.isrende = false;


            int i1 = Random.Range(0, tmpItem.Textures.Length);

            tmpItem.Clip.GetComponent<Renderer>().material.mainTexture = tmpItem.Textures[i1];
            int k5 = Random.Range(5, 15);
            if (itmeClipID == 0)
            {
                itmeClipID = 1;
                tmpItem.Clip.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, k5));
            }
            else
            {
                itmeClipID = 0;
                tmpItem.Clip.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, (360 - k5)));
            }

		}
    
    }
    public void SetRendererScale(Item item, float width, float height)
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
}
