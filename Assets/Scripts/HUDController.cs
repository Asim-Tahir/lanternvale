using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
  #region Pause Panel Buttons
  [SerializeField] private Button _exitButton;
  #endregion

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

  private void SelectUIElement(GameObject uiElement)
  {
    if (EventSystem.current != null && uiElement != null)
    {
      EventSystem.current.SetSelectedGameObject(null);
      EventSystem.current.SetSelectedGameObject(uiElement);
    }
  }
}
