using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public PlayerStatus status;
    public PlayerControl control;
    
    public ItemData itemData;
    public Action addItem;
    
    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        TryGetComponent(out status);
        TryGetComponent(out control);
    }
}