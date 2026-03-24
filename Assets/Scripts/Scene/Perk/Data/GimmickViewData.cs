using UnityEngine;

namespace Scene.Data
{
    [CreateAssetMenu(fileName = "GimmickViewData", menuName = "ScriptableObjects/GimmickViewData", order = 1)]
    public class GimmickViewData : ScriptableObject
    {
        [SerializeField] private int id;
        public int ID => id;
        [SerializeField] private string description;
        public string Description => description;
    }
}
