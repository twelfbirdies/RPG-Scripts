using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.core {

public class PersistantObjectSpawner : MonoBehaviour
{
        static bool hasSpawned = false;
        [SerializeField] GameObject persistantObjectPrefab; 

        private void Awake()
        {
            if (hasSpawned) return;
            SpawnPeristantObject();
            hasSpawned = true;
        }

        private void SpawnPeristantObject()
        {
            GameObject persistantObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(persistantObject);
        }
    }
}