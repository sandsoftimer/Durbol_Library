﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Durbol.Utility
{
    public class BoneController : DurbolBehaviour
    {
        #region ALL UNITY FUNCTIONS

        // Awake is called before Start
        public override void Awake()
        {
            base.Awake();
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        void Update()
        {
            //if (gameState.Equals(GameState.GAME_INITIALIZED) && Input.GetMouseButtonDown(0))
            //{
            //    gameManager.ChangeGameState(GameState.GAME_PLAY_STARTED);
            //    gameState = GameState.GAME_PLAY_STARTED;
            //}

            if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
                return;

        }

        #endregion ALL UNITY FUNCTIONS
        //=================================   
        #region ALL OVERRIDING FUNCTIONS


        #endregion ALL OVERRIDING FUNCTIONS
        //=================================
        #region ALL SELF DECLARE FUNCTIONS


        #endregion ALL SELF DECLARE FUNCTIONS

    }
}
