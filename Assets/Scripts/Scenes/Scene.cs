using UnityEngine;
using System.Collections;

public abstract class Scene : MonoBehaviour
{


    public string sceneName;
    private SUIRoot m_uiRoot = new SUIRoot();
    public SUIRoot uiRoot
    {
        get { return m_uiRoot; }
    }
    /// <summary>
    /// 开启场景
    /// </summary>
    public void Open(Callback<float,bool> callback)
    {
        //TODO base
        m_uiRoot.Show();
        this.OnOpen(callback);
    }
    protected abstract void OnOpen(Callback<float, bool> callback);



    /// <summary>
    /// 关闭场景
    /// </summary>
    public void Close(Callback callback)
    {
        //TODO base
        m_uiRoot.Hide();
        this.OnClose(callback);
    }
    protected abstract void OnClose(Callback callback);

   
}
