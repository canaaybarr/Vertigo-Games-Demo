using Handler;
using Rewards;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class ButtonManager : Singleton<ButtonManager>
    {
        [Header("Buttons Section")] [SerializeField]
        public Button spinButton;
        public Button collectButton;
        [SerializeField] private Button collectPopUpCollectButton;
        [SerializeField] private Button collectPopUpGoBackButton;
        [Header("Collect Section")] public GameObject collectPopUp;
        public Button popUpRestartButton;
        [SerializeField] public Transform wheelPanel;
        
        public void ButtonLists()
        {
            #region Button
            #region Spin
            
            
            spinButton.onClick.AddListener(() =>
            {
                GameHandler.Instance.wheelHandler.SpinStartAction(() =>
                {
                    spinButton.gameObject.SetActive(false);
                    Debug.Log("Spin");
                });
                GameHandler.Instance.wheelHandler.SpinEndAction(async wheelContent =>
                {
                    Debug.Log("Ended Spin: " + wheelContent.Name + "Count ="+ wheelContent.RewardCount);
                            
                    if (wheelContent.RewardClass.ToString() == "Bomb") // Ödülün bomba olup olmadığını kontrol edin
                    {
                        GameHandler.Instance.bombPopUp.SetActive(true);
                        collectButton.gameObject.SetActive(false);
                    }
                    else
                    {
                        await GameHandler.Instance.AddReward(wheelContent.ContentIcon, wheelContent.RewardCount, wheelContent.Name);
                        GameHandler.Instance.SetWheelCarrier();
                        GameHandler.Instance.ZonePanelSlide();
                    }
                });
                GameHandler.Instance.wheelHandler.SpinWheel();
            });
            #endregion
            
            #region Collect Button
            collectButton.onClick.AddListener(() =>
            {
                // Açılır Pencereyi Göster
                collectPopUp.SetActive(true);
            });
            #endregion
            
            #region Collect PopUp Buttons
            collectPopUpCollectButton.onClick.AddListener(() =>
            {
                // Toplama sahnesine git
                GainedRewards.Instance.gainedRewards = GameHandler.Instance.gainedRewards;
                SceneManagers.Instance.RewardScene();
            });
                
            collectPopUpGoBackButton.onClick.AddListener(() =>
            {
                // Açılır pencereyi kapat
                collectPopUp.SetActive(false);
            });
            #endregion
            
            #region Restart Button
            popUpRestartButton.onClick.AddListener(() =>
            {
                // Sahneyi Tekrar Yükle
                SceneManagers.Instance.GameScene();
            });
            #endregion
            #endregion
            
            // Normal bir düğme şeklinde yapılabilir
        }

    }
}
