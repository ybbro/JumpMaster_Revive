using UnityEngine;

[CreateAssetMenu(fileName = "ItemUseEffects", menuName = "New ItemEffects")]
public class ItemUseEffects : ScriptableObject
{
    public void Carrot_UseEffect()
    {
        // �̵� �ӵ� 2��
        float speed = 2 * GameManager.Instance.Player.control.GetOriginSpeed;
        // ���� �ð�
        float time = 10f;
        
        GameManager.Instance.Player.control.Call_Change_MoveSpeed(speed, time);
    }
}
