// using UnityEngine;

public class GameModeManager
{
    private static GameModeManager instance;

    private string gameMode;

    public string GameMode
    {
        get { return gameMode; }
        set { gameMode = value; }
    }

    public bool IsMenuOpen { get; set; }

    private GameModeManager() { }

    public static GameModeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameModeManager();
            }
            return instance;
        }
    }

    public void ChangeMenuState()
    {
        // Toggle boolean state with NOT operator
        IsMenuOpen = !IsMenuOpen;
        // Debug.LogError("Menu state changed to: " + IsMenuOpen);
    }

}
