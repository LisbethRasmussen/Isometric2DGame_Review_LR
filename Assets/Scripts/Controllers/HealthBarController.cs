using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private float _healthPercent;
    [SerializeField] private Gradient _colorGradient;
    [SerializeField] private Image _handleImage;

    private void Start()
    {
        _handleImage.color = _colorGradient.Evaluate(_scrollbar.size);
    }

    private void Update()
    {
        if (_scrollbar.size != _healthPercent)
        {
            _scrollbar.size = Mathf.Lerp(_scrollbar.size, _healthPercent, 0.2f);
            _handleImage.color = _colorGradient.Evaluate(_scrollbar.size);
        }
    }

    public void UpdateHealth(float healthPercent)
    {
        _healthPercent = Mathf.Clamp01(healthPercent);
    }
}
