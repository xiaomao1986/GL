using UnityEngine;
using System.Collections;
using DG.Tweening;
public class ArrowObject : MonoBehaviour 
{

    public GameObject iceStars;
    private Tweener pos;
    private Callback m_Callback2=null;
	void Start () {
	
	}
    public void fire2(Vector3 v, GameObject RoorObj, Callback<GameObject> _Callback, Callback _Callback2)
    {
        if (v == Vector3.zero)
        {
            v = Camera.main.transform.forward * 20;
        }
        gameObject.transform.parent = null;
        gameObject.GetComponent<ArrowObject>().iceStars.SetActive(true);
        pos = gameObject.transform.DOMove(v, 2f);
        _Callback2();
        pos.OnComplete(delegate()
        {
            gameObject.GetComponent<ArrowObject>().iceStars.SetActive(false);
            gameObject.SetActive(false);
            gameObject.transform.SetParent(RoorObj.transform);
            gameObject.transform.position = RoorObj.transform.FindChild("ArrowPos").transform.position;
            Vector3 r = RoorObj.transform.FindChild("ArrowPos").rotation.eulerAngles;
            gameObject.transform.rotation = Quaternion.Euler(r);
            _Callback(gameObject);
        });
    }

    public void fire3(Vector3 v, GameObject RoorObj)
    {
        if (v == Vector3.zero)
        {
            v = MovableScene.Instance.ArrowManager.transform.FindChild("weizhi").transform.position;
        }
        gameObject.transform.parent = null;
        gameObject.GetComponent<ArrowObject>().iceStars.SetActive(true);
        pos = gameObject.transform.DOMove(v, 2f);
        pos.OnComplete(delegate()
        {
            Destroy(gameObject);
        });
    }


}
