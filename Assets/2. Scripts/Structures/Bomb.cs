using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    
    [SerializeField] private GameObject _explosionVFX;

    private bool _isExploded = false;
    
    private void OnEnable()
    {
        _isExploded = false;
    }

    
    private void OnCollisionEnter(Collision other)
    {

        if (_isExploded) return;
        
        _isExploded = true;
        
        GameObject effectObject = Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        
        //Destroy(gameObject);
        BulletFactory.Instance.ReturnBullet(this);
    }
}
