using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_Tools : MonoBehaviour
{
	[SerializeField] private GameObject enemy_prefab;
	[SerializeField] private GameObject boss_prefab;
	[SerializeField] private GameObject powerup_health_prefab;
	[SerializeField] private Transform enemy_spawn;
	[SerializeField] private Transform boss_spawn;
	[SerializeField] private Transform powerup_health_spawn;
	
	public void quit_game()
	{
		Application.Quit();
	}
	
	public void restart_level()
	{
		SceneManager.LoadScene(0);
	}
	
	public void spawn_enemy()
	{
		GameObject enemy = Instantiate(enemy_prefab, enemy_spawn.position + new Vector3(Random.Range(-4.0f, 4.0f), 0.0f, Random.Range(-5.0f, 20.0f)), Quaternion.identity);
	}
	
	public void spawn_enemy_boss()
	{
		GameObject boss = Instantiate(boss_prefab, boss_spawn.position + new Vector3(Random.Range(-2.0f, 2.0f), 0.0f, Random.Range(-3.0f, 3.0f)), Quaternion.identity);
	}
	
	public void spawn_powerup_health()
	{
		GameObject health = Instantiate(powerup_health_prefab, powerup_health_spawn.position + new Vector3(Random.Range(-4.0f, 4.0f), 0.0f, Random.Range(-40.0f, 40.0f)), Quaternion.identity);
	}
}
