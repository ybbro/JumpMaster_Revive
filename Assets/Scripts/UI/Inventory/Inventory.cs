using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField, Tooltip("�κ��丮 ���̴� �κ� ��ü")] private Transform visiblePart;
    [SerializeField, Tooltip("������ ������")] private Transform slotPrefab;
    [SerializeField, Tooltip("�Ժ��丮 ���� �г�")] private Transform layoutPanel;
    [SerializeField, Tooltip("������ ���� ��� �г�")] private InfoPanel infoPanel;
    
    // �κ��丮 ������ ����
    List<Slot> slots = new List<Slot>();
    
    public List<Slot> GetSlots { get { return slots; } }

    void Start()
    {
        // ������ �߰� �޼��带 �÷��̾��� �׼ǿ� ���
        GameManager.Instance.Player.addItem += AddItem;
    }

    // �κ��丮 Ȱ��/��Ȱ��
    public void ActiveChange()
    {
        // Ȱ��/��Ȱ�� ��ȯ
        visiblePart.gameObject.SetActive(!visiblePart.gameObject.activeSelf);
        
        // �κ��丮 ���� ���� ���콺�� ��Ÿ���� ���� ���� ���콺�� ���������
        if(visiblePart.gameObject.activeSelf)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    // �κ��丮�� ������ �߰�
    void AddItem(InventorySlot addItemInfo)
    {
        // ���� �� �ִ� �������̶��
        if (addItemInfo.item.canStack)
        {
            // ��ŭ �׾ƾ��ϴ��� �����ϱ� ���� ����
            int stackAmount = addItemInfo.count;
            // ������ ĭ�鿡 ������ ������ ������ �ִٸ� �ױ�
            for (int i = 0; i < slots.Count; i++)
            {
                // ������ �������̸�
                if (slots[i].slotInfo.item.id == addItemInfo.item.id)
                {
                    // �ش� ĭ�� �װ� ���� �� ��ȯ
                    stackAmount = slots[i].AddCount(addItemInfo.count);
                    // �ش� ĭ�� �� �׾Ƽ� ���� ������ ���ٸ�, �� �̻� ������ ������ �ʿ䰡 ���⿡ break
                    if(stackAmount.Equals(0))
                        break;
                }
            }
            
            // ���� ������ �����ϸ鼭 �־��ֱ�
            // �������� ���� ��� �κ��丮�� �־��ٸ� ��!
            while (stackAmount > 0)
            {
                // �� ĭ ���� �� ������ ���� ǥ��, �������� ���� ���� ��ȯ
                stackAmount = CreateASlot(addItemInfo);
            }
        }
        // ���� �Ұ����� �������̶�� �� ���� ����
        else
        {
            CreateASlot(addItemInfo);
        }
    }

    int CreateASlot(InventorySlot addItemInfo)
    {
        int exceedAmount = 0;
        // �κ��丮 ���� ��ĭ �����Ͽ� �κ��丮 �ڵ�����, Slot ������Ʈ�� �������µ� �����Ͽ��ٸ�
        if (Instantiate(slotPrefab.gameObject, layoutPanel).TryGetComponent(out Slot slot))
        {
            // ������ �Ѱ��ְ� �̸� ǥ��
            exceedAmount = slot.CreateSlot(addItemInfo);
            // ���� ����Ʈ�� �߰�
            slots.Add(slot);
        }
        
        return exceedAmount;
    }

    public void OpenInfoPanel(int slotIndex)
    {
        // �ش� �������� ��ġ �̵�
        infoPanel.transform.position = slots[slotIndex].transform.position;
        // Ȱ��ȭ
        infoPanel.gameObject.SetActive(true);
        // ���� ǥ��
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
    public ItemData item; // ������ ����
    public int count; // ���� ����

    public InventorySlot(ItemData _item, int _count)
    {
        item = _item;
        count = _count;
    }
}
