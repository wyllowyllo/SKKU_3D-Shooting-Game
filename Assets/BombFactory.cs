using System;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;

public class BombFactory : MonoBehaviour
{
    [SerializeField] private GameObject _bombPrefab;
    
    private void Awake()
    {
        Init();
    }

    

    private void Init()
    {
       
    }
}
