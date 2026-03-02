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
    }
}
