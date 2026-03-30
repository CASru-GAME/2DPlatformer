using UnityEngine;
using Perk.Model;
using Unity.Mathematics;

namespace Perk.Data
{
    public class RandomPerk : PerkEffect
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            //PerkEffectStorage.RegisterPerk(21, () => new RandomPerk());
        }

        public override void Add()
        {
            Stack++;
            UseStack += 10;
            if(Stack != 1) return;
            PerkEvents.UsePerk += OnUsePerk;
            PerkEffectStorage.AddUsePerk(21);
        }

        public override void Remove()
        {
            Stack--;
            UseStack = Mathf.Max(0, UseStack - 10);
            if(Stack != 0) return;
            PerkEvents.UsePerk -= OnUsePerk;
        }

        private void OnUsePerk(int id)
        {
            if(id != 21 || UseStack <= 0) return;
            UseStack = math.max(0, UseStack - 1);
            PerkEffectReference.Instance.UseRandomPerkStack = UseStack;
        }
    }
}
