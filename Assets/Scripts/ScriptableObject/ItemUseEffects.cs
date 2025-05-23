using UnityEngine;

[CreateAssetMenu(fileName = "ItemUseEffects", menuName = "New ItemEffects")]
public class ItemUseEffects : ScriptableObject
{
    public void Carrot_UseEffect()
    {
        // 이동 속도 2배
        float speed = 2 * GameManager.Instance.Player.control.GetOriginSpeed;
        // 지속 시간
        float time = 10f;
        
        GameManager.Instance.Player.control.Call_Change_MoveSpeed(speed, time);
    }
}
