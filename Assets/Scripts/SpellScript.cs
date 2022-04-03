using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private float speed = 10f;
    private int spellDamage = 20;
    public bool isFacingRight = true;
    private bool isActive;

    private Animator spellAnim = null;

    // Start is called before the first frame update
    void Start()
    {
        spellAnim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        isActive = true;

        StartCoroutine(DestroySpell(3));
    }

    // Update is called once per frame
    void Update()
    {
        if(isFacingRight && isActive)
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (!isFacingRight && isActive)
            transform.Translate(-Vector3.right * speed * Time.deltaTime);
    }


    IEnumerator DestroySpell(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        isActive = false;
        spellAnim.SetTrigger("isDestroyed");

        yield return new WaitForSeconds(0.3f);

        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.OnUpdateHeatlh(-spellDamage);
            StartCoroutine(DestroySpell(0));
        }
    }
}
