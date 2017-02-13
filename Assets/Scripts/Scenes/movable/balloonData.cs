using UnityEngine;
using System.Collections;
using DG.Tweening;
public class balloonData : MonoBehaviour
{


    public string Activity_id = "";

    public MovableData movabledata;
    void Update()
    {

    }
    public static balloonData Create()
    {
        GameObject objroot=new GameObject();
        GameObject obj = Instantiate(MovableScene.Instance.BalloonObjs[Random.Range(0, MovableScene.Instance.BalloonObjs.Length)]) as GameObject;
        obj.transform.SetParent(objroot.transform);
       // GameObject obj = Instantiate(Resources.Load("movable/balloon")) as GameObject;
        return obj.GetComponent<balloonData>();
    }

    void OnTriggerEnter(Collider colloder)
    {
        if (colloder.gameObject.GetComponent<balloonData>() == null)
        {
            Debug.Log("OnTriggerEnter---" + colloder.gameObject.name);
            gameObject.SetActive(false);
            GameObject obj1 = Instantiate(Resources.Load("movable/BalloonPow")) as GameObject;
            obj1.transform.position = gameObject.transform.position;
            obj1.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles);
            MovableScene.Instance.movableManager.RequestGift(gameObject, Activity_id);
            Destroy(obj1.gameObject, 1);
            Destroy(gameObject.transform.parent.gameObject, 1);
        }

    }
    public void Move()
    {
        gameObject.SetActive(true);
        Vector3 v = gameObject.transform.localPosition;
        gameObject.transform.localPosition = new Vector3(v.x, v.y - 20, v.z);
        Tweener pos= gameObject.transform.DOLocalMove(v, 1);
        string na = "Flap0" + Random.Range(1, 7).ToString();
        pos.OnComplete(delegate() { gameObject.GetComponent<Animator>().Play(na); });
    }
    public void SetTexture2D()
    {
        movabledata.SetTexture2Dproduct(gameObject);
    }
    public void SetColor()
    {
        //gameObject.GetComponent<Renderer>().materials[0].color = MovableScene.Instance.color[Random.Range(0, MovableScene.Instance.color.Length)];
    }
}
