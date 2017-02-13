using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;  
public class SkyUI_Button 
{


    private static Vector2 ButtonSizeDelta = Vector2.one;

    [MenuItem("SkyUI/Button")]
	public static GameObject UI_Button()
    {
        ButtonSizeDelta = new Vector2(100, 20);
        GameObject obj = new GameObject();
        obj.AddComponent<RectTransform>().sizeDelta = ButtonSizeDelta;
        obj.AddComponent<Button>();
        obj.name = "SkyButton";
        obj.AddComponent<SUIButton>();


        GameObject defaultobj = new GameObject();
        defaultobj.AddComponent<RectTransform>().sizeDelta = obj.GetComponent<RectTransform>().sizeDelta;
        defaultobj.AddComponent<Image>();
        defaultobj.transform.SetParent(obj.transform);
        defaultobj.name = "default";

        GameObject pressobj = new GameObject();
        pressobj.AddComponent<RectTransform>().sizeDelta = obj.GetComponent<RectTransform>().sizeDelta;
        pressobj.AddComponent<Image>();
        pressobj.transform.SetParent(obj.transform);
        pressobj.name = "press";

        GameObject loseobj = new GameObject();
        loseobj.AddComponent<RectTransform>().sizeDelta = obj.GetComponent<RectTransform>().sizeDelta;
        loseobj.AddComponent<Image>();
        loseobj.transform.SetParent(obj.transform);
        loseobj.name = "lose";

        GameObject effectobj = new GameObject();
        effectobj.AddComponent<RectTransform>().sizeDelta = obj.GetComponent<RectTransform>().sizeDelta;
        effectobj.AddComponent<Image>();
        effectobj.transform.SetParent(obj.transform);
        effectobj.name = "effect";

        GameObject hintobj = new GameObject();
        hintobj.AddComponent<RectTransform>().sizeDelta = new Vector2(20,20);
        hintobj.GetComponent<RectTransform>().localPosition = new Vector3(40,20,0);
        hintobj.AddComponent<Image>();
        hintobj.transform.SetParent(obj.transform);
        hintobj.name = "hint";

        GameObject hintTextobj = new GameObject();
        hintTextobj.AddComponent<RectTransform>().sizeDelta = obj.GetComponent<RectTransform>().sizeDelta;
        hintTextobj.AddComponent<Text>().text = "";
        hintTextobj.GetComponent<Text>().color = Color.black;
        hintTextobj.transform.SetParent(hintobj.transform);
        hintTextobj.name = "Text";

        GameObject Textobj = new GameObject();
        Textobj.AddComponent<RectTransform>().sizeDelta = new Vector2(100, 20); ;
        Textobj.AddComponent<Text>().text="Button";
        Textobj.GetComponent<Text>().color = Color.black;
        Textobj.transform.SetParent(obj.transform);
        Textobj.name = "Text";
        return obj;
    }
     [MenuItem("SkyUI/Menu")]
    public static void UI_SUIMenu()
    {
        GameObject obj = new GameObject();
        obj.AddComponent<RectTransform>().sizeDelta = new Vector2(100, 200);
        obj.name = "SUIMenu";
        obj.AddComponent<SUIMenu>();
    }
      [MenuItem("SkyUI/HomePage")]
     public static void UI_HomePage()
     {
         GameObject obj = new GameObject();
         obj.AddComponent<RectTransform>().sizeDelta = new Vector2(100, 200);
         obj.name = "HomePage";
         obj.AddComponent<HomePageMag>();
     }
}
