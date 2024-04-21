using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowBat : Entity
{
    private RaycastHit2D isPlayerDetected;
    
    void Start()
    {
        base.Start();
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        isPlayerDetected = Physics2D.Raycast(playerCheck.position,
            Vector2.right, playerCheckDistance * facingDir, whatIsPlayer);
    }
}
