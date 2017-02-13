using UnityEngine;
using System.Collections;
using DG.Tweening;
public class HomePageScriptStyle01 : ScriptStyle
{


    public HomePageScriptStyle01(int _direction, GameObject _obj, int _AnimationType, int _ScriptType)
        : base(_direction, _obj, _AnimationType, _ScriptType)
    {
            ScriptSCC01();
    }
    private Vector3 objPos = Vector3.zero;
    private Tweener pos;
    private void Move(Vector3 v3, float speed, TweenCallback Callback = null)
    {
        obj.SetActive(true);
        pos = obj.GetComponent<RectTransform>().DOLocalMove(v3, speed);
        if (Callback != null)
        {
          pos.OnComplete(Callback);
        }
    }
    private void PunchAnchor(Vector2 v, float speed)
    {
       obj.GetComponent<RectTransform>().DOPunchAnchorPos(v, speed);
    }
    private Vector3 v;
    public void ScriptSCC01()
    {
        obj.transform.DOKill();
        if (objPos==Vector3.zero)
        {
            objPos = obj.GetComponent<RectTransform>().localPosition;
        }
        switch (direction)
        {
            case Direction.upon:
                break;
            case Direction.under:
                {
                    v = obj.GetComponent<RectTransform>().localPosition;
                    v.y = v.y - 300;
                 
                    obj.GetComponent<RectTransform>().localPosition = v;
                    if (ScriptType == 0)
                        Move(objPos, 1);
                    else
                    {
                        Vector2 v2 = new Vector2(0, 2);
                        Move(objPos, 0.5f, () => PunchAnchor(v2, 1));
                    }
                }

                break;
            case Direction.left:
                {
                    v = obj.GetComponent<RectTransform>().localPosition;
                    v.x = v.x - 300;
                   
                    obj.GetComponent<RectTransform>().localPosition = v;
                    if (ScriptType == 0)
                        Move(objPos, 1);
                    else
                    {
                        Vector2 v2 = new Vector2(2, 0);
                        Move(objPos, 0.5f, () => PunchAnchor(v2, 1));
                    }
                }
                break;
            case Direction.right:
                {
                    v = obj.GetComponent<RectTransform>().localPosition;
                    v.x = v.x + 300;
                    obj.GetComponent<RectTransform>().localPosition = v;
                    if (ScriptType == 0)
                        Move(objPos, 1);
                    else
                    {
                        Vector2 v2 = new Vector2(2, 0);
                        Move(objPos, 0.5f, () => PunchAnchor(v2, 1));
                    }
                }
                break;
            case Direction.Up:
                {
                    v = obj.GetComponent<RectTransform>().localPosition;
                    v.y = v.y + 300;
                    obj.GetComponent<RectTransform>().localPosition = v;
                    if (ScriptType == 0)
                        Move(objPos, 1);
                    else
                    {
                        Vector2 v2 = new Vector2(0, 2);
                        Move(objPos, 0.5f, () => PunchAnchor(v2, 1));
                    }
                }
                break;
            default:
                break;
        }
    }

    private Vector3 ObjPos;
    public void ScriptSCC02()
    {
        obj.transform.DOKill();
        switch (direction)
        {
            case Direction.upon:
                break;
            case Direction.under:
                {
                    v = objPos;
                    v.y = v.y - 300;
                    Move(v, 1);
                }
                break;
            case Direction.left:
                {
                    v = objPos;
                    v.x = v.x - 300;
                   
                    Move(v, 1);
                }
                break;
            case Direction.right:
                {
                    v = objPos;
                    v.x = v.x + 300;
                    Move(v, 1);
                }
                break;
            case Direction.Up:
                {
                    v = objPos;
                    v.y = v.y + 300;
                    Move(v, 1);
                }
                break;
            default:
                break;
        }
    }
}
