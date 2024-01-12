using UnityEngine;


public class LightSourceShower : MonoBehaviour
{
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D _light;
    private void Start()
    {
        if (GlobalRepository.SystemVars.LightSourceData == null)
        {
            return;
        }

        _light.pointLightOuterRadius = _light.pointLightOuterRadius * GlobalRepository.SystemVars.LightSourceData.LightRadius;
        _light.intensity = _light.intensity * GlobalRepository.SystemVars.LightSourceData.LightIntensity;
        _light.color = GlobalRepository.SystemVars.LightSourceData.LightColor;
    }
}
