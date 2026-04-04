using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class DmHl : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(4, () => new DmHl());
        }

        public override void Add()
        {
            Stack++;
            if(Stack != 1) return;
            PerkEvents.Damaged += OnDamaged;
        }

        public override void Remove()
        {
            Stack--;
            if(Stack != 0) return;
            PerkEvents.Damaged -= OnDamaged;
        }

        private void OnDamaged()
        {
            if(!DoesHitChance(50)) return;
            PerkEffectReference.Instance.HealStack += 2 * Stack;
            PerkEffectStorage.AddUsedPerkID(4);
        }
    }
}
