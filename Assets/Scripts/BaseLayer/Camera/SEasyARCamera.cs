using UnityEngine;
using System.Collections;
using EasyAR;
using System.IO;

namespace com.imysky.camera
{
    public class SEasyARCamera : SCamera
    {
        private const string KEY = "RbebkeVUoxNtlOIrY5JyafPIH96YvVOSjkVBM3RKHvq1MjyFE9s2hVUlIahy51IPpR0xaQ76OmitoCXjwBPv0VYGjqNI1Ba1Ljo6034f3cfae70f2c57b3452267d4d8d4ca1csER80RhcH0OXjeKZY9jq4AU4ZN33hivcog2HZ4Osaagtvu5sjcprwLOEJkkGVIYI8P";
        private SEasyARCamera(){}
        private GameObject m_gameObject;
        private Camera m_camera;
        private SCameraFlashType m_flashType = SCameraFlashType.CLOSE;
        private CameraDeviceBehaviour m_cameraDeviceBehaviour = null;

        /// <summary>
        /// 创建相机
        /// </summary>
        /// <returns></returns>
        internal static SEasyARCamera create(SCameraStartCallBack callback = null)
        {
            GameObject camera_object = (GameObject)MonoBehaviour.Instantiate(Resources.Load("EasyAR_Startup"));
            camera_object.AddComponent<SEasyARCameraComponent>();
            camera_object.GetComponent<EasyARBehaviour>().Key = KEY;

            SEasyARCamera camera = new SEasyARCamera();
            camera.m_gameObject = camera_object;
            camera.m_camera = camera_object.transform.FindChild("Augmenter").
                FindChild("RenderCamera").gameObject.GetComponent<Camera>();
            camera.m_cameraDeviceBehaviour = camera_object.transform.FindChild("CameraDevice").gameObject.GetComponent<CameraDeviceBehaviour>();
            
            //等相机完全开启后的回调
            if (callback != null)
                SCameraManager.instance.StartCoroutine(camera.StartCallBack(callback));
            return camera;
        }

        /// <summary>
        /// 相机的gameObject
        /// </summary>
        public override GameObject gameObject { get { return m_gameObject; } }
        /// <summary>
        /// 获取相机对象
        /// </summary>
        public override Camera camera { get { return m_camera; } }
        /// <summary>
        /// 获取设置闪光灯类型
        /// </summary>
        public override SCameraFlashType flashType { get { return m_flashType; } set {m_flashType = value; } }

        /// <summary>
        /// 开启相机
        /// </summary>
        /// <returns>成功失败</returns>
        public override bool Start(SCameraStartCallBack callback = null)
        {
            //m_cameraDeviceBehaviour.Device.Stop();
            m_cameraDeviceBehaviour.OpenAndStart();
            //if (callback != null)
            //    SCameraManager.instance.StartCoroutine(StartCallBack(callback));
            return true;
        }
        private IEnumerator StartCallBack(SCameraStartCallBack callback)
        {
            int time = 0;
            while (true)
            {
                if(m_cameraDeviceBehaviour.Device.IsOpened)
                {
                    callback(SCameraErrorCode.SUCCEED);
                    break;
                }
                else
                {
                    if (time >= 60) { callback(SCameraErrorCode.TIME_OUT); break; }
                    time++;
                }
                yield return new WaitForFixedUpdate();
            }
        }
        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <returns>成功失败</returns>
        public override bool Stop(SCameraStopCallBack callback = null)
        {
            m_cameraDeviceBehaviour.Close();
            //m_cameraDeviceBehaviour.Device.Stop();
            return true;
            //bool m_return;

            //m_cameraDeviceBehaviour.Close();
            //m_cameraDeviceBehaviour.Device.Stop();
            //m_cameraDeviceBehaviour.Close();
            //m_return = m_cameraDeviceBehaviour.Device.Close();
            //if (callback != null)
            //    SCameraManager.instance.StartCoroutine(StopCallBack(callback));
            //return m_return;
        }
        private IEnumerator StopCallBack(SCameraStopCallBack callback)
        {
            int time = 0;
            while (true)
            {
                yield return new WaitForFixedUpdate();
                if (!m_cameraDeviceBehaviour.Device.IsOpened)
                {
                    callback(SCameraErrorCode.SUCCEED);
                    break;
                }
                else
                {
                    if (time >= 60) { callback(SCameraErrorCode.TIME_OUT); break; }
                    time++;
                }
            }
        }

