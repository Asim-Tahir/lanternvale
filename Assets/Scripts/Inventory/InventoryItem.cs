using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
  [Header("Basic Info")]
  public string Id;
  public string Label;
  [TextArea(2, 4)]
  public string Description;
  public Sprite Sprite;

  [Header("Stacking")]
  public bool IsStackable = false;
  public int StackCapacity = 1;

  [Header("Classification")]
  public ItemType Type;
}

public enum ItemType
{
  Seed,
  Tool
}
