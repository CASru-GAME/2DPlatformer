using UnityEngine;

public class TestPerk : Perk
{
    public override int Id => 0;

    static TestPerk()
    {
        PerkStorage.RegisterPerk(0, () => new TestPerk());
    }

    public override void OnEnable()
    {
        PerkEvents.Jump += OnJump;
    }

    public override void OnDisable()
    {
        PerkEvents.Jump -= OnJump;
    }

    private void OnJump(int value)
    {
        Debug.Log($"Jump: {value}");
    }
}
