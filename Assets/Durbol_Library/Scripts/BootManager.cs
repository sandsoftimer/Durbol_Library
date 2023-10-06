using System.Collections;

#if HOMA_SDK_INSTALLED
using HomaGames.HomaBelly;
#endif

public class BootManager : DurbolManager
{
    public override void Awake()
    {
        base.Awake();
        ChangeGameState(GameState.GAME_DATA_LOADED);
    }

    public override void GameDataLoad()
    {
        base.GameDataLoad();

#if HOMA_SDK_INSTALLED

        if(!HomaBelly.Instance.IsInitialized)
            Events.onInitialized += OnSupersonicWisdomReady;
#else
        OnSupersonicWisdomReady();
#endif

    }

    private void OnDisable()
    {
#if HOMA_SDK_INSTALLED
        Events.onInitialized -= OnSupersonicWisdomReady;
#endif
    }

    void OnSupersonicWisdomReady()
    {
        // Start your game from this point
        KolpoTools.functionManager.ExecuteAfterSecond(0.5f, () => {

            KolpoTools.sceneManager.LoadLevel(GetValidLevelIndex());
        });
    }
}
