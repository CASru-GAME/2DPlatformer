using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class LfAddJp : PerkEffect
    {
        private bool isAffective = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(6, () => new LfAddJp());
        }

        public override void Add()
        {
            Stack++;
            if(Stack != 1) return;
            PerkEvents.CheckLife += OnCheckLife;
        }

        public override void Remove()
        {
            Stack--;
            if(Stack != 0) return;
            PerkEvents.CheckLife -= OnCheckLife;
        }

        private void OnCheckLife(int currentLife)
        {
            if (currentLife <= 2 && !isAffective)
            {
                isAffective = true;
                PerkEffectReference.Instance.AdditionalJumpCount += Stack;
                PerkEffectStorage.AddUsedPerkID(6);
            }
            else if (currentLife > 2 && isAffective)
            {
                isAffective = false;
                PerkEffectReference.Instance.AdditionalJumpCount -= Stack;
            }
        }
    }
}
