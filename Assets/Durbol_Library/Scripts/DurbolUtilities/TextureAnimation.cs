using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Durbol.Utility
{
    public class TextureAnimation : DurbolBehaviour
    {
        public Vector2 animationSpeed;
        Renderer _renderer;

        #region ALL UNITY FUNCTIONS

        // Awake is called before Start
        public override void Awake()
        {
            base.Awake();

            if (_renderer == null)
                _renderer = GetComponent<Renderer>();
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (_renderer == null)
                return;

            _renderer.material.SetTextureOffset(
                "_BaseMap",
                _renderer.material.GetTextureOffset("_BaseMap") + animationSpeed * Time.deltaTime);

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