using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using com.imysky.camera;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;



public  class SceneData
{
    public string SceneName = "";
    public string MsgADD = "";

    public int Init = 0;

}
public class SkySceneManager : MonoBehaviour
{
    private string currentSceneName = "";
    public static SkySceneManager Instance;
    private SUILoading m_SUILoading=null;
    public int Start3D = 0;
    public BlurOptimized m_BlurOptimized = null;

    public float currentN=-9999;
    public float currentE=-9999;

    /// <summary>
    /// 场景列表
    /// </summary>
    //public List<Scene> curScenes = new List<Scene>();
    private Dictionary<string, Scene> curScenes = new Dictionary<string, Scene>();

    public  bool GetScene()
    {
        if (curScenes.ContainsKey("PhotoScene"))
            return curScenes["PhotoScene"].gameObject.activeSelf;
        return false;
    }
	// Use this for initialization
	void Start ()
	{

#if UNITY_IOS
     Messenger.AddListener("U2N_N_StartAppCamera",
                        delegate(){
                            //TODO 开启场景和AR相机
                            IosStart();
                        });
#endif
#if UNITY_ANDROID
        Instance = this;
        SCameraManager.CreateCamera(SCameraType.EASYAR);
        GameObject Gyro = new GameObject();
        Gyro.name = "GyroController";
        Gyro.AddComponent<MySkyGyroController>();
        Gyro.transform.SetParent(GameObject.Find("Manager").gameObject.transform);
        Gyro.transform.localPosition = Vector3.zero;
        SCameraManager.currentCamera.gameObject.transform.SetParent(Gyro.transform);
        m_BlurOptimized = SCameraManager.currentCamera.camera.gameObject.GetComponent<BlurOptimized>();
        m_SUILoading = SUILoading.CreateLoading();
        Start3D = PlayerPrefs.GetInt("3DStart");
        Start3D++;
        PlayerPrefs.SetInt("3DStart", Start3D);
        // PlayerPrefs.SetInt("GUIDE3D", 1);
        MsgBase.MsgAdd<string>("OnOpenScene", OnOpenScene);
        MsgBase.MsgAdd<string, Callback>("RemoveScene2", RemoveScene2);
        MsgBase.MsgAdd<string>("RemoveScene", RemoveScene1);
        MsgBase.MsgAdd("RemoveTutorial", RemoveTutorial);
        MsgBase.MsgAdd<string>("HideScene", HideScene);
        MsgBase.MsgAdd<bool, float>("ShowBlur", ShowBlur);
        OnOpenScene("UI");
       // if (PlayerPrefs.GetInt("GUIDE3D") == 3)
        {
           // OnOpenScene("PhotoScene");
        }
#endif
    }

    private void IosStart()
    {
        Instance = this;
        SCameraManager.CreateCamera(SCameraType.EASYAR);
        GameObject Gyro = new GameObject();
        Gyro.name = "GyroController";
        Gyro.AddComponent<MySkyGyroController>();
        Gyro.transform.SetParent(GameObject.Find("Manager").gameObject.transform);
        Gyro.transform.localPosition = Vector3.zero;
        SCameraManager.currentCamera.gameObject.transform.SetParent(Gyro.transform);
        m_BlurOptimized = SCameraManager.currentCamera.camera.gameObject.GetComponent<BlurOptimized>();
        m_SUILoading = SUILoading.CreateLoading();
        Start3D = PlayerPrefs.GetInt("3DStart");
        Start3D++;
        PlayerPrefs.SetInt("3DStart", Start3D);
        // PlayerPrefs.SetInt("GUIDE3D", 1);
        MsgBase.MsgAdd<string>("OnOpenScene", OnOpenScene);
        MsgBase.MsgAdd<string, Callback>("RemoveScene2", RemoveScene2);
        MsgBase.MsgAdd<string>("RemoveScene", RemoveScene1);
        MsgBase.MsgAdd("RemoveTutorial", RemoveTutorial);
        MsgBase.MsgAdd<string>("HideScene", HideScene);
        MsgBase.MsgAdd<bool, float>("ShowBlur", ShowBlur);
        OnOpenScene("UI");
        if (PlayerPrefs.GetInt("GUIDE3D") == 3)
        {
            OnOpenScene("PhotoScene");
        }
    }
    private void RemoveScene1(string arg1)
    {
        RemoveScene(arg1, RemoveBack);
    }
    private void RemoveBack()
    {

    }
    private void RemoveScene2(string arg1, Callback arg2)
    {
        RemoveScene(arg1, arg2);
    }
    private void HideScene(string SceneName)
    {
        if (curScenes.ContainsKey(SceneName))
        {
            curScenes[SceneName].gameObject.SetActive(false);
        }
    }
    private void RemoveTutorial()
    {
        RemoveScene("Tutorial");
    }

