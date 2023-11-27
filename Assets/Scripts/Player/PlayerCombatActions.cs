using UnityEngine;
using System;

public class PlayerCombatActions : MonoBehaviour
{
    public static event Action<bool> PlayerDidAction;
    public static event Action<GameObject> SetPlayerTarget;
    public static event Action RemovePlayerTarget;
    public GameObject PlayerTargetObject = null;
    
    private readonly string enemyTag = "Enemy";
    private Character playerStats;

    private void Awake()
    {
        playerStats = GetComponent<Character>();
    }

    private void OnEnable()
    {
        ActionButton.PlayerAttack += PlayerCombatAction;
    }

    private void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if (hit)
			{
				Debug.Log("Hit " + hitInfo.transform.gameObject.name);
				if (hitInfo.transform.gameObject.tag == enemyTag)
				{
                    PlayerTargetObject = hitInfo.transform.gameObject;
                    SetPlayerTarget?.Invoke(PlayerTargetObject);
                }
			}
		}
        if(Input.GetMouseButtonDown(1))
        {
            PlayerTargetObject = null;
            RemovePlayerTarget?.Invoke();
        }
	}

    private void PlayerCombatAction()
    {
        Debug.Log("Attacking");
        Character target = PlayerTargetObject.GetComponent<Character>();
        if(target is IDamageable)
        {
            target.Damage(playerStats.Attack);
        }
        PlayerDidAction?.Invoke(true);
    }
    


    private void OnDisable()
    {
        ActionButton.PlayerAttack += PlayerCombatAction;
    }
}
