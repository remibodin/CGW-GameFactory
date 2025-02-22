using Unity.Mathematics;
using UnityEngine;

using Cgw.Gameplay;
using Cgw;

public class PlayerSpawner : Cgw.SingleBehaviourInScene<PlayerSpawner>
{
    public GameObject PlayerPrefab;
    public GameObject AragnaPrefab;
    public bool SpawnPlayerOnStart = true;
    public bool SpawnAragnaAragna = true;

    private bool m_SpawnRequested = false;
    private float m_SpawnDelay = 0.0f;

    protected void Start()
    {
        if (SpawnPlayerOnStart)
        {
            DoSpawn();
        }
    }

    private void DoSpawn()
    {
        SpawnPlayer();
        if (SpawnAragnaAragna)
        {
            SpawnAragna();
        }
    }

    public void SpawnPlayer()
    {
        if (Player.Instance == null)
        {
            Instantiate(PlayerPrefab, transform.position, Quaternion.identity);
        }
    }

    public void SpawnAragna()
    {
        if (SpiderController.Instance == null)
        {
            Instantiate(AragnaPrefab, Player.Instance.transform.position, Quaternion.identity);
        }
    }

    public void Respawn()
    {
        Player.Instance.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        SpiderController.Instance.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

    [TermCommand]
    private static void Aragna(string p_args)
    {
        if (Instance == null)
        {
            return;
        }

        Instance.SpawnAragna();
    }
}
