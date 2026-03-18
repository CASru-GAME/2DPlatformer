using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class Climb : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(15, () => new Climb());
        }

        public override void Add()
        {
            PerkEffectReference.Instance.ClimbStack += 1;
            Stack++;
        }

        public override void Remove()
        {
            PerkEffectReference.Instance.ClimbStack -= 1;
            Stack--;
        }
    }
}
