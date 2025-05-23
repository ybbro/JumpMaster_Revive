using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player Player { get; private set; }
    public Inventory Inventory { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance == this)
            Destroy(gameObject);
        
        Player = FindObjectOfType<Player>();
        Inventory = FindObjectOfType<Inventory>();
    }
}