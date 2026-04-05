using Perk.Model;

namespace Perk.Data
{
    public abstract class PerkEffect
    {
        public int Stack { get; protected set; } = 0;
        public int UseStack { get; protected set; } = 0;
        public virtual void Add() { }
        public virtual void Remove() { }

        public bool DoesHitChance(int luck)
        {
            return UnityEngine.Random.Range(0, 100) < luck * PerkEffectReference.Instance.LuckMultiplier;
        }
    }
}