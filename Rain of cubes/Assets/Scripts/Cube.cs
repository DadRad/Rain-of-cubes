using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    [SerializeField] private LayerMask _platformLayer;

    private Renderer _cubeRenderer;
    private bool _isFirstCollision = true;
    public event Action<Cube> TimerExpired;
    public event Action<Cube> ColorChanged;
    public Renderer CubeRenderer { get => _cubeRenderer; }
    public void ResetColor()
    {
        _cubeRenderer.material.color = Color.white;
    }

    public void ResetCollisionState()
    {
        _isFirstCollision = true;
    }

    private void Awake()
    {
        _cubeRenderer = GetComponent<Renderer>();
        _cubeRenderer.material = new Material(_cubeRenderer.sharedMaterial);
    }

    public IEnumerator StartTimer()
    {
        float minRandomRange = 2f;
        float maxRandomRange = 5f;
        float delay = UnityEngine.Random.Range(minRandomRange, maxRandomRange);
        yield return new WaitForSeconds(delay);
        TimerExpired?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        int layer = collision.collider.gameObject.layer;
        
        if (_isFirstCollision && ((1 << layer) & _platformLayer.value) != 0)
        {
            ColorChanged?.Invoke(this);
            StartCoroutine(StartTimer());
            _isFirstCollision = false;
        }
    }
}
