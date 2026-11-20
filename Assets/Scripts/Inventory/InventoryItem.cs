using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
  [Header("Basic Info")]
  public string Id;
  public string Name;
  [TextArea(2, 4)]
  public string Description;
  public Sprite Sprite;

  [Header("Stacking")]
  public bool IsStackable = false;
  public int StackCapacity = 1;

  [Header("Classification")]
  public InventoryItemType Type;
}

public enum InventoryItemType : int
{
  Seed,
  Tool
}
