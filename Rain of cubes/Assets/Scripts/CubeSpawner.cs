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

    private ObjectPool<Cube> _cubePool;

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
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        cube.transform.position = spawnAreaCenter + randomPosition;
        cube.transform.rotation = Quaternion.identity;

        SetDefaultPhysicsSettings(cube);
        cube.ResetColor();
        cube.ResetCollisionState();
        cube.gameObject.SetActive(true);

        cube.TimerExpired += HandleTimerExpired;
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
        while (true)
        {
            GetCube();
            yield return new WaitForSeconds(_repeatRate);
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
