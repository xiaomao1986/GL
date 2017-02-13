using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISetScrollItem : MonoBehaviour {


    public Text[] ScrollText;   //0 时间 1地点 2宝贝 3 数量
    
    public void SetText(string Time,string activity_site, string activity_theme,string amount)
    {
        ScrollText[0].text = Time;
        ScrollText[1].text = activity_site;
        ScrollText[2].text = activity_theme;
        ScrollText[3].text = amount;
    }
}
