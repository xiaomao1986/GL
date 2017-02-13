using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using System;
public class My_Common
{
     public static byte[] Serialize<T>(T instance)
      {
           byte[] bytes = null;
           using(var ms=new MemoryStream())
           {
               Serializer.Serialize<T>(ms, instance);
               bytes = new byte[ms.Position];
               var fullBytes = ms.GetBuffer();
               Array.Copy(fullBytes, bytes, bytes.Length);

           }
           return bytes;
       }
       public static T DeSerialize<T>(byte[] bytes)
       {
           using (var ms=new MemoryStream(bytes))
           {
               return Serializer.Deserialize<T>(ms);
           }
       }
	
}
