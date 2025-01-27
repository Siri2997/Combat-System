using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private Image _healthBarSprite;
    
    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        if (currentHealth <= 0)
        {
            _healthBarSprite.fillAmount = 0f;
        }
        else
        {
            _healthBarSprite.fillAmount = currentHealth / maxHealth;
        }
    }
}
