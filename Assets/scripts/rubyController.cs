using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class rubyController : MonoBehaviour
{
    private GameObject cube;
    public int oiled = 0;
    public GameObject oilUI;
    public AudioClip collectedAmmo;
    private int numrobots = 0;
    private int cogs;
    public static int level = 0;
    private AudioSource bingus;
    public AudioClip victorymusic;
    public AudioClip losemusic;
    private int winning;
    private int gameOver;
    public GameObject WIN;
    public GameObject WIN2;
    public GameObject LOSE;
    private int score = 0;
    public Text scoretext;
    public Text cogtext;
    AudioSource audioSource;
    public AudioClip BONKED;
    public AudioClip cogthrow;
    public AudioClip oilslip;

    public ParticleSystem hitEffect;
    
    public float speed = 3.0f;
    
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    public int health { get { return currentHealth; }}
    int currentHealth;
    
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    public GameObject projectilePrefab;




    
    // Start is called before the first frame update
    void Start()
    {
        Scene whatlevel = SceneManager.GetActiveScene ();
        string levelname = whatlevel.name;

        if (levelname == "Main")
        {
            level = 1;
        }

        if (levelname == "Level2")
        {
            level =2;
        }

        cube = GameObject.FindGameObjectWithTag("cube");

        GameObject background = GameObject.FindWithTag("BGmusic");

        if (background != null)
        {

            bingus = background.GetComponent<AudioSource>();

        }

        if (level == 1)
        {
            numrobots = 4;
        }

        if (level == 2)
        {
            numrobots = 5;
        }

        WIN.SetActive(false);
        WIN2.SetActive(false);
        LOSE.SetActive(false);
        scoretext.text = score + "/" + numrobots;
        cogs = 4;
        cogtext.text = cogs + "/4";

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        audioSource= GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

        if (score == 4 && level == 2)
        {
            cube.SetActive(false);
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if(Input.GetKeyDown(KeyCode.C) && cogs != 0)
            {
                Launch();
                cogs = cogs - 1;
                cogtext.text = cogs + "/4";
            }


        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
                {
                    if (winning == 1)
                    {
                        SceneManager.LoadScene("Level2");
                    }

                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();
                    }  
                }
        }

        if (score == 4 && winning == 0 && level == 1)
        {
            WIN.SetActive(true);
            winning += 1;
            PlaySound(victorymusic);
            bingus.Stop();
        }

        if (score == 5 && winning == 0 && level == 2)
        {
            WIN2.SetActive(true);
            winning += 1;
            PlaySound(victorymusic);
            bingus.Stop();
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

         if (Input.GetKey(KeyCode.R))

        {

            if (gameOver == 1)

            {

              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            }

            if (winning == 1)
            {
                SceneManager.LoadScene("Main");
            }

        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);


    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0 && gameOver != 1)
        {
            Instantiate(hitEffect, rigidbody2d.transform.position, Quaternion.identity);
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;

            PlaySound(BONKED);
        }
        
        if (oiled == 0)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        }
        else if (oiled == 1)
        {
            currentHealth = Mathf.Clamp(currentHealth + (amount * 2), 0, maxHealth);
            UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        }

        if (currentHealth == 0 && gameOver != 1)
        {
            LOSE.SetActive(true);
            gameOver += 1;
            speed = 0;
            PlaySound(losemusic);
            bingus.Stop();
        }
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(cogthrow);
    }

    public void PlaySound(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

    public void ChangeScore(int newscore)
    {
        score += newscore;
        scoretext.text = score + "/" + numrobots;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.tag == "Ammo" && cogs == 0)
        {
            PlaySound(collectedAmmo);
            cogs += 4;
            cogtext.text = cogs + "/4";
            Destroy(collision.gameObject);
        }

        if (collision.tag == "oil" && oiled != 1)
        {
            oiled = 1;
            PlaySound(oilslip);
            oilUI.SetActive(true);
            speed = speed - 1;
        }

    }

}

