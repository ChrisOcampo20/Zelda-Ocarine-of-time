using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    //Variables del movimiento del personaje

    public static Link sharedInstance;

    public float jumpForce = 6f;
    public float runningSpeed = 2f;
    private Rigidbody2D rigiBody;
    Animator animator;
    public AudioClip ocarina;
    private AudioSource source;
    Vector3 startPosition;

    private float temporizador = 0f;

    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_THE_GROUND = "isOnTheGround";
    private const string STATE_ISWALKING = "isWalking";
    private const string STATE_ISJUMPING = "isJumping";
    private const string STATE_ISSTATIC = "isStatic";

    private int healthPoints, manaPoints;

    public const int INITIAL_HEALTH = 100, INITIAL_MANA = 15, MAX_HEALTH = 200, MAX_MANA = 30, MIN_HEALTH = 10, MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;
    

   

    public LayerMask groundMask;
    private bool ocarinaMusic;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        rigiBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }

        startPosition = this.transform.position;
    }

    public void StartGame()
    {
        ocarinaMusic = false;
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, true);
        animator.SetBool(STATE_ISWALKING, false);
        animator.SetBool(STATE_ISJUMPING, false);
        animator.SetBool(STATE_ISSTATIC, false);

        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

        Invoke("RestartPosition", 0.1f);
    }

    void RestartPosition()
    {
        this.transform.position = startPosition;
        this.rigiBody.velocity = Vector2.zero;
        GameObject MainCamera = GameObject.Find("Main Camera");
        MainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }



    // Update is called once per frame
    void Update()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)

            if (rigiBody.velocity == Vector2.zero) 
        { 
            temporizador += Time.deltaTime;
        }
        else
        {
            temporizador = 0;
        }
       
        
        
        if (rigiBody.velocity == Vector2.zero && IsTouchingTheGruound() && animator.GetBool(STATE_ALIVE)==true)
        {
           
            animator.SetBool(STATE_ISWALKING, false);
            animator.SetBool(STATE_ISJUMPING, false);
            
            if (temporizador < 5f)
            {
                animator.SetBool(STATE_ISSTATIC, false);
                animator.Play("Link");

            }
            else if (temporizador >= 5f)
            {
                animator.SetBool(STATE_ISSTATIC, true);
                animator.Play("Ocarine");
                if (!source.isPlaying && ocarinaMusic == false)
                {
                    source.clip = ocarina;
                    source.Play();
                    ocarinaMusic = true;

                }
            }
        }
        
        if(animator.GetBool(STATE_ALIVE) == true)
        {
            if (rigiBody.velocity.x != 0 && IsTouchingTheGruound())
            {
                animator.SetBool(STATE_ISSTATIC, false);
                animator.SetBool(STATE_ISWALKING, true);

                animator.Play("Walk");

            }

            if (rigiBody.velocity.y > 0 && !IsTouchingTheGruound())
            {
                animator.SetBool(STATE_ISSTATIC, false);
                animator.SetBool(STATE_ISWALKING, false);
                animator.SetBool(STATE_ISJUMPING, true);
                animator.Play("Jump");
            }

            if (rigiBody.velocity.x != 0 || rigiBody.velocity.y != 0)
            {
                source.Stop();
                ocarinaMusic = false;
            }
        }
        
         

        if(Input.GetButtonDown("Jump"))
            if (GameManager.sharedInstance.currentGameState == GameState.inGame)
            {
                Jump(false);
        }

        if (Input.GetButtonDown("SuperJump"))
            {
                Jump(true);
            }
          

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGruound());
        




        Debug.DrawRay(this.transform.position, Vector2.down * 1.5f, Color.red);
    }

    void FixedUpdate()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
       
            {
                rigiBody.velocity = new Vector2(Input.GetAxis("Horizontal") * runningSpeed, rigiBody.velocity.y);

                if (Input.GetAxis("Horizontal") < 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }

                if (Input.GetAxis("Horizontal") > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }

            }
    }


    void Jump(bool superjump)
    {
        float jumpForceFactor = jumpForce;
        if (superjump && manaPoints >= SUPERJUMP_COST)
        {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }
            if (IsTouchingTheGruound())
        {
         
        rigiBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);

        }
    }

    //Saber si el personaje esta tocando el Suelo
    bool IsTouchingTheGruound()
    {
        
            if (Physics2D.Raycast(this.transform.position, Vector2.down, 0.3f, groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    bool IsJumping()
    {
       
            if (rigiBody.velocity.y > 0 && !IsTouchingTheGruound())
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void Die()
    {
        float travelldDistance = GetTravelledDistance();
        float previousMaxDistances = PlayerPrefs.GetFloat("maxscore", 0f);
        if(travelldDistance > previousMaxDistances)
        {
            PlayerPrefs.SetFloat("maxscore", travelldDistance);
        }


        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        if(this.healthPoints >= MAX_HEALTH)
        {
            this.healthPoints = MAX_HEALTH;
        }

        if(this.healthPoints <= 0)
        {
            Die();
        }

    }
    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if(this.manaPoints >= MAX_MANA)
        {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return healthPoints;
    }

    public int GetMana()
    {
        return manaPoints;
    }

    public float GetTravelledDistance()
    {
        return this.transform.position.x - startPosition.x;
    }


}



