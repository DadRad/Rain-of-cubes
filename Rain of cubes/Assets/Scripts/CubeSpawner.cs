using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private float _repeatRate;
    [SerializeField] private int _poolSize;
    [SerializeField] private int _maxPoolSize;
    [SerializeField] private Vector3 spawnAreaCenter;
    [SerializeField] private Vector3 spawnAreaSize;
    private float _halfAreaScale = 0.5f;

    private ObjectPool<Cube> _cubePool;
    public event System.Action<Cube> CubeSpawned;

    private void Awake()
    {
        _cubePool = new ObjectPool<Cube>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(_cube.gameObject);
                return obj.GetComponent<Cube>();
            },
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolSize,
            maxSize: _maxPoolSize
        );
    }

    private void ActionOnGet(Cube cube)
    {
        Vector3 randomPosition = new Vector3(
        Random.Range(-spawnAreaSize.x * _halfAreaScale, spawnAreaSize.x * _halfAreaScale),
        Random.Range(-spawnAreaSize.y * _halfAreaScale, spawnAreaSize.y * _halfAreaScale),
        Random.Range(-spawnAreaSize.z * _halfAreaScale, spawnAreaSize.z * _halfAreaScale)
        );


        cube.transform.position = spawnAreaCenter + randomPosition;
        cube.transform.rotation = Quaternion.identity;

        SetDefaultPhysicsSettings(cube);
        cube.ResetColor();
        cube.ResetCollisionState();
        cube.gameObject.SetActive(true);

        cube.TimerExpired += HandleTimerExpired;
         CubeSpawned?.Invoke(cube);
    }

    private void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    private void HandleTimerExpired(Cube cube)
    {
        _cubePool.Release(cube);
    }

    private IEnumerator SpawnCubes()
    {
        var WaitForSeconds = new WaitForSeconds(_repeatRate);

        while (enabled)
        {
            GetCube();
            yield return WaitForSeconds;
        }
    }

    private void SetDefaultPhysicsSettings(Cube cube)
    {
        Rigidbody rigidBody = cube.GetComponent<Rigidbody>();

        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }

    private void GetCube()
    {
        Cube cube = _cubePool.Get();
    }
}
