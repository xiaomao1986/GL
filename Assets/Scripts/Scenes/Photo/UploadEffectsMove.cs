using UnityEngine;
using System.Collections;
using DG.Tweening;
public class UploadEffectsMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	public float speed= 5f;
	// Update is called once per frame
	void Update () {
	
	}
    private Tweener pos;
    private Callback callback = null;
    private ItemData m_ItemData = null;
    public void Move(ItemData _ItemData, Callback _callback)
    {
        m_ItemData = _ItemData;
        callback = _callback;
        gameObject.transform.parent = null;
        Vector3 v = m_ItemData.item.Photo.transform.position;
		float t = Vector3.Distance(Vector3.zero, m_ItemData.item.Photo.transform.position);
		Debug.Log ("ttttt-------" + t);
		t = t/10;

		Debug.Log ("ttttt----2---" + t);
		pos=gameObject.transform.DOMove(v, speed-t);
        pos.OnComplete(End);
    }
    private GameObject g;
    private void End()
    {
        g = MonoBehaviour.Instantiate(Resources.Load("photo/CollectExpand01")) as GameObject;
        g.transform.position = m_ItemData.item.Photo.transform.position;

        g.transform.rotation = Quaternion.Euler(new Vector3(m_ItemData.item.Photo.transform.rotation.eulerAngles.x, m_ItemData.item.Photo.transform.rotation.eulerAngles.y, m_ItemData.item.Photo.transform.rotation.eulerAngles.z));
        gameObject.SetActive(false);
        Invoke("Delete",2);
    }
    private void Delete()
    {
        if (callback!=null)
        {
            m_ItemData.ItemRotate();
            callback();
        }
        Destroy(g.gameObject);
        Destroy(gameObject);
    }
}
