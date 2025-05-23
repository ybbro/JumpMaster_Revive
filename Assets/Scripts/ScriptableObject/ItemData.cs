using System;
using UnityEngine;
using UnityEngine.Events;

// ������ Ÿ��
public enum ItemType
{
    Equip,
    Consume,
}

// �Һ��� �� � ���� ä���ִ���
public enum ConsumableType
{
    Health,
}

[Serializable] // �ν����Ϳ� ������ �� �ְԲ�
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value; // �󸶳� ä���ִ��� ��
}


[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] public int id;
    public string itemName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject prefab;

    [Header("Stacking")] public bool canStack; // ������ ���� ���� �� �ִ���
    public int maxStack; // �ִ�� ���� �� �ִ� ������ ����
    
    [Header("Consumable")] // �Һ� �� ȿ����
    public ItemDataConsumable[] consumables;
    public UnityEvent UseEffect;
    
    [Header("Equipment")] // ��� ������ ������
    public GameObject equipmentPrefab;
}
