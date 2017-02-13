using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Item : MonoBehaviour {



    public GameObject Clip;
    public GameObject Load;
    public GameObject Photo;
    public GameObject Flash;
    public GameObject Buff;
    public GameObject BoderGlow;
    public GameObject PhotoFrame;
    public GameObject Image;

    public GameObject PhotoFrame2;
    public Color tmpColor;


    public PhotoShowMesh m_PhotoShowMesh=null;

    public Texture2D[] Textures;

    private List<GameObject> ImageList = new List<GameObject>();

    private bool m_isLoad=false;
    private Renderer m_LoadRenderer;
    private bool m_isFlash = false;
    private Renderer m_LoadFlashRenderer;

    public bool isRendering = false;
    public bool isrende = true;
    void Start()
    {
        tmpColor = Photo.GetComponent<Renderer>().material.color;
        m_LoadFlashRenderer = Flash.GetComponent<Renderer>();
        m_PhotoShowMesh = Photo.GetComponent<PhotoShowMesh>();
        foreach (Transform tmp in Image.transform)
        {
            ImageList.Add(tmp.gameObject);
            tmp.gameObject.SetActive(false);
        }
    }
    private float FlashTime = 1;
    void Update()
    {

       //// isRendering = m_PhotoShowMesh.isRendering;
       // if (isRendering)
       // {
       //     FlashTime -= Time.deltaTime;
       //     if (FlashTime<0)
       //     {
       //         m_isFlash = true;
       //     }
       // }
       // else
       // {
       //     FlashTime = 1;
       // }
       // if (m_isLoad)
       // {
       //    // Vector2 v = m_LoadRenderer.material.GetTextureOffset("_MainTex");
       //     //v.x -= 0.005f;
       //     //m_LoadRenderer.material.SetTextureOffset("_MainTex", v);
       //    // Load.transform.Rotate(Vector3.forward*Time.deltaTime);
       // }
       // if (m_isFlash)
       // {
       //     Vector2 v = m_LoadFlashRenderer.material.GetTextureOffset("_MainTex");
       //     v.y += 0.1f;
       //     if (v.y >= 1)
       //     {
       //         m_isFlash = false;
       //         Flash.SetActive(false);
       //         v = Vector2.zero;
       //     }
       //     m_LoadFlashRenderer.material.SetTextureOffset("_MainTex", v);
       // }
    }
    public void isOpenLoad(bool isLoad)
    {
        if (isLoad)
        {
            Load.SetActive(true);
            m_LoadRenderer = Load.GetComponent<Renderer>();
            m_isLoad = isLoad;
        }else
        {
            m_isLoad = isLoad;
            Load.SetActive(false);
            PhotoFrame.SetActive(true);
            SetImage();
        }
    }
    private void SetImage()
    {
        if (ImageList.Count!=0)
        {
            Image.SetActive(true);
            m_LoadFlashRenderer = Flash.GetComponent<Renderer>();
            Flash.SetActive(true);
          //  m_isFlash = true;
            ImageList[Random.Range(0, ImageList.Count)].SetActive(true);
        }
    }



}
