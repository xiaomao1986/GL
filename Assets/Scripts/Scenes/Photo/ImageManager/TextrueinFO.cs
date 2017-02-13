using UnityEngine;
using System.Collections;

public enum TextureSize_state
{
    Texture_raw = 1,//原图
    Texture_big = 2,  //大图
    Texture_centre = 3,//中图
    Texture_little = 4,//小图
    Texture_tiny = 5,//微图
}
public class TextrueinFO 
{
    public static Texture2D baseTexture;
    //private static Color[] loadingColor={new Color(),new Color(),new Color(),new Color()};
    public Texture2D Texture;
    public bool ISloding = false;
    public byte[] photo_byte = null;
    public string TextureName;
    public int QuoteCount_raw = 0;
    public int QuoteCount_big = 0;
    public int QuoteCount_centre = 0;
    public int QuoteCount_little = 0;
    public int QuoteCount_tiny = 0;
    public TextureSize_state textureSize_state;
    public Callback<Texture2D> callBack;
    public TextrueinFO()
    {
        Texture = new Texture2D(1, 1);
        Texture.SetPixel(0,0,new Color(0.85f,0.78f,0.91f));
        //Texture.LoadImage(baseTexture.EncodeToJPG());
    }
	
}
