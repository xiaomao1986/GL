using UnityEngine;
using System.Collections;
using  UnityEditor;
using LitJson;
public class Arrangement :Editor {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [MenuItem("Photo/Arrangement")]
    static void CreateArrangement()
    {
        Debug.Log("" + Selection.activeGameObject.name);
        if (Selection.activeGameObject != null)
        {
             JsonData jd = new JsonData();
             JsonData jd1 = new JsonData();
 
             jd["Arrangement"] = "1.0.0.0";
            foreach (Transform item in Selection.activeGameObject.transform)
            {
                JsonData tmp = new JsonData();
                JsonData tmp1 = new JsonData();
                JsonData tmp2 = new JsonData();
                JsonData tmp3 = new JsonData();
                JsonData tmp4 = new JsonData();
                tmp2["x"] = item.transform.localPosition.x;
                tmp2["y"] = item.transform.localPosition.y;
                tmp2["z"] = item.transform.localPosition.z;

                tmp3["x"] = item.transform.localRotation.eulerAngles.x;
                tmp3["y"] = item.transform.localRotation.eulerAngles.y;
                tmp3["z"] = item.transform.localRotation.eulerAngles.z;

                tmp4["x"] = item.transform.localScale.x;
                tmp4["y"] = item.transform.localScale.y;
                tmp4["z"] = item.transform.localScale.z;

                tmp1["pos"] = tmp2;
                tmp1["pot"] = tmp3;
                tmp1["scale"] = tmp4;
                tmp["Transform"] = tmp1;
                jd1.Add(tmp);
            }
            jd["Array"] = jd1;
            My_UIEditorToos.CreateFile("Assets/Resources","Arrangement", jd);
            Debug.Log("" + jd.ToJson());
        }
    }
}
