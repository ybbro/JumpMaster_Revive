using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public PlayerStatus status;
    public PlayerControl control;
    
    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        TryGetComponent(out status);
        TryGetComponent(out control);
    }
}