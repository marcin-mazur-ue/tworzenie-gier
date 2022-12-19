using UnityEngine;

public class Enemy_Basic : Character
{
	private Transform player_position;
	
	protected override void initialize()
	{
		base.initialize();
		player_position = FindObjectOfType<Player>().GetComponent<Transform>();
	}
	
	protected override void fixed_update()
	{
		base.fixed_update();
		if (!animator_controller.GetBool("isMoving"))
			animator_controller.SetBool("isMoving", true);

		Vector3 movement_vector = player_position.position - transform.position;
		move(normalize(movement_vector.x), normalize(movement_vector.z));
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
	
	protected override void die()
	{
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
