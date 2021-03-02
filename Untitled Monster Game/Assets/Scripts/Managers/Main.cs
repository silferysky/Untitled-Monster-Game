using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Main : MonoBehaviour
{
    void Start()
    {
        GameStateManager.Start();
    }

    void Update()
    {
        GameStateManager.Update();
    }
}
