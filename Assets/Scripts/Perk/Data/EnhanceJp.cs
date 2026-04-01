using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class EnhanceJp : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(11, () => new EnhanceJp());
        }

        public override void Add()
        {
            Stack++;
            PerkEffectReference.Instance.JumpPowerMultiplierBase += 0.2f;
        }

        public override void Remove()
        {
            Stack--;
            PerkEffectReference.Instance.JumpPowerMultiplierBase -= 0.2f;
        }
    }
}
