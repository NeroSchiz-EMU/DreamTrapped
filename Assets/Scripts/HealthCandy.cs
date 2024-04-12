using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCandy : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Collect());
        }
    }

    private IEnumerator Collect()
    {
        animator.SetBool("collected", true);
        player.changeHealth(-20);
        //audioSource.transform.parent = null; //Remove parent, so that audio can persist after this object is destroyed
        //audioSource.PlayOneShot(collect);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
