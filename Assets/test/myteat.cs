using System;
using UnityEngine;
using System.Collections;
using System;
using System.Globalization;
using LitJson;
public class myteat : MonoBehaviour
{



  
	void Start ()
	{
        TextAsset t = (TextAsset)Resources.Load("yy");
        Debug.Log(""+t.text);
        JsonData jd = JsonMapper.ToObject(t.text);
        int total_rows = int.Parse(jd["total_rows"].ToString());
        Debug.Log("total_rows===" + total_rows);

    }


	
	// Update is called once per frame
	void Update () 
    {
	   
	}


  
    

}
