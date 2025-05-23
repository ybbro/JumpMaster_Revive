using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField, Tooltip("인벤토리 보이는 부분 전체")] private Transform visiblePart;
    [SerializeField, Tooltip("슬롯의 프리펩")] private Transform slotPrefab;
    [SerializeField, Tooltip("입벤토리 정렬 패널")] private Transform layoutPanel;
    [SerializeField, Tooltip("아이템 정보 출력 패널")] private InfoPanel infoPanel;
    
    // 인벤토리 아이템 정보
    List<Slot> slots = new List<Slot>();
    
    public List<Slot> GetSlots { get { return slots; } }

    void Start()
    {
        // 아이템 추가 메서드를 플레이어의 액션에 등록
        GameManager.Instance.Player.addItem += AddItem;
    }

    // 인벤토리 활성/비활성
    public void ActiveChange()
    {
        // 활성/비활성 전환
        visiblePart.gameObject.SetActive(!visiblePart.gameObject.activeSelf);
        
        // 인벤토리 출현 때는 마우스가 나타나고 닫을 때는 마우스가 사라지도록
        if(visiblePart.gameObject.activeSelf)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    // 인벤토리에 아이템 추가
    void AddItem(InventorySlot addItemInfo)
    {
        // 쌓을 수 있는 아이템이라면
        if (addItemInfo.item.canStack)
        {
            // 얼만큼 쌓아야하는지 추적하기 위한 변수
            int stackAmount = addItemInfo.count;
            // 기존의 칸들에 동일한 아이템 슬롯이 있다면 쌓기
            for (int i = 0; i < slots.Count; i++)
            {
                // 동일한 아이템이면
                if (slots[i].slotInfo.item.id == addItemInfo.item.id)
                {
                    // 해당 칸에 쌓고 남는 값 반환
                    stackAmount = slots[i].AddCount(addItemInfo.count);
                    // 해당 칸에 다 쌓아서 남는 갯수가 없다면, 더 이상 루프를 진행할 필요가 없기에 break
                    if(stackAmount.Equals(0))
                        break;
                }
            }
            
            // 새로 슬롯을 생성하면서 넣어주기
            // 쌓으려는 수를 모두 인벤토리에 넣었다면 끝!
            while (stackAmount > 0)
            {
                // 새 칸 생성 및 아이템 정보 표시, 적재하지 못한 값을 반환
                stackAmount = CreateASlot(addItemInfo);
            }
        }
        // 적재 불가능한 아이템이라면 새 슬롯 생성
        else
        {
            CreateASlot(addItemInfo);
        }
    }

    int CreateASlot(InventorySlot addItemInfo)
    {
        int exceedAmount = 0;
        // 인벤토리 새로 한칸 생성하여 인벤토리 자동정렬, Slot 컴포넌트를 가져오는데 성공하였다면
        if (Instantiate(slotPrefab.gameObject, layoutPanel).TryGetComponent(out Slot slot))
        {
            // 정보를 넘겨주고 이를 표시
            exceedAmount = slot.CreateSlot(addItemInfo);
            // 슬롯 리스트에 추가
            slots.Add(slot);
        }
        
        return exceedAmount;
    }

    public void OpenInfoPanel(int slotIndex)
    {
        // 해당 슬롯으로 위치 이동
        infoPanel.transform.position = slots[slotIndex].transform.position;
        // 활성화
        infoPanel.gameObject.SetActive(true);
        // 정보 표시
        ItemData data = slots[slotIndex].slotInfo.item;
        infoPanel.InitInfos(data.itemName, data.description);
    }
    public void CloseInfoPanel()
    {
        infoPanel.gameObject.SetActive(false);
    }
}

public class InventorySlot
{
    public ItemData item; // 아이템 정보
    public int count; // 보유 갯수

    public InventorySlot(ItemData _item, int _count)
    {
        item = _item;
        count = _count;
    }
}
