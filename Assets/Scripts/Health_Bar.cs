using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private Image _healthBarSprite;
    public TMP_Text health_update_text;


    private void Start()
    {
        // Ensure the text component is assigned
        if (health_update_text == null)
        {
            health_update_text = GetComponentInChildren<TMP_Text>();
        }
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        // Update the health bar fill
        _healthBarSprite.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
        if (currentHealth <= 0)
        {
            _healthBarSprite.fillAmount = 0f;
            // Display "Deceased" when health is zero or below
            health_update_text.text = "Deceased";
        }
        else
        {
            _healthBarSprite.fillAmount = currentHealth / maxHealth;

            // Display current health rounded to the nearest integer
            health_update_text.text = $"Health: {Mathf.CeilToInt(currentHealth)}";
        }
    }
}
