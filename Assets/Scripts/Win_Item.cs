using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win_Item : MonoBehaviour
{
    SpriteRenderer sr;
    float startTime;
    [SerializeField] float speed = 1.0f;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startTime = Time.time;
    }

    private void Update()
    {
        float t = (Mathf.Sin(Time.time - startTime) * speed);
        sr.material.color = Color.Lerp(Color.white, Color.black, t);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            SceneManager.LoadSceneAsync(1);
        }
    }
}
