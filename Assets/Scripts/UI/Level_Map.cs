using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Map : MonoBehaviour
{
	[SerializeField] private GameObject[] icon_locked = new GameObject[Game_Data.levels_amount - 1];
	[SerializeField] private GameObject[] icon_unlocked = new GameObject[Game_Data.levels_amount - 1];
	[SerializeField] private GameObject[] stars = new GameObject[Game_Data.levels_amount * Game_Data.stars_per_level]; // Unity nie wyświetla dwuwymiarowych tablic w inspektorze, a nie ma czasu na obchodzenie tego żeby kod był bardziej elegancki
	[SerializeField] private GameObject save_panel;
	
	private const int main_menu_scene_id = 0;
	private const int level_select_scene_id = 1;

	void Start()
	{
		for (int i = 0; i < Game_Data.levels_amount - 1; i++)
		{
			bool unlocked = Game_Data_Manager.is_level_unlocked(i + 1);
			icon_locked[i].SetActive(!unlocked);
			icon_unlocked[i].SetActive(unlocked);
		}
		for (int i = 0; i < Game_Data.levels_amount; i++)
		{
			for (int j = 0; j < Game_Data.stars_per_level; j++)
				stars[i * Game_Data.stars_per_level + j].SetActive(Game_Data_Manager.is_star_gained(i, j));
		}
	}
	
	public void return_to_main_menu()
	{
		SceneManager.LoadScene(main_menu_scene_id);
	}
	
	public void save_game(int save_slot_index)
	{
		Game_Data_Manager.save_to_file(save_slot_index);
		SceneManager.LoadScene(level_select_scene_id);
	}
	
	public void show_save_panel(bool enabled)
	{
		save_panel.SetActive(enabled);
	}

	public void start_level(int level_scene_index)
	{
		SceneManager.LoadScene(level_scene_index);
	}
}
