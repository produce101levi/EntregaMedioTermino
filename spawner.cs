using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class Spawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    private Coroutine currentRoutine;
    private int bulletCount = 0;
    public TextMeshProUGUI bulletCounterText;

    private bool moveRight = true;

    private int roundCounter = 0;
    private int maxRounds = 3;

    private bool isOver = false;

    void Start() 
    {
        UpdateBulletCounter();
        StartCoroutine(ChangePatternRoutine());
    }

    IEnumerator ShootBulletsCircular(){
        float angle = 0;
        bulletSpeed = 2;
        while(true){
            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            angle += 10;
            float radian = angle * Mathf.Deg2Rad;
            rb.velocity = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * bulletSpeed;
            yield return new WaitForSeconds(0.1f);
            bulletCount++;

            UpdateBulletCounter();
        }
    }

    IEnumerator ShootBulletsSinusoidal(){
        float time = 0f;
        bulletCount = 0;
        float changeDir = 2f;
        float timeSinceChange = 0f;

        while (true){
            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bulletCount++;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();


            if(timeSinceChange >= changeDir){
                moveRight = !moveRight;
                timeSinceChange = 0f;
            }

            float dir;

            if (moveRight){
                dir = bulletSpeed;
            }
            else{
                dir = -bulletSpeed;
            }

            rb.velocity = new Vector2(dir, Mathf.Sin(time * bulletSpeed) * 2);
            time += 0.1f;
            timeSinceChange += 0.1f;

            yield return new WaitForSeconds(0.1f);
            UpdateBulletCounter();

        }
    }

    IEnumerator ShootBulletsSpiral(){
            float angle = 0;
            bulletSpeed = 3;
            bulletCount = 0;
            while(true)
            {
                var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bulletCount++;
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                angle += 10f; 
                float radian = angle * Mathf.Deg2Rad;
                rb.velocity = new Vector2(Mathf.Cos(radian) * angle, Mathf.Sin(radian) * angle) * 0.05f;
                yield return new WaitForSeconds(0.5f);
                UpdateBulletCounter();
            }
        }

    IEnumerator ChangePatternRoutine()
    {
        while (roundCounter < maxRounds)
        {
            // Patrón #1
            currentRoutine = StartCoroutine(ShootBulletsCircular());
            roundCounter++;
            yield return new WaitForSeconds(10);

            // Patron #2
            currentRoutine = StartCoroutine(ShootBulletsSinusoidal());
            roundCounter++;
            yield return new WaitForSeconds(10);

            // Patrón #3
            currentRoutine = StartCoroutine(ShootBulletsSpiral());
            roundCounter++;
            yield return new WaitForSeconds(10);
        }
        
        isOver = true;
        gameObject.SetActive(false);
    }

    void UpdateBulletCounter()
    {
        if (bulletCounterText != null)
        {
            bulletCounterText.text = "Balas: " + bulletCount;
        }

        if (isOver)
        {
            bulletCounterText.text = "Juego terminado.";
        }
    }
}