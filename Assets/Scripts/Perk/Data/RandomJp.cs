using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class RandomJp : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
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
            PerkEffectReference.Instance.JumpPowerMultiplierRandom = -1f;
        }

        private void OnJump()
        {
            float rand = Random.Range(0.5f, 1.5f);
            PerkEffectReference.Instance.JumpPowerMultiplierRandom = rand;
        }
    }
}
