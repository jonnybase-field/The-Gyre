using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newAnim : MonoBehaviour
{
    [SerializeField] float displayTime;
    private Animator animator;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.Play("newUser", -1, float.NegativeInfinity);
        StartCoroutine(reverseAnim());
    }

    private IEnumerator reverseAnim()
    {
        yield return new WaitForSeconds(displayTime);
        animator.SetFloat("Direction", -1);
        animator.Play("newUser", -1, float.NegativeInfinity);
    }
}
