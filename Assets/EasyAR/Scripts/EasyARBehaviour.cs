/**
* Copyright (c) 2015-2016 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
* EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
* and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
*/

using UnityEngine;

namespace EasyAR
{
    public class EasyARBehaviour : MonoBehaviour
    {
        [TextArea(1, 10)]
        public string Key;
        private bool initialized;

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                Engine.Pause();
            }
            else
            {
                Engine.Resume();
            }
        }

        protected virtual void OnApplicationQuit()
        {
            Engine.OnApplicationQuit();
        }

        public void Initialize()
        {
            if (initialized)
                return;
            initialized = true;
            //Key = "GCWvbJBpfJfnnz9Ai7nygzYrUdqKcdKieaP8WVhViDX7WAbhEpOA3NzCiR68J2umLEBRJsAm3pF1ABOsWWkVv5N56zTFeUdsllkbb8253937bff1a10a03498438c3d10386BtcmQedzybqacQyhiE3BE4CcBqYxQgXA3m1kBDhGM6SS2dwkDsk7CMuQ3iGo7HBEnV2I";
            ARBuilder.Instance.InitializeEasyAR(Key);
            if (!ARBuilder.Instance.EasyBuild())
                Debug.LogError("fail to build AR");
        }
    }
}
