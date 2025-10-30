using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private void OnEnable()
    {
        // Подписываемся на событие, когда куб берется из пула
        _cubeSpawner.CubeSpawned += OnCubeSpawned;
    }

    private void OnDisable()
    {
        _cubeSpawner.CubeSpawned -= OnCubeSpawned;
    }

    private void OnCubeSpawned(Cube cube)
    {
        // Подписываемся на событие ColorChanged у конкретного куба
        cube.ColorChanged += SetRandomColor;
    }

    public void SetRandomColor(Cube cube)
    {
        cube.CubeRenderer.material.color = UnityEngine.Random.ColorHSV();
    }
}