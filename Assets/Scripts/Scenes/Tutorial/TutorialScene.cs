using UnityEngine;
using System.Collections;
using com.imysky.camera;
using DG.Tweening;
public class TutorialScene : Scene 
{

    public static TutorialScene Instance;
    public GameObject PhotoCameraObj = null;
    public GameObject Gyro = null;

    public GameObject Royboj = null;
    public GameObject[] TutorialUIObj;
    public GameObject[] TutorialUIObj2;
    public Texture2D[] Textures;
    public Texture2D[] Textures2;
    private GameObject FindConfig = null;
    private TutorialProcess2 tutorialProcess2 = null;
    public Find_Item_Config m_FindConfig= null;
    public PhotoConfigArray m_PhotoConfigArray = null;
    void Awake()
    {
        Instance = this;
    }

    private float Tutorial1Time =1f;
	void Update ()
    {
        if (tutorialProcess2 == null || PhotoCameraObj==null)
        {
            return;
        }
       
        Tutorial1Time -= Time.deltaTime;
        if (Tutorial1Time < 0)
        {
            Tutorial1Time = 0.1f;
            if (tutorialProcess2.isTutorial1 == false)
            {
                tutorialProcess2.Tutorial1(IsUserAngle());
            }
            else
            {
                tutorialProcess2.Tutorial1(true);
            }
        }
       
        Vector3 V = gameObject.transform.rotation.eulerAngles;
        Color r = Royboj.GetComponent<Renderer>().material.color;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, Speed, 0) + V);
        if (Speed == 0)
        {
            SetRoybojColor(false);
        }
        else if (Speed != 0 && m_tmpDragState != MySkyInputEvent.DragState.OnDrag)
        {
            SetRoybojColor(true);
            Royboj.transform.localRotation = Quaternion.Euler(new Vector3(0, Speed, 0) + V);
   
        }
        if (m_tmpDragState != MySkyInputEvent.DragState.OnDrag)
        {
            if (Speed > 0)
            {
                Speed -= 0.01f;
                if (Speed < 0)
                {
                    Speed = 0;
                    m_tmpDragState = MySkyInputEvent.DragState.OnDrag;
                }
            }
            else if (Speed < 0)
            {
                Speed += 0.01f;
                if (Speed > 0)
                {
                    Speed = 0;
                    m_tmpDragState = MySkyInputEvent.DragState.OnDrag;
                }
            }
            else
            {
                m_tmpDragState = MySkyInputEvent.DragState.OnDrag;
            }
        }

	}
    protected override void OnOpen(Callback<float, bool> callback)
    {
        MsgBase.SendMsg<bool, float>("ShowBlur", true, 10);
        MsgBase.SendMsg<Callback>("OffGyroControllerCallback", OffGyroallback);
        callback(0, false);
    }
    private void OffGyroallback()
    {
        PlayerPrefs.SetInt("GUIDE3D", 3);
        MsgBase.MsgAdd<GameObject, MySkyInputEvent.DragState, Vector2, Vector2, Vector3>("EventDrag", EventDrag);
        MsgBase.MsgAdd<GameObject, MySkyInputEvent.ClickState, Vector2>("EventClick", OnClickPhoto);


        PhotoCameraObj = new GameObject();
        PhotoCameraObj.name = "PhotoCameraObj1111111";
        PhotoCameraObj.transform.SetParent(SCameraManager.currentCamera.gameObject.transform.parent.gameObject.transform);



        Royboj = Instantiate(Resources.Load("photo/Rotation")) as GameObject;
        Royboj.SetActive(false);
        Royboj.transform.SetParent(PhotoCameraObj.transform);
        Royboj.transform.localPosition = new Vector3(0, -2.5f, 6.22f);

        tutorialProcess2 = new TutorialProcess2(TutorialUIObj, TutorialUIObj2);


        FindConfig = MonoBehaviour.Instantiate(Resources.Load("photo/Find_config")) as GameObject;
        FindConfig.name = "FindConfig";
        FindConfig.transform.SetParent(gameObject.transform);
        m_FindConfig = FindConfig.GetComponent<Find_Item_Config>();
        m_PhotoConfigArray = new PhotoConfigArray();
        TutorialItem tutorialItem = new TutorialItem(gameObject);
        tutorialItem.Init(20);
        MsgBase.SendMsg("OpenGyroController");
    }


    public bool isOnClickPhoto = false;
    private void OnClickPhoto(GameObject arg1, MySkyInputEvent.ClickState arg2, Vector2 arg3)
    {
        if (isOnClickPhoto)
        {
            return;
        }
        if (arg2 == MySkyInputEvent.ClickState.Down)
        {
            if (arg1!=null)
            {
                if (arg1.GetComponent<Item>()!=null)
                {
                    arg1.GetComponent<Item>().BoderGlow.SetActive(true);
                }
            }
            if (m_tmpDragState != MySkyInputEvent.DragState.OnDrag)
            {
                Speed = 0;
                m_tmpDragState = MySkyInputEvent.DragState.OnDrag;
                return;
            }
        }else
        {
            if (arg1 != null)
            {
                if (arg1.GetComponent<Item>() != null)
                {
                    uiRoot.ShowReminder("“当正式使用时，点击照片，可浏览详情、评论”");
                    arg1.GetComponent<Item>().BoderGlow.SetActive(false);
                }
            }
        }
    }
    private MySkyInputEvent.DragState m_tmpDragState = MySkyInputEvent.DragState.OnDrag;
    private float Speed = 0;
    private void EventDrag(GameObject obj, MySkyInputEvent.DragState tmpDragState, Vector2 pos, Vector2 speed, Vector3 newPos)
    {
        m_tmpDragState = tmpDragState;
        if (tmpDragState == MySkyInputEvent.DragState.OnDrag)
        {
            Royboj.SetActive(true);
            Speed = speed.x * 5f * Time.deltaTime;
            if (Speed <= -2)
            {
                Speed = -2;
            }
            else if (Speed >= 2)
            {
                Speed = 2;
            }

        }
        if (tmpDragState == MySkyInputEvent.DragState.Start)
        {
            if (obj!=null)
            {
                if (obj.GetComponent<Item>() != null)
                {
                    obj.GetComponent<Item>().BoderGlow.SetActive(true);
                    obj.GetComponent<Item>().gameObject.transform.DOKill();
                }
            }

        }
        if (tmpDragState == MySkyInputEvent.DragState.OnDrag)
        {
            if (obj != null)
            {
                if (obj.GetComponent<Item>() != null)
                {
                    obj.GetComponent<Item>().GetComponent<Animator>().speed = 0;
                    obj.GetComponent<Item>().BoderGlow.SetActive(true);
                    Royboj.SetActive(false);
                    Vector3 v = new Vector3(obj.GetComponent<Item>().transform.position.x, newPos.y, obj.GetComponent<Item>().transform.position.z);
                    obj.GetComponent<Item>().transform.position = v;
                }
            }
        }
        if (tmpDragState == MySkyInputEvent.DragState.End)
        {
            if (obj != null)
            {
                if (obj.GetComponent<Item>() != null)
                {
                    obj.GetComponent<Item>().BoderGlow.SetActive(false);
                    obj.GetComponent<Item>().gameObject.transform.DOLocalMove(Vector3.zero, 1);
                }
            }
        }
    }
    protected override void OnClose(Callback callback)
    {
        tutorialProcess2 = null;
        m_PhotoConfigArray = null;
        Destroy(PhotoCameraObj);
        MsgBase.MsgRemove<GameObject, MySkyInputEvent.DragState, Vector2, Vector2, Vector3>("EventDrag", EventDrag);
        MsgBase.MsgRemove<GameObject, MySkyInputEvent.ClickState, Vector2>("EventClick", OnClickPhoto);
        MsgBase.SendMsg<string>("OnOpenScene", "PhotoScene");
    }
    private readonly float SHOW_PHOTO_ANGLE = 50f;
    public bool IsUserAngle()
    {
        Vector3 q1 = PhotoCameraObj.transform.forward;
        // m_ArCamera.transform.forward;
        Vector3 q2 = new Vector3(0, -1f, 0);
        float angle = Vector3.Angle(q1, q2);
        //如果没有竖立起来
        if (angle < SHOW_PHOTO_ANGLE)
        {
            return false;
        }
        return true;

    }
    private void SetRoybojColor(bool isShow)
    {
        if (isShow)
        {
            Color r = Royboj.GetComponent<Renderer>().material.color;
            Royboj.SetActive(isShow);
            Royboj.GetComponent<Renderer>().material.DOColor(new Color(r.r, r.g, r.b, 1), 1);
        }
        else
        {
            Color r = Royboj.GetComponent<Renderer>().material.color;
            Royboj.GetComponent<Renderer>().material.DOColor(new Color(r.r, r.g, r.b, 0), 1);
        }
    }
}
