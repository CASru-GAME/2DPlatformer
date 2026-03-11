using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class DmHlInvincible : PerkEffect
    {
        static DmHlInvincible()
        {
            PerkEffectStorage.RegisterPerk(3, () => new DmHlInvincible());
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
            PerkEffectReference.Instance.InvincibleSeconds -= 2f;
            Stack--;
            if(Stack != 0) return;
            PerkEvents.Damaged -= OnDamaged;
        }

        private void OnDamaged()
        {
            if(!DoesHitChance(50)) return;
            PerkEffectReference.Instance.InvincibleSeconds += 2f * Stack;
        }

        private static void OnUpdate()
        {
            PerkEffectReference.Instance.InvincibleSeconds = Mathf.Max(0f, PerkEffectReference.Instance.InvincibleSeconds - Time.deltaTime);
        }
    }
}
