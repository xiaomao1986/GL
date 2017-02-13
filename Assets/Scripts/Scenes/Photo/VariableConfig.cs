using UnityEngine;
using System.Collections;

public class VariableConfig : MonoBehaviour
{

    public string ClickName;

    public Vector3 Click_Move_pos=new Vector3(0,0,15);
    public float   Click_Move_Speed=1;

    public Color Click_ui_color1=new Color(1,1,1,0.1f);
    public Color Click_ui_color2=new Color(1,1,1,1f);
    public float Click_ui_Speed=1;
    public float Click_ui_Speed2=0.5f;


    public string FlyManName;


    public Vector3 FlyMan_Grad_pos=new Vector3(0,0,10);

    public Vector3 FlyMan_photo_Scale = new Vector3(0.01f, 0.01f, 0.01f);
    public float FlyMan_photo_speed=0.5f;
    public Color FlyMan_ui_color1=new Color(1,1,1,0.1f);
    public Color FlyMan_ui_color2 = new Color(1, 1, 1, 1f);
    public float FlyMan_ui_Speed=1;
    public float FlyMan_ui_Speed2=0.5f;



    public string ScaleName;
    public float Photo_Scale_big=8;
    public float Photo_Scale_adj=70;
    public float Photo_Scale_Speed=1;



    public string photoEnterName;
    public float Photo_Enter_high = 10;
    public float Photo_Enter_Speed = 2;
    public float Photo_Load_Speed =0.005f;
    public float Photo_Flash_Speed = 0.1f;
    public float Photo_pot_Speed = 3f;

    public string RespondName;

    public float Photo_Respond_Speed = 1f;

    public float Photo_Respond_angle = 100f;
    private static VariableConfig instance;


    public static VariableConfig Instance
    {
        get { return VariableConfig.instance; }
    }

    void Awake()
    {
        instance = this;
    }
}
