using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
	[SerializeField] private Transform camera_position;
	[SerializeField] private Text health_text;
	[SerializeField] private Text attack_cooldown_text;
	
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
		camera_position.position = new Vector3(camera_position.position.x, camera_position.position.y, transform.position.z);
		if (Input.GetKeyDown(KeyCode.LeftControl) && can_attack() == true)
			attack();
	}
	
	protected override void die()
	{
		base.die();
		Time.timeScale = 0.0f;
	}
	
	protected override void update_health()
	{
		base.update_health();
		health_text.text = "HP: " + (current_health.ToString().PadLeft(3, ' '));
	}
	
	protected override void update_attack_cooldown()
	{
		base.update_attack_cooldown();
		if (is_attack_on_cooldown() == false)
			attack_cooldown_text.text = "";
		else
			attack_cooldown_text.text = "CD: " + (current_attack_cooldown.ToString("F2").PadLeft(4, ' ')) + "s";
	}
}
