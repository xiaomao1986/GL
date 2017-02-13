using UnityEngine;
using System.Collections;

namespace com.imysky.camera
{
    public delegate void SCameraManagerCallBack();
    public enum SCameraType
    {
        EASYAR = 0
    }
    public class SCameraManager : MonoBehaviour
    {
        private static SCameraManager m_instance = null;

        public static SCameraManager instance
        {
            get { return SCameraManager.m_instance; }
        }

        private static SCamera m_camera = null;

        public static SCamera currentCamera
        {
            get { return SCameraManager.m_camera; }
        }

        private void Awake()
        {
            if (m_instance != null)
                Debug.LogError("不可以同时启动两个SCameraManager对象");
            m_instance = this;
        }

        /// <summary>
        /// 创建一个相机，
        /// 注意此方法为了控制产品中使用相机设备的唯一性。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SCamera CreateCamera(SCameraType type, SCameraStartCallBack callback = null)
        {
            switch(type)
            {
                case SCameraType.EASYAR:
                default:
                    if (m_camera != null)
                    {
                        MonoBehaviour.Destroy(m_camera.gameObject);
                        m_camera = SEasyARCamera.create(callback);
                    }
                    else m_camera = SEasyARCamera.create(callback);
                   MySkyInputEvent.instance.SetCamera(m_camera.camera);
                   MySkyInputEvent.instance.SetUICamera(m_camera.camera);
                   return m_camera;
            }
        }
        private IEnumerator CreateCameraCallBack(SCameraManagerCallBack callback)
        {
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(0.5f);
            callback();
        }
        public static void DestroyCamera()
        {
            //销毁相机
            MonoBehaviour.Destroy(m_camera.gameObject);
            m_camera = null;
        }

    }



}

