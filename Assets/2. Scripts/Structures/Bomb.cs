using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    
    [SerializeField] private GameObject _explosionVFX;

    private void OnCollisionEnter(Collision other)
    {

        GameObject effectObject = Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
        
    }
}
