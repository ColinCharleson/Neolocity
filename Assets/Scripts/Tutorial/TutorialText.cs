using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    public float textDistance = 2f;
    public TMP_Text tutorialText;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tutorialText.alpha = 0f; 
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= textDistance)
        {
            tutorialText.alpha = 1f; 
        }
        else
        {
            tutorialText.alpha = 0f; 
        }
    }
}