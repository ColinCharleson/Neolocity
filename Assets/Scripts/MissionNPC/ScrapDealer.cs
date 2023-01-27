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
    public GameObject textBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TalkToNPC(int playerScrap)
    {
        textBox.SetActive(true);
        nameText.text = "Scrap Dealer";
        if (playerScrap > 0)
        {
            int sellPrice = playerScrap * Random.Range(5, 20);
            messageText.text = "I see you have " + playerScrap.ToString() + " scrap I'll buy that all off of you for " + sellPrice.ToString();
            player.GetComponent<PlayerController>().cash += sellPrice;
            player.GetComponent<PlayerController>().scrap = 0;
        }
		else
		{
            messageText.text = "Bring me some robot scrap, I'll make it worth your time.";
        }

    }
}
