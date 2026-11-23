using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  [SerializeField] private Button _exitButton;
  [SerializeField] private UIDialogController _pauseDialog;

  public static GameManager Instance { get; private set; }

  private readonly GamePrefManager _gamePrefManager = GamePrefManager.GetInstance();

  public bool CanContinueGame
  {
    get
    {
      return !(_gamePrefManager.Level == _gamePrefManager.DefaultLevel);
    }
  }

  public bool IsPaused { get; private set; }

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
      return;
    }
  }

  private void Start()
  {
    if (_exitButton != null)
    {
      _exitButton.onClick.AddListener(OnExitClicked);
    }
  }

  private void OnDestroy()
  {
    if (_exitButton != null)
    {
      _exitButton.onClick.RemoveListener(OnExitClicked);
    }
  }

  private void OnExitClicked()
  {
    Application.Quit();
  }

  /// <summary>
  /// Pauses the game and opens the pause dialog.
  /// </summary>
  public void Pause()
  {
    if (_pauseDialog != null)
    {
      _pauseDialog.SafeOpen();
      IsPaused = true;
    }
  }

  /// <summary>
  /// Resumes the paused game and closes the pause dialog.
  /// </summary>
  public void Resume()
  {
    if (_pauseDialog != null)
    {
      _pauseDialog.SafeClose();
      IsPaused = false;
    }
  }

  /// <summary>
  /// Toggles the current pause state of the game.
  /// </summary>
  public void TogglePause()
  {
    if (IsPaused)
    {
      Resume();
    }
    else
    {
      Pause();
    }
  }

  public void LoadLevel()
  {
    SceneManager.LoadScene(_gamePrefManager.Level);
  }

  public void LoadDefaultLevel()
  {
    SceneManager.LoadScene(_gamePrefManager.DefaultLevel);
  }

  public void NewGame()
  {
    _gamePrefManager.ClearSaved();

    LoadDefaultLevel();
  }

  public void ContinueGame()
  {
    if (!CanContinueGame) return;

    LoadLevel();
  }
}