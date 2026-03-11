using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class Glide : PerkEffect
    {
        static Glide()
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
