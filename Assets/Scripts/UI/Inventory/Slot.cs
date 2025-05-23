using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI count;

    public InventorySlot slotInfo { get; private set; }

    // 아이템 슬롯을 생성할 때 호출
    public int CreateSlot(InventorySlot _inventorySlot)
    {
        // 아이템 정보 가지고 있게끔
        slotInfo = _inventorySlot;
        // 아이콘 표시
        icon.sprite = slotInfo.item.icon;
        // 수량 표시
        int exceedAmount = AddCount(0);
        // 넘치는 수량을 반환
        return exceedAmount;
    }
    
    // 아이템 수량 증가
    public int AddCount(int amount)
    {
        // 수량 증가, 표시
        slotInfo.count += amount;
        count.text = Mathf.Clamp(slotInfo.count, 0, slotInfo.item.maxStack).ToString();

        // 한 칸에 최대로 쌓을 수 있는 갯수를 초과하는 아이템 수
        int exceedAmount = slotInfo.count - slotInfo.item.maxStack;
        // 최대 적재량을 넘었다면 그만큼을 반환
        if (exceedAmount > 0)
            return exceedAmount;
        // 넘지 않았다면 0을 반환
        else
            return 0;
    }

    void UseOrEquipItem()
    {
        switch (slotInfo.item.type)
        {
            // 소비 아이템
            case ItemType.Consume:
                // 해당 아이템 특수 효과 발동
                slotInfo.item.UseEffect?.Invoke();
                // 해당 소비 아이템의 소비 속성 수만큼
                for(int i = 0; i < slotInfo.item.consumables.Length; i++)
                {
                    // 해당 속성을 value에 맞게 회복
                    switch (slotInfo.item.consumables[i].type)
                    {
                        case ConsumableType.Health:
                            GameManager.Instance.Player.status.Heal(slotInfo.item.consumables[i].value);
                            break;
                    }
                }
                
                // 수량 1개 감소 >> 0개 이하가 되었다면 
                if (--slotInfo.count <= 0)
                {
                    // 아이템 설명 패널 사라지게
                    GameManager.Instance.Inventory.CloseInfoPanel();
                    // 해당 슬롯 파괴
                    GameManager.Instance.Inventory.GetSlots.Remove(this);
                    Destroy(gameObject);
                }
                // 수량 감소 표시
                else
                    count.text = slotInfo.count.ToString();
                break;
            
            // 장착 아이템
            case ItemType.Equip:
                break;
            default:
                break;
        }
    }

    // 해당 슬롯 위에 마우스가 올라갔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.Inventory.OpenInfoPanel(transform.GetSiblingIndex());
    }

    // 해당 슬롯 위에서 마우스가 벗어났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.Inventory.CloseInfoPanel();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseOrEquipItem();
        }
    }

    // 해당 오브젝트 드래그 도중
    public void OnDrag(PointerEventData eventData)
    {
        // 마우스 위치를 오브젝트가 따라다니게끔
    }

    // 드래그가 끝날 때
    public void OnEndDrag(PointerEventData eventData)
    {
        // 마우스 위치가 인벤토리 밖이라면
        // 아이템 버리기
    }
}
