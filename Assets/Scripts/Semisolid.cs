using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semisolid : MonoBehaviour
{
    private GameObject currentSemisolid;

    [SerializeField] private CapsuleCollider2D playerCollider;
    
    void Update()
    {
        if (Input.GetButton("Jump") || Input.GetKeyDown(KeyCode.S))
        {
            if (currentSemisolid != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Semisolid"))
        {
            currentSemisolid = collision.gameObject;
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Semisolid"))
        {
            currentSemisolid = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        CapsuleCollider2D platformCollider = currentSemisolid.GetComponent<CapsuleCollider2D>();
        
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
