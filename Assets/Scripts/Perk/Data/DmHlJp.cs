using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class DmHlJp : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            //PerkEffectStorage.RegisterPerk(1, () => new DmHlJp());
        }

        public override void Add()
        {
            Stack++;
            if(Stack != 1) return;
            PerkEvents.Damaged += OnDamaged;
            PerkEvents.Jump += OnJump;
        }

        public override void Remove()
        {
            Stack--;
            if(Stack != 0) return;
            PerkEvents.Damaged -= OnDamaged;
            PerkEvents.Jump -= OnJump;
            PerkEffectReference.Instance.ForcedJumpStack = 0;
        }

        private void OnDamaged()
        {
            if(!DoesHitChance(20)) return;
            //PerkEffectReference.Instance.HealStack += Stack; 
            PerkEffectReference.Instance.ForcedJumpStack += Stack;
            //PerkEffectStorage.AddUsedPerkText(1);
        }

        private void OnJump()
        {
            PerkEffectReference.Instance.ForcedJumpStack = 0;
        }
    }
}
