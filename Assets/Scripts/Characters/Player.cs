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
			new Attack(20, 0.6f, 0.6f, 2.0f, 30000),	// Podstawowy	- Szybki
			new Attack(30, 0.9f, 0.8f, 3.0f, 60000),	// Podstawowy	- Silny
			new Attack(20, 0.4f, 0.6f, 2.0f, 25000),	// Combo	- Szybki + Szybki
			new Attack(30, 0.5f, 0.8f, 4.0f, 80000),	// Combo	- Szybki + Silny
			new Attack(60, 1.1f, 0.8f, 4.0f, 100000)	// Combo	- Silny + Silny
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
		if (can_attack() == false)
			return;
		
		if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Z)) // Szybki
		{
			if (current_attack == null)
			{
				animator_controller.SetTrigger("Trigger_Attack_Basic_Fast");
				attack(0);
			}
			else if (current_attack == attacks[0])
			{
				animator_controller.SetTrigger("Trigger_Attack_Combo_Fast_Fast");
				attack(2);
			}
		}
		else if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.X)) // Silny
		{
			if (current_attack == null)
			{
				animator_controller.SetTrigger("Trigger_Attack_Basic_Strong");
				attack(1);
			}
			else if (current_attack == attacks[0])
			{
				animator_controller.SetTrigger("Trigger_Attack_Combo_Fast_Strong");
				attack(3);
			}
			else if (current_attack == attacks[1])
			{
				animator_controller.SetTrigger("Trigger_Attack_Combo_Strong_Strong");
				attack(4);
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
		return current_attack == null || (is_attack_on_cooldown() == true);
	}
}
