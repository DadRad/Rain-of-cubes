using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private Renderer _cubeRenderer;
    private ObjectPool<GameObject> _cubePool;
    private bool _isFirstCollision = true;

    private void Awake()
    {
        _cubeRenderer = GetComponent<Renderer>();
        _cubeRenderer.material = new Material(_cubeRenderer.sharedMaterial);
    }

    private void SetRandomColor()
    {
        _cubeRenderer.material.color = Random.ColorHSV();
    }

    public void SetPool(ObjectPool<GameObject> pool)
    {
        _cubePool = pool;
    }

    private IEnumerator StartTimer()
    {
        float delay = Random.Range(2f, 5f);
        yield return new WaitForSeconds(delay);

        _cubePool.Release(gameObject);
    }

    public void ResetColor()
    {
        _cubeRenderer.material.color = Color.white;
    }

    public void ResetCollisionState()
    {
        _isFirstCollision = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isFirstCollision)
        {
            SetRandomColor();
            StartCoroutine(StartTimer());
        }
    }
}
