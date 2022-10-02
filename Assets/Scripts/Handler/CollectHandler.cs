using System.Collections.Generic;
using System.Threading.Tasks;
using Rewards;
using UnityEngine;
using UnityEngine.UI;

namespace Handler
{
    public class CollectHandler : Singleton<CollectHandler>
    {
        #region Variables
        public List<Reward> gainedRewards = new List<Reward>();

        [Header("Reward Values")]
        [SerializeField] private GameObject rewardObject;
        [SerializeField] private Text rewardName;
        [SerializeField] private Text rewardCount;
        [SerializeField] private Image rewardImage;

        [Header("Reward Panel")]
        public Reward reward;
        [Header("Others")]
        [SerializeField] private RectTransform rewardBg;
        bool firstLoad = true;
        #endregion
    
        void Start()
        {
            gainedRewards = GainedRewards.Instance.gainedRewards;
        }
        async void Update()
        {
            // Başlat Item in etrafındaki Effecti
            rewardBg.Rotate(new Vector3(0f, 0f, 100f) * Time.deltaTime);
            await RewardAnimation();
        }
        async Task RewardAnimation()
        {
            if (firstLoad) // Yükte biraz bekle
            {
                await Task.Delay(500);
                firstLoad = false;
                if(rewardObject!=null)
                    rewardObject.SetActive(true);
            }
            // Ödül Değerlerini Ayarlama
            if (RewardManager.Instance.animationOn && rewardName != null && rewardCount != null && rewardImage != null)
            {
                reward = gainedRewards[RewardManager.Instance.rewardIndex];
                RewardUI();
            }
            else
            {
                rewardObject.SetActive(false);
            }
        }

        void RewardUI()
        {
            rewardName.text = reward.Name;
            rewardCount.text = reward.Count.ToString();
            rewardImage.sprite = reward.Sprite;
            RewardManager.Instance.ShowAwards();
        }
    }
}
