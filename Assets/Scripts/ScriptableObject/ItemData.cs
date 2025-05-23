using System;
using UnityEngine;
using UnityEngine.Events;

// 아이템 타입
public enum ItemType
{
    Equip,
    Consume,
}

// 소비할 때 어떤 것을 채워주는지
public enum ConsumableType
{
    Health,
}

[Serializable] // 인스펙터에 출현할 수 있게끔
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value; // 얼마나 채워주는지 값
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

    [Header("Stacking")] public bool canStack; // 아이템 수를 쌓을 수 있는지
    public int maxStack; // 최대로 쌓을 수 있는 아이템 갯수
    
    [Header("Consumable")] // 소비 시 효과들
    public ItemDataConsumable[] consumables;
    public UnityEvent UseEffect;
    
    [Header("Equipment")] // 장비 아이템 프리팹
    public GameObject equipmentPrefab;
}
