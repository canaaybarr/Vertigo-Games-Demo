using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Manager;
using Rewards;
using UnityEngine;
using UnityEngine.UI;

namespace Handler
{
    public class GameHandler : Singleton<GameHandler>
    {
        #region Variables
    
        [Header("Zone Section")] public int zoneLevel = 1;
        public GameObject zonePrefab;
        public GameObject zoneContainer;
        public Sprite greenZone;
    
        [Header("Reward Section")]
        public List<Reward> gainedRewards = new List<Reward>();
        public GameObject rewardContainer;
        
        int minZone = 1;
        public bool zoneAnimation = false;
        float nextTargetRectX = -140f;
        
    
        [Header("Wheel Section")] 
        public WheelHandler wheelHandler;
        List<GameObject> instantiatedWheels = new List<GameObject>();
        public RectTransform indicatorPosition;
        
        [Header("Bomb Section")] 
        public GameObject bombPopUp;
        #endregion
        
    
        void Start()
        {
            ButtonManager.Instance.ButtonLists();
            // Bölge seviyesini kontrol edin ve tekerlek tipini ayarlayın
            SetWheelCarrier();
            // Set top panel
            SettingZonePanel();
        }
        
        void Update()
        {
            // Kazanılan ödül yoksa, topla butonunu devre dışı bırakın
            if (gainedRewards.Count != 0)
            {
                ButtonManager.Instance.collectButton.gameObject.SetActive(true);
            }
        }

        public void SetWheelCarrier()
        {
            // Eski tekerlekleri temizle
            if (instantiatedWheels.Count != 0)
            {
                foreach (GameObject wheelObject in instantiatedWheels)
                {
                    Destroy(wheelObject, 0f);
                }
                instantiatedWheels.Clear();
            }
            int index = 0;
            // Bölge seviyesini kontrol et
            if (zoneLevel == 30)
            {
                index = 2;
                Debug.Log(index + "  Gold");
                zoneLevel++;
            }
            else if (zoneLevel % 5 == 0 || zoneLevel == 1)
            {
                index = 1;
                Debug.Log(index + "  Silver");
                zoneLevel++;
            }
            
            else
            {
                Debug.Log(index + "  Bronz");
                zoneLevel++;
            }

            GameObject wheel;
            // wheelprefab ayarla
            Addressables.Instance.wheelAssetReferences[index].InstantiateAsync(ButtonManager
                    .Instance.wheelPanel, false).Completed +=
                (op) =>
                {
                    wheel = op.Result;
                    wheel.transform.SetAsFirstSibling();
                    wheel.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    instantiatedWheels.Add(wheel);
                    wheelHandler = wheel.transform.GetComponent<WheelHandler>();
                    ButtonManager.Instance.spinButton.gameObject.SetActive(true);
                };
        }
    
        #region Zone Panel
    
        
        void SettingZonePanel()// ZoneLevel değişkenine göre bölge seviyesi panelini değiştirin
        {
            // Bölge ilerledikçe, aralıklara göre yeni bölge kartları oluşturulacaktır.
            for (int i = minZone; i < minZone + 20; i++)
            {
                GameObject zone;
    
                zone = Instantiate(zonePrefab);
                zone.transform.SetParent(zoneContainer.transform, false);
                zone.transform.GetComponentInChildren<Text>().text = i.ToString();
                if (i == 1 || i % 5 == 0)
                {
                    zone.gameObject.GetComponent<Image>().sprite = greenZone;
                }
            }
            minZone += 20;
        }
    
        public void ZonePanelSlide()
        {
            // Optimizasyon açısından, bölge ilerledikçe yeni bölge kartları oluşturulur.
            if (zoneLevel % 10 == 0)
            {
                SettingZonePanel();
            }
            zoneContainer.transform.GetComponent<RectTransform>()
                .DOAnchorPosX(nextTargetRectX, 0.5f); // Bölge ilerledikçe panel sola kayar
            nextTargetRectX += -140f;
        }
    
        #endregion
    
        #region Reward Panel
        public async Task AddReward(Sprite rewardSprite, int rewardCount, string rewardName)
        {
            // Ödülün varlığını kontrol et
            var reward = gainedRewards.Where(x => x.Name == rewardName).FirstOrDefault();
            if (reward != null)
            {
                await RewardAnimation(
                    rewardContainer.transform.Find(rewardName).transform.GetChild(1).GetComponent<RectTransform>().position,
                    rewardSprite);
                var count = reward.Count;
                reward.Count = count + rewardCount;
                Animations.Instance.animatedText = rewardContainer.transform.Find(rewardName).GetComponentInChildren<Text>();
                Animations.Instance.targetrewardText = reward.Count;
                Animations.Instance.textAnimationActive = true;
            }
            // Eğer yoksa şimdi ekleyin
            else
            {
                Addressables.Instance.objectRewardPrefabAssetReference
                    .InstantiateAsync(rewardContainer.transform, false).Completed += async (op) =>
                {
                    var newReward = op.Result;
                    newReward.GetComponentInChildren<Image>().sprite = rewardSprite;
                    newReward.GetComponentInChildren<Text>().text = "0";
                    newReward.name = rewardName;
                    await RewardAnimation(
                        rewardContainer.transform.Find(rewardName).transform.GetChild(1).transform
                            .GetComponent<RectTransform>().position, rewardSprite);
                    Animations.Instance.animatedText = newReward.GetComponentInChildren<Text>();
                    Animations.Instance.targetrewardText = rewardCount;
                    Animations.Instance.textAnimationActive = true;
                    var newRewardItem = new Reward();
                    newRewardItem.Name = rewardName;
                    newRewardItem.Sprite = rewardSprite;
                    newRewardItem.Count = rewardCount;
                    gainedRewards.Add(newRewardItem);
                };
                await Task.Delay(1200);
            }
        }
        #endregion
    
        #region Basic Code Animations
        async Task RewardAnimation(Vector3 targetPos, Sprite rewardSprite)
        {
            // addressables nesne al

            Addressables.Instance.objectRewardAnimationPrefabAssetReference.InstantiateAsync(indicatorPosition)
                .Completed += async (op) =>
            {
                GameObject rewardAnimation = op.Result;
                rewardAnimation.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                rewardAnimation.GetComponent<Image>().sprite = rewardSprite;
                await Task.Delay(200);
                rewardAnimation.GetComponent<RectTransform>().DOMove(targetPos, 1f);
                await Task.Delay(1000);
                Destroy(rewardAnimation);
            };
            // Tekerlek değişene kadar görevi bekleyin
            await Task.Delay(1200);
        }
       
        #endregion
    }
}