using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class Shield : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(7, () => new Shield());
        }

        public override void Add()
        {
            PerkEffectReference.Instance.ShieldStack += 3;
            Stack += 3;
            if(Stack != 3) return;
            PerkEvents.Update += OnUpdate;
        }

        public override void Remove()
        {
            PerkEffectReference.Instance.ShieldStack = Mathf.Max(0, PerkEffectReference.Instance.ShieldStack - 1);
            Stack--;
            if(Stack != 0) return;
            PerkEvents.Update -= OnUpdate;
        }

        private void OnUpdate()
        {
            Stack = PerkEffectReference.Instance.ShieldStack;
            if(Stack == 0) Remove();
        }
    }
}
