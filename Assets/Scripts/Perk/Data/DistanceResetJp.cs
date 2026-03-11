using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class DistanceResetJp : PerkEffect
    {
        private float lastY = 0f;
        private float currentY = 0f;

        static DistanceResetJp()
        {
            PerkEffectStorage.RegisterPerk(18, () => new DistanceResetJp());
        }

        public override void Add()
        {
            Stack++;
            if(Stack != 1) return;
            PerkEvents.Jump += OnJump;
            PerkEvents.Land += OnLand;
            PerkEvents.CheckPosition += OnCheckPosition;
        }

        public override void Remove()
        {
            Stack--;
            if(Stack != 0) return;
            PerkEvents.Jump -= OnJump;
            PerkEvents.Land -= OnLand;
            PerkEvents.CheckPosition -= OnCheckPosition;
        }

        private void OnJump()
        {
            lastY = currentY;
        }

        private void OnLand()
        {
            lastY = currentY;
        }

        private void OnCheckPosition(Vector2 position)
        {
            currentY = position.y;
            if(lastY - currentY >= 5f)
            {
                PerkEffectReference.Instance.ResetJumpCountStack += 1;
                lastY = currentY;
            }
        }
    }
}
