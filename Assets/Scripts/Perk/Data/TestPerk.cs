using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class TestPerk : PerkEffect
    {
        static TestPerk()
        {
            PerkEffectStorage.RegisterPerk(0, () => new TestPerk());
        }

        public override void Add()
        {
            PerkEvents.Jump += OnJump;
            Stack++;
        }

        public override void Remove()
        {
            PerkEvents.Jump -= OnJump;
            Stack--;
        }

        private void OnJump()
        {
            Debug.Log("Jump");
        }
    }
}
