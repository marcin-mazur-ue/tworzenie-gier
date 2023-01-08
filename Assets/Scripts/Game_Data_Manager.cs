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
	
	public static void load_from_file(int save_slot_index)
	{
		string file_path = get_save_file_path(save_slot_index);
		if (File.Exists(file_path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream file = File.Open(file_path, FileMode.Open);
			instance.data = (Game_Data)formatter.Deserialize(file);
			file.Close();
		}
		else
			Debug.LogError("Nie można otworzyć pliku \"" + file_path + "\"!"); //TODO okno z komunikatem o błędzie zamiast tego
	}
	
	public static void save_to_file(int save_slot_index)
	{
		BinaryFormatter formatter = new BinaryFormatter(); 
		FileStream file = File.Create(get_save_file_path(save_slot_index));
		formatter.Serialize(file, instance.data);
		file.Close();
	}
	
	public static void unlock_level(int index)
	{
		instance.data.levels_unlocked[index] = true;
	}
	
	private static string get_save_file_path(int save_slot_index)
	{
		return Application.persistentDataPath + "/save" + save_slot_index.ToString();
	}
}

[Serializable]
class Game_Data
{
	private const int levels_amount = 2;
	private const int stars_amount = 3;
	private const int weapons_amount = 3;
	
	public bool[] levels_unlocked = new bool[levels_amount - 1]; // pierwszy poziom jest zawsze odblokowany, więc levels_unlocked[0] oznacza drugi, levels_unlocked[1] trzeci itd.
	public bool[,] stars_gained = new bool[levels_amount, stars_amount];
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
}