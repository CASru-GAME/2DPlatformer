using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class Glide : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(17, () => new Glide());
        }

        public override void Add()
        {
            PerkEffectReference.Instance.GlideStack += 1;
            Stack++;
        }

        public override void Remove()
        {
            PerkEffectReference.Instance.GlideStack -= 1;
            Stack--;
        }
    }
}
