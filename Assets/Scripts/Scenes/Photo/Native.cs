using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LitJson;

public class Native : MonoBehaviour
{



    public static Native Instance;
    public Text T;
#if UNITY_ANDROID
        private AndroidJavaClass javaClass;
        private AndroidJavaObject javaObject;
#endif
    void Awake()
    {
        Instance = this;
    }


    // Use this for initialization
    void Start()
    {
#if UNITY_ANDROID
        try
        {
            javaClass = new AndroidJavaClass("com.imopan.ar.unity.Unity_Receive");
            javaObject = javaClass.CallStatic<AndroidJavaObject>("getInstance");
        }
        catch (System.Exception e)
        {
        }
#endif
        JsonData jd4= new JsonData();
        jd4["Event"] = "Start";
       // SendNative(jd4);
    }
    public bool isHttpUpDate = true;
    public void NativeToUnity(string s)
    {
        Debug.Log("   安卓传递过来的消息   --------"+s);
        //T.text = "安卓传递过来的消息===" + s;
        JsonData jd = JsonMapper.ToObject(s);
        string NativeEvent = jd["Event"].ToString();
        switch (NativeEvent)
        {
            case "EventClickItme":
                //  GLItemManager.Instance.ClickBakc();
                PhotoScene.Instance.itemManager.StoryCallback();
                break;

            case "photograph":
            {
                if (jd["status"].ToString()=="0")
                {
                        // GLUImanager.Instance.gameObject.transform.FindChild("UI").gameObject.SetActive(true);
                     PhotoScene.Instance.itemManager.UpPhoto(false, jd);
                }
                else
                {
                     PhotoScene.Instance.itemManager.UpPhoto(true, jd);
                }
            }
                break;
            case "Screenshots":
               // GLItemManager.Instance.ClickBakc();
                break;
            case "nearby":
              //  GLItemManager.Instance.EventnearbyBack();
                break;
            case "NearScene":
                // GLItemManager.Instance.LoadJson(jd);
                PhotoScene.Instance.itemManager.LoadJsonData(jd);
                break;
        }
    }
#if UNITY_IOS
    [System.Runtime.InteropServices.DllImport("__Internal")]
    public static extern void Unity_CallBack(string jsStr);
#endif
    public void SendNative(JsonData jd)
    {
        Debug.Log("SendNative====="+jd.ToJson());
#if UNITY_ANDROID
        javaObject.Call<string>("Unity_CallBack", jd.ToJson());
#endif
#if UNITY_IOS
       Unity_CallBack(jd.ToJson());
#endif
    }


}
