using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private Image swordIcon;
    private Image bootsIcon;
    private Image gunIcon;
    private Image dashIcon;

    private Image swordButton;
    private Image bootsButton;
    private Image gunButton;
    private Image dashButton;

    [SerializeField] Player player;

    [Header("Item Sprites")]
    [SerializeField] private Sprite swordFull;
    [SerializeField] private Sprite bootsFull;
    [SerializeField] private Sprite gunFull;
    [SerializeField] private Sprite dashFull;

    // Start is called before the first frame update
    void Start()
    {
        swordIcon = transform.Find("Sword").GetComponent<Image>();
        bootsIcon = transform.Find("Boots").GetComponent<Image>();
        gunIcon = transform.Find("Gun").GetComponent<Image>();
        dashIcon = transform.Find("Dash").GetComponent<Image>();

        swordButton = transform.Find("Sword-button").GetComponent<Image>();
        bootsButton = transform.Find("Boots-button").GetComponent<Image>();
        gunButton = transform.Find("Gun-button").GetComponent<Image>();
        dashButton = transform.Find("Dash-button").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //Optimize these later so they only run when the player obtains an ability

        if (player.getHasSword())
        {
            swordIcon.sprite = swordFull;
            swordButton.enabled = true;
        }
        if (player.getHasBoots())
        {
            bootsIcon.sprite = bootsFull;
            bootsButton.enabled = true;
        }
        if (player.getHasGun())
        {
            gunIcon.sprite = gunFull;
            gunButton.enabled = true;
        }
        if (player.getHasDash())
        {
            dashIcon.sprite = dashFull;
            dashButton.enabled = true;
        }
    }
}
