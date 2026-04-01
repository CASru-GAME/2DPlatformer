using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class DmEnhanceJp : PerkEffect
    {
        private bool isCharging = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
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
            if (isCharging) PerkEffectReference.Instance.JumpPowerMultiplierBase -= 0.2f; 
            Stack--;
            if(Stack != 0) return;
            PerkEvents.Damaged -= OnDamaged;
            PerkEvents.Jump -= OnJump;
        }

        private void OnDamaged()
        {
            PerkEffectReference.Instance.JumpPowerMultiplierBase += Stack * 0.2f; 
            isCharging = true;
        }

        private void OnJump()
        {
            if(isCharging) PerkEffectReference.Instance.JumpPowerMultiplierBase -= Stack * 0.2f; 
            isCharging = false;
        }
    }
}
