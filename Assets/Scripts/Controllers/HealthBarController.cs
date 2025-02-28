using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller script for the health bar prefab used by entities.
/// </summary>
public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private float _healthPercent;
    [SerializeField] private Gradient _colorGradient;
    [SerializeField] private Image _handleImage;

    private void Start()
    {
        // Set the color of the health bar, in case it was changed in the scene
        _handleImage.color = _colorGradient.Evaluate(_scrollbar.size);
    }

    private void Update()
    {
        // Only update the health bar, if the health has changed
        if (_scrollbar.size != _healthPercent)
        {
            _scrollbar.size = Mathf.Lerp(_scrollbar.size, _healthPercent, 0.2f);
            _handleImage.color = _colorGradient.Evaluate(_scrollbar.size);
        }
    }

    /// <summary>
    /// Updates the displayed health of the entity.
    /// </summary>
    /// <param name="healthPercent">The current health as a normalized value (0 to 1).</param>
    public void UpdateHealth(float healthPercent)
    {
        // Keep the value between 0 and 1, as it is a percentage
        _healthPercent = Mathf.Clamp01(healthPercent);
    }
}
