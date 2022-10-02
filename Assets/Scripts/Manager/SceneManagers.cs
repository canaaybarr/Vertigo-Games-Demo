using UnityEngine.SceneManagement;

namespace Manager
{
    public class SceneManagers : Singleton<SceneManagers>
    {
        public string rewardSceneName;
        public string gameSceneName;
    
        public void RewardScene()
        {
            SceneManager.LoadScene(rewardSceneName);
        }
        public void GameScene()
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }
}
