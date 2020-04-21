using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Slider slider;
	
    // Start is called before the first frame update
    public void setHealth(int value)
    {
        slider.value = value;
    }
}
