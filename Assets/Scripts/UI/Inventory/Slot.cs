using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI count;

    public InventorySlot slotInfo { get; private set; }

    // ������ ������ ������ �� ȣ��
    public int CreateSlot(InventorySlot _inventorySlot)
    {
        // ������ ���� ������ �ְԲ�
        slotInfo = _inventorySlot;
        // ������ ǥ��
        icon.sprite = slotInfo.item.icon;
        // ���� ǥ��
        int exceedAmount = AddCount(0);
        // ��ġ�� ������ ��ȯ
        return exceedAmount;
    }
    
    // ������ ���� ����
    public int AddCount(int amount)
    {
        // ���� ����, ǥ��
        slotInfo.count += amount;
        count.text = Mathf.Clamp(slotInfo.count, 0, slotInfo.item.maxStack).ToString();

        // �� ĭ�� �ִ�� ���� �� �ִ� ������ �ʰ��ϴ� ������ ��
        int exceedAmount = slotInfo.count - slotInfo.item.maxStack;
        // �ִ� ���緮�� �Ѿ��ٸ� �׸�ŭ�� ��ȯ
        if (exceedAmount > 0)
            return exceedAmount;
        // ���� �ʾҴٸ� 0�� ��ȯ
        else
            return 0;
    }

    void UseOrEquipItem()
    {
        switch (slotInfo.item.type)
        {
            // �Һ� ������
            case ItemType.Consume:
                // �ش� ������ Ư�� ȿ�� �ߵ�
                slotInfo.item.UseEffect?.Invoke();
                // �ش� �Һ� �������� �Һ� �Ӽ� ����ŭ
                for(int i = 0; i < slotInfo.item.consumables.Length; i++)
                {
                    // �ش� �Ӽ��� value�� �°� ȸ��
                    switch (slotInfo.item.consumables[i].type)
                    {
                        case ConsumableType.Health:
                            GameManager.Instance.Player.status.Heal(slotInfo.item.consumables[i].value);
                            break;
                    }
                }
                
                // ���� 1�� ���� >> 0�� ���ϰ� �Ǿ��ٸ� 
                if (--slotInfo.count <= 0)
                {
                    // ������ ���� �г� �������
                    GameManager.Instance.Inventory.CloseInfoPanel();
                    // �ش� ���� �ı�
                    GameManager.Instance.Inventory.GetSlots.Remove(this);
                    Destroy(gameObject);
                }
                // ���� ���� ǥ��
                else
                    count.text = slotInfo.count.ToString();
                break;
            
            // ���� ������
            case ItemType.Equip:
                break;
            default:
                break;
        }
    }

    // �ش� ���� ���� ���콺�� �ö��� ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.Inventory.OpenInfoPanel(transform.GetSiblingIndex());
    }

    // �ش� ���� ������ ���콺�� ����� ��
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

    // �ش� ������Ʈ �巡�� ����
    public void OnDrag(PointerEventData eventData)
    {
        // ���콺 ��ġ�� ������Ʈ�� ����ٴϰԲ�
    }

    // �巡�װ� ���� ��
    public void OnEndDrag(PointerEventData eventData)
    {
        // ���콺 ��ġ�� �κ��丮 ���̶��
        // ������ ������
    }
}
