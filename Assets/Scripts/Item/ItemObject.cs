using UnityEngine;

// ��ȣ�ۿ� �����ϰԲ� �� ������Ʈ�� ������ �������̽�
public interface IInteractable
{
    public string GetInfo();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    // ������ ������
    [SerializeField] private ItemData data;
    
    // ������ ������ �޾Ƽ� ǥ��
    public string GetInfo()
    {
        string str = $"{data.itemName}\n{data.description}";
        return str;
    }
    
    // ��ȣ�ۿ��� ���� �� �ߵ��� ���
    public void OnInteract()
    {
        // ������ ����, ����
        InventorySlot itemToAdd = new InventorySlot(data, 1);
        // �κ��丮�� �ش� ������ �߰�
        GameManager.Instance.Player.addItem?.Invoke(itemToAdd);
        // �κ��丮�� �־����� ������ ������Ʈ �ı�
        Destroy(gameObject);
    }
}
