using UnityEngine;

[CreateAssetMenu(fileName = "Tool", menuName = "Inventory/Items/Tool Item")]
public sealed class ToolItem : Item
{
  [Header("Stacking")]
  public override int StackCapacity => 1;

  [Header("Classification")]
  public override ItemType Type => ItemType.Tool;
  public ToolType ToolType;
}

public enum ToolType : int
{
  Hoe,
  WaterPitcher
}