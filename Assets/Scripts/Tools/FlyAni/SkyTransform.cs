using UnityEngine;
using System.Collections;

public struct SkyTransform
{
    public SkyTransform(Transform t)
    {
        _position = t.position;
        _scale = t.localScale;
        _rotation = t.rotation;
    }

    public void set(Transform t)
    {
        _position = t.position;
        _scale = t.localScale;
        _rotation = t.rotation;
    }
    private Vector3 _position;

    public Vector3 position
    {
        get { return _position; }
        set { _position = value; }
    }
    private Vector3 _scale;

    public Vector3 scale
    {
        get { return _scale; }
        set { _scale = value; }
    }
    private Quaternion _rotation;

    public Quaternion rotation
    {
        get { return _rotation; }
        set { _rotation = value; }
    }
}
