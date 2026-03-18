using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class Warp : PerkEffect
    {
        private int useStack = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(19, () => new Warp());
        }

        public override void Add()
        {
            Stack++;
            useStack += 2;
            if(Stack != 1) return;
            PerkEvents.UsePerk += OnUsePerk;
        }

        public override void Remove()
        {
            Stack--;
            useStack -= 2;
            if(Stack != 0) return;
            PerkEvents.UsePerk -= OnUsePerk;
        }

        private void OnUsePerk(int id)
        {
            if(id != 19) return;
            Remove();
            PerkEffectReference.Instance.WarpStack = useStack;
        }
    }
}
