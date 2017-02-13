// ***********************************************************
// Written by Heyworks Unity Studio http://unity.heyworks.com/
// ***********************************************************
//#define USE_NATIVE_R

using UnityEngine;

/// <summary>
/// 陀螺仪   Gyroscope controller that works with any device orientation.
/// </summary>
public class MySkyGyroController : MonoBehaviour
{
    #region [Private fields]
	public static MySkyGyroController instance;
	public Transform m_transform;
    public bool gyroEnabled = false;
    private const float lowPassFilterFactor = 0.2f;

    private readonly Quaternion baseIdentity = Quaternion.Euler(90, 0, 0);
    private readonly Quaternion landscapeRight = Quaternion.Euler(0, 0, 90);
    private readonly Quaternion landscapeLeft = Quaternion.Euler(0, 0, -90);
    private readonly Quaternion upsideDown = Quaternion.Euler(0, 0, 180);

    private Quaternion cameraBase = Quaternion.identity;
    private Quaternion calibration = Quaternion.identity;
    private Quaternion baseOrientation = Quaternion.Euler(90, 0, 0);
    private Quaternion baseOrientationRotationFix = Quaternion.identity;

    private Quaternion referanceRotation = Quaternion.identity;
    private bool debug = true;
    private bool isOpen = false;
    #endregion

    #region [Unity events]

    protected void Start()
    {
        MsgBase.MsgAdd("OpenGyroController", OpenGyroController);
        MsgBase.MsgAdd("OffGyroController", OffGyroController);
        MsgBase.MsgAdd<Callback>("OffGyroControllerCallback", OffGyroController1);
		instance = this;
		m_transform = this.gameObject.transform;
#if USE_NATIVE_R
        Messenger.AddListener<bool>("Native_startRotationCallBack", Native_startRotationCallBack);
        SkyNativeManager.Instance.startRotation();
        isStartNativeRotation = false;
#endif

		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) 
        {
			AttachGyro();
			Input.gyro.enabled = true;
			EnableGyro (true);
		}
        isOpen = true;
    }

    private void OffGyroController()
    {
        isOpen = false;
    }
    private void OpenGyroController()
    {
        isOpen = true;
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        
    }
    private void OffGyroController1(Callback callback)
    {
        isOpen = true;
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        callback();

    }
    private void onDistory()
    {
#if USE_NATIVE_R
        Messenger.RemoveListener<bool>("Native_startRotationCallBack", Native_startRotationCallBack);
#endif


    }
    protected void Update()
    {

        if (isOpen==false)
        {
            return;
        }
#if USE_NATIVE_R
        //if (isStartNativeRotation)
        {
            float[] data = SkyNativeManager.Instance.getRotation();
            if (data != null && data.Length>=3)
                transform.rotation = Quaternion.Euler(new Vector3(data[0], data[1], data[2]));
        }
#else
		m_transform.rotation = Quaternion.Slerp(m_transform.rotation,
                cameraBase * (ConvertRotation(referanceRotation * Input.gyro.attitude) * GetRotFix()), lowPassFilterFactor);
        //Debug.Log("transform.rotation===========" + transform.rotation);
        //transform.RotateAround(transform.position, Vector3.left, 180);
        
#endif

    }
    #endregion

    #region [Public methods]

    /// <summary>
    /// Attaches gyro controller to the transform.
    /// </summary>
    private void AttachGyro()
    {
       // gyroEnabled = true;
        ResetBaseOrientation();
        UpdateCalibration(true);
        UpdateCameraBaseRotation(true);
        RecalculateReferenceRotation();
    }

    /// <summary>
    /// Detaches gyro controller from the transform
    /// </summary>
    private void DetachGyro()
    {
        gyroEnabled = false;
    }

	public void EnableGyro(bool pause)
	{
		gyroEnabled = pause;
	}

    #endregion

    #region [Private methods]

