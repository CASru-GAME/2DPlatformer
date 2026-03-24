using UnityEngine;

namespace Scene.Data
{
    [CreateAssetMenu(fileName = "PerkViewData", menuName = "ScriptableObjects/PerkViewData", order = 1)]
    public class PerkViewData : ScriptableObject
    {
        [SerializeField] private int id;
        public int ID => id;
        [SerializeField] private Sprite sprite;
        public Sprite Sprite => sprite;
        [SerializeField] private string effectDescription;
        public string Description => effectDescription;
    }
}
