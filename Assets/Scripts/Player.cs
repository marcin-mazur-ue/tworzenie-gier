using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	[SerializeField] private float speed = 7.0f;
	[SerializeField] private int health = 100;
	[SerializeField] private int attack_strength = 20;
	[SerializeField] private float attack_cooldown = 2.0f;
	[SerializeField] private float attack_range = 2.5f;
	private int current_health;
	private float current_attack_cooldown;

	private Rigidbody rb;
	private float direction;

	[SerializeField] private Transform camera_position;
	[SerializeField] private BoxCollider attack_area;
	[SerializeField] private Text health_text;
	[SerializeField] private Text attack_cooldown_text;
	[SerializeField] private Slider health_bar;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		attack_area.transform.localScale = new Vector3(1.0f, 1.0f, attack_range);
		attack_area.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f * (attack_range + 1.0f));
		direction = 1.0f;
		current_health = health;
		current_attack_cooldown = 0.0f;
		update_health_text(current_health);
		update_health_bar();
	}

	void FixedUpdate()
	{
		float new_direction = Input.GetAxis("Horizontal");
		if(new_direction * direction < 0.0f)
		{
			Vector3 rotation = new Vector3(0.0f, 180.0f, 0.0f);
			transform.Rotate(rotation);
			health_bar.transform.Rotate(rotation);
			direction = (new_direction / Mathf.Abs(new_direction));
		}
		rb.velocity = transform.rotation * new Vector3(-Input.GetAxis("Vertical"), 0.0f, Input.GetAxis("Horizontal")) * direction * speed;
	}

	void Update()
	{
		camera_position.position = new Vector3(camera_position.position.x, camera_position.position.y, transform.position.z);
		if(is_attack_on_cooldown() == true)
		{
			if(current_attack_cooldown <= Time.deltaTime)
				current_attack_cooldown = 0.0f;
			else
				current_attack_cooldown -= Time.deltaTime;
		}
		update_attack_cooldown_text(current_attack_cooldown);
		if(Input.GetKeyDown(KeyCode.Space) && is_attack_on_cooldown() == false)
			attack();
	}

	public void receive_damage(int amount)
	{
		current_health -= amount;
		update_health_text(current_health);
		update_health_bar();
		if(current_health <= 0)
		{
			update_health_text(0);
			die();
		}
	}

	private void attack()
	{
		current_attack_cooldown = attack_cooldown;

		Collider[] attack_area_colliders = Physics.OverlapBox(attack_area.transform.position, attack_area.transform.lossyScale / 2.0f, Quaternion.identity);
		foreach(Collider collider in attack_area_colliders)
		{
			if(collider.gameObject.tag == "Enemy")
				collider.gameObject.GetComponent<Enemy>().receive_damage(attack_strength);
		}
	}

	private void die()
	{
		gameObject.SetActive(false);
	}

	private bool is_attack_on_cooldown()
	{
		return current_attack_cooldown > 0.0f;
	}
	
	private float get_health_percentage()
	{
		return (float)(current_health) / (float)(health);
	}

	private void update_attack_cooldown_text(float cooldown)
	{
		if(is_attack_on_cooldown() == false)
			attack_cooldown_text.text = "";
		else
			attack_cooldown_text.text = "CD: " + (cooldown.ToString("F2").PadLeft(4, ' ')) + "s";
	}
	
	private void update_health_bar()
	{
		health_bar.value = get_health_percentage();
	}

	private void update_health_text(int amount)
	{
		health_text.text = "HP: " + (amount.ToString().PadLeft(3, ' '));
	}
}
