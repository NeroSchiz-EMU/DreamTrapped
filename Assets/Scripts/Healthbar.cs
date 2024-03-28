using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] private Image healthBarFill;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBarFill.fillAmount = player.getHealth() / player.getMaxHealth();
        //Optimize this later so it only runs when player is damaged
    }
}
