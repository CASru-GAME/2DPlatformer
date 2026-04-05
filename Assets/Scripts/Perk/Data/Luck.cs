using UnityEngine;
using Perk.Model;
using System.Collections;

namespace Perk.Data
{
    public class Luck : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(8, () => new Luck());
        }

        public override void Add()
        {
            PerkEffectReference.Instance.LuckMultiplier += 2;
            Stack++;
        }

        public override void Remove()
        {
            PerkEffectReference.Instance.LuckMultiplier -= 2;
            Stack--;
        }
    }
}
