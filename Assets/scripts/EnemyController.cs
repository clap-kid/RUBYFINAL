using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private rubyController RubyController; // this line of code creates a variable called "rubyController" to store information about the RubyController script!
    public ParticleSystem smokeEffect;
    bool broken = true;
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public int damage;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController"); //this line of code finds the RubyController script by looking for a "RubyController" tag on Ruby

        if (rubyControllerObject != null)
        {

            RubyController = rubyControllerObject.GetComponent<rubyController>(); //and this line of code finds the rubyController and then stores it in a variable

            print ("Found the RubyConroller Script!");

        }

        if (RubyController == null)

        {

            print ("Cannot find GameController Script!");

        }

        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        if(!broken)
            {
                return;
            }

    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
        
        rigidbody2D.MovePosition(position);

        if(!broken)
            {
                return;
            }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        rubyController player = other.gameObject.GetComponent<rubyController >();

        if (player != null)
        {
            player.ChangeHealth(-damage);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        smokeEffect.Stop();
        animator.SetTrigger("Fixed");

        if (RubyController != null)
        {
                RubyController.ChangeScore(1); //this line of code is increasing Ruby's health by 1!
        }
    }
}
