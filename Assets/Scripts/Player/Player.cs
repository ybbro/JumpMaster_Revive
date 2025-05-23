using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public PlayerStatus status;
    public PlayerControl control;
    
    public Action<InventorySlot> addItem;
    
    private void Awake()
    {
        TryGetComponent(out status);
        TryGetComponent(out control);
    }
}