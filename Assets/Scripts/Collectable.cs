using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum CollectableType
{
    healPotion, manaPotion, rupia
}


public class Collectable : MonoBehaviour
{
    public CollectableType type = CollectableType.rupia;

    private SpriteRenderer sprite;
    private BoxCollider2D itemCollider;

    //bool hasBeenCollected = false;
    public int valueGreen = 1;
    public int valueBlue = 3;
    public int valueRed = 5;
    public int valueHealth = 100;
    public int valueMana = 15;


    GameObject player;

    public static Collectable sharedInstance;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<BoxCollider2D>();

        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    private void Start()
    {
        player = GameObject.Find("Link");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (this.name.Contains("Rupia"))
            {
                type = CollectableType.rupia;
            }
            Collect();

        }
    }

    void Collect()
    {
        //hasBeenCollected = true;

        switch (this.type)
        {
            case CollectableType.rupia:
                if (this.name.Contains("RupiaGreen"))
                {
                    GameManager.sharedInstance.collectedObjectGreen += valueGreen;
                    Destroi();
                    
                }
                if (this.name.Contains("RupiaBlue"))
                {
                    GameManager.sharedInstance.collectedObjectBlue += valueBlue;
                    Destroi();
                    
                }
                if (this.name.Contains("RupiaRed"))
                {
                    GameManager.sharedInstance.collectedObjectRed += valueRed;
                    Destroi();
                 
                }
                break;
            case CollectableType.healPotion:
                if (this.name.Contains("Health"))
                {
                    GameManager.sharedInstance.collectedObjectHealth += valueHealth;
                    Link.sharedInstance.CollectHealth(valueHealth);
                    Destroi();
                }

                break;

            case CollectableType.manaPotion:
                if (this.name.Contains("Mana"))
                {
                    GameManager.sharedInstance.collectedObjectMana += valueMana;
                    Link.sharedInstance.CollectMana(valueMana);
                    Destroi();
                }
                break;
        }
    }

   

    void Destroi()
    {
        Destroy(this.gameObject);
    }

}
