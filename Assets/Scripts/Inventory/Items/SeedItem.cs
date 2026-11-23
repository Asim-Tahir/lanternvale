using UnityEngine;

[CreateAssetMenu(fileName = "Seed", menuName = "Inventory/Items/Seed Item")]
public sealed class SeedItem : Item
{
  [Header("Stacking")]
  public override int StackCapacity => 4;

  [Header("Classification")]
  public override ItemType Type => ItemType.Seed;
  public SeedType SeedType;
}

public enum SeedType : int
{
  Virgo,
  Apple
}