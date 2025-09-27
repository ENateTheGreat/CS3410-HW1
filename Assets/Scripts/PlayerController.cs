/*
 * Author: E. Nathan Lee
 * Date: 9/27/2025
 * Description: Controls player movement, win/lose conditions, UI updates, game twist logic, as well as some (attempted) gameplay tuning.
 */

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // UI
    public TMP_Text countText;
    public TMP_Text winText;
    public Button restartButton;

    // Gameplay functionality
    public float speed;
    private float killRadius = 2.1f; // unsure of if this is necessary, but it provides data without making another collider for a trigger
    private Rigidbody2D rb2d;
    public LayerMask hazard; // "PickUps" layer
    float startClock = 60f;
    float clock;
    bool end; // Win/Lose state

    // Special Twist Items
    public GameObject item1; // unused, got too hard too fast
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    public GameObject item5;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        clock = startClock;
        UpdateClockUI();
        winText.text = "";
        restartButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (end) return;

        clock -= Time.deltaTime; // Counting the seconds based on the example was only counting up, so I reversed the logic

        if (clock <= 0f)
        {
            clock = 0f;
            Win();
            return;
        }
        UpdateClockUI();
        ClockTrigger();
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (end) { rb2d.velocity = Vector2.zero; return; } // Stop movement on win/lose

        // Movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.velocity = movement * speed;

        // Hazard detection logic
        if (clock > 0f && Physics2D.OverlapCircle(rb2d.position, killRadius, hazard))
        {
                GameOver();
        }
    }

    // Lose condition
    void GameOver()
    {
        if (end) return; // Prevent loss after win
        end = true;
        winText.text = "GAME OVER";
        restartButton.gameObject.SetActive(true);
    }

    // Win condition
    void Win()
    {
        if (end) return; // Prevent win after loss
        end = true;
        winText.text = "YOU WIN!";
        restartButton.gameObject.SetActive(true);
        countText.text = "Remaining Time: 0"; // Something with the time would always freeze at 1 second (round up?)
    }

    // Restart Functionality
    public void OnRestartButtonPress()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Function to handle clock UI instead of calling the main logic everywhere
    void UpdateClockUI()
    {
        countText.text = "Remaining Time: " + Mathf.CeilToInt(clock).ToString();
    }

    // My secret twist - adding new hazards, but supplementing with speed. Luck aspect with random spawn while in motion
    void ClockTrigger()
    {
        if (clock < 7f)
        {
            item2.SetActive(true);
        }
        else if (clock < 15f)
        {
            item3.SetActive(true);
            if (speed > 14) speed += 3f;
        }
        else if (clock < 30f)
        {
            item4.SetActive(true);
            if (speed < 12) speed += 2.5f;
        }
        else if (clock < 45f)
        {
            item5.SetActive(true);
        }
    }

    // Visualizer for the kill radius in scene view
    void OnDrawGizmosSelected()
    {
        if (rb2d != null)
        {
            Gizmos.color = Color.red;
            Vector2 pos = transform.position;
            Gizmos.DrawWireSphere(pos, killRadius);
        }
    }
}
