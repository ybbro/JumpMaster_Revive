using System;
using UnityEngine;

public enum ItemType
{
    Equip,
    Consume,
    Resource
}

// � ���� ä���ִ���
public enum ConsumableType
{
    Health,
    Stamina,
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value; // �󸶳� ä���ִ��� ��
}


[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] public string itemName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject prefab;

    [Header("Stacking")] public bool canStack; // ������ ���� ���� �� �ִ���
    public int maxStack; // �ִ�� ���� �� �ִ� ������ ����
    
    [Header("Consumable")] // �Һ� �� ȿ����
    public ItemDataConsumable[] consumables;
    
    [Header("Equipment")]
    public GameObject equipmentPrefab;
}
