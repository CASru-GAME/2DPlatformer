using UnityEngine;
using Perk.Model;

namespace Perk.Data
{
    public class GetAllPerk : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            PerkEffectStorage.RegisterPerk(22, () => new GetAllPerk());
        }

        public override void Add()
        {
            Stack++;
            PerkEffectStorage.AddAllPerk();
            if(Stack != 1) return;
            PerkEvents.Land += OnLand;    
        }

        public override void Remove()
        {
            Stack--;
            if(Stack != 0) return;
            PerkEvents.Land -= OnLand;
        }

        private void OnLand()
        {
            for(int i = 0; i < Stack; i++)
                PerkEffectStorage.RemovePerkAtRandom(22);
        }
    }
}
