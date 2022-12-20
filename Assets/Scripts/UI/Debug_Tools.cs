using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_Tools : MonoBehaviour
{
	[SerializeField] private GameObject show_debug_tools_button;
	[SerializeField] private GameObject debug_buttons_group;
	[SerializeField] private GameObject enemy_prefab;
	[SerializeField] private GameObject powerup_health_prefab;
	[SerializeField] private Transform enemy_spawn;
	[SerializeField] private Transform powerup_health_spawn;
	
	public void show_debug_tools()
	{
		debug_buttons_group.SetActive(true);
		show_debug_tools_button.SetActive(false);
	}
	
	public void hide_debug_tools()
	{
		show_debug_tools_button.SetActive(true);
		debug_buttons_group.SetActive(false);
	}
	
	public void quit_to_desktop()
	{
		Application.Quit();
	}
	
	public void quit_to_menu_menu()
	{
		SceneManager.LoadScene(0);
	}
	
	public void restart_level()
	{
		SceneManager.LoadScene(2);
		Time.timeScale = 1.0f;
	}
	
	public void spawn_enemy()
	{
		Instantiate(enemy_prefab, enemy_spawn.position + new Vector3(Random.Range(-4.0f, 4.0f), 0.0f, Random.Range(-5.0f, 20.0f)), Quaternion.identity);
	}
	
	public void spawn_powerup_health()
	{
		Instantiate(powerup_health_prefab, powerup_health_spawn.position + new Vector3(Random.Range(-4.0f, 4.0f), 0.0f, Random.Range(-40.0f, 40.0f)), Quaternion.identity);
	}
}
