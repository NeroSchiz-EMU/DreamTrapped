using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRandomize : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator.Play(0, -1, Random.value);
    }

}