    private IEnumerator LoadScene()
    {
        WWW download = WWW.LoadFromCacheOrDownload("file://" + Application.dataPath + "/MyScene.unity3d", 1);
        yield return download;
        var bundle = download.assetBundle;
        TextAsset ScneJson = (TextAsset)Resources.Load("UnitySceneData");
        LoadSceneJsonConfig(ScneJson.text);
        InitScene();
    }
    //读取场景配置
    private Dictionary<string, SceneData> ScenesDictionary = new Dictionary<string, SceneData>();
    public void LoadSceneJsonConfig(string  json)
    {
        JsonData jd = JsonMapper.ToObject(json);
        for (int i = 0; i < jd["Scene"].Count; i++)
        {
           JsonData tmpJson = jd["Scene"][i];
           SceneData tmpSceneData = new SceneData();
           tmpSceneData.SceneName = tmpJson["SceneName"].ToString();
           tmpSceneData.Init = int.Parse(tmpJson["Event"]["init"].ToJson());
           ScenesDictionary.Add(tmpJson["SceneName"].ToString(), tmpSceneData);
        }
    }
    public void OnOpenScene(string sceneName)
    {
        if (curScenes.ContainsKey(sceneName))
        {
            if (curScenes[sceneName].gameObject.activeSelf==false)
            {
                curScenes[sceneName].gameObject.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(AddSceneAdditive(sceneName));
        }
    }
    private void RemoveScene(string sceneName,Callback callback=null)
    {
        if (curScenes.ContainsKey(sceneName))
        {
            curScenes[sceneName].Close(callback);
            SceneManager.UnloadScene(sceneName);
            curScenes.Remove(sceneName);
            return;
        }
    }

    private AsyncOperation loadingAsync = null;
    private IEnumerator AddSceneAdditive(string sceneName)
    {
        if (!curScenes.ContainsKey(sceneName))
        {
            currentSceneName = sceneName;
            loadingAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            yield return loadingAsync;
            GameObject go = GameObject.Find(sceneName);
            Scene scene = null;
            if (go != null)
            {
                scene = go.GetComponent<Scene>();
                scene.sceneName = sceneName;
                scene.Open(Loading);
                curScenes.Add(sceneName, scene);
            }else
            {
               
            }
        }
    }
    private bool SceneEnd = false;
    private void Loading(float t,bool isLoad)
    {
        if (isLoad)
        {
            m_SUILoading.SetLoadingText(t.ToString("f0"));
        }
        else
        {
            m_SUILoading.gameObject.SetActive(false);
            if (PlayerPrefs.GetInt("GUIDE3D")!=3)
            {
               // OnOpenScene("Tutorial");
                 //OnOpenScene("PhotoScene");
                return;
            }
        }
    }
    private void LoadingEnd()
    {
        m_SUILoading.gameObject.SetActive(false);
    }
    private void InitScene()
    {
        foreach (var tmp in ScenesDictionary)
        {
            if (tmp.Value.Init!=0)
            {
                OnOpenScene(tmp.Value.SceneName);
            }
        }
    }
    private bool isBlur = false;
    private bool isBlur2 = false;
    private int Iterations = 4;
    private float Size = 10;
    private float BlurTime = 0;
    private float blurSize2 = 0;
    private void ShowBlur(bool ishow, float _blurSize)
    {
        if (ishow)
        {
            if (isBlur == false)
            {
                Size = 10;
                Iterations = 4;
                blurSize2 = _blurSize;
                isBlur = true;
                isBlur2 = false;
            }

        }else
        {
            if (isBlur2==false)
            {
                isBlur = false;
                isBlur2 = true;
                blurSize2 = _blurSize;
                Size = 0;
                Iterations = 0;
            }
          
        }
    }
    void Update()
    {
        if (isBlur)
        {
            BlurTime += Time.deltaTime;
            if (BlurTime>0.002f)
            {
                BlurTime = 0;
                Size = Size - 2f;
                Iterations = Iterations - 1;
                if (m_BlurOptimized.blurSize==0)
                {
                    return;
                }
                m_BlurOptimized.downsample = 0;
                m_BlurOptimized.blurSize = Size;
                m_BlurOptimized.blurIterations = Iterations;
                if (m_BlurOptimized.blurIterations<0)
                {
                    m_BlurOptimized.blurIterations = 0;
                }
                if (Size==0)
                {
                    isBlur = false;
                }
            }
        }
        if (isBlur2)
        {
              BlurTime += Time.deltaTime;
            if (BlurTime>0.002f)
            {
                BlurTime = 0;
                Size = Size + 2f;
                Iterations = Iterations + 1;
                m_BlurOptimized.downsample = 2;

                if (m_BlurOptimized.blurSize >= blurSize2)
                {
                    return;
                }
                m_BlurOptimized.blurSize = Size;
                m_BlurOptimized.blurIterations = Iterations;
                if (m_BlurOptimized.blurIterations > 4)
                {
                    m_BlurOptimized.blurIterations = 4;
                }
                if (Size==10)
                {
                    isBlur2 = false;
                }
            }
        }


    }
}
