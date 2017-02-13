using UnityEngine;
using System.Collections;

public class Huangdong : MonoBehaviour {

    public float m_gotoA=30f;
    public float m_mmm=0.05f;
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        float f = Input.acceleration.x;
        Vector3 p = transform.localRotation.eulerAngles;
        p.z = m_gotoA * f;
        transform.localRotation=Quaternion.Lerp(transform.localRotation, Quaternion.Euler(p.x, p.y, p.z), m_mmm);
    }
}
