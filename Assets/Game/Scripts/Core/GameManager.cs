public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        UnitManager.Instance.Init();
        StageManager.Instance.Init();
    }
}