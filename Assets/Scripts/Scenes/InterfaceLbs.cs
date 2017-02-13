using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InterfaceLbs : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    JumpScene();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

     private AsyncOperation loadingAsync = null;
    
    private void JumpScene()
    {
        ///启动 异步
        StartCoroutine(AddSceneAdditive("ARScene"));
    }
    private IEnumerator AddSceneAdditive(string sceneName)
    {
        //加载场景
        loadingAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return loadingAsync;
    }
}
