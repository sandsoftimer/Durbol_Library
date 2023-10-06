/*
 * Developer Name: Md. Imran Hossain
 * E-mail: sandsoftimer@gmail.com
 * FB: https://www.facebook.com/md.imran.hossain.902
 * in: https://www.linkedin.com/in/md-imran-hossain-69768826/
 * 
 * This is a manager which will give all possible input response at runtime. 
 * like, 
 * DRAGGING, SWIPPING, TAPPING, TAP & HOLD etc.
 * 
 * N.B: If any script need to recieve input response then
 *      script should be under KolpoBehaviour rather Monobehaviour
 *      & Awake should call this --> Registar_For_Input_Callback();
 *      Now just override input response functions to get notified about inputtype.
 *      Example_1: "public override void OnTapStart()"
 *      Example_2: "public override void OnSwipRight()"
 *      Example_3: "public override void OnDraggingInput(Vector3 dragAmount)"
 */

using UnityEngine;

namespace Com.Durbol.Utility
{
    public class InputManager : DurbolBehaviour
    {
        float tapStartedTime;
        float tap_n_hold_threshold_time = ConstantManager.TAP_N_HOLD_THRESHOLD;
        float dragging_threshold = ConstantManager.DRAGGING_THRESHOLD;

        Vector3 startMousePosition, lastMousePosition;
        DL_TappingType inputType = DL_TappingType.NONE;

        public bool inputTestingModeOn;

        #region ALL UNITY FUNCTIONS
        public override void Awake()
        {
            base.Awake();

            Input.multiTouchEnabled = false;
            Registar_For_Input_Callback();
        }

        void Update()
        {
            if (gameManager == null)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                tapStartedTime = Time.time;
                startMousePosition = Input.mousePosition;
                lastMousePosition = Input.mousePosition;
                inputType = DL_TappingType.TAP_START;
                gameManager.ProcessTapping(inputType, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            else if (Input.GetMouseButton(0))
            {
                if (Vector3.Distance(Input.mousePosition, lastMousePosition) > dragging_threshold)
                {
                    gameManager.ProcessDragging(Input.mousePosition - lastMousePosition);
                }
                if((Time.time - tapStartedTime) >= tap_n_hold_threshold_time)
                {
                    inputType = DL_TappingType.TAP_N_HOLD;
                    gameManager.ProcessTapping(inputType, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                inputType = DL_TappingType.TAP_END;
                gameManager.ProcessTapping(inputType, Camera.main.ScreenToWorldPoint(Input.mousePosition));

                if((Time.time - tapStartedTime) <= ConstantManager.SINGLE_TAP_THRESHOLD)
                {
                    inputType = DL_TappingType.SINGLE_TAP;
                    gameManager.ProcessTapping(inputType, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }

                float dist = (Input.mousePosition - startMousePosition).magnitude;
                if (dist >= ConstantManager.SWIPPING_THRESHOLD)
                {
                    float dX = Input.mousePosition.x - startMousePosition.x;
                    float dY = Input.mousePosition.y - startMousePosition.y;
                    if (Mathf.Abs(dX) > Mathf.Abs(dY))
                    {
                        if (dX > 0)
                            gameManager.ProcessSwipping(DL_SwippingType.SWIPE_RIGHT);
                        else
                            gameManager.ProcessSwipping(DL_SwippingType.SWIPE_LEFT);
                    }
                    else
                    {
                        if (dY > 0)
                            gameManager.ProcessSwipping(DL_SwippingType.SWIPE_UP);
                        else
                            gameManager.ProcessSwipping(DL_SwippingType.SWIPE_DOWN);
                    }
                }
            }

            #region Mobile Only

            if (Application.isMobilePlatform)
            {
                // Check if the screen was touched
                if (Input.touchCount == 2)
                {
                    // Get the two touches
                    Touch touch1 = Input.GetTouch(0);
                    Touch touch2 = Input.GetTouch(1);

                    // Get the current and previous positions of the touches
                    Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                    Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;
                    Vector2 touch1Pos = touch1.position;
                    Vector2 touch2Pos = touch2.position;

                    // Get the magnitude of the vector (the distance) between the touches in each frame.
                    float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
                    float touchDeltaMag = (touch1Pos - touch2Pos).magnitude;

                    // Find the difference in the distances between each frame.
                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                    gameManager.ProcessZooming(deltaMagnitudeDiff);
                }
            }

            #endregion Mobile Only

            #region DEBUG COMMAND

            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.DebugBreak();
            }

            #endregion DEBUG COMMAND

        }

        void LateUpdate()
        {
            lastMousePosition = Input.mousePosition;
        }

        #endregion ALL UNITY FUNCTIONS
        //=================================   
        #region ALL OVERRIDING FUNCTIONS

        public override void OnTapStart(Vector3 tapOnWorldSpace)
        {
            base.OnTapStart(tapOnWorldSpace);

            if(inputTestingModeOn)
                Debug.Log("TAP START");
        }
        public override void OnTapEnd(Vector3 tapOnWorldSpace)
        {
            base.OnTapEnd(tapOnWorldSpace);

            if (inputTestingModeOn)
                Debug.Log("TAP END");
        }

        public override void OnSingleTap(Vector3 dragAmount)
        {
            base.OnSingleTap(dragAmount);

            if (inputTestingModeOn)
                Debug.Log("SINGLE TAP DETECTED");
        }

        public override void OnTapAndHold(Vector3 tapOnWorldSpace)
        {
            base.OnTapAndHold(tapOnWorldSpace);

            if (inputTestingModeOn)
                Debug.Log("TAP N Hold");
        }
        public override void OnDrag(Vector3 dragAmount)
        {
            base.OnDrag(dragAmount);

            if (inputTestingModeOn)
                Debug.Log("Dragging");
        }
        public override void OnSwipeUp()
        {
            base.OnSwipeUp();

            if (inputTestingModeOn)
                Debug.Log("Swip Up");
        }
        public override void OnSwipeDown()
        {
            base.OnSwipeDown();

            if (inputTestingModeOn)
                Debug.Log("Swip Down");
        }
        public override void OnSwipeLeft()
        {
            base.OnSwipeLeft();

            if (inputTestingModeOn)
                Debug.Log("Swip Left");
        }
        public override void OnSwipeRight()
        {
            base.OnSwipeRight();

            if (inputTestingModeOn)
                Debug.Log("Swip Right");
        }
        #endregion ALL OVERRIDING FUNCTIONS
        //=================================
        #region ALL SELF DECLARE FUNCTIONS


        #endregion ALL SELF DECLARE FUNCTIONS
    }
}
