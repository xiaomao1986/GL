using UnityEngine;
using System.Collections;

public class MoveToMagicWandController
{
    private GameObject configObj = null;
    private Animator configAni = null;
    private bool isPlay = false;
	private float runTime = 1f;

    /// <summary>
    /// 初始化动画控制器
    /// </summary>
    /// <param name="item">传入的必须是动画控制文件</param>
    /// <param name="time"></param>
    public GameObject init(string item,float time = 0)
    {
        configObj = MonoBehaviour.Instantiate<GameObject>(Resources.Load<GameObject>(item));
        if (configObj == null) return null;
        configAni = configObj.GetComponent<Animator>();
        if (time > 0) {
			runTime = configAni.GetCurrentAnimatorStateInfo (0).length / time;
		} else {
			runTime = 1f;
		}
		configAni.speed = runTime;
        return configObj;
    }

    /// <summary>
    /// 开始播放运动
    /// </summary>
    public void play()
    {
        if (isPlay) return;
        isPlay = true;
        configAni.Play("goto1");
		configAni.speed = runTime;
    }

    /// <summary>
    /// 获取物体的旋转，移动，放缩
    /// </summary>
    /// <param name="t">控制目标</param>
    /// <param name="s">开启状态</param>
    /// <param name="e">结束状态</param>
    /// <returns>是否播放到最后</returns>
    public bool setTransform(Transform t, SkyTransform s, SkyTransform e)
    {
        if (configObj == null)
			return false;

		float rotation_size = configAni.GetCurrentAnimatorStateInfo(0).normalizedTime%1f;

		if(configAni.GetCurrentAnimatorStateInfo(0).IsName("NULL"))
        {
            isPlay = false;
			return true;
        }

		//设置差值的结果
        t.position = Vector3.Lerp(s.position, e.position, rotation_size*1.1f);
		t.localScale = Vector3.Lerp(s.scale, e.scale, rotation_size*1.1f);
		t.rotation = Quaternion.Slerp(s.rotation, e.rotation, rotation_size*1.1f);

		//计算每隔方向分量
		Vector3 p_y = Vector3.up;
		Vector3 p_x = e.position-s.position;

		Vector3 p_z = Vector3.Cross (p_y, p_x);
		p_y = Vector3.Cross (p_x,p_z);
		p_x.Normalize ();
		p_y.Normalize ();
		p_z.Normalize ();
		p_x = p_x * configObj.transform.position.x;
		p_y = p_y * configObj.transform.position.y;
		p_z = p_z * configObj.transform.position.z;

		t.position += (p_x+p_y+p_z);
        t.rotation = t.rotation * configObj.transform.rotation;
        t.localScale = new Vector3(t.localScale.x * configObj.transform.localScale.x, t.localScale.y * configObj.transform.localScale.y, t.localScale.z * configObj.transform.localScale.z);
        return false;
    }
}
