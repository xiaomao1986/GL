using UnityEngine;
using System.Collections;
using com.imysky.camera;
public class AppStart : MonoBehaviour 
{


	void Awake()
    {
        PlayerPrefs.SetString("unity_version", "u_12_20_15_00");
        gameObject.AddComponent<SUIManager>();
        gameObject.AddComponent<SCameraManager>();
        gameObject.AddComponent<MySkyInputEvent>();
       // gameObject.AddComponent<SU2NConnection>();
        gameObject.AddComponent<SkySceneManager>();

    }

    void OnApplicationPause(bool isPause)
    {
        if (isPause)
        {
            Application.targetFrameRate = 1;
        }
        else
        {
            Application.targetFrameRate = 60;
        }
    }
}
