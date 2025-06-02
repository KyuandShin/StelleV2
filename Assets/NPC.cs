using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPC : MonoBehaviour
{
    public GameObject dialogPanel;
    public Text dialogText;
    public string[] dialog;
    private int index;

    public GameObject contButton;

    public float wordSpeed;
    public bool playerIsClose;

/*************  ✨ Windsurf Command ⭐  *************/
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    /// <remarks>
    /// If the player presses the F key and is close to the NPC, toggle the dialog panel.
    /// If the dialog text is the full text of the current line, enable the continue button.
    /// </remarks>
/*******  f095027c-e3a3-4457-b6b4-ef4eae28e9f1  *******/
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerIsClose)
        {
            if (dialogPanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                dialogPanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }
        
        if (dialogText.text == dialog[index])
        {
            contButton.SetActive(true);
        }

    }

    public void zeroText()
    {
        dialogText.text = "";
        index = 0;
        dialogPanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialog[index].ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {

        contButton.SetActive(false);
        
        if (index < dialog.Length - 1)
        {
            index++;
            dialogText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }

    }
    
}