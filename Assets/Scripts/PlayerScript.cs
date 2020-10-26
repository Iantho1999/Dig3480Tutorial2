using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public Text score, winText, lives;
    private int scoreValue = 0, livesValue = 3;
    private float hozMovement;

    private bool facingRight = true, jumping = false;

    public AudioSource musicSource;

    public AudioClip bgMusic;
    public AudioClip winMusic;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        winText.text = "";

        hozMovement = rd2d.velocity.x;

        musicSource.clip = bgMusic;
        musicSource.loop = true;
        musicSource.Play();

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        hozMovement = rd2d.velocity.x;

        if (jumping == false)
        {
            if (Input.GetKey(KeyCode.A))
            {
                anim.SetInteger("State", 1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
               anim.SetInteger("State", 1);
            }
            else
            {
                anim.SetInteger("State", 0);
            }
        }

        
        if (facingRight == false && hozMovement > 0.0f)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0.0f)
        {
            Flip();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if (scoreValue == 4)
            {
                livesValue = 3;
                transform.position = new Vector2(50.0f, 0.0f);
            }
            else if (scoreValue >= 8)
            {
                musicSource.clip = winMusic;
                musicSource.loop = false;
                musicSource.Play();

                winText.text = "You win! Game created by Ian Thomas";
            }
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives:" + livesValue.ToString();
            Destroy(collision.collider.gameObject);

            if (livesValue <= 0)
            {
                winText.text = "Game over";
                Destroy(this);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                jumping = true;
                anim.SetInteger("State", 2);
            }
            else
            {
                jumping = false;
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
