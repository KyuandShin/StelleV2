using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollPickupController : MonoBehaviour
{
    private bool playerNearby = false;
    public GameObject canvas;
    public Image[] images;
    private int currentImageIndex = 0;
    public int C = 0;
    public static int scrollsPickedUp = 0;
    public Text booksText;
   // public Image[] endingImages; // Final story images
    private int currentEndingImageIndex = 0; // Index of the currently displayed story image
   // public GameObject endingCanvas; // Canvas displayed at the end of the game

    private void Start()
    {
        // Initialize text if available
        if (booksText != null)
        {
            booksText.text = "0";
        }
        else
        {
            Debug.LogWarning("booksText is not assigned in ScrollPickupController");
        }
        
        scrollsPickedUp = 0;
        
        // Check if canvas is assigned before using it
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
        else
        {
            Debug.LogError("Canvas is not assigned in ScrollPickupController. Please assign it in the Inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
            collision.GetComponent<PlayerMovement>().EnablePickUp();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            collision.GetComponent<PlayerMovement>().DisablePickUp();
        }
    }

    private void Update()
    {
        // Try to get C value from ChannelScript if available
        try
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                ChannelScript channelScript = player.GetComponent<ChannelScript>();
                if (channelScript != null)
                {
                    C = channelScript.C;
                }
            }
        }
        catch (System.Exception ex)
        {
            // If there's an error, just continue with current C value
            Debug.LogWarning("Error getting ChannelScript: " + ex.Message);
        }

        // Check for pickup action - use E key or Space or channel input
        if (playerNearby && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || C == 1))
        {
            // Hide scroll object
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            // Increase the number of scrolls picked up
            scrollsPickedUp++;
            
            // Display Canvas and show the first image
            if (canvas != null)
            {
                canvas.SetActive(true);
                ShowImage(0);
            }
            else
            {
                Debug.LogError("Canvas is not assigned in ScrollPickupController");
            }
        }

        // If left mouse button is clicked and Canvas is active
        if (Input.GetMouseButtonDown(0) && canvas != null && canvas.activeSelf)
        {
            currentImageIndex++;
            // If there are more images
            if (currentImageIndex < images.Length)
            {
                // Show the next image
                ShowImage(currentImageIndex);
            }
            else
            {
                // No more images, hide Canvas
                canvas.SetActive(false);
                // Reset currentImageIndex for next viewing
                currentImageIndex = 0;
            }
        }

        // Check if all scrolls have been picked up
        if (scrollsPickedUp >= 7)
        {
            // If all scrolls have been picked up, end the game
            GameManager.instance.EndGame();
        }

        // Update books text if available
        if (booksText != null)
        {
            booksText.text = scrollsPickedUp.ToString();  // Update BooksText display
        }
    }

    void ShowImage(int index)
    {
        // Check if images array is valid
        if (images == null || images.Length == 0)
        {
            Debug.LogError("Images array is not assigned or empty in ScrollPickupController");
            return;
        }
        
        // Make sure index is within bounds
        if (index < 0 || index >= images.Length)
        {
            Debug.LogWarning("Image index out of bounds: " + index);
            return;
        }
        
        // Show the selected image and hide others
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i] != null)
            {
                images[i].enabled = (i == index);
            }
        }
    }
}
