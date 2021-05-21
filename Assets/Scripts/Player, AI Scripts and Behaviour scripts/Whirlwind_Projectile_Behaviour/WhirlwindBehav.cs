using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlwindBehav : MonoBehaviour
{
///////////////////////////////
REWORK! REWORK! REWORK! REWORK!
///////////////////////////////

    [Header("Movement speed, and attributes")]
    public float pSpeed;
    Rigidbody2D pBody;

    void Start()
    {
        pBody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3.5f); 
    }
    void Update()
    {
        pBody.velocity = Vector2.right * pSpeed * Time.deltaTime;
        StartCoroutine(ScaleTransformer());
    }
    #region Delay shit
    IEnumerator ScaleTransformer()
    {
        transform.localScale = new Vector3(transform.localScale.x - 0.005f, transform.localScale.y - 0.005f, transform.localScale.z);
        yield return new WaitForSeconds(0.3f);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}