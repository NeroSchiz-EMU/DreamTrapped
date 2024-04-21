using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Renderer bgRenderer;

    [SerializeField] private EndDoor endDoor;

    // Update is called once per frame
    void Update()
    {
        if (endDoor.getAbilityAmount() == 0) speed = 0.0005f;
        if (endDoor.getAbilityAmount() == 1) speed = 0.0007f;
        if (endDoor.getAbilityAmount() == 2) speed = 0.0010f;
        if (endDoor.getAbilityAmount() == 3) speed = 0.0014f;
        if (endDoor.getAbilityAmount() == 4) speed = 0.0020f;

        bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
