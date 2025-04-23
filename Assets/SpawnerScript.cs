using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    enum SpawnerType { Straight, Spin }

    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 1f;
    public float speed = 1f;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float firingRate = 1f;
    [SerializeField] private float moveSpeed = 20f; // Speed at which the spawner moves left to right
    [SerializeField] private float moveRange = 3f; // How far the spawner will move left and right
    private float moveTime = 1f;


    private GameObject spawnedBullet;
    private float timer = 0f;
    private float startPositionX; // Starting x position of the spawner

    // Start is called before the first frame update
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        moveTime += Time.deltaTime;

        if (spawnerType == SpawnerType.Spin)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 1f, transform.eulerAngles.z);
        }

        float offset = Mathf.PingPong(moveTime * moveSpeed, moveRange * 2) - moveRange;

        transform.position = transform.parent != null
            ? transform.parent.position + transform.right * offset
            : initialPosition + transform.right * offset;

        if (timer >= firingRate)
        {
            Fire();
            timer = 0;
        }
    }


    private void Fire()
    {
        if (bullet)
        {
            spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            spawnedBullet.GetComponent<Bullet>().speed = speed;
            spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
            spawnedBullet.transform.rotation = transform.rotation;
        }
    }
}
