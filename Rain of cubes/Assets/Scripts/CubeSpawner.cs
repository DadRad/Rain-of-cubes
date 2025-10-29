using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _poolSize;
    [SerializeField] private int _maxPoolSize;
    [SerializeField] private Vector3 spawnAreaCenter;
    [SerializeField] private Vector3 spawnAreaSize;

    private ObjectPool<GameObject> _cubePool;

    private void Awake()
    {
        _cubePool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_cubePrefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolSize,
            maxSize: _maxPoolSize
            );
    }

    private void ActionOnGet(GameObject obj)
    {
        Vector3 randomPosition = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

        obj.transform.position = spawnAreaCenter + randomPosition;
        obj.transform.rotation = Quaternion.identity;
        Rigidbody rigidBody = obj.GetComponent<Rigidbody>();

        if (rigidBody != null)
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
        }

        Cube cubeComponent = obj.GetComponent<Cube>();

        if (cubeComponent != null)
        {
            cubeComponent.ResetColor();
            cubeComponent.ResetCollisionState();
            cubeComponent.SetPool(_cubePool);
        }

        obj.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        _cubePool.Get();
    }
}
