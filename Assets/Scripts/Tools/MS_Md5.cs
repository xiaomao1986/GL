using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Security.Cryptography;
public class MS_Md5 
{
    public static MS_Md5 ms_MD5;

    public static MS_Md5 Init_MD5()
    {
        if (ms_MD5==null)
        {
            ms_MD5 = new MS_Md5();
        }
        return ms_MD5;
    }

    public static string UserMd5_32(string str)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.Default.GetBytes(str);
        byte[] result = md5.ComputeHash(data);
        String ret = "";
        for (int i = 0; i < result.Length; i++)
            ret += result[i].ToString("x").PadLeft(2, '0');
        return ret;
    }


}
