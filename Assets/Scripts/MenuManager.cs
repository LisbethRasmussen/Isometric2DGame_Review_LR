using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private GameObject _endScreen;

    private void Start()
    {
        _endScreen.SetActive(false);
    }

    public void ChangeEquipment(int equipmentIndex)
    {
        GameManager.Instance.PlayerTransform.GetComponent<PlayerController>().ChangeEquipment(equipmentIndex);
    }

    public void OpenEndScreen()
    {
        _endScreen.SetActive(true);
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }
}
