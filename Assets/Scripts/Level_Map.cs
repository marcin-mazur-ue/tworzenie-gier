using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Map : MonoBehaviour
{
    [SerializeField] private GameObject level2;
    [SerializeField] private GameObject level2lock;
    [SerializeField] private GameObject level3;
    [SerializeField] private GameObject level3lock;
    [SerializeField] private GameObject level1star1;
    [SerializeField] private GameObject level1star2;
    [SerializeField] private GameObject level1star3;
    [SerializeField] private GameObject level2star1;
    [SerializeField] private GameObject level2star2;
    [SerializeField] private GameObject level2star3;
    [SerializeField] private GameObject level3star1;
    [SerializeField] private GameObject level3star2;
    [SerializeField] private GameObject level3star3;
    [SerializeField] private Texture2D starimage;
    private Color c;
    // Start is called before the first frame update
    void Start()
    {
        get_unlocked_levels();
        get_stars();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void level_1_button()
    {
        SceneManager.LoadScene(2);
    }
    public void level_2_button()
    {
        SceneManager.LoadScene(3);
    }
    public void level_3_button()
    {
        SceneManager.LoadScene(4);
    }
    void get_unlocked_levels()
    {
        if (Game_Data_Manager.get_stars_gained_amount(0) > 0)
        {
            level2lock.SetActive(false);
            c = level2.GetComponent<RawImage>().color;
            c.a = 1f;
            level2.GetComponent<RawImage>().color = c;
            level2.GetComponent<Button>().enabled = true;
        }
        if (Game_Data_Manager.get_stars_gained_amount(1) > 0)
        {
            level3lock.SetActive(false);
            c = level3.GetComponent<RawImage>().color;
            c.a = 1f;
            level3.GetComponent<RawImage>().color = c;
            level3.GetComponent<Button>().enabled = true;
        }
    }
    void get_stars()
    {
        if (Game_Data_Manager.is_star_gained(0, 0))
        {
            level1star1.GetComponent<RawImage>().texture = starimage;
        }
        if (Game_Data_Manager.is_star_gained(0, 1))
        {
            level1star2.GetComponent<RawImage>().texture = starimage;
        }
        if (Game_Data_Manager.is_star_gained(0, 2))
        {
            level1star3.GetComponent<RawImage>().texture = starimage;
        }
        if (Game_Data_Manager.is_star_gained(1, 0))
        {
            level2star1.GetComponent<RawImage>().texture = starimage;
        }
        if (Game_Data_Manager.is_star_gained(1, 1))
        {
            level2star2.GetComponent<RawImage>().texture = starimage;
        }
        if (Game_Data_Manager.is_star_gained(1, 2))
        {
            level2star3.GetComponent<RawImage>().texture = starimage;
        }
        if (Game_Data_Manager.is_star_gained(2, 0))
        {
            level3star1.GetComponent<RawImage>().texture = starimage;
        }
        if (Game_Data_Manager.is_star_gained(2, 1))
        {
            level3star2.GetComponent<RawImage>().texture = starimage;
        }
        if (Game_Data_Manager.is_star_gained(2, 2))
        {
            level3star3.GetComponent<RawImage>().texture = starimage;
        }
    }
}
