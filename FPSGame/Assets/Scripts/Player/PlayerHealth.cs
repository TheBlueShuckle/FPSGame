using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image frontHealthBar;
    public Image backHealthBar;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;

    private float health;
    private float lerpTimer;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealth();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(Random.Range(5, 10));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RestoreHealth(Random.Range(5, 10));
        }
    }

    public void UpdateHealth()
    {
        Debug.Log(health);
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = Mathf.Pow(percentComplete, 2);
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (hFraction > fillF)
        {            
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = Mathf.Pow(percentComplete, 2);
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }
}
