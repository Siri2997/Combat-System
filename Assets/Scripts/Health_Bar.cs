using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health_Bar : MonoBehaviour
{
    [SerializeField] private Image _healthBarSprite;
    public TMP_Text health_update_text;


    private void Start()
    {
        health_update_text= GetComponentInChildren<TMP_Text>();
    }
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
