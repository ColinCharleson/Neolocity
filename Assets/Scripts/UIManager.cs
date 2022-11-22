using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas gameUI, pauseUI;

    public Image healthBar;
    public Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        gameUI.enabled = true;
        pauseUI.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = PlayerHealth.hp.health / PlayerHealth.hp.maxHealth;
        healthText.text = "Health: " + PlayerHealth.hp.health.ToString("000") + "/" + PlayerHealth.hp.maxHealth.ToString("000");

        if (Input.GetKeyDown(KeyCode.Escape))
		{
            gameUI.enabled = !gameUI.enabled;
            pauseUI.enabled = !pauseUI.enabled;
        }

        if(pauseUI.enabled == true)
		{
            Time.timeScale = 0;
		}
		else
		{
            Time.timeScale = 1;
		}
    }
}
