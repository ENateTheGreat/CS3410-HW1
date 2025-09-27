using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{

  private float randomX;
  private float randomY;
  private Rigidbody2D rb2d;

  // Start is called before the first frame update
  void Start()
  {
    rb2d = GetComponent<Rigidbody2D>();
    randomX = Random.Range(-10.0f, 10.0f);
    randomY = Random.Range(-10.0f, 10.0f);
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    Vector2 randomMovement = new Vector2(randomX, randomY);
    rb2d.velocity = randomMovement * 2;
  }
}
