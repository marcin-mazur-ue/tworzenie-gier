using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
	[SerializeField] private GameObject main_panel;
	[SerializeField] private GameObject load_panel;
	[SerializeField] private GameObject achievements_panel;
	[SerializeField] private GameObject settings_panel;
    [SerializeField] private GameObject credits_panel;

    [SerializeField] private Toggle fullscreen_toggle;
	[SerializeField] private Toggle vsync_toggle;
	[SerializeField] private Dropdown resolutions_dropdown;
	
	private List<Resolution> resolutions = new List<Resolution>();
	private const int level_select_scene_id = 1;
	
	void Start()
	{
		fullscreen_toggle.isOn = Screen.fullScreen;
		vsync_toggle.isOn = (QualitySettings.vSyncCount != 0);
		List<string> resolution_strings = new List<string>();
		int active_resolution_index = -1;
		for (int i = 0; i < Screen.resolutions.Length; i++)
		{
			Resolution current_resolution = Screen.resolutions[i];
			resolutions.Add(current_resolution);
			resolution_strings.Add(current_resolution.width + "x" + current_resolution.height + " " + current_resolution.refreshRate + "Hz");
			if (current_resolution.width == Screen.width && current_resolution.height == Screen.height && current_resolution.refreshRate == Screen.currentResolution.refreshRate)
				active_resolution_index = i;
		}
		resolutions_dropdown.ClearOptions();
		resolutions_dropdown.AddOptions(resolution_strings);
		resolutions_dropdown.value = active_resolution_index;
		resolutions_dropdown.RefreshShownValue();
	}
	
	public void apply_settings()
	{
		if (vsync_toggle.isOn == true)
			QualitySettings.vSyncCount = 1;
		else
			QualitySettings.vSyncCount = 0;
		Resolution active_resolution = resolutions[resolutions_dropdown.value];
		Screen.SetResolution(active_resolution.width, active_resolution.height, fullscreen_toggle.isOn);
	}
	
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
        credits_panel.SetActive(false);
    }
	
	public void show_saved_games()
	{
		main_panel.SetActive(false);
		load_panel.SetActive(true);
		achievements_panel.SetActive(false);
		settings_panel.SetActive(false);
        credits_panel.SetActive(false);
    }
	
	public void show_achievements()
	{
		main_panel.SetActive(false);
		load_panel.SetActive(false);
		achievements_panel.SetActive(true);
		settings_panel.SetActive(false);
        credits_panel.SetActive(false);
    }
	
	public void show_settings()
	{
		main_panel.SetActive(false);
		load_panel.SetActive(false);
		achievements_panel.SetActive(false);
		settings_panel.SetActive(true);
        credits_panel.SetActive(false);
    }

    public void show_credits()
    {
        main_panel.SetActive(false);
        load_panel.SetActive(false);
        achievements_panel.SetActive(false);
        settings_panel.SetActive(false);
        credits_panel.SetActive(true);
    }

    public void quit()
	{
		Application.Quit();
	}
}
