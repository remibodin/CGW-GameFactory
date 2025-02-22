using Unity.Mathematics;
using UnityEngine;

using Cgw.Gameplay;

public class PlayerSpawner : Cgw.SingleBehaviour<PlayerSpawner>
{
    public GameObject PlayerPrefab;
    public GameObject AragnaPrefab;
    public bool SpawnOnStart = true;
    public bool SpawnAragna = true;

    private bool m_SpawnRequested = false;
    private float m_SpawnDelay = 0.0f;

    protected void Start()
    {
        if (SpawnOnStart)
        {
            DoSpawn();
        }
    }

    private void DoSpawn()
    {
        Instantiate(PlayerPrefab, transform.position, Quaternion.identity);
        if (SpawnAragna)
        {
            Instantiate(AragnaPrefab, transform.position, Quaternion.identity);
        }
    }

    public void Respawn()
    {
        Player.Instance.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        SpiderController.Instance.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }
}
