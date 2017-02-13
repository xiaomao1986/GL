using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;


public sealed class CSVProvider
{
	private static string[][] Array;

	private CSVProvider()
	{

	}

	public static bool LoadCSV(string InData, char split_char = ',')
	{
		//读取csv二进制文件  
		//TextAsset binAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;    
		//s = binAsset.text;

		//读取每一行的内容  System.Environment.NewLine
		//string [] lineArray = binAsset.text.TrimEnd().Split("\r"[0]);  

		string[] lineArray = InData.TrimEnd().Split(System.Environment.NewLine.ToCharArray());

		//创建二维数组  
		Array = new string[lineArray.Length][];

		//把csv中的数据储存在二位数组中  
		for (int i = 0; i < lineArray.Length; i++)
		{
			Array[i] = lineArray[i].Trim().Split(split_char);
		}

		return true;
	}

	//根据行列的索引取得内容.
	public static string GetDataByRowAndCol(int nRow, int nCol)
	{
		if (Array.Length <= 0 || nRow >= Array.Length)
		{
			return "";
		}
		if (nCol >= Array[0].Length)
		{
			return "";
		}

		return Array[nRow][nCol];
	}

	//根据行列的头来取得内容.
	public static string GetDataByIdAndName(int nId, string strName)
	{
		if (Array.Length <= 0)
		{
			return "";
		}

		int nRow = Array.Length;
		int nCol = Array[0].Length;
		for (int i = 1; i < nRow; ++i)
		{
			string strId = string.Format("\n{0}", nId);
			if (Array[i][0] == strId)
			{
				for (int j = 0; j < nCol; ++j)
				{
					if (Array[0][j] == strName)
					{
						return Array[i][j];
					}
				}
			}
		}

		return "";
	}

	//获得指定列的所有内容.
	public static string[] GetColumnData(int nCol)
	{
		string[] ret = new string[Array.Length];
		for (int i = 0, len = ret.Length; i < len; i++)
		{
			ret[i] = Array[i][nCol];
		}
		return ret;
	}

	//获得指定行的所有内容.
	public static string[] GetRowData(int nRow)
	{
		return Array[nRow];
	}
}
