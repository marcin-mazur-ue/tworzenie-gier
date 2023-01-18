using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Manager : MonoBehaviour
{
	public static Level_Manager instance;
	
	[SerializeField] private int level_index;
	[SerializeField] private float time_to_gain_star;
	[SerializeField] private int max_damage_to_gain_star;
	
	[SerializeField] private Text level_complete_text;
	[SerializeField] private Text time_elapsed_text;
	[SerializeField] private Text damage_taken_text;
	[SerializeField] private GameObject[] star_gained_image = new GameObject[Game_Data.stars_per_level];
	[SerializeField] private Text[] star_description_text = new Text[Game_Data.stars_per_level];
	
	[SerializeField] private GameObject level_finished_screen;
	[SerializeField] private GameObject pause_screen;
	
	[SerializeField] private int current_level_scene_id;
	private const int main_menu_scene_id = 0;
	private const int level_select_scene_id = 3;
	
	private int player_damage_taken = 0;
	private float time_elapsed = 0.0f;
	
	void Awake()
	{
		if (instance != null)
			Destroy(gameObject);
		else
			instance = this;
		
		star_description_text[0].text = "Ukończ poziom";
		star_description_text[1].text = "Ukończ poziom w czasie poniżej " + ((int)(time_to_gain_star)).ToString() + " sekund";
		star_description_text[2].text = "Ukończ poziom otrzymując mniej niż " + max_damage_to_gain_star.ToString() + " obrażeń";
	}
	
	void Update()
	{
		time_elapsed += Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.Escape) == true)
		{
			if (pause_screen.activeSelf == true)
				unpause();
			else
				pause();
		}
	}
	
	public void finish_level(bool won)
	{
		pause();
		if (won == false)
			level_complete_text.text = "Nie żyjesz!";
		else
		{
			level_complete_text.text = "Poziom " + (level_index + 1).ToString() + " zaliczony!";
			Game_Data_Manager.set_level_complete(level_index, time_elapsed < time_to_gain_star, player_damage_taken < max_damage_to_gain_star);
		}
		time_elapsed_text.text = "Twój czas:\n" + ((int)(time_elapsed)).ToString() + "s";
		damage_taken_text.text = "Otrzymane obrażenia:\n" + player_damage_taken.ToString();
		for (int i = 0; i < Game_Data.stars_per_level; i++)
			star_gained_image[i].SetActive(Game_Data_Manager.is_star_gained(level_index, i));
		level_finished_screen.SetActive(true);
	}
	
	public void player_died()
	{
		finish_level(false);
	}
	
	public void player_received_damage(int amount)
	{
		player_damage_taken += amount;
	}
	
	public void return_to_level_select()
	{
		unpause();
		SceneManager.LoadScene(level_select_scene_id);
	}
	
	public void return_to_main_menu()
	{
		unpause();
		SceneManager.LoadScene(main_menu_scene_id);
	}
	
	public void restart_level()
	{
		unpause();
		SceneManager.LoadScene(current_level_scene_id);
	}
	
	public void pause()
	{
		Time.timeScale = 0.0f;
		pause_screen.SetActive(true);
	}
	
	public void unpause()
	{
		pause_screen.SetActive(false);
		Time.timeScale = 1.0f;
	}
}
