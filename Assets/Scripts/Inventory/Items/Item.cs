using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
  [Header("Basic Info")]
  public string Id;
  public string Name;
  [TextArea(2, 4)]
  public string Description;
  public Sprite Sprite;

  [Header("Stacking")]
  public bool IsStackable => StackCapacity > 1;
  public int StackCapacity = 1;

  [Header("Classification")]
  public ItemType Type;
}

public enum ItemType : int
{
  Seed,
  Tool
}