        /// <summary>
        /// 设置相机对焦模式
        /// </summary>
        /// <param name="mode"></param>
        public override void SetFocusMode(SCameraFocusMode mode)
        {
            //m_cameraDeviceBehaviour
            switch(mode)
            {
                case SCameraFocusMode.Continousauto:
                    m_cameraDeviceBehaviour.Device.SetFocusMode(CameraDevice.FocusMode.Continousauto);
                    break;
                case SCameraFocusMode.Infinity:
                    m_cameraDeviceBehaviour.Device.SetFocusMode(CameraDevice.FocusMode.Infinity);
                    break;
                case SCameraFocusMode.Macro:
                    m_cameraDeviceBehaviour.Device.SetFocusMode(CameraDevice.FocusMode.Macro);
                    break;
                case SCameraFocusMode.Normal:
                    m_cameraDeviceBehaviour.Device.SetFocusMode(CameraDevice.FocusMode.Normal);
                    break;
                case SCameraFocusMode.Triggerauto:
                    m_cameraDeviceBehaviour.Device.SetFocusMode(CameraDevice.FocusMode.Triggerauto);
                    break;
            }
        }

        /// <summary>
        /// 把图片拍摄到内存中
        /// </summary>
        /// <param name="callback">回调函数</param>
        public override void PhotographMemory(SCameraPhotographMemoryCallBack callback)
        {
            if (m_flashType == SCameraFlashType.OPEN)
            {
                m_cameraDeviceBehaviour.Device.SetFlashTorchMode(true);
            }
            SCameraManager.instance.StartCoroutine(PhotographMemory2(callback));
        }
        private IEnumerator PhotographMemory2(SCameraPhotographMemoryCallBack callback)
        {
            if (m_flashType == SCameraFlashType.OPEN)
                yield return new WaitForSeconds(0.5f);

            // 先创建一个的空纹理，大小可根据实现需要来设置  
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            // 读取屏幕像素信息并存储为纹理数据，  
            texture.ReadPixels(new Rect(0,0,Screen.width,Screen.height), 0, 0);
            texture.Apply();
            
            callback(true, texture);

            yield return new WaitForSeconds(0.5f);
            m_cameraDeviceBehaviour.Device.SetFlashTorchMode(false);
        }

        /// <summary>
        /// 把图片拍摄到文件中
        /// </summary>
        /// <param name="callback">回调函数</param>
        public override void PhotographFile(SCameraPhotographFileCallBack callback)
        {
            PhotographMemory(delegate(bool isOK, Texture2D tex) {
                try
                {
                    string baseFile = "/photo_" + System.DateTime.Now.ToString("yyyy_MMdd_hhssmm") + ".png";
                    string file = Application.persistentDataPath + baseFile;
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                    FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
                    byte[] b = tex.EncodeToPNG();
                    fs.Write(b, 0, b.Length);
                    fs.Close();
                    callback(true, file);
                }
                catch (System.Exception e)
                {
                    callback(false, e.ToString());
                }
            });
        }
        private IEnumerator PhotographFile2(SCameraPhotographFileCallBack callback)
        {
            if (m_flashType == SCameraFlashType.OPEN)
                yield return new WaitForSeconds(0.5f);
            string baseFile = "/photo_" + System.DateTime.Now.ToString("yyyy_MMdd_hhmm") + ".png";
            Debug.LogError("baseFile" + baseFile);
            string file = Application.persistentDataPath + baseFile;
            if (File.Exists(file))
            {
                File.Delete(file);
            }
            Application.CaptureScreenshot(baseFile, 0);
            callback(true, file);

            yield return new WaitForSeconds(0.5f);
            m_cameraDeviceBehaviour.Device.SetFlashTorchMode(false);
        }

        /// <summary>
        /// 设置不同的摄像头（前后摄像头）
        /// </summary>
        /// <param name="type">摄像头类型</param>
        public override void SelectCamera(SCameraHardwareType type)
        {
            m_cameraDeviceBehaviour.Close();
            switch(type)
            {
                case SCameraHardwareType.BACK:
                    m_cameraDeviceBehaviour.CameraDeviceType = CameraDevice.Device.Back;
                    break;
                case SCameraHardwareType.FRONT:
                    m_cameraDeviceBehaviour.CameraDeviceType = CameraDevice.Device.Front;
                    break;
                case SCameraHardwareType.DEFAULT:
                    m_cameraDeviceBehaviour.CameraDeviceType = CameraDevice.Device.Default;
                    break;
            }
            m_cameraDeviceBehaviour.OpenAndStart();
        }

    }

}

