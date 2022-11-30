using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float speed = 2.0f;
	[SerializeField] private int health = 20;
	[SerializeField] private int attack_strength = 20;
	[SerializeField] private float attack_cooldown = 2.0f;
	[SerializeField] private float attack_range = 1.0f;
	private int current_health;
	private float current_attack_cooldown;

	private Rigidbody rb;
	private Transform player_position;
	private float direction;

	[SerializeField] private BoxCollider attack_area;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		attack_area.transform.localScale = new Vector3(1.0f, 1.0f, attack_range);
		attack_area.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f * (attack_range + 1.0f));
		player_position = FindObjectOfType<Player>().GetComponent<Transform>();
		direction = 1.0f;
		current_health = health;
		current_attack_cooldown = 0.0f;
	}

	void FixedUpdate()
	{
		Vector3 movement_vector = player_position.position - transform.position;
		Vector3 movement_vector_normalized = new Vector3(movement_vector.x / Mathf.Abs(movement_vector.x), 0.0f, movement_vector.z / Mathf.Abs(movement_vector.z));
		if(float.IsNaN(movement_vector_normalized.x))
			movement_vector_normalized.x = 0.0f;
		if(float.IsNaN(movement_vector_normalized.z))
			movement_vector_normalized.z = 0.0f;
		if(movement_vector_normalized.z * direction < 0.0f)
		{
			transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
			direction = movement_vector_normalized.z;
		}
		rb.velocity = transform.rotation * new Vector3(movement_vector_normalized.x, 0.0f, movement_vector_normalized.z) * direction * speed;
	}

	void Update()
	{
		if(is_attack_on_cooldown() == true)
		{
			if(current_attack_cooldown <= Time.deltaTime)
				current_attack_cooldown = 0.0f;
			else
				current_attack_cooldown -= Time.deltaTime;
		}
		if(can_attack_player() == true)
			attack();
	}

	public void receive_damage(int amount)
	{
		current_health -= amount;
		if(current_health <= 0)
			die();
	}

	private void attack()
	{
		current_attack_cooldown = attack_cooldown;

		Collider[] attack_area_colliders = Physics.OverlapBox(attack_area.transform.position, attack_area.transform.localScale / 2.0f, Quaternion.identity);
		foreach(Collider collider in attack_area_colliders)
		{
			if(collider.gameObject.tag == "Player")
				collider.gameObject.GetComponent<Player>().receive_damage(attack_strength);
		}
	}

	private bool can_attack_player()
	{
		if(is_attack_on_cooldown() == true)
			return false;

		Collider[] attack_area_colliders = Physics.OverlapBox(attack_area.transform.position, attack_area.transform.localScale / 2.0f, Quaternion.identity);
		foreach(Collider collider in attack_area_colliders)
		{
			if(collider.gameObject.tag == "Player")
				return true;
		}
		return false;
	}

	private void die()
	{
		Destroy(gameObject);
	}

	private bool is_attack_on_cooldown()
	{
		return current_attack_cooldown > 0.0f;
	}
}
