using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlinkLight : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _animationSpeed;

    private float _cursor = 0;
    private Light2D _light;
    private float _baseValue;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _baseValue = _light.intensity;
    }

    private void Update()
    {
        _cursor = (_cursor + Time.deltaTime * _animationSpeed) % 1;
        float value =_curve.Evaluate(_cursor);
        _light.intensity = _baseValue * value;
    }
}
