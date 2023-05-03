using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightSourceShower : MonoBehaviour
{
    [SerializeField] private Light2D _light;
    private void Start()
    {
        if (GlobalRepository.LightSourceData == null)
        {
            return;
        }

        _light.pointLightOuterRadius = _light.pointLightOuterRadius * GlobalRepository.LightSourceData.LightRadius;
        _light.intensity = _light.intensity * GlobalRepository.LightSourceData.LightIntensity;
        _light.color = GlobalRepository.LightSourceData.LightColor;
    }
}
