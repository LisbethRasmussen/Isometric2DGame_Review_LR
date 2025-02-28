using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main class for managing UI elements.
/// </summary>
public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject _menuScreen;
    [SerializeField] private GameObject _endScreen;

    private void Start()
    {
        _menuScreen.SetActive(true);
        _endScreen.SetActive(false);
    }

    /// <summary>
    /// Changes the the currently equipped weapon of the player.
    /// </summary>
    /// <param name="equipmentIndex">The index of the desired weapon in the player's weapon controller collection.</param>
    public void ChangeEquipment(int equipmentIndex)
    {
        GameManager.Instance.PlayerTransform.GetComponent<PlayerController>().ChangeEquipment(equipmentIndex);
    }

    /// <summary>
    /// Executes start logic.
    /// </summary>
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    /// <summary>
    /// Executes restart logic.
    /// </summary>
    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    /// <summary>
    /// Displays the UI for the end screen.
    /// </summary>
    public void OpenEndScreen()
    {
        _endScreen.SetActive(true);
    }

    /// <summary>
    /// Hides the menu UI from the screen.
    /// </summary>
    public void CloseMenuScreen()
    {
        _menuScreen.SetActive(false);
    }
}
