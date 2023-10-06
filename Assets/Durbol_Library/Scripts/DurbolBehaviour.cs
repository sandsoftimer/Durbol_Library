using UnityEngine;
using Com.Durbol.Utility;
using System;
using System.Collections.Generic;

[DefaultExecutionOrder(ConstantManager.KolpoBehaviourOrder)]
public class DurbolBehaviour : MonoBehaviour, IHierarchyIcon
{
    [HideInInspector]
    public DurbolManager gameManager;
    [HideInInspector]
    public DurbolTools DurbolTools;

    [HideInInspector]
    public GameplayData gameplayData;
    [HideInInspector]
    public GameState gameState;

    bool registeredForInput;

    public virtual void Awake()
    {
        DurbolTools = DurbolTools.Instance;
    }

    public virtual void Start()
    {
    }

    public virtual void OnEnable()
    {
        DurbolManager.OnSelfDistributionAction += OnSelfDistributionAction;
        DurbolManager.OnNone += OnNone;
        DurbolManager.OnGameDataLoad += OnGameDataLoad;
        DurbolManager.OnGameInitialize += OnGameInitializing;
        DurbolManager.OnGameStart += OnGameStart;
        DurbolManager.OnGameOver += OnGameOver;
        DurbolManager.OnPauseGamePlay += OnPauseGamePlay;
        DurbolManager.OnUnPauseGamePlay += OnUnPauseGamePlay;
        DurbolManager.OnChangeGameState += OnChangeGameState;
        DurbolManager.OnCompleteTask += OnCompleteTask;
        DurbolManager.OnIncompleteTask += OnIncompleteTask;

        if (registeredForInput)
        {
            DurbolManager.OnTap += ProccessInputTapping;
            DurbolManager.OnDrag += OnDrag;
            DurbolManager.OnSwip += ProcessInputSwipping;
            DurbolManager.OnZoom += ProcessInputZooming;
        }
        DurbolManager.OnAddKolpoBehaviour?.Invoke();

        #region GAME SPECIFIC SPACE


        #endregion GAME SPECIFIC SPACE
    }

    public virtual void OnDisable()
    {
        DurbolManager.OnSelfDistributionAction -= OnSelfDistributionAction;
        DurbolManager.OnNone -= OnNone;
        DurbolManager.OnGameDataLoad -= OnGameDataLoad;
        DurbolManager.OnGameInitialize -= OnGameInitializing;
        DurbolManager.OnGameStart -= OnGameStart;
        DurbolManager.OnGameOver -= OnGameOver;
        DurbolManager.OnPauseGamePlay -= OnPauseGamePlay;
        DurbolManager.OnUnPauseGamePlay -= OnUnPauseGamePlay;
        DurbolManager.OnChangeGameState -= OnChangeGameState;
        DurbolManager.OnCompleteTask -= OnCompleteTask;
        DurbolManager.OnIncompleteTask -= OnIncompleteTask;

        if (registeredForInput)
        {
            DurbolManager.OnTap -= ProccessInputTapping;
            DurbolManager.OnDrag -= OnDrag;
            DurbolManager.OnSwip -= ProcessInputSwipping;
            DurbolManager.OnZoom -= ProcessInputZooming;
        }

        #region GAME SPECIFIC SPACE


        #endregion GAME SPECIFIC SPACE
    }

    public void Registar_For_Input_Callback()
    {
        registeredForInput = true;
    }

    void ProccessInputTapping(DL_TappingType inputType, Vector3 tapOnWorldSpace)
    {
        switch (inputType)
        {
            case DL_TappingType.NONE:
                break;
            case DL_TappingType.TAP_START:
                OnTapStart(tapOnWorldSpace);
                break;
            case DL_TappingType.TAP_END:
                OnTapEnd(tapOnWorldSpace);
                break;
            case DL_TappingType.SINGLE_TAP:
                OnSingleTap(tapOnWorldSpace);
                break;
            case DL_TappingType.TAP_N_HOLD:
                OnTapAndHold(tapOnWorldSpace);
                break;
        }
    }

    public virtual void OnTapStart(Vector3 tapOnWorldSpace)
    {
    }

    public virtual void OnTapAndHold(Vector3 tapOnWorldSpace)
    {
    }

    public virtual void OnTapEnd(Vector3 tapOnWorldSpace)
    {
    }

    public virtual void OnSingleTap(Vector3 tapOnWorldSpace)
    {
    }

    public virtual void OnDrag(Vector3 dragAmount)
    {
    }


    void ProcessInputSwipping(DL_SwippingType swippingType)
    {
        switch (swippingType)
        {
            case DL_SwippingType.SWIPE_UP:
                OnSwipeUp();
                break;
            case DL_SwippingType.SWIPE_DOWN:
                OnSwipeDown();
                break;
            case DL_SwippingType.SWIPE_LEFT:
                OnSwipeLeft();
                break;
            case DL_SwippingType.SWIPE_RIGHT:
                OnSwipeRight();
                break;
        }
    }

    public virtual void OnSwipeUp()
    {
    }

    public virtual void OnSwipeDown()
    {
    }

    public virtual void OnSwipeLeft()
    {
    }

    public virtual void OnSwipeRight()
    {
    }

    void ProcessInputZooming(float zoom)
    {
        OnZoom(zoom);
    }

    public virtual void OnZoom(float zoomDelta)
    {
    }


    public virtual void OnSelfDistributionAction(DurbolManager kolpoPManager)
    {
        gameManager = kolpoPManager;
        gameplayData = kolpoPManager.gameplayData;
        gameState = kolpoPManager.gameState;
    }

    public virtual void OnChangeGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public virtual void OnIncompleteTask()
    {
    }

    public virtual void OnNone()
    {
    }

    public virtual void OnGameDataLoad()
    {
    }

    public virtual void OnGameInitializing()
    {
    }

    public virtual void OnGameStart()
    {
    }

    public virtual void OnGameOver()
    {
    }

    public virtual void OnPauseGamePlay()
    {
    }

    public virtual void OnUnPauseGamePlay()
    {
    }

    public virtual void OnCompleteTask()
    {
    }

    public void SaveGame()
    {
        gameManager.SaveGame();
    }

    public string EditorIconPath { get { return "KolpoBehaviourIcon"; } }
}
