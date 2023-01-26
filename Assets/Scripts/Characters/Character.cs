using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
	[SerializeField] protected float speed = 2.0f;
	[SerializeField] protected int max_health = 20;
	[SerializeField] protected Attack[] attacks;
	protected int current_health;
	protected Attack current_attack;
	
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
		add_attacks();
		current_health = max_health;
		current_attack = null;
		rb = GetComponent<Rigidbody>();
		animator_controller = GetComponent<Animator>();
		facing_direction = 1.0f;
		update_health();
	}
	
	protected virtual void add_attacks()
	{
		attacks = new Attack[0];
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
	
	public virtual void suffer_knockback(int force, Vector3 direction)
	{
		rb.AddForce(direction * force);
	}
	
	protected virtual void attack(int attack_index)
	{
		current_attack = attacks[attack_index];
		StartCoroutine(delayed_attack());
	}
	
	protected IEnumerator delayed_attack() 
	{
		current_attack.start();
		attack_area.gameObject.SetActive(true);
		update_attack_area(current_attack);
		yield return new WaitForSeconds(current_attack.get_windup_time());
		List<GameObject> targets = get_valid_attack_targets(current_attack);
		foreach (GameObject target in targets)
		{
			target.GetComponent<Character>().receive_damage(current_attack.get_damage_dealt());
			target.GetComponent<Character>().suffer_knockback(current_attack.get_knockback_strength(), transform.forward);
		}
		attack_area.gameObject.SetActive(false);
		yield return new WaitForSeconds(current_attack.get_cooldown());
		current_attack = null;
	}
	
	protected virtual void die()
	{
		gameObject.SetActive(false);
	}
	
	protected virtual void flip()
	{
		transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
	}
	
	protected virtual void move(Vector3 movement_vector)
	{
		update_direction(movement_vector.z);
		rb.velocity = transform.rotation * movement_vector * facing_direction * speed;
	}
	
	protected void update_attack_area(Attack attack)
	{	
		BoxCollider collider = gameObject.GetComponent<BoxCollider>();
		attack_area.transform.localScale = new Vector3(attack_area.transform.localScale.x, attack_area.transform.localScale.y, attack.get_range() * collider.size.z);
		attack_area.transform.localPosition = new Vector3(0.0f, 0.0f, (collider.size.z + attack_area.transform.localScale.z) / 2.0f);
	}
	
	protected virtual void update_attack_cooldown()
	{
		if (current_attack != null)
			current_attack.update_cooldown();
	}
	
	protected virtual void update_direction(float new_direction)
	{
		if (new_direction * facing_direction < 0.0f)
		{
			flip();
			facing_direction = new_direction / Mathf.Abs(new_direction);
		}
	}
	
	protected virtual void update_health()
	{
		health_bar.value = get_health_percentage();
	}
	
	protected virtual bool can_attack()
	{
		if (is_attack_on_cooldown() == true) 
			return false;
		foreach (Attack attack in attacks)
		{
			if (get_valid_attack_targets(attack).Count > 0)
				return true;
		}
		return false;
	}
	
	protected virtual bool can_move()
	{
		return current_attack == null || current_attack.is_on_cooldown() == true; // podczas ataku nie można się ruszać, ale można się obracać
	}
	
	protected virtual bool is_attack_on_cooldown()
	{
		return current_attack != null;
	}
	
	protected float get_health_percentage()
	{
		return (float)(current_health) / (float)(max_health);
	}
	
	protected List<GameObject> get_valid_attack_targets(Attack attack)
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
