using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Map : MonoBehaviour
{
    private GameObject lock1;
    private GameObject lock2;
    // Start is called before the first frame update
    void Start()
    {
        lock1 = GameObject.Find("Icon_lock_1");
        lock2 = GameObject.Find("Icon_lock_2");
        if (Game_Data_Manager.get_stars_gained_amount(0) > 0)
        {
            lock1.SetActive(false);
        }
        if (Game_Data_Manager.get_stars_gained_amount(1) > 0)
        {
            lock2.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void load_level(int level_id)
    {
        SceneManager.LoadScene(level_id);
    }
}
