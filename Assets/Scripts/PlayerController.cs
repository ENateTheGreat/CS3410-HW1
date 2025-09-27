using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb2d;
    public float speed;
    private int count;
    public TMP_Text countText;
    public TMP_Text winText;
    public Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        count = 0;
        countText.text = "Count: " + count.ToString();
        winText.text = "";
        restartButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2d.velocity = movement * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            countText.text = "Count: " + count.ToString();
            if (count >= 12)
            {
                winText.text = "You win!";
                restartButton.gameObject.SetActive(true);
            }
        }
    }

    public void OnRestartButtonPress()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
