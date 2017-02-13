using UnityEngine;
using System.Collections;
using System;

public class TimeTools
{
	public static string GetCurrentSystemTime()
	{
		return System.DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss");
	}

	public static bool IfSystemTimeIsNewDay(string lastSystemTime)
	{
		System.DateTime last = System.Convert.ToDateTime(lastSystemTime);
		System.DateTime cur = System.DateTime.Now;
		if( ( cur.Year > last.Year ) || (cur.DayOfYear - last.DayOfYear >= 1) ){
		//if (cur.Millisecond - last.Millisecond >= 10) {
			Debug.LogWarning ("SystemTimeIsNewDay");
			return true;
		}
		else
			return false;		
	}

	public static bool IfSystemTimeIsNewDay(System.DateTime last)
	{
		System.DateTime cur = System.DateTime.Now;
		if( ( cur.Year > last.Year ) || (cur.DayOfYear - last.DayOfYear >= 1) ){
		//if (cur.Millisecond - last.Millisecond >= 10) {
			Debug.LogWarning ("SystemTimeIsNewDay");
			return true;
		}
		else
			return false;		
	}
}

