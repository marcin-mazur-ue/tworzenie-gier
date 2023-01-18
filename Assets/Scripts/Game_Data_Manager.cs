using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Game_Data_Manager : MonoBehaviour
{
	public static Game_Data_Manager instance;
	
	private Game_Data data = new Game_Data();
	
	void Awake()
	{
		if (instance != null)
			Destroy(gameObject);
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
	
	public static bool is_star_gained(int level_index, int star_index)
	{
		return instance.data.stars_gained[level_index, star_index];
	}
	
	public static void load_and_apply(int save_slot_index)
	{
		Game_Data loaded_data = load_from_file(save_slot_index);
		if (loaded_data == null)
			Debug.LogError("Nie można wczytać pliku \"" + get_save_file_path(save_slot_index) + "\"!"); //TODO okno z komunikatem o błędzie zamiast tego
		else
			instance.data = loaded_data;
	}
	
	public static Game_Data load_from_file(int save_slot_index)
	{
		string file_path = get_save_file_path(save_slot_index);
		if (File.Exists(file_path) == false)
			return null;
		
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream file = File.Open(file_path, FileMode.Open);
		Game_Data loaded_data = (Game_Data)(formatter.Deserialize(file));
		file.Close();
		return loaded_data;
	}
	
	public static void save_to_file(int save_slot_index)
	{
		BinaryFormatter formatter = new BinaryFormatter(); 
		FileStream file = File.Create(get_save_file_path(save_slot_index));
		formatter.Serialize(file, instance.data);
		file.Close();
	}
	
	public static void set_level_complete(int index, bool star_gained_time, bool star_gained_damage)
	{
		instance.data.stars_gained[index, 0] = true;
		if(index < Game_Data.levels_amount - 1)
			instance.data.levels_unlocked[index] = true; // indeksem tak naprawdę jest [index + 1 - 1], ponieważ odblokowany powinien zawsze zostać następny poziom, ale tablica levels_unlocked nie uwzględnia pierwszego poziomu
		if(star_gained_time == true)
			instance.data.stars_gained[index, 1] = true;
		if(star_gained_damage == true)
			instance.data.stars_gained[index, 2] = true;
	}
	
	private static string get_save_file_path(int save_slot_index)
	{
		return Application.persistentDataPath + "/save" + save_slot_index.ToString();
	}
}

[Serializable]
public class Game_Data
{
	public const int levels_amount = 2;
	public const int stars_per_level = 3;
	public const int weapons_amount = 3;
	
	public bool[] levels_unlocked = new bool[levels_amount - 1]; // pierwszy poziom jest zawsze odblokowany, więc levels_unlocked[0] oznacza drugi, levels_unlocked[1] trzeci itd.
	public bool[,] stars_gained = new bool[levels_amount, stars_per_level]; // [x, 0] - przejście poziomu; [x, 1] - przejście poziomu w wyznaczonym czasie; [x, 2] - przejście poziomu nie przekraczając określonej ilości otrzymanych obrażeń
	public bool[] weapons_unlocked = new bool[weapons_amount - 1]; // "brak broni" liczy się jako broń, ale jest zawsze odblokowany
	
	public Game_Data()
	{
		for (int i = 0; i < levels_unlocked.Length; i++)
			levels_unlocked[i] = false;
		for (int i = 0; i < stars_gained.GetLength(0); i++)
		{
			for (int j = 0; j < stars_gained.GetLength(1); j++)
				stars_gained[i, j] = false;
		}
		for (int i = 0; i < weapons_unlocked.Length; i++)
			weapons_unlocked[i] = false;
	}
	
	public float get_game_completion_percentage()
	{
		return (float)(get_stars_gained_amount() + get_weapons_unlocked_amount()) / (float)(levels_amount * stars_per_level + weapons_amount);
	}
	
	public int get_highest_unlocked_level()
	{
		for (int i = 0; i < levels_unlocked.Length; i++)
		{
			if (levels_unlocked[i] == true)
				return i + 2;
		}
		return 1;
	}
	
	public int get_stars_gained_amount()
	{
		int stars_gained_amount = 0;
		for (int i = 0; i < stars_gained.GetLength(0); i++)
		{
			for (int j = 0; j < stars_gained.GetLength(1); j++)
			{
				if (stars_gained[i, j] == true)
					stars_gained_amount++;
			}
		}
		return stars_gained_amount;
	}
	
	public int get_weapons_unlocked_amount()
	{
		int weapons_unlocked_amount = 1;
		for (int i = 0; i < weapons_unlocked.Length; i++)
		{
			if (weapons_unlocked[i] == true)
				weapons_unlocked_amount++;
		}
		return weapons_unlocked_amount;
	}
}