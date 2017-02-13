using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LitJson;

public class U2NTest : MonoBehaviour {

    public Button b1;
    public Button b2;
    public Button b3;
    public Button b4;
    public Button b5;
    public Button b6;
    public Button b7;
    public Text t1;
    public Text t2;
    public Text t3;
    public Text t4;
    public Text t5;
    public Text t6;
    public Text t7;

    public Text showText;

    private string showstring = "";

	// Use this for initialization
	void Start () {
        SU2NConnection.Create();

        b1.onClick.AddListener( delegate()
        {
            Messenger.Broadcast<Callback<bool, string>>("U2N_U_OpenMainMenu",
                delegate (bool isErr, string err){
                    t1.text = "isErr:" + isErr + " err:" + err;
                });
        });
        b2.onClick.AddListener(delegate()
        {
            Messenger.Broadcast<Callback<bool, string>>("U2N_U_OpenNearby",
                delegate(bool isErr, string err)
                {
                    t2.text = "isErr:" + isErr + " err:" + err;
                });
        });
        b3.onClick.AddListener(delegate()
        {
            Messenger.Broadcast<string, Callback<bool, string>>("U2N_U_OpenStory", "aaaaaaaa",
                delegate( bool isErr, string err)
                {
                    t3.text = "isErr:" + isErr + " err:" + err;
                });
        });
        b4.onClick.AddListener(delegate()
        {
            Messenger.Broadcast<Callback<bool, string>>("U2N_U_OpenMap",
                delegate(bool isErr, string err)
                {
                    t4.text = "isErr:" + isErr + " err:" + err;
                });
        });
        b5.onClick.AddListener(delegate()
        {
            Messenger.Broadcast<Callback<bool, string>>("U2N_U_OpenLogin",
                delegate(bool isErr, string err)
                {
                    t5.text = "isErr:" + isErr + " err:" + err;
                });
        });
        b6.onClick.AddListener(delegate()
        {
            Messenger.Broadcast<Callback<bool, string>>("U2N_U_OpenSendStory",
                delegate(bool isErr, string err)
                {
                    t6.text = "isErr:" + isErr + " err:" + err;
                });
        });
        b7.onClick.AddListener(delegate()
        {
            Messenger.Broadcast<Callback<bool, string>>("U2N_U_GPS",
                delegate(bool isErr, string err)
                {
                    t7.text = "isErr:" + isErr + " err:" + err;
                });
        });

        Messenger.AddListener("U2N_N_CloseMainMenu", delegate()
        {
            showstring += "U2N_N_CloseMainMenu /n";
            showText.text = showstring;
        });
        Messenger.AddListener("U2N_N_CloseNearby", delegate()
        {
            showstring += "U2N_N_CloseNearby /n";
            showText.text = showstring;
        });
        Messenger.AddListener("U2N_N_CloseStory", delegate()
        {
            showstring += "U2N_N_CloseStory /n";
            showText.text = showstring;
        });
        Messenger.AddListener("U2N_N_CloseMap", delegate()
        {
            showstring += "U2N_N_CloseMap /n";
            showText.text = showstring;
        });
        Messenger.AddListener("U2N_N_Cheese", delegate()
        {
            showstring += "U2N_N_Cheese /n";
            showText.text = showstring;
            Messenger.Broadcast<string ,Callback<bool, string>>("U2N_U_CheeseBack","url:xxxxx",
                delegate(bool isErr, string err)
                {
                    t4.text = "isErr:" + isErr + " err:" + err;
                });
        });
        Messenger.AddListener<bool, string>("U2N_N_CloseLogin", delegate(bool isOk, string errorCode)
        {
            showstring += "U2N_N_CloseLogin /n";
            showText.text = showstring;
        });
        Messenger.AddListener<JsonData>("U2N_N_CloseSendStory", delegate(JsonData json)
        {
            showstring += "U2N_N_CloseSendStory /n";
            showText.text = showstring;
        });
        Messenger.AddListener<bool , string , System.DateTime , float , float , float , float , float>
            ("U2N_N_GPS", delegate(bool isOk, string errorCode, System.DateTime time, float longitude, float latitude, float altitude, float LAccuracy, float HAccuracy)
        {
            showstring += "U2N_N_GPS isOk" + isOk + ",errorCode:" + errorCode + ",time:" + time + ",longitude:" + longitude + ",latitude:" + latitude + ",altitude:" + altitude + ",LAccuracy:" + LAccuracy + ",HAccuracy:" + HAccuracy;
            showstring += "/n";
            showText.text = showstring;
        });
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("KeyCode.Alpha0 s:" + Messenger.Broadcast_R<bool, string>("XXXX", "AAAAAaaaaa"));
        }
	}
}
