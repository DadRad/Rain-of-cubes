using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private Renderer _cubeRenderer;
    private bool _isFirstCollision = true;
    public event Action<Cube> TimerExpired;

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

    private void SetRandomColor()
    {
        _cubeRenderer.material.color = UnityEngine.Random.ColorHSV();
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
        if (_isFirstCollision && collision.collider.CompareTag("Platform"))
        {
            SetRandomColor();
            StartCoroutine(StartTimer());
        }
    }
}
