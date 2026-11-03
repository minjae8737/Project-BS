using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;
    private int pauseCount = 0;

    protected override void Awake()
    {
        base.Awake();

        playerActionMap = InputSystem.actions.FindActionMap(InputActionNames.PlayerMapName, throwIfNotFound: true);
        uiActionMap = InputSystem.actions.FindActionMap(InputActionNames.UIMapName, throwIfNotFound: true);
    }

    private void Start()
    {
        //FIXME
        InputAction testAction = InputSystem.actions.FindAction(InputActionNames.TestInteractName, throwIfNotFound: true);
        testAction.performed += Test;
        
        //FIXME 씬 추가되면 변경
        playerActionMap.Enable();
        uiActionMap.Disable();

        StageManager.Instance.Init();
    }

    public void RequestPause()
    {
        if (pauseCount == 0)
        {
            Time.timeScale = 0;
            playerActionMap.Disable();
            uiActionMap.Enable();
        }

        pauseCount++;
    }

    public void RequestResume()
    {
        pauseCount--;

        if (pauseCount < 0)
        {
            pauseCount = 0;
            Debug.LogError("[GameManager] 중복된 Resume 요청이 있습니다.", this);
        }
        else if (pauseCount == 0)
        {
            Time.timeScale = 1;
            playerActionMap.Enable();
            uiActionMap.Disable();
        }
    }

    private void Test(InputAction.CallbackContext context)
    {
        if (pauseCount == 0)
        {
            RequestPause();
        }
        else
        {
            RequestResume();
        }
    }
}