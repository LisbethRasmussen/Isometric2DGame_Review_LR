using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    public void ChangeEquipment(int equipmentIndex)
    {
        GameManager.Instance.PlayerTransform.GetComponent<PlayerController>().ChangeEquipment(equipmentIndex);
    }
}
