using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public GameObject spawnerPrefab;
    public float floorWidth = 3f;   // 3x3 floor
    public float floorDepth = 3f;
    public float offset = 2f;       // Distance outside the edge

    void Start()
    {
        Vector3[] positions = new Vector3[]
        {
            new Vector3(-floorWidth / 2 - offset, 0.5f, 0),     // Left
            new Vector3(floorWidth / 2 + offset, 0.5f, 0),      // Right
            new Vector3(0, 0.5f, floorDepth / 2 + offset),      // Top
            new Vector3(0, 0.5f, -floorDepth / 2 - offset)      // Bottom
        };

        foreach (Vector3 pos in positions)
        {
            GameObject spawner = Instantiate(spawnerPrefab, pos, Quaternion.identity);

            // Calculate the direction each spawner should face (towards the center of the floor)
            Vector3 direction = (Vector3.zero - pos).normalized;  // Direction from spawner to center

            // Calculate the Y-axis rotation only and keep the X and Z rotation the same
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // Only modify the Y-axis of the rotation (fix vertical firing direction)
            spawner.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

            // Ensure spawners' position is always at y = 0.5f
            spawner.transform.position = new Vector3(spawner.transform.position.x, 0.5f, spawner.transform.position.z);
        }
    }
}
