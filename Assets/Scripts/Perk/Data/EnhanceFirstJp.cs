using UnityEngine;
using Perk.Model;
using System;

namespace Perk.Data
{
    public class EnhanceFirstJp : PerkEffect
    {
        private bool isCharging = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(12, () => new EnhanceFirstJp());
        }

        public override void Add()
        {
            Stack++;
            if(Stack != 1) return;
            PerkEvents.Land += OnLand;
            PerkEvents.Jump += OnJump;
        }

        public override void Remove()
        {
            Stack--;
            if(isCharging) PerkEffectReference.Instance.JumpPowerMultiplierBase -= 0.2f;
            if(Stack != 0) return;
            PerkEvents.Land -= OnLand;
            PerkEvents.Jump -= OnJump;
        }

        private void OnLand()
        {
            Debug.Log("Land:" + isCharging);
            if(isCharging) return;
            isCharging = true;
            PerkEffectReference.Instance.JumpPowerMultiplierBase += Stack * 0.2f;
            PerkEffectStorage.AddUsedPerkID(12);
        }

        private void OnJump()
        {
            Debug.Log("Jump:" + isCharging);
            if(!isCharging) return;
            isCharging = false;
            PerkEffectReference.Instance.JumpPowerMultiplierBase -= Stack * 0.2f;
        }
    }
}
