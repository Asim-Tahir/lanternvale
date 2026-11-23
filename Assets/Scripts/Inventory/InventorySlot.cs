using System;
using UnityEngine;

[Serializable]
public struct InventorySlot
{
    public Vector2Int Position;
    public Item Item;
    public int Quantity;

    public readonly bool HasItem => Item != null;

    public InventorySlot(Vector2Int position, Item item, int quantity)
    {
        Position = position;
        Item = item;
        Quantity = item.IsStackable && quantity > 1 ? quantity : 1;
    }
}
