using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main class for managing UI elements.
/// </summary>
public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject _menuScreen;
    [SerializeField] private GameObject _endScreen;

    //--- Lisbeth: I would suggest that MenuManager shouldn't be a Singleton,
    // but rather just a MonoBehaviour, which could be located on the same GO ad the
    // GameManager, and that the GameManager could keep a public reference to the MenuManager.
    // This way, you can kill of the below Start(), and make an initialization method instead,
    // called from the GameManager, giving you greater control over the execution order of your scripts.
    // Tip: Reducing Awake(), Enable(), Start(), etc. methods from MonoBehaviour will increase performance
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
        //--- Lisbeth: Should be cached instead, either when the player initializes,
        // or by a direct Serialized Field
        GameManager.Instance.PlayerTransform.GetComponent<PlayerController>().ChangeEquipment(equipmentIndex);
    }

    //--- Lisbeth: Consider making a script for buttons instead.
    // The MenuManager could be kept cleaner by only focusing on turning the UI on and off
    // and resetting UI elements, while a ButtonManager could be the intermediator instead.

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
