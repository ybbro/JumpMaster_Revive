using UnityEngine;

// 상호작용 가능하게끔 할 오브젝트에 부착할 인터페이스
public interface IInteractable
{
    public string GetInfo();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    // 아이템 데이터
    [SerializeField] private ItemData data;
    
    // 아이템 정보를 받아서 표시
    public string GetInfo()
    {
        string str = $"{data.itemName}\n{data.description}";
        return str;
    }
    
    // 상호작용을 했을 때 발동할 기능
    public void OnInteract()
    {
        // 아이템 정보, 갯수
        InventorySlot itemToAdd = new InventorySlot(data, 1);
        // 인벤토리에 해당 아이템 추가
        GameManager.Instance.Player.addItem?.Invoke(itemToAdd);
        // 인벤토리에 넣었으니 아이템 오브젝트 파괴
        Destroy(gameObject);
    }
}
