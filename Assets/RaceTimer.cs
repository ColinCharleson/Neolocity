using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaceTimer : MonoBehaviour
{
    public bool raceStart;
    private bool raceInit;
    public float time;

    public TextMeshProUGUI timeDisplay;
    public TextMeshProUGUI winDisplay;

    public GameObject player1, player2;
    public GameObject winner;
    public Transform startPosition, endPosition;
    // Update is called once per frame
    void Update()
    {
        if(raceStart)
		{
            time += Time.deltaTime;
            timeDisplay.text = time.ToString("0.00");

        }

        if(!raceInit)
        if(Vector3.Distance(player1.transform.position, startPosition.position) < 3 && Vector3.Distance(player2.transform.position, startPosition.position) < 3)
		{
            raceInit = true;
            StartRace();
		}

        if (Vector3.Distance(player1.transform.position, endPosition.position) < 3)
        {
            EndRace(player1);
        }
        if(Vector3.Distance(player2.transform.position, endPosition.position) < 3)
		{
            EndRace(player2);
		}
    }

    public void StartRace()
	{
        StartCoroutine(Countdown());
	} 
    public void EndRace(GameObject winner)
	{
        winDisplay.text = winner.name + " Won The Race";
        raceStart = false;
	}

    IEnumerator Countdown()
    {
        timeDisplay.color = Color.red;
        timeDisplay.text = "3";
        yield return new WaitForSeconds(1);
        timeDisplay.color = Color.yellow;
        timeDisplay.text = "2";
        yield return new WaitForSeconds(1);
        timeDisplay.color = Color.green;
        timeDisplay.text = "1";
        yield return new WaitForSeconds(1);
        timeDisplay.color = Color.white;
        raceStart = true;
    }
}
