using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
	[SerializeField] private GameObject main_panel;
	[SerializeField] private GameObject load_panel;
	[SerializeField] private GameObject achievements_panel;
	[SerializeField] private GameObject settings_panel;
	
	public void load_save()
	{
		//TODO
		//wczytanie sceny z mapa swiata lub z ostatnio rozgrywanym poziomem i ustawianie odpowiednich zmiennych wyznaczajacych postep gracza w danym save'ie
	}
	
	public void start_new_game()
	{
		SceneManager.LoadScene(2);
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
	
	private void get_saves_list()
	{
		//TODO
		//wczytanie znajdujacych sie na dysku save'ow i dodanie ich do listy z ktorej gracz moze wybrac jeden z nich
	}
}
