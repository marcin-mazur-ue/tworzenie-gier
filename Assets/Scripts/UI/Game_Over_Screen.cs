using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Over_Screen : MonoBehaviour
{
	[SerializeField] private int current_level_scene_id;
	
	public void return_to_level_select()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(1); //placeholder, w tej chwili to jeszcze nie jest mapa
	}
	
	public void return_to_main_menu()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(0);
	}
	
	public void restart_level()
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(current_level_scene_id);
	}
}
