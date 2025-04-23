using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLife = 1f;  // How long before the bullet is destroyed
    public float speed = 1f;       // Movement speed

    private Vector3 spawnPoint;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > bulletLife)
        {
            Destroy(this.gameObject);
        }

        timer += Time.deltaTime;
        transform.position = Movement(timer);
    }

    private Vector3 Movement(float time)
    {
        // Move in the direction the bullet is facing
        return spawnPoint + transform.forward * time * speed;
    }
}
