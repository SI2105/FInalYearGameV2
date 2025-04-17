using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseFloor : MonoBehaviour
{
    [Tooltip("False Floor Duration")]
    public float disappearDuration = 2f;

    Collider2D col;
    SpriteRenderer[] sr;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        //sr = GetComponentsInChildren<SpriteRenderer>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && collision.transform.GetComponent<PlayerController>().isGrounded())
        {
            StartCoroutine(DisappearRoutine());
        }
        
    }

    IEnumerator DisappearRoutine()
    {
        col.enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(disappearDuration);
        col.enabled = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
