using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class LdDistanceHl : PerkEffect
    {
        private Vector2 lastPosition = Vector2.zero;
        private Vector2 currentPosition = Vector2.zero;
        private bool isLanding = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(14, () => new LdDistanceHl());
        }

        public override void Add()
        {
            Stack++;
            if(Stack != 1) return;
            PerkEvents.Land += OnLand;
            PerkEvents.Jump += OnJump;
            PerkEvents.CheckPosition += OnCheckPosition;
        }

        public override void Remove()
        {
            Stack--;
            if(Stack != 0) return;
            PerkEvents.Land -= OnLand;
            PerkEvents.Jump -= OnJump;
            PerkEvents.CheckPosition -= OnCheckPosition;
        }

        private void OnLand()
        {
            isLanding = true;
            if(Vector2.Distance(lastPosition, currentPosition) < 5f) return;
            PerkEffectReference.Instance.HealStack += Stack;
            PerkEffectStorage.AddUsedPerkText("回復");
        }

        private void OnJump()
        {
            if(!isLanding) return;
            isLanding = false;
        }

        private void OnCheckPosition(Vector2 position)
        {
            currentPosition = position;
            if(isLanding) lastPosition = currentPosition;
        }
    }
}
