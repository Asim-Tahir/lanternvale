using UnityEngine;

public abstract class Item : ScriptableObject
{
  [Header("Basic Info")]
  public string Id;
  public string Name;
  [TextArea(2, 4)]
  public string Description;
  public Sprite Sprite;

  [Header("Stacking")]
  public virtual bool IsStackable => StackCapacity > 1;
  public virtual int StackCapacity => 1;

  [Header("Classification")]
  public abstract ItemType Type { get; }
}

public enum ItemType : int
{
  Seed,
  Tool
}
