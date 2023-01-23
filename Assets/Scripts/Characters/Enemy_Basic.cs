using UnityEngine;
using UnityEngine.AI;

public class Enemy_Basic : Character
{
	[SerializeField] protected float min_speed_percentage = 0.6f;
	
	private Transform player_position;
	private NavMeshAgent agent;
	
	protected override void initialize()
	{
		base.initialize();
		player_position = FindObjectOfType<Player>().GetComponent<Transform>();
		agent = GetComponent<NavMeshAgent>();
		speed *= Random.Range(min_speed_percentage, 1.0f);
		agent.speed = speed;
	}
	
	protected override void fixed_update()
	{
		base.fixed_update();
		if (!animator_controller.GetBool("isMoving"))
			animator_controller.SetBool("isMoving", true);
		agent.destination = player_position.position;
		Vector3 movement_vector = player_position.position - transform.position;
		update_direction(normalize(movement_vector.z));
	}
	
	protected override void update()
	{
		base.update();
		if (can_attack() == true)
		{
			animator_controller.SetTrigger("TrAttack");
			attack();
		}
	}
	protected override void flip()
	{
		base.flip();
		health_bar.transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
	}

	protected override void die()
	{
		Game_Data_Manager.increase_achievement_progress(5, 1);
		Destroy(gameObject);
	}
	
	private float normalize(float value) //zwraca -1 dla ujemnych wartości, 1 dla dodatnich, lub niezmienione 0
	{
		value /= Mathf.Abs(value);
		if (float.IsNaN(value))
			return 0.0f;
		else
			return value;
	}
}
