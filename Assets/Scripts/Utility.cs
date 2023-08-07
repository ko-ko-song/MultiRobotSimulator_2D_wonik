using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility 
{
    private static Utility instance;

    public const float zOffset = 20;

    public static Utility Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Utility();
            }
            return instance;
        }
    }

    /// <summary>
    /// 유니티에서 외부로 나가는 position 변경
    /// ex) 10, 40, 1 --> 10, 10, 2
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns> 
    public static Vector3 ChangeExitPosition(Vector3 position)
    {
        int y = Convert.ToInt32(Math.Round(position.y));
        
        if (position.z >= 2)
        {
            return new Vector3(position.x, position.y - zOffset, position.z);
        }

        return position;
    }
    
    /// <summary>
    /// 외부에서 유니티로 들어오는 position 변경
    /// ex) 10, 10, 2 --> 10, 40, 1
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector3 ChangeEntrancePosition(Vector3 position)
    {
        if(position.z >= 2)
        {
            return new Vector3(position.x, position.y + zOffset, position.z);
        }
        return position;
    }
}
