using System;
using UnityEngine;

public class GamePrefManager
{
  public static GamePrefManager Instance { get; private set; }

  private const string SAVED_PREF_KEY = "SavedGameData";
  private const string DEFAULT_LEVEL = "GameScene";

  [Serializable]
  private class GamePrefData
  {
    public string level = DEFAULT_LEVEL;
  }

  private GamePrefData _prefData = new();

  public string DefaultLevel => DEFAULT_LEVEL;
  public string Level => _prefData.level;

  private GamePrefManager() { }

  public static GamePrefManager GetInstance()
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
  /// Saves the current game state to <see cref="PlayerPrefs"/>.
  /// </summary>
  public void Save()
  {
    string json = JsonUtility.ToJson(_prefData);
    PlayerPrefs.SetString(SAVED_PREF_KEY, json);
    PlayerPrefs.Save();

    Debug.Log("Game state saved to PlayerPrefs.");
  }

  /// <summary>
  /// Deletes the saved game state from <see cref="PlayerPrefs"/>.
  /// </summary>
  public void ClearSaved()
  {
    _prefData = new();

    PlayerPrefs.DeleteKey(SAVED_PREF_KEY);
  }

  /// <summary>
  /// Updates and saves the game level.
  /// </summary>
  /// <param name="level">Scene name as level</param>
  public void SetLevel(string level)
  {
    _prefData.level = level;
    Save();
  }

  /// <summary>
  /// Loads game state from <see cref="PlayerPrefs"/> when saved data exists.
  /// If no saved data exists, the current in-memory game state are unchanged.
  /// </summary>
  public void Load()
  {
    if (!PlayerPrefs.HasKey(SAVED_PREF_KEY)) return;

    string json = PlayerPrefs.GetString(SAVED_PREF_KEY);
    _prefData = JsonUtility.FromJson<GamePrefData>(json);

    Debug.Log("Game state loaded from PlayerPrefs.");
  }
}
