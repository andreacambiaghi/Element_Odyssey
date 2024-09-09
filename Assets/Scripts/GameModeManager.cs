public class GameModeManager 
{
    private static GameModeManager instance;

    private string gameMode;

    public string GameMode
    {
        get { return gameMode; }
        set { gameMode = value; }
    }

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

}
