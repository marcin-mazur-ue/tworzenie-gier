using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
	[SerializeField] private Text health_text;
	[SerializeField] private Image current_weapon_panel;
	
	[SerializeField] private ParticleSystem particle_system_damage;
	[SerializeField] private ParticleSystem particle_system_heal;
	
	[SerializeField] private Level_Manager level_manager;
	
	protected override void initialize()
	{
		base.initialize();
		animator_controller.SetBool("isMoving", false);
	}
	
	protected override void fixed_update()
	{
		base.fixed_update();
		animator_controller.SetBool("isMoving", Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f);
		move(-Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
	}
	
	protected override void update()
	{
		base.update();
		if (is_attacking == true)
			current_weapon_panel.color = new Color(0.9f, 0.9f, 0.9f, 0.4f);
		else
			current_weapon_panel.color = new Color(0.7f, 0.7f, 0.7f, 0.4f);
		
		if (Input.GetKeyDown(KeyCode.LeftControl) && can_attack() == true)
			attack();
	}
	
	public override void heal(int amount)
	{
		particle_system_heal.Play();
		base.heal(amount);
	}
	
	public override void receive_damage(int amount)
	{
		particle_system_damage.Play();
		level_manager.player_received_damage(amount);
		base.receive_damage(amount);
	}
	
	protected override void die()
	{
		base.die();
		level_manager.player_died();
	}
	
	protected override void update_attack_cooldown()
	{
		base.update_attack_cooldown();
		current_weapon_panel.fillAmount = 1.0f - ((float)(current_attack_cooldown) / (float)(attack_cooldown));
	}
	
	protected override void update_health()
	{
		base.update_health();
		health_text.text = current_health.ToString().PadLeft(3, ' ') + "/" + max_health.ToString().PadLeft(3, ' ');
	}
	
	protected override bool can_attack()
	{
		return is_attacking == false && is_attack_on_cooldown() == false;
	}
}
