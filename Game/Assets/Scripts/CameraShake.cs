using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shaketime = 0f;
    private float shaketimetotal = 0f;
    private float startingintensity = 0f;

    // 0.5f, 0.2f, 10f
    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    public void SetCameraShake(float intensity, float time, float fr)
    {
        if (cinemachineVirtualCamera != null)
        {
            CinemachineBasicMultiChannelPerlin perlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = intensity;
            perlin.m_FrequencyGain = fr;
            shaketime = time;
            shaketimetotal = time;
            startingintensity = intensity;
        }

    }

    private void Update()
    {
        if (shaketime > 0)
        {
            shaketime -= Time.deltaTime;
            if (shaketime <= 0f)
            {
                CinemachineBasicMultiChannelPerlin perlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = Mathf.Lerp(startingintensity, 0f, 1 - (shaketime / shaketimetotal));
            }
        }
    }
}
