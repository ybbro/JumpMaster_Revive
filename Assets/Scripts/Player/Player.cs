using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public PlayerStatus status;
    
    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        TryGetComponent(out status);
    }
}