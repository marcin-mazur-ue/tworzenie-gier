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
	
	protected override void add_attacks()
	{
		attacks = new Attack[]
		{
			new Attack(20, 0.4f, 1.0f, 2.5f, 40000),
			new Attack(30, 0.55f, 1.5f, 3.0f, 70000)
		};
	}
	
	protected override void fixed_update()
	{
		base.fixed_update();
		if (can_move() == true)
		{
			animator_controller.SetBool("isMoving", Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f);
			move(new Vector3(-Input.GetAxis("Vertical"), 0.0f, Input.GetAxis("Horizontal")));
		}
	}
	
	protected override void update()
	{
		base.update();
		if (can_attack() == true)
		{
			if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Z)) 
			{
				animator_controller.SetTrigger("Trigger_Attack_Fast");
				attack(0);
			}
			else if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.X)) 
			{
				animator_controller.SetTrigger("Trigger_Attack_Strong");
				attack(1);
			}
		}
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
		if (current_attack != null)
		{
			current_weapon_panel.color = new Color(0.9f, 0.9f, 0.9f, 0.4f);
			current_weapon_panel.fillAmount = 1.0f - current_attack.get_current_cooldown_percentage();
		}
		else
			current_weapon_panel.color = new Color(0.7f, 0.7f, 0.7f, 0.4f);
	}
	
	protected override void update_health()
	{
		base.update_health();
		health_text.text = current_health.ToString().PadLeft(3, ' ') + "/" + max_health.ToString().PadLeft(3, ' ');
	}
	
	protected override bool can_attack()
	{
		return is_attack_on_cooldown() == false;
	}
}
