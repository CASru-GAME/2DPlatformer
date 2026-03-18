using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class TestPerk : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
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
