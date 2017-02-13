using UnityEngine;
using System.Collections;
using LitJson;

public class Gift
{

    public int activity_type = 1;

    public JsonData result;




    public Gift(JsonData jd)
    {
        result = jd;
    }


}
