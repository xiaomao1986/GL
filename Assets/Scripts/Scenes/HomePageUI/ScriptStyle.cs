using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ScriptStyle  
{
    public enum Direction
    {
        upon = 0,
        under = 1,
        left = 2,
        right = 3,
        Up = 4,
    }
    public GameObject obj;

    public int AnimationType = 0;

    public int ScriptType = 0;
    public Direction direction;
    public Dictionary<string, GameObject> objDictionary = new Dictionary<string, GameObject>();
    public ScriptStyle(int _direction, GameObject _obj, int _AnimationType, int _ScriptType)
    {
        this.obj = _obj;
        this.AnimationType = _AnimationType;
        this.ScriptType = _ScriptType;
        this.direction = (Direction)System.Enum.ToObject(typeof(Direction), _direction);
    }
    public ScriptStyle(int _direction, Dictionary<string, GameObject> _objDictionary, int _AnimationType, int _ScriptType)
    {
        this.objDictionary = _objDictionary;
        this.AnimationType = _AnimationType;
        this.ScriptType = _ScriptType;
        this.direction = (Direction)System.Enum.ToObject(typeof(Direction), _direction);
    }


}
