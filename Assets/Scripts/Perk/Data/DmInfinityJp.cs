using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class DmInfinityJp : PerkEffect
    {
        static DmInfinityJp()
        {
            PerkEffectStorage.RegisterPerk(5, () => new DmInfinityJp());
            PerkEvents.Update += OnUpdate;
        }

        public override void Add()
        {
            Stack++;
            if(Stack != 1) return;
            PerkEvents.Damaged += OnDamaged;
        }

        public override void Remove()
        {
            PerkEffectReference.Instance.JumpInfinitySeconds = Mathf.Max(0f, PerkEffectReference.Instance.JumpInfinitySeconds - 2f);
            Stack--;
            if(Stack != 0) return;
            PerkEvents.Damaged -= OnDamaged;
        }

        private void OnDamaged()
        {
            PerkEffectReference.Instance.JumpInfinitySeconds += 2f * Stack;
        }

        private static void OnUpdate()
        {
            PerkEffectReference.Instance.JumpInfinitySeconds = Mathf.Max(0f, PerkEffectReference.Instance.JumpInfinitySeconds - Time.deltaTime);
        }
    }
}
