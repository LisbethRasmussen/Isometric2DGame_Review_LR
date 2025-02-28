using UnityEngine;

/// <summary>
/// Main class for managing UI elements.
/// </summary>
public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject _endScreen;

    private void Start()
    {
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
    /// Displays the UI for the end screen.
    /// </summary>
    public void OpenEndScreen()
    {
        _endScreen.SetActive(true);
    }

    /// <summary>
    /// Executes restart logic.
    /// </summary>
    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }
}
