using UnityEngine;
using System.Collections;
using DG.Tweening;
public class ClickTest : MyUIBaes {

	// Use this for initialization


	void Start () 
    {
        MsgBase.MsgAdd("CloseAR", CloseAR);

        HomePageChildDictionary["ar"].SetActive(true);


	}

    private void CloseAR()
    {
        Debug.Log("CloseAR------------");
    }
	


	// Update is called once per frame
	void Update () {
	
	}
 
}
