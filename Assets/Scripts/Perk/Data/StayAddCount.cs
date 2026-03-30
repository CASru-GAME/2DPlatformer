using UnityEngine;
using Perk.Model;
using Unity.Mathematics;

namespace Perk.Data
{
    public class StayAddCount : PerkEffect
    {
        private float lastY;
        private float staySeconds = 0f;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            //PerkEffectStorage.RegisterPerk(16, () => new StayAddCount());
        }

        public override void Add()
        {
            Stack++;
            if(Stack != 1) return;
            PerkEvents.CheckPosition += OnCheckPosition;
        }

        public override void Remove()
        {
            Stack--;
            if(Stack == 0)
                PerkEvents.CheckPosition -= OnCheckPosition;
        }

        private void OnCheckPosition(Vector2 position)
        {
            staySeconds += Time.deltaTime;
            if(Mathf.Abs(position.y - lastY) > 0.1f)
            {
                lastY = position.y;
                staySeconds = 0f;
            }
            else if(staySeconds >= 1f)
            {
                PerkEffectReference.Instance.AdditionalPerkStack += Stack;
                staySeconds = 0f;
            }
        }
    }
}
