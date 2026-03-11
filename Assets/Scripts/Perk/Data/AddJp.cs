using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class AddJp : PerkEffect
    {
        static AddJp()
        {
            PerkEffectStorage.RegisterPerk(10, () => new AddJp());
        }

        public override void Add()
        {
            PerkEffectReference.Instance.AdditionalJumpCount += 1;
            Stack++;
        }

        public override void Remove()
        {
            PerkEffectReference.Instance.AdditionalJumpCount -= 1;
            Stack--;
        }
    }
}
