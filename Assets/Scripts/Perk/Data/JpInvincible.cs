using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class JpInvincible : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(13, () => new JpInvincible());
            PerkEvents.Update += OnUpdate;
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
            PerkEffectReference.Instance.InvincibleSeconds -= 2f;
            if(Stack != 0) return;
            PerkEvents.Jump -= OnJump;
        }

        private void OnJump()
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
