using UnityEngine;
using System.Collections;
using LitJson;

public class DebugNetGaneObj : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public  void CreateJson(GameObject obj)
    {
        JsonData jd = new JsonData();
        jd["gameObjectName"] = gameObject.name;
        jd["transformPos"] = "10,10,10";
        jd["transformRot"] = "10,10,10";
        jd["transformSca"] = "1,1,1";
        jd["Active"] = "1";
        foreach (Transform item in gameObject.transform)
        {
            
        }

    }

}
