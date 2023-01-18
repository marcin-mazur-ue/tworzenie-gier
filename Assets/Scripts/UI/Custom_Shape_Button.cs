using UnityEngine;
using UnityEngine.UI;

public class Custom_Shape_Button : MonoBehaviour
{
	void Start()
	{
		this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
	}
}
