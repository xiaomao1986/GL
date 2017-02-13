using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Find_Item_Config : MonoBehaviour 
{
	public static int mm = 0;
	public int mmm = 0;
    [System.Serializable]
    public class ConfigData
    {
        public int number = 9999;
        public Vector3 distance_excursion = new Vector3(0.5f, .5f, .5f);//距离浮动值
        public float radius_min = 5f;       //最小半径
        public float altitude = 3f;         //初始高度

        public float angle_Random_size = 10f;   //角度密度随机值
        public float angle_Random_min = 10f;    //角度密度值
        public float long_distance_sparse = 10f;    //远距离稀疏

        public Vector2 radius_Random_size = new Vector2(1f, 1f);     //远离随机值
        public Vector2 radius_Random_min = new Vector2(2f, 2f);      //远离度
        public float radius_curve = .5f;      //弯曲度

        [HideInInspector]
        public float angle;                        //当前角度
        [HideInInspector]
        public Vector3 radius = Vector3.zero;      //当前半径
        [HideInInspector]
        public float radius_curve_value = 0;           //弯曲度
        [HideInInspector]
        public float v_long_distance_sparse = 0;       //远距离稀疏
    }

    public ConfigData[] config;

    private int configIndex = 0;
    private Transform m_transform;
    void Awake()
    {
        m_transform = this.transform;
		mmm=mm ++;
        init();
        for (int i = 0; i < 500; i++)
        {
            points.Add(getNextT());
        }

    }

    //配置文件
	private List<Vector3> randomList = new List<Vector3>();
	public List<Vector3> points = new List<Vector3>();
	private int pointsIndex = 0;
	public Vector3 getNext(int Inde)
	{
        return points[Inde];
	}
	public void reset ()
	{
		pointsIndex = 0;
	}
    public Vector3 getNextT()
    {
        Vector3 returnV ;

        if(randomList.Count>=1)
        {
            returnV = randomList[0];
            randomList.RemoveAt(0);
           // MSDebug.Log("returnV______________" + returnV.ToString());
            return returnV;
        }

        ConfigData data;
		int m = 0;
		do{
            data = config[configIndex];
            data.angle += Random.Range(data.angle_Random_min, data.angle_Random_min + data.angle_Random_size + data.v_long_distance_sparse);
            m_transform.localPosition = data.radius + new Vector3(Random.Range(0, data.distance_excursion.x), Random.Range(0, data.distance_excursion.y), Random.Range(0, data.distance_excursion.z));
            m_transform.RotateAround(Vector3.zero, Vector3.up, data.angle);
            randomList.Add(m_transform.localPosition);
			if(m++>1000)break;
        } while (data.angle < 360 );


        //切换下一圈
        data.radius_curve_value += data.radius_curve;
        float rx = data.radius_curve_value + data.radius_Random_min.x + Random.Range(0, data.radius_Random_size.x);
        float ry = data.radius_Random_min.y + Random.Range(0, data.radius_Random_size.y);
        data.radius = new Vector3(data.radius.x + rx, data.radius.y + ry, 0);
        data.v_long_distance_sparse += Random.Range(0, data.long_distance_sparse);
        data.angle -= 360;

        float size = 99999f;
        for (int i = 0; i < config.Length; i++)
        {
            ConfigData datat = config[i];
            if (datat.number <= 0) continue;
            float msize = Vector2.Distance(Vector2.zero, new Vector2(datat.radius.x, datat.radius.y));
            if (msize < size)
            {
                size = msize;
                configIndex = i;
                    
            }
        }
        data = config[configIndex];
        data.number--;

		//随机分布当前圈
		MSRandom.SetRandomList<Vector3>(randomList);
		
		//返回最后一圈内容
        returnV = randomList[0];
        randomList.RemoveAt(0);
       // MSDebug.Log("returnV______________" + returnV.ToString());
		//MSDebug.Log("+++++++++ box:" + gameObject.transform.parent.name);
        return returnV;
    }

    public void randomNext()
    {

    }


#region 初始化随机信息
    public void init()
    {
        if (config.Length <= 0)

        configIndex = 0;
		randomList.Clear();
        for (int i = 0; i < config.Length; i++)
        {
            ConfigData data = config[i];
            data.radius_curve_value = 0;
            data.angle = 0;
            data.radius = new Vector3(data.radius_min, data.altitude, 0);
        }
        float size = 99999f;
        //检查距离用户最近的一组数据初始化
        for (int i = 0; i < config.Length; i++)
        {
            ConfigData data = config[i];
            float msize = Vector2.Distance(Vector2.zero, new Vector2(data.radius_min, data.altitude));
            if (msize < size)
            {
                size = msize;
                configIndex = i;
            }
        }
    }
#endregion
}
