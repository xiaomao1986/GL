using UnityEngine;
using System.Collections;


/// <summary>
/// 产品的配置文件
/// </summary>
public class MySkyConfig{

	public const float REQUEST_INTERVAL_TIME = 5f;
	public const string VUFORIA_SN = "AZqNsE3/////AAAAAB7EfUbCjE0JskZb4ST7HpxF+LCkyRcypy08cSPfopKT2b5t/wIrD0G5M0a/8AapUhakrxMx+GKYVDWoI5qE4nfrdr5+98lRDiSm/KVhGI3TiGt7m2GPU5f5QbD/i8cQcG1BXsSmQTS8vMOLriJDqPQ1Tzr0o2oNVNiCPDKhO4zky4w4DOfwFZRRY/avqf9dIkb4kSX18jBxKyD7vkJRaTwL62FLQT1Kog/Pku3A3/ngjFgdCqGwe9z9bETjmOsF2U1b6wT3JrMIOrTuDrHz0jlkq+42hJ1tqxNP/vwHERcw3s/zCAR8/v/EvfszYF/xZL3AN9nHQtVkWZv525s3fcMu9sJdQZklGHxc2teMuzIO";

	public const string ANDROID_SERVER_URL = "http://192.168.1.30:6080";
	public const string IOS_SERVER_URL = "http://192.168.1.30:6080";

    //Unity3D 的产品版本号
    public const string UNITY_VERSION = "unity 1.4.0_007";


    #region ********************************* UiManager的配置信息 ***********************************
    //主UI的位置
    public const string UI_MANAGER_RESOURCE_MAINCANVAS = "UI/main/MainCanvas";
    //提示信息界面的名称
    public const string UI_MANAGER_RESOURCE_REMINDER = "Reminder";
    //保持性提示界面的名称
    public const string UI_MANAGER_RESOURCE_REMINDERKEEP = "ReminderKeep";
    //主UI的名称
    public const string UI_MANAGER_VIEWMANAGER_NAME = "ViewManager";
    //警告框的名称
    public const string UI_MANAGER_RESOURCE_ALERT = "Alert";
    #endregion
}

