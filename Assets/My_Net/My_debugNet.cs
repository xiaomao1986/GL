
using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Lite;
using System.Collections.Generic;
using ProtoBuf;
using System;

//传递的数据类型

public class My_debugNet : MonoBehaviour,IPhotonPeerListener
{
  
	// Use this for initialization
    public static LitePeer peer;


    public GameObject obj;
    private enum Debuglog:byte
    {

        Log=230
    }
   
	void Start () 
    {
        peer = new LitePeer(this, ConnectionProtocol.Udp);
        peer.Connect("192.168.1.116:5055", "MyDebugNet");
	}
	
	// Update is called once per frame
    int timeSpanMs = 100;
    int nextSendTickCount = Environment.TickCount;
	void Update () 
    {
        if (Environment.TickCount > nextSendTickCount)
        {
            peer.Service();
            nextSendTickCount = Environment.TickCount + timeSpanMs;
        }
	} 

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log("message==="+message);
    }


    /// <summary>
    ///   248-255 房间事件占用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEvent(EventData eventData)
    {
        if (eventData.Code>=248&&eventData.Code<=255)
        {
            
        }else
        {
            Net_Photo.SendPhoto(eventData, GetDataBytes(eventData),obj);
        }
    }
    private byte[] GetDataBytes(EventData eventData)
    {
        Debug.Log("==GetDataBytes=" + eventData.Code.ToString());
        Hashtable custonEventContent = eventData.Parameters[LiteEventKey.Data] as Hashtable;
        byte[] dataBytes = (byte[])custonEventContent[(byte)eventData.Code];
        return dataBytes;
    }
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        Debug.Log("" + operationResponse.ToStringFull());
        switch (operationResponse.OperationCode)
        {
            default:
                break;
        }
    }

    public void SendMessage(My_debug_Code code,Dictionary<byte, object> para)
    {
        peer.OpCustom((byte)code,para, true);
   
    }

    /// <summary>
    /// 向服务器发送 DebugLog
    /// </summary>
    public static void SendDebugLog(string Log)
    {
        Dictionary<byte, object> para = new Dictionary<byte, object>();
        para[(byte)Debuglog.Log] = Log;
        peer.OpCustom((byte)Debuglog.Log, para, true);
    }
    string message;
    void OnGUI()
    {
        //GUI.Label(new Rect(Screen.width / 2 - 50, 200, 150, 20), message);

        //if (GUI.Button(new Rect(Screen.width / 2 - 50, 300, 100, 30), "发送消息"))
        //{
        //    TextAsset HomePageJson = (TextAsset)Resources.Load("HomePage");
        //    My_PhotoData data = new My_PhotoData();
        //    data.objName = HomePageJson.text;




        //    SendEvent<My_PhotoData>(data, EventCode.Vector3);
        //    SendDebugLog("2222222222222222");
        //}

    }

    public void Login()
    {
        Hashtable gameProperties, actorroperties;
        gameProperties = new Hashtable();
        actorroperties = new Hashtable();
        actorroperties.Add((byte)My_debug_Code.Name, "4111111116");
        peer.OpJoin("qqqqqqqqqqqq", gameProperties, actorroperties, true);
    }

    /// <summary>
    /// 向服务器发送 事件消息  广播模式
    /// </summary>

    public static void SendEvent<T>(T _My_Data, EventCode _EventCode)
    {
        Hashtable chatContent = new Hashtable();
        chatContent.Add((byte)_EventCode, My_Common.Serialize<T>(_My_Data));
        peer.OpRaiseEvent((byte)_EventCode, chatContent, true);
    }
    public void OnStatusChanged(StatusCode statusCode)
    {
        switch (statusCode)
        {
            case StatusCode.Connect:
                Debug.Log("连接成功！");
                Login();
                break;
            case StatusCode.Disconnect:

                break;
            case StatusCode.DisconnectByServer:
                break;
            case StatusCode.DisconnectByServerLogic:
                break;
            case StatusCode.DisconnectByServerUserLimit:
                break;
            case StatusCode.EncryptionEstablished:
                break;
            case StatusCode.EncryptionFailedToEstablish:
                break;
            case StatusCode.Exception:
                break;
            case StatusCode.ExceptionOnConnect:
                break;
            case StatusCode.InternalReceiveException:
                break;
            case StatusCode.QueueIncomingReliableWarning:
                break;
            case StatusCode.QueueIncomingUnreliableWarning:
                break;
            case StatusCode.QueueOutgoingAcksWarning:
                break;
            case StatusCode.QueueOutgoingReliableWarning:
                break;
            case StatusCode.QueueOutgoingUnreliableWarning:
                break;
            case StatusCode.QueueSentWarning:
                break;
            case StatusCode.SecurityExceptionOnConnect:
                break;
            case StatusCode.SendError:
                break;
            case StatusCode.TcpRouterResponseEndpointUnknown:
                break;
            case StatusCode.TcpRouterResponseNodeIdUnknown:
                break;
            case StatusCode.TcpRouterResponseNodeNotReady:
                break;
            case StatusCode.TcpRouterResponseOk:
                break;
            case StatusCode.TimeoutDisconnect:
                break;
            default:
                break;
        }
    }
}