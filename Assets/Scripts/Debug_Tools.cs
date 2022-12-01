using UnityEngine;
using UnityEngine.SceneManagement;

public class Debug_Tools : MonoBehaviour
{
	[SerializeField] private GameObject enemy_prefab;
	[SerializeField] private Transform enemy_spawn;
	
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
		GameObject enemy = Instantiate(enemy_prefab, enemy_spawn.position + new Vector3(Random.Range(-3.0f, 3.0f), 0.0f, Random.Range(-3.0f, 3.0f)), Quaternion.identity);
	}
}
