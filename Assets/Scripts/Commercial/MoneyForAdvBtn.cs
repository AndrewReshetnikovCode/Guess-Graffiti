using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;


public class MoneyForAdvBtn : MonoBehaviour
{
    public string rewardID;

    [SerializeField] Image _cdProgress;
    [SerializeField] Toggle _b;
    private void OnEnable()
    {
        YG2.onRewardAdv += OnReward;
        YG2.onCloseRewardedAdv += OnRewardCancel;
        YG2.onErrorRewardedAdv += OnRewardCancel;
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= OnReward;
        YG2.onCloseRewardedAdv -= OnRewardCancel;
        YG2.onErrorRewardedAdv -= OnRewardCancel;
    }

    public void Show(string id)
    {
        //ActionManager.I.SetAudioActive(false);

        //GP_Ads.ShowRewarded(id, OnReward, null, OnRewardCancel);
        YG2.RewardedAdvShow(rewardID);
    }
    void OnRewardCancel()
    {
        //ActionManager.I.SetAudioActive(true);
    }
    private void OnReward(string id)
    {
        //ActionManager.I.SetAudioActive(true);

        if (id == rewardID)
        {
            //ActionManager.I.st.money += ActionManager.I.se.moneyForAdv;
            //ActionManager.I.EmitCoins(Camera.main.ScreenToWorldPoint(transform.position), ActionManager.I.se.moneyForAdv);

            GraffitiGuessGame.I.DisplayHint();
        }
    }
}
