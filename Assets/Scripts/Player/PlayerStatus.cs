using UnityEngine;
using System;

public class PlayerStatus
{
    // 外部からは読み取れるga直接書き換えられないようにする
    public int CurrentLives { get; private set; }
    public int MaxLives { get; private set; }

    // イベント：残機が変化したときに、Unityに通知する
    public event Action<int> OnLivesChanged;
    public event Action OnGameOver;

    // コンストラクタ：ゲーム開始時の残機を設定
    public PlayerStatus(int initialLives)
    {
        MaxLives = initialLives;
        CurrentLives = initialLives;
    }

    // 残機を減らす
    public void DecreaseLife()
    {
        if (CurrentLives <= 0) return;

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