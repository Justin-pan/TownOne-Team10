using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
public class PointsBar : MonoBehaviour
{
    public const int MAX_POINTS = 200;
    public const float POINTS_SPEED = 2f;
    [SerializeField]
    private TextMeshProUGUI textBox;

    [SerializeField]
    private Slider slider;

    private float timing = 0;



    private bool updatePoints = false;
    private int oldValues;
    private int newValues;

    public void Start()
    {
        slider.maxValue = MAX_POINTS;
    }

    public void Update()
    {
        if (updatePoints)
        {

            timing += Time.deltaTime;
            slider.value = Mathf.Lerp(oldValues, newValues, timing / POINTS_SPEED);

            if (slider.value == newValues)
            {
                oldValues = newValues;
                updatePoints = false;
                timing = 0;
            }
        }
        
    }

    public void SetText(string value)
    {
        textBox.text = value;
    }

    public void SetSlider(int value)
    {
        slider.value = value;
    }

    public void UpdatePoints(int value)
    {
        newValues = value;
        updatePoints = true;
    }
} 
