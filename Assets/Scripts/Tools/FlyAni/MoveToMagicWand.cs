//#define MoveToMagicWand_TestCode

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class MoveToMagicWand : MonoBehaviour
{
	private const float SPEED = 0.07f;
	private const float SPEED2= 0.05f;
	public struct MyTransform
	{
		public Vector3 position;
		public Vector3 scale;
		public Quaternion rotation;
	};
	#if MoveToMagicWand_TestCode
	public GameObject[] testO;
	public GameObject toO;
	// Use this for initialization
	void Start () {

	}
	public void __Back(List<GameObject> gs)
	{
		Debug.Log ("---------------------------");
		GameObject g = GameObject.Find ("ButtonGameObject");
		Animator a = g.GetComponent<Animator> ();
		AnimatorStateInfo  info = a.GetCurrentAnimatorStateInfo(0);
	}
	public void __Back()
	{
		Debug.Log ("---------------------------");
		GameObject g = GameObject.Find ("ButtonGameObject");
		Animator a = g.GetComponent<Animator> ();
		AnimatorStateInfo  info = a.GetCurrentAnimatorStateInfo(0);
	}
#endif


    public GameObject Init(string a)
    {
        return new GameObject();
    }
	// Update is called once per frame
	void Update () {
		#if MoveToMagicWand_TestCode
		if (Input.GetKeyDown (KeyCode.Space)) {
			for(int i = 0 ;i < testO.Length ; i ++)
			{
				this.addItem(testO[i]);
			}
			this.flash(toO,__Back);
		}
		else if(Input.GetKeyDown (KeyCode.Alpha0))
		{
			moveBack(__Back);
		}
#endif
		if (state == 1) {
			for(int i = 0 ; i < items.Count ; i ++)
			{
				GameObject go = items[i];
                //go.transform.localPosition = Vector3.Lerp(go.transform.position, toItem.transform.position, rotation_size*0.4f); 
                //go.transform.localScale = Vector3.Lerp(go.transform.localScale, toItem.transform.localScale, rotation_size*0.4f);
                //go.transform.localRotation = Quaternion.Lerp(itemsStartPoint[i].rotation, toItem.transform.rotation, rotation_size * 1.5f);

                go.transform.position = Vector3.Lerp(go.transform.position, toItem.transform.position, rotation_size * 0.4f);
                go.transform.localScale = Vector3.Lerp(go.transform.localScale, toItem.transform.localScale, rotation_size * 0.4f);
                go.transform.rotation = Quaternion.Lerp(itemsStartPoint[i].rotation, toItem.transform.rotation, rotation_size * 1.5f);

				//go.transform.position += (toItem.transform.position - go.transform.position) * SPEED;
				//go.transform.localScale += (toItem.transform.localScale - go.transform.localScale)*SPEED;


//				if(Vector3.Distance(go.transform.localScale , toItem.transform.localScale) <= 0.02f && 
//				   Vector3.Distance(go.transform.position , toItem.transform.position)<=0.02f)
				if(rotation_size >= 1.1f)
				{
					go.transform.position = toItem.transform.position;
					go.transform.localScale = toItem.transform.localScale;
					go.transform.rotation = toItem.transform.rotation;
				}
			}
			rotation_size+=Time.deltaTime;
//			Debug.Log(rotation_size);
			if(rotation_size>=1.1f)
//			if(m>=items.Count)
			{
				state = 2;
				callback1();
			}
		}
		else if(state == 3)
		{

			for(int i = 0 ; i < items.Count ; i ++)
			{
				GameObject go = items[i];
				go.transform.position = Vector3.Lerp(toItem.transform.position, itemsStartPoint[i].position, rotation_size);
				go.transform.localScale = Vector3.Lerp(toItem.transform.localScale, itemsStartPoint[i].scale, rotation_size);
                go.transform.rotation = Quaternion.Lerp(toItem.transform.rotation, itemsStartPoint[i].rotation, rotation_size);

//				go.transform.position += (itemsStartPoint[i].position - go.transform.position) * SPEED2;
//				go.transform.localScale += (itemsStartPoint[i].scale - go.transform.localScale) * SPEED2;
//				go.transform.rotation = Quaternion.Slerp (toItem.transform.rotation,itemsStartPoint[i].rotation, rotation_size);
//				rotation_size+=Time.deltaTime;
//				if(Vector3.Distance(go.transform.localScale , itemsStartPoint[i].scale) <= 0.03f && 
//				   Vector3.Distance(go.transform.position , itemsStartPoint[i].position)<=0.1f)
				if(rotation_size>=1.1f)
				{
					go.transform.position = itemsStartPoint[i].position;
					go.transform.localScale = itemsStartPoint[i].scale;
					go.transform.rotation = itemsStartPoint[i].rotation;
				}
			}
			rotation_size+=Time.deltaTime;
			if(rotation_size>=1.1f)
			//if(m>=items.Count)
			{
				state = 0;
				callback2(items);
				items.Clear();
				itemsStartPoint.Clear();
			}
		}
	}

	private List<GameObject> items = new List<GameObject> ();
	private List<MyTransform> itemsStartPoint = new List<MyTransform> ();
	private GameObject toItem;
	private int state = 0;
	private Callback callback1;
	private Callback<List<GameObject>> callback2;
	private float rotation_size;

	public void addItem(GameObject g)
	{
		if (state != 0)
			return;
		items.Add (g);
		MyTransform myt = new MyTransform();
		myt.rotation = g.transform.rotation;
		myt.position = g.transform.position;
		myt.scale = g.transform.localScale;
		itemsStartPoint.Add (myt);
	}

	public void flash(GameObject to,Callback callback)
	{
		if (state != 0)
			return;
		toItem = to;
		callback1 = callback;
		state = 1;
		rotation_size = 0;
	}

	public void moveBack(Callback<List<GameObject>> callback)
	{
		if (state != 2)
			return;
		callback2 = callback;
		state = 3;
		rotation_size = 0;
	}


}