#if USE_NATIVE_R
    private bool isStartNativeRotation = false;

    public void Native_startRotationCallBack(bool isok)
    {
        isStartNativeRotation = isok;
    }
#endif
    /// <summary>
    /// Update the gyro calibration.
    /// </summary>
    private void UpdateCalibration(bool onlyHorizontal)
    {
        if (onlyHorizontal)
        {
            var fw = (Input.gyro.attitude) * (-Vector3.forward);
            fw.z = 0;
            if (fw == Vector3.zero)
            {
                calibration = Quaternion.identity;
            }
            else
            {
                calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
            }
        }
        else
        {
            calibration = Input.gyro.attitude;
        }
    }

    /// <summary>
    /// Update the camera base rotation.
    /// </summary>
    /// <param name='onlyHorizontal'>
    /// Only y rotation.
    /// </param>
    private void UpdateCameraBaseRotation(bool onlyHorizontal)
    {
        if (onlyHorizontal)
        {
            var fw = transform.forward;
            fw.y = 0;
            if (fw == Vector3.zero)
            {
                cameraBase = Quaternion.identity;
            }
            else
            {
                cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
            }
        }
        else
        {
            cameraBase = transform.rotation;
        }
    }

    /// <summary>
    /// Converts the rotation from right handed to left handed.
    /// </summary>
    /// <returns>
    /// The result rotation.
    /// </returns>
    /// <param name='q'>
    /// The rotation to convert.
    /// </param>
    private static Quaternion ConvertRotation(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    /// <summary>
    /// Gets the rot fix for different orientations.
    /// </summary>
    /// <returns>
    /// The rot fix.
    /// </returns>
    private Quaternion GetRotFix()
    {
#if UNITY_3_5
        if (Screen.orientation == ScreenOrientation.Portrait)
            return Quaternion.identity;
       
        if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.Landscape)
            return landscapeLeft;
               
        if (Screen.orientation == ScreenOrientation.LandscapeRight)
            return landscapeRight;
               
        if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            return upsideDown;
        return Quaternion.identity;
#else
        return Quaternion.identity;
#endif
    }

    /// <summary>
    /// Recalculates reference system.
    /// </summary>
    private void ResetBaseOrientation()
    {
        baseOrientationRotationFix = GetRotFix();
        baseOrientation = baseOrientationRotationFix * baseIdentity;
    }

    /// <summary>
    /// Recalculates reference rotation.
    /// </summary>
    private void RecalculateReferenceRotation()
    {
        referanceRotation = Quaternion.Inverse(baseOrientation) * Quaternion.Inverse(calibration);
       
    }

    #endregion

        private void OnGUI()
        {
            //Vector3 v = transform.rotation.eulerAngles;
//            GUI.skin.label.fontSize = 30;
//           // GUI.Label(new Rect(20, 120, 1000, 60), "x:" + v.x+" y:"+v.y + "z:"+v.z);
//
//		GUI.Label(new Rect(20, 20, 1000, 60), "attitude:" + Input.gyro.attitude);
//		GUI.Label(new Rect(20, 120, 1000, 60), "enabled:" + Input.gyro.enabled);
//		GUI.Label(new Rect(20, 220, 1000, 60), "gravity:" + Input.gyro.gravity);
//		GUI.Label(new Rect(20, 320, 1000, 60), "rotationRate:" + Input.gyro.rotationRate);
//		GUI.Label(new Rect(20, 420, 1000, 60), "rotationRateUnbiased:" + Input.gyro.rotationRateUnbiased);
//		GUI.Label(new Rect(20, 520, 1000, 60), "updateInterval:" + Input.gyro.updateInterval);
//		GUI.Label(new Rect(20, 620, 1000, 60), "userAcceleration:" + Input.gyro.userAcceleration);
//		GUI.Label(new Rect(20, 720, 1000, 60), "supportsGyroscope:" + SystemInfo.supportsGyroscope);
//
        }
}
