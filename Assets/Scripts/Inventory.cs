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
    }

    // Update is called once per frame
    void Update()
    {
        //Optimize these later so they only run when the player obtains an ability

        if (player.getHasSword())
        {
            swordIcon.sprite = swordFull;
        }
        if (player.getHasBoots())
        {
            bootsIcon.sprite = bootsFull;
        }
        if (player.getHasGun())
        {
            gunIcon.sprite = gunFull;
        }
        if (player.getHasDash())
        {
            dashIcon.sprite = dashFull;
        }
    }
}
