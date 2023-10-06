using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenHandler : DurbolBehaviour
{
    public new Camera camera;
    public Vector2 offset;
    public Vector2 iconOffset = new Vector2(50f, 50f);
    public bool rotateIcon;
    public GameObject[] offScreenDisables;


    [Space(10)]
    public bool showOffScreenIndicator;
    public GameObject offScreenUIIndicatorPrefab;
    public Transform canvasParent;

    GameObject offScreenIndicator;
    bool isOffScreen;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();
        if (camera == null)
            camera = Camera.main;

        if (canvasParent == null)
            canvasParent = FindObjectOfType<Canvas>().transform;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        //if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
        //    return;

        isOffScreen = transform.DL_IsOffScreen(camera, offset);
        for (int i = 0; i < offScreenDisables.Length; i++)
        {
            offScreenDisables[i].SetActive(!isOffScreen);
        }

        if (offScreenUIIndicatorPrefab != null && showOffScreenIndicator)
        {
            if (isOffScreen)
            {
                if (offScreenIndicator == null)
                {
                    offScreenIndicator = DurbolTools.poolManager.Instantiate(offScreenUIIndicatorPrefab, Vector2.zero, Quaternion.identity, canvasParent);
                }
                Vector2 point;
                point = transform.DL_OffScreenIndicatorPoint(camera, iconOffset);
                offScreenIndicator.transform.position = point;

                if (rotateIcon)
                {
                    Vector2 objectScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
                    double angle = Math.Atan2(objectScreenPoint.y - point.y, objectScreenPoint.x - point.x);
                    offScreenIndicator.transform.rotation = Quaternion.Euler(0, 0, (float)angle * Mathf.Rad2Deg);
                }
            }
            else
            {
                if (offScreenIndicator != null)
                {
                    DurbolTools.poolManager.Destroy(offScreenIndicator);
                    offScreenIndicator = null;
                }
            }
        }
        else
        {
            if (offScreenIndicator != null)
            {
                DurbolTools.poolManager.Destroy(offScreenIndicator);
                offScreenIndicator = null;
            }
        }
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void Dispose()
    {
        showOffScreenIndicator = false;

        if(offScreenIndicator)
            DurbolTools.poolManager.Destroy(offScreenIndicator);

        offScreenIndicator = null;
        Destroy(this);
    }
    
    #endregion ALL SELF DECLARE FUNCTIONS

}
