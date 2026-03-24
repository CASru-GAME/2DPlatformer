using System;
using UnityEngine;

namespace Perk.Data
{
    public static class PerkEvents
    {
        //ジャンプしたとき
        public static Action Jump;
        //ダメージを受けたとき
        public static Action Damaged;
        //現在のライフを(毎フレーム)確認したとき
        public static Action<int> CheckLife;
        //パークを入手したとき
        public static Action Gotten;
        //地面か壁に着地したとき
        public static Action Land;
        //現在の座標を(毎フレーム)確認したとき
        public static Action<Vector2> CheckPosition;
        //引数の番号のパークを使用したとき
        public static Action<int> UsePerk;
        //別のパークによってパークの効果を発動(コピーみたいな)するとき
        public static Action<int> CopyPerk;
        //毎フレーム
        public static Action Update;
        //空中にいるとき
        public static Action InAir;
    }
}
