using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Canvas gameUI, pauseUI, deathUI;

    public Image healthBar;
    public Text healthText;

    public Image staminaBar;

    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        gameUI.enabled = true;
        pauseUI.enabled = false;
    }

    void Update()
    {
        //stamina
        float staminaPercentage =  player.stamina / 100;
        staminaBar.fillAmount = staminaPercentage;

        //health
        float healthPercentage = PlayerHealth.hp.health / PlayerHealth.hp.maxHealth;
        healthBar.fillAmount = healthPercentage;

        healthText.text = "Health: " + PlayerHealth.hp.health.ToString("000") + "/" + PlayerHealth.hp.maxHealth.ToString("000");

        if (Input.GetKeyDown(KeyCode.Escape))
		{
            gameUI.enabled = !gameUI.enabled;
            pauseUI.enabled = !pauseUI.enabled;
        }

        if(pauseUI.enabled)
		{
            Time.timeScale = 0;
		}
		else
		{
            Time.timeScale = 1;
		}

        if(PlayerHealth.hp.health == 0)
		{
            StartCoroutine(GameReset());
		}
    }

    IEnumerator GameReset()
    {
        deathUI.enabled = true;
        pauseUI.enabled = false;
        gameUI.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        player.tempKasa.SetTrigger("Dead");
        player.isAlive = false;

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
