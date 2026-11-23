using System;
using System.Collections.Generic;
using UnityEngine;

public class TilePrefManager
{
    public static TilePrefManager Instance { get; private set; }

    private const string SAVED_PREF_KEY = "SavedTileData";

    /// <summary>
    /// Stores the tile position and tile-list index for a placed tile.
    /// </summary>
    [Serializable]
    public class TilePrefTileEntry
    {
        public Vector3Int tilePosition;
        public int tileIndex;
    }

    [Serializable]
    private class TilePrefData
    {
        public List<TilePrefTileEntry> tileEntries = new();
    }
    private TilePrefData _prefData = new();

    /// <summary>
    /// Gets the list of tiles currently stored in memory.
    /// </summary>
    public List<TilePrefTileEntry> TileEntries => _prefData.tileEntries;

    private TilePrefManager() { }

    public static TilePrefManager GetInstance()
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
    /// Serializes the current tile data and saves it to <see cref="PlayerPrefs"/>.
    /// </summary>
    public void Save()
    {
        string json = JsonUtility.ToJson(_prefData);
        PlayerPrefs.SetString(SAVED_PREF_KEY, json);
        PlayerPrefs.Save();

        Debug.Log("Tiles saved to PlayerPrefs.");
    }

    /// <summary>
    /// Adds a tile record to memory and immediately saves the tiles data.
    /// </summary>
    /// <param name="entry">The Tile record to add.</param>
    public void SavedAdd(TilePrefTileEntry entry)
    {
        _prefData.tileEntries.Add(entry);
        Save();
    }

    /// <summary>
    /// Removes every placed-tile record matching the supplied predicate and saves the updated data.
    /// </summary>
    /// <param name="match">The predicate used to identify records to remove.</param>
    public void SavedRemoveAll(Predicate<TilePrefTileEntry> match)
    {
        _prefData.tileEntries.RemoveAll(match);
        Save();
    }

    public void ClearSaved()
    {
        PlayerPrefs.DeleteKey(SAVED_PREF_KEY);
    }

    /// <summary>
    /// Loads Tile data from <see cref="PlayerPrefs"/> when saved data exists.
    /// If no saved data exists, the current in-memory Tile data is unchanged.
    /// </summary>
    public void Load()
    {
        if (!PlayerPrefs.HasKey(SAVED_PREF_KEY)) return;

        string json = PlayerPrefs.GetString(SAVED_PREF_KEY);
        _prefData = JsonUtility.FromJson<TilePrefData>(json);

        Debug.Log("Tiles loaded from PlayerPrefs.");
    }
}
