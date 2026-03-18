using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class AddLf : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(9, () => new AddLf());
        }

        public override void Add()
        {
            PerkEffectReference.Instance.AdditionalMaxLife += 1;
            Stack++;
        }

        public override void Remove()
        {
            PerkEffectReference.Instance.AdditionalMaxLife -= 1;
            Stack--;
        }
    }
}
