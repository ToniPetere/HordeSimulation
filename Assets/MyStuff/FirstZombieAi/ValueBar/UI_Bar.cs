using UnityEngine;
using UnityEngine.UI;

public class UI_Bar : MonoBehaviour
{
    [SerializeField] Image bar;
    
    public void UpdateBar(float _value, float _maxValue)
    {
        bar.fillAmount = _value / _maxValue;
    }
}
