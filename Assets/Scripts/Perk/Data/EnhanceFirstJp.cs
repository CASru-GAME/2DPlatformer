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
            if(Stack != 0) return;
            PerkEvents.Land -= OnLand;
            PerkEvents.Jump -= OnJump;
        }

        private void OnLand()
        {
            if(isCharging) return;
            isCharging = true;
            PerkEffectReference.Instance.JumpPowerMultiplier += Stack * 0.5f;
        }

        private void OnJump()
        {
            if(isCharging) return;
            isCharging = false;
            PerkEffectReference.Instance.JumpPowerMultiplier -= Stack * 0.5f;
        }
    }
}
