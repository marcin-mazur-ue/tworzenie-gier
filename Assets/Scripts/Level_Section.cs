using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Section : MonoBehaviour
{
	[SerializeField] private bool first_section;
	[SerializeField] private bool last_section;
	[SerializeField] private float enemy_spawn_delay;
	[SerializeField] private float camera_speed = 50.0f;
	
	[SerializeField] private GameObject enemies_group;
	[SerializeField] private GameObject exit_barrier;
	[SerializeField] private Transform section_camera_position;
	
	[SerializeField] private Text active_enemies_text;
	[SerializeField] private GameObject level_complete_screen;
	[SerializeField] private Camera level_camera;
	
	private bool active;
	private bool cleared;
	private bool enemies_spawned;
	
	private BoxCollider section_collider;
	
	void Start()
	{
		section_collider = GetComponent<BoxCollider>();
		active = false;
		cleared = false;
		enemies_spawned = false;
		if (first_section == true)
			activate();
	}

	void Update()
	{
		if (active == false || cleared == true)
			return;
		
		move_camera();
		if (enemies_spawned == false)
			return;
		
		int active_enemies = count_active_enemies();
		active_enemies_text.text = active_enemies.ToString().PadLeft(2, ' ');
		if (active_enemies == 0)
		{
			cleared = true;
			if (last_section == false)
				exit_barrier.SetActive(false);
			else
			{
				level_complete_screen.SetActive(true);
				Time.timeScale = 0.0f;
			}
		}
	}
	
	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "Player")
			activate();
	}
	
	private void activate()
	{
		active = true;
		if (exit_barrier != null)
			exit_barrier.SetActive(true);
		StartCoroutine(spawn_enemies());
	}
	
	private IEnumerator spawn_enemies()
	{
		yield return new WaitForSeconds(enemy_spawn_delay);
		enemies_group.SetActive(true);
		enemies_spawned = true;
	}
	
	private int count_active_enemies()
	{
		int result = 0;
		Collider[] level_section_colliders = Physics.OverlapBox(section_collider.transform.position, section_collider.transform.lossyScale / 2.0f, Quaternion.identity);
		foreach (Collider collider in level_section_colliders)
		{
			if (collider.gameObject.tag == "Enemy")
				result++;
		}
		return result;
	}
	
	private void move_camera()
	{
		float camera_distance = section_camera_position.position.z - level_camera.transform.position.z;
		float distance_to_move = camera_speed * Time.deltaTime;
		if (camera_distance > distance_to_move)
			level_camera.transform.position += new Vector3(0.0f, 0.0f, distance_to_move);
		else
			level_camera.transform.position = section_camera_position.position;
	}
}
