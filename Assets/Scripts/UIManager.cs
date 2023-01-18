using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    public Canvas gameUI, pauseUI, deathUI;

    public Image healthBar;
    public TextMeshProUGUI healthText;

    public Image staminaBar , attackBar;

    public PlayerController player;

    public KasaAttack kasa;

    public GameObject load;

    public AudioSource footstepsSource, sprintSource;

    // Start is called before the first frame update
    void Start()
    {
        gameUI.enabled = true;
        pauseUI.enabled = false;
    }

    void Update()
    {
        //load
        string filePath = Application.dataPath + "/save.txt";
        if (System.IO.File.Exists(filePath))
        {
            load.SetActive(true);
        }
        else
        {
            load.SetActive(false);
        }

        //stamina
        float staminaPercentage =  player.stamina / 100;
     
        staminaBar.fillAmount = staminaPercentage;

        //Attack
        float attackFill = kasa.attackBar / 100;

        attackBar.fillAmount = attackFill;

        //health
        float healthPercentage = PlayerHealth.hp.health / PlayerHealth.hp.maxHealth;
        healthBar.fillAmount = healthPercentage;

        healthText.text = "Health: " + PlayerHealth.hp.health.ToString("000") + "/" + PlayerHealth.hp.maxHealth.ToString("000");

        if (Input.GetKeyDown(InputSystem.key.pause) && PlayerHealth.hp.health > 0)
		{
            gameUI.enabled = !gameUI.enabled;
            pauseUI.enabled = !pauseUI.enabled;
        }

        if(pauseUI.enabled)
		{
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            footstepsSource.enabled = false;
            sprintSource.enabled = false;
        }
		else
		{
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(PlayerHealth.hp.health == 0)
		{
            Debug.Log("hp is " + PlayerHealth.hp.health);
          //  StartCoroutine(GameReset());
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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Quit()
    {
        Debug.Log("You Cant Leave :D");
        Application.Quit();
    }
}
