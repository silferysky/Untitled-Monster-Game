using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameState
{
    Running = 1,
    ChipMenu = 2,
    Pause = 0,
    Exit = -1
}

static class GameStateManager
{
    public static GameState gameState;

    public static void Start()
    {
        gameState = GameState.Running;
    }

    public static void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (gameState)
            {
                case GameState.Pause:
                    gameState = GameState.Running;
                    Console.WriteLine("UNPAUSED");
                    break;
                case GameState.Running:
                    gameState = GameState.Pause;
                    Console.WriteLine("PAUSED");
                    break;
                default:
                    break;
            }
        }
    }
}
