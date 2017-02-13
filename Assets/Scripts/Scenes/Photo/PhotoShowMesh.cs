using UnityEngine;
using System.Collections;

public class PhotoShowMesh : MonoBehaviour {


    public bool isRendering = false;
    private float lastTime = 0;
    private float curtTime = 0;
    private bool isrende = true;
	void Start () {
	
	}
	void Update () 
    {
        isRendering = curtTime != lastTime ? true : false;
        lastTime = curtTime;
	}
    void OnWillRenderObject()
    {
        Debug.Log("OnWillRenderObject");
        if (isrende)
        {
            curtTime = Time.time;
        }

    }
}
