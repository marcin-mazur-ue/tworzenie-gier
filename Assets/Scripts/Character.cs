using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
	[SerializeField] protected float speed = 2.0f;
	[SerializeField] protected int max_health = 20;
	[SerializeField] protected int attack_strength = 20;
	[SerializeField] protected float attack_cooldown = 2.0f;
	[SerializeField] protected float attack_delay_length = 2.0f;
	[SerializeField] protected float attack_range = 1.0f;
	[SerializeField] protected float knockback_strength = 500000.0f;
	protected int current_health;
	protected float current_attack_cooldown;
	protected bool is_attacking;
	
	protected Rigidbody rb;
	protected Animator animator_controller;
	protected float facing_direction;
	
	[SerializeField] protected List<string> valid_attack_target_tags = new List<string>();
	[SerializeField] protected BoxCollider attack_area;
	[SerializeField] protected Slider health_bar;
	
	void Start()
	{
		initialize();
	}
	
	void FixedUpdate()
	{
		fixed_update();
	}

	void Update()
	{
		update();
	}
	
	protected virtual void initialize()
	{
		current_health = max_health;
		current_attack_cooldown = 0.0f;
		is_attacking = false;
		rb = GetComponent<Rigidbody>();
		animator_controller = GetComponent<Animator>();
		facing_direction = 1.0f;
		update_attack_range();
		update_health();
	}
	
	protected virtual void fixed_update()
	{
		
	}
	
	protected virtual void update()
	{
		update_attack_cooldown();
	}
	
	public virtual void heal(int amount)
	{
		if (current_health + amount >= max_health)
			current_health = max_health;
		else
			current_health += amount;
		update_health();
	}
	
	public virtual void receive_damage(int amount)
	{
		current_health -= amount;
		if (current_health <= 0)
		{
			current_health = 0;
			update_health();
			die();
		}
		else
			update_health();
	}
	
	protected virtual void attack()
	{
		is_attacking = true;
		StartCoroutine(delayed_attack());
	}
	
	protected IEnumerator delayed_attack() 
	{
		yield return new WaitForSeconds(attack_delay_length);
		List<GameObject> targets = get_valid_attack_targets();
		foreach (GameObject target in targets)
			target.GetComponent<Character>().receive_damage(attack_strength);
		is_attacking = false;
		current_attack_cooldown = attack_cooldown;
	}
	
	protected virtual void die()
	{
		gameObject.SetActive(false);
	}
	
	protected virtual void move(float x, float z)
	{
		rb.velocity = transform.rotation * new Vector3(x, 0.0f, z) * facing_direction * speed;
		update_direction(z);
	}
	
	protected virtual void update_attack_cooldown()
	{
		if (is_attack_on_cooldown() == true)
		{
			if (current_attack_cooldown <= Time.deltaTime)
				current_attack_cooldown = 0.0f;
			else
				current_attack_cooldown -= Time.deltaTime;
		}
	}
	
	protected void update_attack_range()
	{
		BoxCollider collider = gameObject.GetComponent<BoxCollider>();
		attack_area.transform.localScale = new Vector3(attack_area.transform.localScale.x, attack_area.transform.localScale.y, attack_range * collider.size.z);
		attack_area.transform.localPosition = new Vector3(0.0f, 0.0f, (collider.size.z + attack_area.transform.localScale.z) / 2.0f);
	}
	
	protected virtual void update_direction(float new_direction)
	{
		if (new_direction * facing_direction < 0.0f)
		{
			Vector3 rotation = new Vector3(0.0f, 180.0f, 0.0f);
			transform.Rotate(rotation);
			health_bar.transform.Rotate(rotation);
			facing_direction = new_direction / Mathf.Abs(new_direction);
		}
	}
	
	protected virtual void update_health()
	{
		health_bar.value = get_health_percentage();
	}
	
	protected virtual bool can_attack()
	{
		return is_attacking == false && is_attack_on_cooldown() == false && get_valid_attack_targets().Count > 0;
	}
	
	protected virtual bool is_attack_on_cooldown()
	{
		return current_attack_cooldown > 0.0f;
	}
	
	protected float get_health_percentage()
	{
		return (float)(current_health) / (float)(max_health);
	}
	
	protected List<GameObject> get_valid_attack_targets()
	{
		List<GameObject> targets = new List<GameObject>();
		Collider[] attack_area_colliders = Physics.OverlapBox(attack_area.transform.position, attack_area.transform.lossyScale / 2.0f, Quaternion.identity);
		foreach (Collider collider in attack_area_colliders)
		{
			foreach (string target_tag in valid_attack_target_tags)
			{
				if(collider.gameObject.tag == target_tag)
					targets.Add(collider.gameObject);
			}
		}
		return targets;
	}
}
