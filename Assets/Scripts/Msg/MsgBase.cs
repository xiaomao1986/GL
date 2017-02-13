using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class MsgBase
{
    public static void SendMsg(string eventType)
    {
        Messenger.Broadcast(eventType);
    }
    public static void SendMsg<T>(string eventType, T arg1)
    {
        Messenger.Broadcast<T>(eventType, arg1);
    }
    public static void SendMsg<T, U>(string eventType, T arg1, U arg2)
    {
        Messenger.Broadcast<T, U>(eventType, arg1, arg2);
    }
    public static void SendMsg<T, U, V>(string eventType, T arg1, U arg2, V arg3) 
    {
        Messenger.Broadcast<T, U, V>(eventType,arg1,arg2,arg3);
    }
    public static void SendMsg<T, U, V, W>(string eventType, T arg1, U arg2, V arg3, W arg4) 
    {
        Messenger.Broadcast<T, U, V, W>(eventType, arg1, arg2,arg3,arg4);
    }
    public static void SendMsg<T, U, V, W, X>(string eventType, T arg1, U arg2, V arg3, W arg4, X arg5)
    {
        Messenger.Broadcast<T, U, V, W, X>(eventType, arg1, arg2, arg3, arg4, arg5);
    }
    public static void SendMsg<T, U, V, W, X,Y, Z>(string eventType, T arg1, U arg2, V arg3, W arg4, X arg5,Y arg6,Z arg7)
    {
        Messenger.Broadcast<T, U, V, W, X, Y, Z>(eventType, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static void MsgAdd(string eventType, Callback MsgCallback)
    {
        Messenger.AddListener(eventType,MsgCallback);
    }
    public static void MsgAdd<T>(string eventType, Callback<T> MsgCallback)
    {
        Messenger.AddListener<T>(eventType, MsgCallback);
    }
    public static void MsgAdd<T, U>(string eventType, Callback<T, U> MsgCallback)
    {
        Messenger.AddListener<T, U>(eventType, MsgCallback);
    }
    public static void MsgAdd<T, U, V>(string eventType,Callback<T,U,V> MsgCallback) 
    {
        Messenger.AddListener<T, U, V>(eventType, MsgCallback);
    }
    public static void MsgAdd<T, U, V, W>(string eventType, Callback<T, U, V, W> MsgCallback)
    {
        Messenger.AddListener<T, U, V, W>(eventType, MsgCallback);
    }
    public static void MsgAdd<T, U, V, W, X>(string eventType, Callback<T, U, V, W, X> MsgCallback)
    {
        Messenger.AddListener<T, U, V, W, X>(eventType, MsgCallback);
    }
    public static void MsgAdd<T, U, V, W, X, Y>(string eventType, Callback<T, U, V, W, X, Y> MsgCallback)
    {
        Messenger.AddListener<T, U, V, W, X, Y>(eventType, MsgCallback);
    }
    public static void MsgAdd<T, U, V, W, X, Y, Z>(string eventType, Callback<T, U, V, W, X, Y, Z> MsgCallback)
    {
        Messenger.AddListener<T, U, V, W, X, Y, Z>(eventType, MsgCallback);
    }
    public static void MsgAdd<T, U, V, W, X, Y, Z, T2>(string eventType, Callback<T, U, V, W, X, Y, Z, T2> MsgCallback)
    {
        Messenger.AddListener<T, U, V, W, X, Y, Z, T2>(eventType, MsgCallback);
    }





    public static void MsgRemove(string eventType, Callback MsgCallback)
    {
        Messenger.RemoveListener(eventType, MsgCallback);
    }
    public static void MsgRemove<T>(string eventType, Callback<T> MsgCallback)
    {
        Messenger.RemoveListener<T>(eventType, MsgCallback);
    }
    public static void MsgRemove<T, U>(string eventType, Callback<T, U> MsgCallback)
    {
        Messenger.RemoveListener<T, U>(eventType, MsgCallback);
    }
    public static void MsgRemove<T, U, V>(string eventType, Callback<T, U, V> MsgCallback)
    {
        Messenger.RemoveListener<T, U, V>(eventType, MsgCallback);
    }
    public static void MsgRemove<T, U, V, W>(string eventType, Callback<T, U, V, W> MsgCallback)
    {
        Messenger.RemoveListener<T, U, V, W>(eventType, MsgCallback);
    }
    public static void MsgRemove<T, U, V, W, X>(string eventType, Callback<T, U, V, W, X> MsgCallback)
    {
        Messenger.RemoveListener<T, U, V, W, X>(eventType, MsgCallback);
    }
    public static void MsgRemove<T, U, V, W, X, Y>(string eventType, Callback<T, U, V, W, X, Y> MsgCallback)
    {
        Messenger.RemoveListener<T, U, V, W, X, Y>(eventType, MsgCallback);
    }
    public static void MsgRemove<T, U, V, W, X, Y, Z>(string eventType, Callback<T, U, V, W, X, Y, Z> MsgCallback)
    {
        Messenger.RemoveListener<T, U, V, W, X, Y, Z>(eventType, MsgCallback);
    }

    public static void MsgRemove<T, U, V, W, X, Y, Z, T2>(string eventType, Callback<T, U, V, W, X, Y, Z, T2> MsgCallback)
    {
       Messenger.RemoveListener<T, U, V, W, X, Y, Z, T2>(eventType, MsgCallback);
    }

 

    public static void ShowUI()
    {
    
        MsgBase.SendMsg("HomePageMagShowUI");
    }
    public static void QuitUI()
    {
        MsgBase.SendMsg("HomePageMagQuitUI");
        MsgBase.SendMsg("BaseButtonEvnet2");
    }
    public static void QuitBaseUI()
    {
        MsgBase.SendMsg("BaseButtonEvnet2");
    }
}