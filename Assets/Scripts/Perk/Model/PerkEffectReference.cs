using UnityEngine;

namespace Perk.Model
{
    public class PerkEffectReference
    {
        private static PerkEffectReference instance;
        public static PerkEffectReference Instance
        {
            get
            {
                instance ??= new PerkEffectReference();
                return instance;
            }
        }

        //以下、Playerが参照するパークの効果の変数
        public int ForcedJumpStack = 0;
        //強制的にジャンプさせるかどうか
        public bool IsForcedJump => ForcedJumpStack > 0;
        //ジャンプ力の倍率
        public float JumpPowerMultiplier => JumpPowerMultiplierRandom > 0 ? JumpPowerMultiplierRandom : JumpPowerMultiplierBase;
        public float JumpPowerMultiplierBase = 1f;
        public float JumpPowerMultiplierRandom = -1f;
        public float InvincibleSeconds = 0;
        //現在無敵かどうか
        public bool IsInvincible => InvincibleSeconds > 0;
        public int ShieldStack = 0;
        //現在無敵かどうか
        public bool HasShield => ShieldStack > 0;
        public int HealStack = 0;
        //現在回復できるかどうか
        public bool CanHeal => HealStack > 0;
        public float JumpInfinitySeconds = 0;
        //現在ジャンプ無限かどうか
        public bool IsJumpInfinity => JumpInfinitySeconds > 0;
        //追加のジャンプ回数
        public int AdditionalJumpCount = 0;
        public int AdditionalLuck = 0;
        //追加の最大体力
        public int AdditionalMaxLife = 0;
        public int ClimbStack = 1;
        //壁つかまりできるかどうか
        public bool CanClimb => ClimbStack > 0;
        public int AdditionalPerkStack = 0;
        public bool DoesAddPerkCount => AdditionalPerkStack > 0;
        public int GlideStack = 1;
        //滑空できるかどうか
        public bool CanGlide => GlideStack > 0;
        public int ResetJumpCountStack = 0;
        //ジャンプがリセットされるかどうか
        public bool DoesResetJumpCount => ResetJumpCountStack > 0;
        //水平方向の移動速度の倍率
        public float MoveSpeedMultiplier = 1f;
        public int WarpStack = 0;
        //ワープできるかどうか
        public bool CanWarp => WarpStack > 0;
        public int UseRandomPerkStack = 0;
        //ランダムなパークを使用できるかどうか
        public bool CanUseRandomPerk => UseRandomPerkStack > 0;

        //参照の仕方
        //PerkEffectReference.Instance.変数名
    }
}
