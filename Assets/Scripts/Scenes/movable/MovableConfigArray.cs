using UnityEngine;
using System.Collections;

public class MovableConfigArray  
{
    private PhotoConfigArray ConfigArra=null;
    public MovableConfigArray()
    {
        ConfigArra = new PhotoConfigArray("BalloonArrangement");
    }
    public Vector3 GetPos(int Index)
    {
        return ConfigArra.GetPos(Index);
    }

    public void Delete()
    {
        ConfigArra = null;
    }
}
