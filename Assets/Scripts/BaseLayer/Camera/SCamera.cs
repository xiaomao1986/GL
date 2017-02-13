using UnityEngine;
using System.Collections;
using System;

namespace com.imysky.camera
{
    public enum SCameraFlashType
    {
        OPEN = 0,
        CLOSE,
        AUTO
    }

    public enum SCameraFocusMode
    {
        Normal = 0,
        Triggerauto = 1,
        Continousauto = 2,
        Infinity = 3,
        Macro = 4,
    }
    public enum SCameraHardwareType
    {
        DEFAULT = 0, 
        FRONT,       //后置摄像头
        BACK         //前置摄像头
    }

    public enum SCameraErrorCode
    {
        SUCCEED = 0,
        ERROR,       //错误
        TIME_OUT         //超时
    }

    /// <summary>
    /// 内存拍照返回函数
    /// </summary>
    /// <param name="isok">拍照是否成功</param>
    /// <param name="tex">返回的图片</param>
    public delegate void SCameraPhotographMemoryCallBack(bool isok, Texture2D tex);
    /// <summary>
    /// 文件拍照返回函数
    /// </summary>
    /// <param name="isok">拍照是否成功</param>
    /// <param name="filename">返回的图片本地路径</param>
    public delegate void SCameraPhotographFileCallBack(bool isok, string filename);

    /// <summary>
    /// 相机开启回调
    /// </summary>
    /// <param name="errorCode">错误号</param>
    public delegate void SCameraStartCallBack(SCameraErrorCode errorCode);

    /// <summary>
    /// 文件拍照返回函数
    /// </summary>
    /// <param name="isok">拍照是否成功</param>
    /// <param name="filename">返回的图片本地路径</param>
    public delegate void SCameraStopCallBack(SCameraErrorCode errorCode);
    public abstract class SCamera : System.Object
    {

        /// <summary>
        /// 相机的gameObject
        /// </summary>
        public abstract GameObject gameObject { get; }
        /// <summary>
        /// 获取相机对象
        /// </summary>
        public abstract Camera camera { get; }
        /// <summary>
        /// 获取设置闪光灯类型
        /// </summary>
        public abstract SCameraFlashType flashType { get; set; }

        /// <summary>
        /// 开启相机
        /// </summary>
        /// <returns>成功失败</returns>
        public abstract bool Start(SCameraStartCallBack callback = null);
        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <returns>成功失败</returns>
        public abstract bool Stop(SCameraStopCallBack callback = null);
        /// <summary>
        /// 设置相机对焦模式
        /// </summary> 
        /// <param mode="mode">对焦模式</param> 
        public abstract void SetFocusMode(SCameraFocusMode mode);

        /// <summary>
        /// 把图片拍摄到内存中
        /// </summary>
        /// <param name="callback">回调函数</param>
        public abstract void PhotographMemory(SCameraPhotographMemoryCallBack callback);

        /// <summary>
        /// 把图片拍摄到文件中
        /// </summary>
        /// <param name="callback">回调函数</param>
        public abstract void PhotographFile(SCameraPhotographFileCallBack callback);
        /// <summary>
        /// 设置不同的摄像头（前后摄像头）
        /// </summary>
        /// <param name="type">摄像头类型</param>
        public abstract void SelectCamera(SCameraHardwareType type);


    }
}