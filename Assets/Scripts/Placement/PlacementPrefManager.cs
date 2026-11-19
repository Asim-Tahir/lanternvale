using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacementPrefManager
{
    private const string SAVED_PREF_KEY = "SavedPlacementData";

    /// <summary>
    /// Stores the tile position and tile-list index for a placed tile.
    /// </summary>
    [Serializable]
    public class PlacedTilePrefData
    {
        public Vector3Int position;
        public int index;
    }

    [Serializable]
    private class PlacementPrefData
    {
        public List<PlacedTilePrefData> tiles = new();
    }
    private PlacementPrefData _prefData = new();

    /// <summary>
    /// Gets the list of tiles currently stored in memory.
    /// </summary>
    public List<PlacedTilePrefData> Tiles => _prefData.tiles;

    /// <summary>
    /// Serializes the current placement data and saves it to <see cref="PlayerPrefs"/>.
    /// </summary>
    public void Save()
    {
        string json = JsonUtility.ToJson(_prefData);
        PlayerPrefs.SetString(SAVED_PREF_KEY, json);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Adds a placed-tile record to memory and immediately saves the placement data.
    /// </summary>
    /// <param name="placedTilePrefData">The placement record to add.</param>
    public void SavedAdd(PlacedTilePrefData placedTilePrefData)
    {
        _prefData.tiles.Add(placedTilePrefData);
        Save();
    }

    /// <summary>
    /// Removes every placed-tile record matching the supplied predicate and saves the updated data.
    /// </summary>
    /// <param name="match">The predicate used to identify records to remove.</param>
    public void SavedRemoveAll(Predicate<PlacedTilePrefData> match)
    {
        _prefData.tiles.RemoveAll(match);
        Save();
    }

    /// <summary>
    /// Loads placement data from <see cref="PlayerPrefs"/> when saved data exists.
    /// If no saved data exists, the current in-memory placement data is unchanged.
    /// </summary>
    public void Load()
    {
        if (!PlayerPrefs.HasKey(SAVED_PREF_KEY)) return;

        string json = PlayerPrefs.GetString(SAVED_PREF_KEY);
        _prefData = JsonUtility.FromJson<PlacementPrefData>(json);
    }
}
