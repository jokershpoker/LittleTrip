using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener



{

    public PlayerMovement PM;

    // Start is called before the first frame update
    void Start()
    {

        if (Advertisement.isSupported)
        {
            Advertisement.AddListener(this);
            Advertisement.Initialize("3248268", false);
        }
    }

    public void WatchAds()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("video");
        }
    }

    public void RebornBtn()
    {
        PM.RebornFirstLaunch();
        gameObject.SetActive(false);
        MainMenuController.FirstLaunch = false;

    }

    public void OnUnityAdsReady(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        PM.AdsCallBack.Invoke();
    }
}
