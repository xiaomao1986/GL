using UnityEngine;
using System.Collections;
using com.imysky.camera;
public class MovableScene : Scene {

	// Use this for initialization

    public GameObject cameraObj;


    public int BalloonCount;
    public GameObject[] BalloonObjs;

    public static MovableScene Instance;
    public GameObject objd;
    public GameObject objd2;
    public GameObject Crossbow;

    private Arrow sArrow;
    public GameObject ArrowManager;
    public MovableManager movableManager=null;
	void Start () 
    {
        Instance = this;
       //movableManager=new MovableManager(gameObject);
       // movableManager.CreateBalloon();
       // sArrow = new Arrow();
       // ArrowManager.transform.SetParent(cameraObj.gameObject.transform);
    }
    private Vector3 BalloonV3 = Vector3.zero;
    public bool isCrossbowAnim = false;
	void Update ()
	{
        if (movableManager == null)
        {
            return;
        }
	    if (ArrowManager.activeSelf)
	    {
	        AnimatorStateInfo AnstateInfo = Crossbow.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
	        if (AnstateInfo.IsName("Crossbow"))
	        {
                Debug.Log("AnstateInfo.normalizedTime======" + AnstateInfo.normalizedTime);
                if (AnstateInfo.normalizedTime >= 0.2f && isCrossbowAnim==false)
                {
                    // Crossbow.GetComponent<Animator>().speed = 0;
                    Crossbow.GetComponent<Animator>().Play("CrossbowLoading");
                    isCrossbowAnim = true;
                }
	        }
            if (AnstateInfo.IsName("CrossbowLoading"))
            {
                if (AnstateInfo.normalizedTime >= 1.6f)
                {
                    isCrossbowAnim = false;
                    Crossbow.GetComponent<Animator>().Play("State");
                    MsgBase.SendMsg("LoadLoadArrow");
                }
            }
	    }
	    Ray ray = Camera.main.ScreenPointToRay(objd.GetComponent<RectTransform>().position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            BalloonV3 = Camera.main.ScreenToWorldPoint(objd.GetComponent<RectTransform>().position);
            Debug.DrawLine(objd2.transform.position, hit.collider.transform.position);
        }
        else
        {
            BalloonV3 = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            movableManager.arrow.fire(BalloonV3);
            BalloonV3 = Vector3.zero;
        }
	}

    public void fire()
    {
       // sArrow.fire(BalloonV3);
        movableManager.arrow.fire(BalloonV3);
        BalloonV3 = Vector3.zero;
    }
    protected override void OnOpen(Callback<float, bool> callback)
    {
        Instance = this;
        m_callback = callback;
        Load(0);
        TimeEnd = 30;
        isTimeEnd = true;
        StartCoroutine(LoadTimeEnd());
        movableManager = new MovableManager(gameObject);
        MsgBase.SendMsg<Callback>("OffGyroControllerCallback", OffGyroallback);
        MsgBase.SendMsg<bool, float>("ShowBlur", true, 10);
    }
    public int TimeEnd = 30;
    public bool isTimeEnd = false;
    private IEnumerator LoadTimeEnd()
    {
        while (isTimeEnd)
        {
            TimeEnd--;
            if (TimeEnd<=0)
            {
                TimeEnd = 30;
                MovableUImanager.Instance.LoadBackButton.gameObject.SetActive(true);
                isTimeEnd = false;
            }
            yield return  new WaitForSeconds(1);
        }
    }

    private void OffGyroallback()
    {
        // 创建弓箭 UI
        ArrowManager.transform.SetParent(SCameraManager.currentCamera.gameObject.transform.parent.gameObject.transform);
        MsgBase.SendMsg("OpenGyroController");
        movableManager.GetGps2(SkySceneManager.Instance.currentE, SkySceneManager.Instance.currentN);

    }
    protected override void OnClose(Callback callback)
    {
        movableManager.Delete();
        movableManager = null;
        Destroy(ArrowManager.gameObject);
    }
    private Callback<float, bool> m_callback=null;
    public bool IsLoadingData = false;
    private float speed = 0;

    public void Load(float t)
    {
        if (t!=0)
        {
            isTimeEnd = false;
        }
        IsLoadingData = true;
        StartCoroutine(LoadingData(t));
    }
    IEnumerator LoadingData(float t = 0)
    {
        while (speed < 99 && IsLoadingData)
        {
            speed = speed + t;
            m_callback(speed, true);
            yield return new WaitForSeconds(0.001f);
        }
    }
}
