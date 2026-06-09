using UnityEngine;

public class RisingWater : Obstacle
{
    public static RisingWater Instance { get; private set; }

    [Header("Rising Settings")]
    [SerializeField] private float riseSpeed = 0.5f;
    [SerializeField] private float startY = -10f;
    [SerializeField] private float maxY = 10f;

    private enum WaterState { Idle, Rising, Stopped }
    private WaterState currentState = WaterState.Idle;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.position = new Vector3(0f, startY, 0f);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (currentState == WaterState.Idle)
        {
            StartRising();
        }
    }

    private void Update()
    {
        if (currentState == WaterState.Rising)
        {
            Vector3 pos = transform.position;
            pos.y += riseSpeed * Time.deltaTime;

            if (pos.y >= maxY)
            {
                pos.y = maxY;
                currentState = WaterState.Stopped;
            }

            transform.position = pos;
        }
    }

    public void StartRising()
    {
        if (currentState == WaterState.Idle)
        {
            currentState = WaterState.Rising;
            Debug.Log("Air mulai naik");
        }
    }

    public override void OnHitPlayer(Player player)
    {
        GameManager.Instance.LevelFailed();
    }
}