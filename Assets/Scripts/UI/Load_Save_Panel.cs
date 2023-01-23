using UnityEngine;
using UnityEngine.UI;

public class Load_Save_Panel : MonoBehaviour
{
	[SerializeField] private int save_slot_index;
	[SerializeField] private GameObject panel_save_info;
	[SerializeField] private Text completion_percentage;
	[SerializeField] private Text highest_level_unlocked;
	[SerializeField] private Text stars_gained;
	[SerializeField] private Text weapons_unlocked;
	
	private Game_Data data;
	
	void Start()
	{
		data = Game_Data_Manager.load_from_file(save_slot_index);
		if (data == null)
		{
			panel_save_info.SetActive(false);
			return;
		}
		completion_percentage.text = (Mathf.Floor(data.get_game_completion_percentage() * 100.0f)).ToString() + "%";
		highest_level_unlocked.text = "Poziom " + data.get_highest_unlocked_level().ToString();
		stars_gained.text = data.get_total_stars_gained_amount().ToString().PadLeft(2, ' ') + "/" + (Game_Data.levels_amount * Game_Data.stars_per_level).ToString();
		weapons_unlocked.text = data.get_weapons_unlocked_amount().ToString().PadLeft(2, ' ') + "/" + Game_Data.weapons_amount.ToString();
	}
}
