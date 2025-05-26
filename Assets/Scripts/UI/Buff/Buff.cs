using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Buff : MonoBehaviour
{
    public UnityEvent BuffEffect;

    private float changedValue, time_remain, time_init;

    void OnEnable()
    {
        BuffEffect?.Invoke();
    }
}
