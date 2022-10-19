using AdmobAds;
using System.Threading.Tasks;
using UnityEngine;

public class AdsCounter : MonoBehaviour
{
    [SerializeField] private int adsResetTime = 300;
    [SerializeField] private int timeOut = 120;
    public bool isIdle;
    private bool isResetRunning = false;
    private bool istimeOutRunning = false;

    public static AdsCounter Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void Start()
    {
        InitializeAdsResetTime();
        InitializeTimeOutTime();
    }

    public void InitializeAdsResetTime()
    {
        adsResetTime = 300;
        adRunning = false;
        timerAd = false;
    }

    private void InitializeTimeOutTime()
    {
        timeOut = 120;
        isIdle = false;
    }

    public bool adRunning;
    public bool timerAd;
    public async void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            InitializeTimeOutTime();
        }
        else
        {
            if (!istimeOutRunning)
            {
                if (timeOut > 0)
                {
                    istimeOutRunning = true;
                    await Task.Delay(1000);
                    timeOut -= 1;
                    isIdle = false;
                    istimeOutRunning = false;
                }
                else
                {
                    isIdle = true;
                    istimeOutRunning = false;
                }
            }
        }

        if (!adRunning)
        {
            if (!isResetRunning)
            {
                if (adsResetTime > 0)
                {
                    isResetRunning = true;
                    await Task.Delay(1000);
                    adsResetTime -= 1;
                    isResetRunning = false;
                }
                else
                {
                    if (AdsManager.Instance.isRewardVideoLoaded() && AdsManager.Instance.isInterstitialLoaded())
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            timerAd = true;
                            AdsManager.Instance.ShowRewardedAd();
                            adRunning = true;
                        }
                        else
                        {
                            timerAd = true;
                            AdsManager.Instance.ShowInterstitialAd();
                            adRunning = true;
                        }
                    }
                    else if (AdsManager.Instance.isRewardVideoLoaded())
                    {
                        timerAd = true;
                        AdsManager.Instance.ShowRewardedAd();
                        adRunning = true;
                    }
                    else if (AdsManager.Instance.isInterstitialLoaded())
                    {
                        timerAd = true;
                        AdsManager.Instance.ShowInterstitialAd();
                        adRunning = true;
                    }
                }
            }
        }


    }

}
