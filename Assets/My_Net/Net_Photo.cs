using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using ProtoBuf;
using LitJson;
/// <summary>
/// 自 定义 数据 
/// </summary>
[ProtoContract]
public class My_PhotoData
{
    [ProtoMember(1)]
    public float x;
    [ProtoMember(2)]
    public float y;
    [ProtoMember(3)]
    public float z;

    [ProtoMember(4)]
    public float GpsN;
    [ProtoMember(5)]
    public float GpsE;
    [ProtoMember(6)]
    public string objName;
    [ProtoMember(7)]
    public string TrackableName;
}


/// <summary>
/// 自 定义 事件 枚举
/// </summary>
public enum EventCode : byte
{
    Vector3 = 10,
    pos = 11,
    ARcameraPot = 12,
    Init = 13,
    OnDrag = 14,
    OnTrackingFound = 15,

}
public static class Net_Photo 
{

    /// <summary>
    /// 定制 自己模块的 消息发送
    /// </summary>
    public static void SendPhoto(EventData _eventData, byte[] _My_DataBytes,GameObject obj)
    {
        switch (_eventData.Code)
        {
            case (byte)EventCode.Vector3:
                {
                    My_PhotoData tmpDebugGameobj = My_Common.DeSerialize<My_PhotoData>(_My_DataBytes);
                    Debug.Log("123-----" + tmpDebugGameobj.objName.ToString());
                   
                }
                break;
            case (byte)EventCode.OnTrackingFound:
                {
                
                }
                break;
            default:
                break;
        }


    }



}
