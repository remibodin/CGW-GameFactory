using Unity.Mathematics;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private static PlayerSpawner s_Instance;
    public static PlayerSpawner Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = new PlayerSpawner();
            }
            return s_Instance;
        }
    }

    public GameObject PlayerPrefab;
    public GameObject AragnaPrefab;
    public bool SpawnOnStart = true;

    private bool m_SpawnRequested = false;
    private float m_SpawnDelay = 0.0f;

    protected void Start()
    {
        if (SpawnOnStart)
        {
            DoSpawn();
        }
    }

    private void Update()
    {
        m_SpawnDelay -= math.clamp(Time.deltaTime, 0.0f, 1.0f);

        if (m_SpawnRequested && m_SpawnDelay <= 0.0f)
        {
            DoSpawn();

            m_SpawnRequested = false;
        }
    }

    private void DoSpawn()
    {
        Instantiate(PlayerPrefab, transform.position, Quaternion.identity);
        Instantiate(AragnaPrefab, transform.position, Quaternion.identity);
    }

    public void RequestSpawn(float delay = 0.0f)
    {
        m_SpawnDelay = delay;
        m_SpawnRequested = true;
    }
}
