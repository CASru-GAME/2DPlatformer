using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class RandomJp : PerkEffect
    {
        static RandomJp()
        {
            PerkEffectStorage.RegisterPerk(20, () => new RandomJp());
        }

        public override void Add()
        {
            Stack++;
            if(Stack != 1) return;
            PerkEvents.Jump += OnJump;
        }

        public override void Remove()
        {
            Stack--;
            if(Stack != 0) return;
            PerkEvents.Jump -= OnJump;
        }

        private void OnJump()
        {
            float rand = Random.Range(1, Stack * 3);
            PerkEffectReference.Instance.JumpPowerMultiplier = rand;
        }
    }
}
