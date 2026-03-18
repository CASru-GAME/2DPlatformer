using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class RandomPerk : PerkEffect
    {
        private int useStack = 0;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(21, () => new RandomPerk());
        }

        public override void Add()
        {
            Stack++;
            useStack += 10;
            if(Stack != 1) return;
            PerkEvents.UsePerk += OnUsePerk;
        }

        public override void Remove()
        {
            Stack--;
            useStack -= 10;
            if(Stack != 0) return;
            PerkEvents.UsePerk -= OnUsePerk;
        }

        private void OnUsePerk(int id)
        {
            if(id != 21) return;
            Remove();
            PerkEffectReference.Instance.UseRandomPerkStack = useStack;
        }
    }
}
