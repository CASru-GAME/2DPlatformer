using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class Shield : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(7, () => new Shield());
        }

        public override void Add()
        {
            PerkEffectReference.Instance.ShieldStack += 3; 
            Stack++;
        }

        public override void Remove()
        {
            PerkEffectReference.Instance.ShieldStack = Mathf.Max(0, PerkEffectReference.Instance.ShieldStack - 3);
            Stack--;
        }
    }
}
