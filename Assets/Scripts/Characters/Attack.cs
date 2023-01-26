using UnityEngine;

public class Attack
{
	[SerializeField] private int damage_dealt;
	[SerializeField] private float windup_time;
	[SerializeField] private float cooldown;
	[SerializeField] private float range;
	[SerializeField] private int knockback_strength;
	
	private float current_cooldown = 0.0f;
	
	public Attack(int _damage_dealt, float _windup_time, float _cooldown, float _range, int _knockback_strength)
	{
		damage_dealt = _damage_dealt;
		windup_time = _windup_time;
		cooldown = _cooldown;
		range = _range;
		knockback_strength = _knockback_strength;
	}
	
	public int get_damage_dealt()
	{
		return damage_dealt;
	}
	
	public float get_windup_time()
	{
		return windup_time;
	}
	
	public float get_cooldown()
	{
		return cooldown;
	}
	
	public float get_range()
	{
		return range;
	}
	
	public int get_knockback_strength()
	{
		return knockback_strength;
	}
	
	public float get_current_cooldown_percentage()
	{
		return current_cooldown / cooldown;
	}
	
	public bool is_on_cooldown()
	{
		return current_cooldown > 0.0f && current_cooldown <= cooldown;
	}
	
	public bool is_winding_up()
	{
		return current_cooldown > cooldown;
	}
	
	public void start()
	{
		current_cooldown = windup_time + cooldown;
	}
	
	public void update_cooldown()
	{
		if (current_cooldown <= Time.deltaTime)
			current_cooldown = 0.0f;
		else
			current_cooldown -= Time.deltaTime;
	}
}
