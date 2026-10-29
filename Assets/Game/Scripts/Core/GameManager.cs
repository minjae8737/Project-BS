public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        StageManager.Instance.Init();
    }
}