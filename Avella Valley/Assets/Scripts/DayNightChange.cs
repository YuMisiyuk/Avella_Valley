using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DayColors
{
    public Color skyColor;
}

public class DayNightChange : MonoBehaviour
{
    public bool StartDay; //start game as day time
    public UnityEngine.Material sky;
    public Light directionalLight;
    public float SecondsInAFullDay = 120f;
    [Range(0, 1)]
    public float currentTime = 0;
    public float timeMultiplier = 1f;
    public int currentDay = 0;
    float lightIntensity;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        lightIntensity = directionalLight.intensity;
        RenderSettings.skybox = sky;
        if (StartDay)
        {
            currentTime = 0.3f; //start at morning
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateLight();
        currentTime += (Time.deltaTime / SecondsInAFullDay) * timeMultiplier;
        if (currentTime >= 1)
        {
            currentTime = 0;//once we hit "midnight"; any time after that sunrise will begin.
            currentDay++; //make the day counter go up
        }
    }

    void UpdateLight()
    {
        directionalLight.transform.localRotation = Quaternion.Euler((currentTime * 360f) - 90, 170, 0);

        float intensityMultiplier = 1;

        if (currentTime <= 0.23f || currentTime >= 0.75f)
        {
            intensityMultiplier = 0; //when the sun is below the horizon, or setting, the intensity needs to be 0 or else it'll look weird
        }
        else if (currentTime <= 0.25f)
        {
            intensityMultiplier = Mathf.Clamp01((currentTime - 0.23f) * (1 / 0.02f));
        }
        else if (currentTime <= 0.73f)
        {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTime - 0.73f) * (1 / 0.02f)));
        }

        directionalLight.intensity = lightIntensity * intensityMultiplier;
    }
}

