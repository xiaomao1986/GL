using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
public class Arrow  
{
    private GameObject ArrowObj;
    private GameObject[] ArrowObjs;
    private Queue<GameObject> ArrowObjQueue = new Queue<GameObject>(); 
    private GameObject RoorObj = null;

    private int Type = 0;


    public void Delete()
    {
        MsgBase.MsgRemove("LoadLoadArrow", LoadLoadArrow);
        ArrowObj = null;
        for (int i = 0; i < ArrowObjs.Length; i++)
        {
            ArrowObjs[i] = null;
        }
        ArrowObjQueue.Clear();
    }
	public Arrow()
    {
        Type = 3;
        ArrowObjs=new GameObject[8];
        CreateArrow(Type);
        MsgBase.MsgAdd("LoadLoadArrow", LoadLoadArrow);
    }

    private bool isCreateArrow = false;


    public void CreateArrow(int type)
    {
        if (isCreateArrow)
        {
            return;
        }
        RoorObj = MovableScene.Instance.ArrowManager;
        switch (type)
	    {
                case 1:
               {
                    ArrowObj = MonoBehaviour.Instantiate(Resources.Load("movable/Arrow")) as GameObject;
                    ArrowObj.SetActive(false);
                    ArrowObj.transform.SetParent(RoorObj.transform.FindChild("Crossbow").transform);
                    ArrowObj.transform.position = RoorObj.transform.FindChild("ArrowPos").transform.position;
                   // ArrowObj.transform.localScale = RoorObj.transform.FindChild("ArrowPos").transform.localScale;
                    Vector3 r = RoorObj.transform.FindChild("ArrowPos").rotation.eulerAngles;
                    ArrowObj.transform.rotation = Quaternion.Euler(r);
                    LoadArrow(type);
               }
               break;
               case 2:
               {
                   for (int i = 0; i < ArrowObjs.Length; i++)
                   {
                       if (ArrowObjs[i]!=null)
                       {
                           MonoBehaviour.Destroy(ArrowObjs[i]);
                       }
                       ArrowObjs[i] = null;
                   }
                   ArrowObjQueue.Clear();
                   for (int i = 0; i < 5; i++)
                   {
                       ArrowObjs[i] = MonoBehaviour.Instantiate(Resources.Load("movable/Arrow")) as GameObject;
                       ArrowObjs[i].SetActive(false);
                       ArrowObjs[i].transform.SetParent(RoorObj.transform);
                       ArrowObjs[i].transform.position = RoorObj.transform.FindChild("ArrowPos").transform.position;
                       ArrowObjs[i].transform.localScale = RoorObj.transform.FindChild("ArrowPos").transform.localScale;
                       Vector3 r = RoorObj.transform.FindChild("ArrowPos").rotation.eulerAngles;
                       ArrowObjs[i].transform.rotation = Quaternion.Euler(r);
                       ArrowObjQueue.Enqueue(ArrowObjs[i]);
                   }
                   LoadArrow(type);
               }
               break;
            case 3:
	        {
                ArrowObj = MonoBehaviour.Instantiate(Resources.Load("movable/Arrow")) as GameObject;
                ArrowObj.SetActive(true);
                    //.FindChild("Crossbow").transform
                ArrowObj.transform.SetParent(RoorObj.transform);
                ArrowObj.transform.position = RoorObj.transform.FindChild("ArrowPos").transform.position;
                ArrowObj.transform.localScale = new Vector3(1,1,1);
                Vector3 r = RoorObj.transform.FindChild("ArrowPos").rotation.eulerAngles;
                ArrowObj.transform.rotation = Quaternion.Euler(r);
	            isCreateArrow = true;
	        }
            break;
	    }
    }

    private Tweener pos;

    public void fire(Vector3 v)
    {
        switch (Type)
        {
            case 1:
                {
                    fire1(v);
                }
                break;
            case 2:
                {
                    Debug.Log("--fire------");
                    MovableScene.Instance.Crossbow.GetComponent<Animator>().Play("Crossbow");
                    ArrowObjQueue.Dequeue().GetComponent<ArrowObject>().fire2(v, RoorObj, delegate(GameObject obj) { ArrowObjQueue.Enqueue(obj); }, delegate() {  });
                }
                break;

            case 3:
                {
                    Debug.Log("--fire------");
                    ArrowObj.GetComponent<ArrowObject>().fire3(v, RoorObj);
                    MovableScene.Instance.Crossbow.GetComponent<Animator>().Play("Crossbow");
                    isCreateArrow = false;
                }
                break;

            default:
                break;
        }
    }

    private IEnumerator ArrowTime()
    {
        Debug.Log("--ArrowTime------");
        yield return new WaitForSeconds(0.1f);
        LoadArrow(Type);
    }

    private void fire1(Vector3 v)
    {
        if (v == Vector3.zero)
        {
            v = Camera.main.transform.forward * 20;
        }
        ArrowObj.transform.parent = null;
        ArrowObj.GetComponent<ArrowObject>().iceStars.SetActive(true);
        pos = ArrowObj.transform.DOMove(v, 2f);
        pos.OnComplete(delegate()
        {
            ArrowObj.GetComponent<ArrowObject>().iceStars.SetActive(false);
            ArrowObj.SetActive(false);
            ArrowObj.transform.SetParent(RoorObj.transform);
            ArrowObj.transform.position = RoorObj.transform.FindChild("ArrowPos").transform.position;
            Vector3 r = RoorObj.transform.FindChild("ArrowPos").rotation.eulerAngles;
            ArrowObj.transform.rotation = Quaternion.Euler(r);
            ArrowObj.SetActive(true);
  
        });
    }




    public void LoadLoadArrow()
    {
        LoadArrow(Type);
    }

    public void LoadArrow(int type)
    {
        switch (type)
        {
            case 1:
                {
                    ArrowObj.SetActive(true);
                }
                break;
            case 2:
                {
        


                    if (ArrowObjQueue.Count==0)
                    {
                       
                        CreateArrow(Type);
                    }
                    try
                    {
                        if (ArrowObjQueue.Peek().gameObject.activeSelf)
                        {
                           
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                      
                     Debug.Log(e.ToString());
                    }
                  ArrowObjQueue.Peek().SetActive(true);
                }
                break;
            case 3:
                {
                    CreateArrow(Type);
                }
                break;
            default:
                break;
        }
    }

}
