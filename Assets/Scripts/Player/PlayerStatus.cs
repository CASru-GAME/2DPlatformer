using UnityEngine;
using System;
using Perk.Model;

public class PlayerStatus
{
    // 外部からは読み取れるが直接書き換えられないようにする
    public int CurrentLives { get; private set; }
    public int MaxLives { get; private set; }

    // イベント：残機が変化したときに、Unityに通知する
    public event Action<int> OnLivesChanged;
    public event Action OnGameOver;

    // コンストラクタ：ゲーム開始時の残機を設定
    public PlayerStatus(int initialLives)
    {
        MaxLives = initialLives;
        CurrentLives = MaxLives;
    }

    // ダメージを受けるか判定するメソッド
    public bool CanTakeDamage()
    {
        var perk = Perk.Model.PerkEffectReference.Instance;

        // 無敵状態の場合はダメージを受けない
        if (perk.IsInvincible)
        {
            Debug.Log("ダメージを受けない（無敵状態）");
            return false;
        }

        // シールドがある場合はスタックを減らしてガード
        if (perk.HasShield)
        {
            perk.ShieldStack--;
            Debug.Log($"ガード　残りシールドスタック: {perk.ShieldStack}");
            
            // シールドを消費したらしばらく無敵
            perk.InvincibleSeconds = 1f; // 例: 1秒間無敵
            return false;
        }

        // それ以外の場合はダメージを受ける(デバッグはPlayerControllerの方で行う)
        return true;
    }

    public void InitializeStatus()
    {
        var perk = PerkEffectReference.Instance;
        if (perk != null)
        {
            MaxLives += perk.AdditionalMaxLife; // パークの追加残機を反映
            CurrentLives = MaxLives; // 現在の残機も最大に合わせて更新
        }
        OnLivesChanged?.Invoke(CurrentLives);
    }
    
    // 残機を減らす
    public void DecreaseLife()
    {
        if (CurrentLives <= 0) 
        {
            return;
        }

        CurrentLives--;
        
        // 通知を送る（UIの更新などに使われる）
        OnLivesChanged?.Invoke(CurrentLives);

        if (CurrentLives <= 0)
        {
            OnGameOver?.Invoke();
        }
    }

    // 残機を増やすメソッド
    public void AddLife()
    {
        CurrentLives++;
        OnLivesChanged?.Invoke(CurrentLives);
    }

    // リセット（コンティニュー時など）
    public void ResetStatus()
    {
        CurrentLives = MaxLives;
        OnLivesChanged?.Invoke(CurrentLives);
    }
}