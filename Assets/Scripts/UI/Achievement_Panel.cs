using UnityEngine;
using UnityEngine.UI;

public class Achievement_Panel : MonoBehaviour
{
	[SerializeField] private int achievement_index;
	[SerializeField] private GameObject icon_overlay_locked;
	[SerializeField] private Text name_text;
	[SerializeField] private Text description_text;
	[SerializeField] private Text progress_percentage_text;
	[SerializeField] private Slider progress_bar;
	
	void Start()
	{
		name_text.text = Game_Data_Manager.get_achievement_name(achievement_index);
		description_text.text = Game_Data_Manager.get_achievement_description(achievement_index);
		float progress_percentage = Game_Data_Manager.get_achievement_progress(achievement_index);
		progress_percentage_text.text = ((int)(progress_percentage * 100.0f)).ToString() + "%";
		progress_bar.value = progress_percentage;
		if (Game_Data_Manager.is_achievement_unlocked(achievement_index) == true)
			icon_overlay_locked.SetActive(false);
	}
}
