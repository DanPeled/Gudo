using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    #region Vars
    public Animator anim;
    public HealthBarScript healthBar;
    public SpriteRenderer[] sprite;
    public GameObject spriteRend;
    public static int currentIndex = 1;
    public int index = 1;
    List<object> type_ = Types.instance.Get_(currentIndex);
    float horizontal;
    public float defaultRunSpeed = 15f;
    float vertical;
    TypesRun typesRun;
    GameObject hitObject;
    float moveLimiter = 0.7f;
    Rigidbody2D body;
    public bool playerActive = false;
    RaycastHit2D hit;
    public float runSpeed = 15f;
    public Texture2D[] cursors;
    public int health = 6;
    public float animationTime = 0;
    public Slot[] inventory;
    public enum ToolState
    {
        None,
        Mele,
        Range,
        Mining,
    }
    public enum PlayerState
    {
        Idle = 0,
        Walking = 1,
        Digging = 2,
        Mele = 3
    }
    public ToolState state = ToolState.None;
    public PlayerState playerState = PlayerState.Idle;
    #endregion
    void Start()
    {
        healthBar.SetMaxHealth(health);
        anim = sprite[index].gameObject.GetComponent<Animator>();
        for (int i = 0; i < sprite.Length; i++)
        {
            if (i != index)
            {
                sprite[i].gameObject.SetActive(false);
            }
            else
            {
                sprite[i].gameObject.SetActive(true);
            }
        }
        typesRun = GetComponent<TypesRun>();
        spriteRend = typesRun.typeSetup(index);
        body = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        inventory = this.GetComponent<Building>().inventory;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (Input.GetMouseButtonDown(0) && inventory[GetComponent<Building>().blockIndex].ItemName == "shovel")
        {
            hitObject = hit.collider.gameObject;
            state = ToolState.Mining;
        }
        anim.SetInteger("state", (int)playerState);
        animationTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        #region playerActive
        if (playerActive)
        {
            if (inventory[GetComponent<Building>().blockIndex].ItemName == "sword") playerState = PlayerState.Mele;
            if (inventory[GetComponent<Building>().blockIndex].ItemName == "shovel") playerState = PlayerState.Digging;
            #region Mele
            if (state == ToolState.Mele)
            {
                
            }
            #endregion
            #region Mining
            if (state == ToolState.Mining && inventory[GetComponent<Building>().blockIndex].ItemName == "shovel")
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject != hitObject)
                    {
                        animationTime = 0;
                    }
                    if (playerState != PlayerState.Walking &&
                     state != ToolState.None &&
                      inventory[GetComponent<Building>().blockIndex].ItemName == "shovel") playerState = PlayerState.Digging;
                    if (state == ToolState.None) playerState = PlayerState.Idle;
                    Block block = hit.collider.GetComponent<Block>();
                    Plant plant = hit.collider.GetComponent<Plant>();
                    if (block == null)
                    {
                        playerState = PlayerState.Idle;
                        state = ToolState.None;
                    }
                    if (block != null && Input.GetMouseButton(0) &&
                     playerState == PlayerState.Digging &&
                      inventory[GetComponent<Building>().blockIndex].ItemName == "shovel")
                    {
                        if (animationTime >= block.health)
                        {
                            block.health--;
                            animationTime = 0;
                        }

                        if (block.health <= 0)
                        {
                            Vector2 pos = block.transform.position;
                            GameObject replaceWith = Instantiate(block.replaceWith);
                            GameObject drop = Instantiate(block.drop);
                            replaceWith.transform.position = pos;
                            drop.transform.position = pos;
                            Destroy(block.gameObject);
                            Debug.Log(string.Format("Destroyed {0} at {1}",
                            block.gameObject.name,
                            block.transform.position));
                            state = ToolState.None;
                        }
                    }
                    else if (plant != null && Input.GetMouseButtonDown(0))
                    {
                        GameObject drop = Instantiate(plant.drop);
                        drop.transform.position = plant.transform.position;
                        Destroy(plant.gameObject);
                        Debug.Log(string.Format("Destroyed {0} at {1}", plant.gameObject.name, plant.transform.position));
                    }
                }
            }
            #endregion
            #region Movement

            currentIndex = index;
            // Gives a value between -1 and 1
            horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
            vertical = Input.GetAxisRaw("Vertical"); // -1 is down

            if ((horizontal != 0 || vertical != 0))
            {
                if (horizontal == -1)
                {
                    sprite[index].flipX = true;
                }
                if (horizontal == 1)
                {
                    sprite[index].flipX = false;
                }
                playerState = PlayerState.Walking;
            }

            else
            {
                if (horizontal == 0 && vertical == 0 && !Input.GetMouseButton(0))
                {
                    playerState = PlayerState.Idle;

                }
                else if (Input.GetMouseButton(0) &&
                    inventory[GetComponent<Building>().blockIndex].ItemName == "shovel")
                {
                    playerState = PlayerState.Digging;
                    anim.SetInteger("state", 2);
                }
            }
            #endregion
        }
        #endregion

    }

    void FixedUpdate()
    {
        if (playerActive)
        {
            if (horizontal != 0 && vertical != 0) // Check for diagonal movement

            {

                // limit movement speed diagonally, so you move at 70% speed
                horizontal *= moveLimiter;
                vertical *= moveLimiter;
            }

            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
            if (Input.GetKeyDown("space"))
            {
                type_ = Types.instance.Get_(index);
                Debug.Log(type_[0] + " " + type_[1]);
            }
        }
    }
    #region Water Walking speed change
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {

            this.runSpeed = 5f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            this.runSpeed = defaultRunSpeed;
        }
    }
    #endregion
    void TakeDamage(int damage)
    {
        this.health -= damage;
        healthBar.SetHealth(health);
    }

}