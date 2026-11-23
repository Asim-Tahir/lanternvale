using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  [SerializeField] private Button _exitButton;
  [SerializeField] private UIDialogController _pauseDialog;

  public static GameManager Instance { get; private set; }

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
    _exitButton.onClick.AddListener(OnExitClicked);
  }

  private void OnDestroy()
  {
    _exitButton.onClick.RemoveListener(OnExitClicked);
  }

  private void OnExitClicked()
  {
    Application.Quit();
  }

  /// <summary>
  /// Pause game and open pause dialog
  /// </summary>
  public void Pause()
  {
    if (_pauseDialog != null)
    {
      _pauseDialog.SafeOpen();
      IsPaused = true;
    }
  }

  public void Resume()
  {
    if (_pauseDialog != null)
    {
      _pauseDialog.SafeClose();
      IsPaused = false;
    }
  }

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
}