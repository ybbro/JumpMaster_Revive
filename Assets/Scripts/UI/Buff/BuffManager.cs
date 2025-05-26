using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    moveSpeed,
}

public class BuffManager : MonoBehaviour
{
    private Buff[] buffs;

    private void Start()
    {
        buffs = transform.GetComponentsInChildren<Buff>();
    }
}
