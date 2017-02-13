using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;


public class SkyJsonTools 
{
    private static SkyJsonTools instance;
    public static SkyJsonTools Instance
    {
        get
        {
            if (instance==null)
            {
                instance = new SkyJsonTools();
            }
            return instance;
        }

    }
    Dictionary<string, string> JsonDiction1;
    

	public string SkyJsonADD(Dictionary<string,string> JsonDiction)
    {
        string s="";

        StringBuilder josn = new StringBuilder();
        JsonWriter writer = new JsonWriter(josn);
        writer.WriteObjectStart();
        int i = 0;


        if (JsonDiction.Count==1)
        {
            writer.WritePropertyName("fun");
            writer.Write(JsonDiction["fun"]);
            writer.WritePropertyName("data");
            writer.WriteArrayStart();
            writer.WriteObjectStart();
            writer.WritePropertyName("null");
            writer.Write("null");

            writer.WriteObjectEnd();
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
            s = josn.ToString();
            return s;
        }
        foreach (KeyValuePair<string,string> item in JsonDiction)
        {
            if (i==0)
            {
                writer.WritePropertyName("fun");
                writer.Write(item.Value);
                i++;
                continue;
            }
            if (i==1)
            {
                writer.WritePropertyName("data");
				writer.WriteObjectStart();
                i++;
            }
            writer.WritePropertyName(item.Key);
            writer.Write(item.Value);

        } 
        writer.WriteObjectEnd();
        writer.WriteObjectEnd();
        s = josn.ToString();

        return s;
    }




}
