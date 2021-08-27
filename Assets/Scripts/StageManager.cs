using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateType
{
    None,
    Play,
    Menu,
}
public class StageManager : Singleton<StageManager>
{
    [SerializeField] GameStateType gameState = GameStateType.Play;
    public static GameStateType GameState
    {
        get => Instance.gameState;
        set
        {
            if (Instance == null)
                return;

            if (Instance.gameState == value)
                return;

            if (value == GameStateType.Menu)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
            else
                Time.timeScale = 1;

            Debug.Log($"{Instance.gameState} -> {value}, TimeScale : {Time.timeScale}");
            Instance.gameState = value;
        }
    }

    void Awake()
    {
        GameState = GameStateType.Play;
    }
}
