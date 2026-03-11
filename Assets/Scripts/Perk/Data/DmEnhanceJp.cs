using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class DmEnhanceJp : PerkEffect
    {
        private bool isCharging = false;

        static DmEnhanceJp()
        {
            PerkEffectStorage.RegisterPerk(2, () => new DmEnhanceJp());
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
            if (isCharging) PerkEffectReference.Instance.JumpPowerMultiplier -= 1f; 
            Stack--;
            if(Stack != 0) return;
            PerkEvents.Damaged -= OnDamaged;
            PerkEvents.Jump -= OnJump;
        }

        private void OnDamaged()
        {
            PerkEffectReference.Instance.JumpPowerMultiplier += Stack; 
            isCharging = true;
        }

        private void OnJump()
        {
            PerkEffectReference.Instance.JumpPowerMultiplier -= Stack; 
            isCharging = false;
        }
    }
}
