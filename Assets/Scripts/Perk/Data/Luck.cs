using UnityEngine;
using Perk.Model;
using System.Collections;

namespace Perk.Data
{
    public class Luck : PerkEffect
    {
        static Luck()
        {
            PerkEffectStorage.RegisterPerk(8, () => new Luck());
        }

        public override void Add()
        {
            PerkEffectReference.Instance.AdditionalLuck += 30;
            Stack++;
        }

        public override void Remove()
        {
            PerkEffectReference.Instance.AdditionalLuck -= 30;
            Stack--;
        }
    }
}
