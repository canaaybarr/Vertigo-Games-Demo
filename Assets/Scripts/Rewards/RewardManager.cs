using Handler;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Rewards
{
    public class RewardManager : Singleton<RewardManager>
    {
        #region Variables
        [SerializeField] private RectTransform rewardGardient;
        [SerializeField] private GameObject rewardContainer;
        float rewardTime = 0f;
        float rewardRate = 2.4f;
        public bool animationOn = true;
        public int rewardIndex = 0;
        #endregion
        
        
        public void ShowAwards()
        {
            rewardTime += Time.deltaTime;
            if (rewardTime > rewardRate)
            {
                rewardTime = 0f;
                // Aralık dışı hatadan kaçının, indeks değerini kontrol edin
                if (rewardIndex + 1 >= CollectHandler.Instance.gainedRewards.Count)
                {
                    animationOn = false;
                    rewardGardient.DOAnchorPosX(0f, 2f);
                }
                else
                {
                    rewardIndex++;
                }
                AddReward(CollectHandler.Instance.reward.Sprite,CollectHandler.Instance.reward.Count);
            }
        }
        
        public void AddReward(Sprite rewardSprite, int rewardCount)
        {
            //Merkezde gösterildikten sonra panele Ödül ekle
            Addressables.Instance.objectRewardPrefabAssetReference.InstantiateAsync(rewardContainer.transform, false).Completed += (op) =>
            {
                GameObject newReward = op.Result;
                newReward.transform.SetAsFirstSibling();
                newReward.GetComponentInChildren<Image>().sprite = rewardSprite;
                newReward.GetComponentInChildren<Text>().text = rewardCount.ToString();
            };
        }
    }
}
