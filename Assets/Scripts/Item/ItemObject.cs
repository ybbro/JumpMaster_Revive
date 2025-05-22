using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInfo();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData data;

    public string GetInfo()
    {
        string str = $"{data.itemName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        //Player ��ũ��Ʈ ���� ����
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
