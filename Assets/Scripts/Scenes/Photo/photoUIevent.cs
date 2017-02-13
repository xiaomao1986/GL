using UnityEngine;
using System.Collections;

public class photoUIevent : MyUIBaes
{

    void Awake()
    {
        MsgBase.MsgAdd("photoUIeventShowUI", base.Play);
        MsgBase.MsgAdd("photoUIeventQuitUI", base.Quit);
    }

    void OnDestroy()
    {
        MsgBase.MsgRemove("photoUIeventShowUI", base.Play);
        MsgBase.MsgRemove("photoUIeventQuitUI", base.Quit);
    }
	// Use this for initialization
	void Start ()
    {
       
	}
	// Update is called once per frame
	void Update () {
	
	}
}
