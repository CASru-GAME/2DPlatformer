using UnityEngine;

namespace Scene.Data
{
    [CreateAssetMenu(fileName = "SceneNameData", menuName = "ScriptableObjects/SceneNameData")]
    public class SceneNameData : ScriptableObject
    {
        [SerializeField] private string titleSceneName;
        public string TitleSceneName => titleSceneName;
        [SerializeField] private string perkSceneName;
        public string PerkSceneName => perkSceneName;
        [SerializeField] private string gameSceneName;
        public string GameSceneName => gameSceneName;
        [SerializeField] private string currentPerkSceneName;
        public string CurrentPerkSceneName => currentPerkSceneName;
        [SerializeField] private string transitionSceneName;
        public string TransitionSceneName => transitionSceneName;
    }
}
