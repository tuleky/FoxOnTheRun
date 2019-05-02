using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour {

	public static void ShowAd()
	{
		if (Advertisement.IsReady())
		{
			Advertisement.Show();
		}
	}

	public static bool CheckForAd()
	{
		bool adShowing;
		adShowing = Advertisement.isShowing;
		return adShowing;
	}
	
}
