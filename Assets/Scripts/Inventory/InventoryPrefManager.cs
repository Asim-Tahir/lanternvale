using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPrefManager
{
    public static InventoryPrefManager Instance { get; private set; }

    private const string SAVED_PREF_KEY = "SavedInventoryData";

    [Serializable]
    public class InventoryPrefSlotData
    {
        public Vector2Int position;
        public string itemId;
        public int quantity;
    }

    [Serializable]
    public class InventoryPrefData
    {
        public List<InventoryPrefSlotData> slots = new();
    }

    private InventoryPrefData _prefData = new();

    /// <summary>
    /// Gets the list of inventory slot entries currently stored in memory.
    /// </summary>
    public List<InventoryPrefSlotData> InventorySlots => _prefData.slots;

    private InventoryPrefManager() { }

    /// <summary>
    /// Gets the <see cref="InventoryManager"/> singleton instance, creating it if one
    /// does not already exist.
    /// </summary>
    /// <returns>The <see cref="InventoryManager"/> singleton instance.</returns>
    /// <remarks>
    /// Call this method from Unity's main thread
    /// </remarks>
    public static InventoryPrefManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = new();
            return Instance;
        }
        else
        {
            return Instance;
        }
    }

    /// <summary>
    /// Serializes the current inventory slot data and saves it to <see cref="PlayerPrefs"/>.
    /// </summary>
    public void Save()
    {
        string json = JsonUtility.ToJson(_prefData);
        PlayerPrefs.SetString(SAVED_PREF_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("Inventory saved to PlayerPrefs.");
    }

    /// <summary>
    /// Replaces the saved slot data with the current inventory state and persists it.
    /// </summary>
    /// <param name="inventory">The current inventory slot grid.</param>
    public void ReplaceSaved(InventorySlot[,] inventory)
    {
        _prefData.slots.Clear();

        for (int row = 0; row < inventory.GetLength(0); row++)
        {
            for (int column = 0; column < inventory.GetLength(1); column++)
            {
                var slot = inventory[row, column];
                if (slot.Item == null)
                {
                    continue;
                }

                _prefData.slots.Add(new InventoryPrefSlotData
                {
                    position = new Vector2Int(row, column),
                    itemId = slot.Item.Id,
                    quantity = slot.Quantity
                });
            }
        }

        Save();
    }

    /// <summary>
    /// Deletes the saved inventory data from <see cref="PlayerPrefs"/>.
    /// </summary>
    public void ClearSaved()
    {
        PlayerPrefs.DeleteKey(SAVED_PREF_KEY);
    }

    /// <summary>
    /// Loads inventory slot data from <see cref="PlayerPrefs"/> when saved data exists.
    /// If no saved data exists, the current in-memory inventory data is unchanged.
    /// </summary>
    public void Load()
    {
        if (!PlayerPrefs.HasKey(SAVED_PREF_KEY)) return;

        string json = PlayerPrefs.GetString(SAVED_PREF_KEY);
        _prefData = JsonUtility.FromJson<InventoryPrefData>(json);

        Debug.Log("Inventory loaded from PlayerPrefs.");
    }
}