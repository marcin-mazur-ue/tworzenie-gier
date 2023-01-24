using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
	[SerializeField] private GameObject main_panel;
	[SerializeField] private GameObject load_panel;
	[SerializeField] private GameObject achievements_panel;
	[SerializeField] private GameObject settings_panel;
	
	private const int level_select_scene_id = 1;
	
	public void load_save(int save_slot_id)
	{
		Game_Data_Manager.load_and_apply(save_slot_id);
		SceneManager.LoadScene(level_select_scene_id);
	}
	
	public void start_new_game()
	{
		Game_Data_Manager.reset();
		SceneManager.LoadScene(level_select_scene_id);
	}
	
	public void show_main_menu()
	{
		main_panel.SetActive(true);
		load_panel.SetActive(false);
		achievements_panel.SetActive(false);
		settings_panel.SetActive(false);
	}
	
	public void show_saved_games()
	{
		main_panel.SetActive(false);
		load_panel.SetActive(true);
		achievements_panel.SetActive(false);
		settings_panel.SetActive(false);
	}
	
	public void show_achievements()
	{
		main_panel.SetActive(false);
		load_panel.SetActive(false);
		achievements_panel.SetActive(true);
		settings_panel.SetActive(false);
	}
	
	public void show_settings()
	{
		main_panel.SetActive(false);
		load_panel.SetActive(false);
		achievements_panel.SetActive(false);
		settings_panel.SetActive(true);
	}
	
	public void quit()
	{
		Application.Quit();
	}
}
