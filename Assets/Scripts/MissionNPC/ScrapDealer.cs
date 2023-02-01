using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrapDealer : MonoBehaviour
{
    [Header("Character Data")]
    private GameObject player;

    [Header("Text Box Data")]
    //Text Box
    public GameObject skillsUI;
    public TextMeshProUGUI quitPrompt;
    public Skillshop shop;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(InputSystem.key.cancelTalk))
        {
            skillsUI.SetActive(false);
        }
    }

    public void TalkToNPC(int playerScrap)
    {
        quitPrompt.text = "Press " + InputSystem.key.cancelTalk.ToString() + " to stop trading with the scrap dealer";
        shop.CashUpdate();
        Cursor.lockState = CursorLockMode.None;
        skillsUI.SetActive(true);
    }
}
