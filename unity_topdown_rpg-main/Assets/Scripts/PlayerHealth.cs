using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    private float health = 0f;
	[SerializeField] private float maxHealth = 100f;
	[SerializeField] private Slider healthSlider;
    [SerializeField] public GameObject gamover;
    public static PlayerHealth Instance { get; private set; }

    private void Start() {
        health = maxHealth;
		healthSlider.maxValue = maxHealth;
        gamover.SetActive(false);
    }

	public void UpdateHealth(float mod) {
		if (DataManager.instance.isLoadGame)
		{
			health = DataManager.instance.data.health;
            Debug.Log("Health" + health);
            DataManager.instance.isLoadGame = false;
        }
		else
		{
            health += mod;

            if (health > maxHealth)
            {
                health = maxHealth;
            }
            else if (health <= 0f)
            {
                health = 0f;
                healthSlider.value = health;
                Destroy(gameObject);
                gamover.SetActive(true);
            }
            DataManager.instance.data.health = health;
        }
	}

	public void AddHealth(float x)
	{
		this.maxHealth += x;
		this.health += x;
		healthSlider.maxValue = this.maxHealth;
	}

	private void OnGUI() {
        if (DataManager.instance.isLoadGame)
        {
            var loadHealth = DataManager.instance.data.health;
            DataManager.instance.isLoadGame = false;
            float t = Time.deltaTime / 1f;
            healthSlider.value = Mathf.Lerp(healthSlider.value, loadHealth, t);
        }
        else
        {
            float t = Time.deltaTime / 1f;
            healthSlider.value = Mathf.Lerp(healthSlider.value, health, t);
        }
	}
}
