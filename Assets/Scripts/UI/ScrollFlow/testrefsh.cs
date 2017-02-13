using UnityEngine;
using System.Collections;
using System.Resources;

public class testrefsh : MonoBehaviour {
    public UI_Control_ScrollFlow _ScrollFlow;
	// Use this for initialization
	void Start () 
    {


	    for (int i = 0; i < 2; i++)
	    {
	        GameObject g=Instantiate(Resources.Load("Image"))as GameObject;
            g.transform.SetParent(gameObject.transform);
            g.transform.transform.localPosition=Vector3.zero;


	    }

     _ScrollFlow.Refresh();


     
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
