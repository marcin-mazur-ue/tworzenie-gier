using UnityEngine;
using UnityEngine.AI;

public class Enemy_Basic : Character
{
	[SerializeField] protected float min_speed_percentage = 0.6f;
	[SerializeField] protected float max_speed_percentage = 1.4f;
	
	private Transform player_position;
	private NavMeshAgent agent;
	private bool knocked_back = false;
	
	protected override void initialize()
	{
		base.initialize();
		update_attack_area(attacks[0]);
		player_position = FindObjectOfType<Player>().GetComponent<Transform>();
		agent = GetComponent<NavMeshAgent>();
		speed *= Random.Range(min_speed_percentage, max_speed_percentage);
		agent.speed = speed;
	}
	
	protected override void add_attacks()
	{
		attacks = new Attack[]
		{
			new Attack(20, 0.35f, 1.5f, 1.0f, 40)
		};
	}
	
	protected override void fixed_update()
	{
		base.fixed_update();
		if (knocked_back == true)
			knocked_back = false;
		else if (rb.velocity.magnitude < 0.1f)
		{
			rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			rb.isKinematic = true;
			agent.enabled = true;
		}
		
		if (can_move() == true)
		{
			if (agent.enabled == true)
				agent.isStopped = false;
			if (animator_controller.GetBool("isMoving") == false)
				animator_controller.SetBool("isMoving", true);
			move(new Vector3(player_position.position.x, transform.position.y, player_position.position.z));
		}
		else
		{
			if (agent.enabled == true)
				agent.isStopped = true;
			animator_controller.SetBool("isMoving", false);
			move(transform.position);
		}
	}
	
	protected override void update()
	{
		base.update();
		if (can_attack() == true)
		{
			animator_controller.SetTrigger("TrAttack");
			attack(0);
		}
	}
	
	public override void suffer_knockback(int force, Vector3 direction)
	{
		rb.isKinematic = false;
		agent.enabled = false;
		knocked_back = true;
		base.suffer_knockback(force, direction);
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
	
	protected override void move(Vector3 movement_vector)
	{
		update_direction(movement_vector.z - transform.position.z);
		if (agent.enabled == true && movement_vector.Equals(transform.position) == false)
			agent.destination = movement_vector;
	}
}
