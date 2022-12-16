using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
	[SerializeField] public float speed = 2.0f;
	[SerializeField] public int health = 20;
	[SerializeField] public int attack_strength = 20;
	[SerializeField] public float attack_cooldown = 2.0f;
	[SerializeField] public float attack_range = 1.0f;
	[SerializeField] public float knockback = 500000.0f;
	public int current_health;
	public float current_attack_cooldown;

	public Rigidbody rb;
    private Animator animator_controller;
    public GameObject player;
	public Transform player_position;
	public float direction;
	public bool isAttacking = false;

	[SerializeField] public BoxCollider attack_area;
	[SerializeField] public Slider health_bar;


	public virtual void initialization() 
	{

		rb = GetComponent<Rigidbody>();
		animator_controller = GetComponent<Animator>();
		attack_area.transform.localScale = new Vector3(1.0f, 1.0f, attack_range);
		attack_area.transform.localPosition = new Vector3(0.0f, 0.0f, 0.5f * (attack_range + 1.0f));
		player_position = FindObjectOfType<Player>().GetComponent<Transform>();
		direction = 1.0f;
		current_health = health;
		current_attack_cooldown = 0.0f;
		update_health_bar();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public virtual void movement() 
	{
        if (!animator_controller.GetBool("isMoving"))
            animator_controller.SetBool("isMoving", true);

        if (player.activeSelf) 
		{
			Vector3 movement_vector = player_position.position - transform.position;
			Vector3 movement_vector_normalized = new Vector3(movement_vector.x / Mathf.Abs(movement_vector.x), 0.0f, movement_vector.z / Mathf.Abs(movement_vector.z));
			if (float.IsNaN(movement_vector_normalized.x))
				movement_vector_normalized.x = 0.0f;
			if (float.IsNaN(movement_vector_normalized.z))
				movement_vector_normalized.z = 0.0f;
			if (movement_vector_normalized.z * direction < 0.0f)
			{
				Vector3 rotation = new Vector3(0.0f, 180.0f, 0.0f);
				transform.Rotate(rotation);
				health_bar.transform.Rotate(rotation);
				direction = movement_vector_normalized.z;
			}
			rb.velocity = transform.rotation * new Vector3(movement_vector_normalized.x, 0.0f, movement_vector_normalized.z) * direction * speed;
		}

	}

	public void attack_check() 
	{
		if (is_attack_on_cooldown() == true)
		{
			if (current_attack_cooldown <= Time.deltaTime)
				current_attack_cooldown = 0.0f;
			else
				current_attack_cooldown -= Time.deltaTime;
		}
		if (can_attack_player() == true && !isAttacking) 
		{
			isAttacking = true;
            animator_controller.SetTrigger("TrAttack");
            StartCoroutine(Delay(0.3f));
			
        }

	}

	public void receive_damage(int amount)
	{
		current_health -= amount;
		//apply_knockback();
		if (current_health <= 0)
		{
			current_health = 0;
			update_health_bar();
			die();
		}
		else
			update_health_bar();
	}

	public void attack()
	{
		current_attack_cooldown = attack_cooldown;

		Collider[] attack_area_colliders = Physics.OverlapBox(attack_area.transform.position, attack_area.transform.lossyScale / 2.0f, Quaternion.identity);
		foreach (Collider collider in attack_area_colliders)
		{
			if (collider.gameObject.tag == "Player")
				collider.gameObject.GetComponent<Player>().receive_damage(attack_strength);
		}
	}

	public bool can_attack_player()
	{
		if (is_attack_on_cooldown() == true)
			return false;

		Collider[] attack_area_colliders = Physics.OverlapBox(attack_area.transform.position, attack_area.transform.lossyScale / 2.0f, Quaternion.identity);
		foreach (Collider collider in attack_area_colliders)
		{
			if (collider.gameObject.tag == "Player")
				return true;
		}
		return false;
	}

	public void die()
	{
		Destroy(gameObject);
	}

	public float get_health_percentage()
	{
		return (float)(current_health) / (float)(health);
	}

	public bool is_attack_on_cooldown()
	{
		return current_attack_cooldown > 0.0f;
	}

	public void update_health_bar()
	{
		health_bar.value = get_health_percentage();
	}

	public virtual void apply_knockback() 
	{
		if (player.transform.rotation.y == 0.0f)
			rb.AddForce(new Vector3(0, 0, knockback));
		else
			rb.AddForce(new Vector3(0, 0, -knockback));
	}

	IEnumerator Delay(float time) 
	{
		yield return new WaitForSeconds(time);
        attack();
        isAttacking = false;
    }

}
