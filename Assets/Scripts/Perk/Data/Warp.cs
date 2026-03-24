using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class Warp : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(19, () => new Warp());
        }

        public override void Add()
        {
            Stack++;
            UseStack += 2;
            if(Stack != 1) return;
            PerkEvents.UsePerk += OnUsePerk;
            PerkEffectStorage.AddUsePerk(19);
        }

        public override void Remove()
        {
            Stack--;
            UseStack = Mathf.Max(0, UseStack - 2);
            if(Stack != 0) return;
            PerkEvents.UsePerk -= OnUsePerk;
        }

        private void OnUsePerk(int id)
        {
            if(id != 19 || UseStack <= 0) return;
            UseStack = Mathf.Max(0, UseStack - 1);
            PerkEffectReference.Instance.WarpStack = UseStack;
        }
    }
}
