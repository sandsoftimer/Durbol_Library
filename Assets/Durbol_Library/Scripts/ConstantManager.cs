using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(ConstantManagerOrder)]
public partial class ConstantManager
{
    #region Custom attributes of this game

    public const string TOTAL_SPAWNED = "TOTAL_SPAWNED";
    public const string DRAG_TO_MOVE_DONE = "DRAG_TO_MOVE_DONE";
    public const string CLICK_TO_UPGRDE_TUTORIAL_DONE = "CLICK_TO_UPGRDE_TUTORIAL_DONE";

    #endregion Custom attributes of this game

    public const string LAST_BUILD_NUMBER = "LAST_BUILD_NUMBER";
    public const string SOUND = "SOUND";
    public const string MUSIC = "MUSIC";
    public const string CURRENT_SCORE = "CURRENT_SCORE";
    public const string HIGH_SCORE = "HIGH_SCORE";
    public const string HIDE = "Hide";

    // RuntimeExecution Order of KolpoLibray Scripts.
    public const int ConstantManagerOrder = -30;
    public const int KolpoToolOrder = -20;
    public const int KolpoBehaviourOrder = -10;
    public const int KolpoManagerOrder = -2;

    public const int TOTAL_GAME_LEVELS = 1;
    public const float DEFAULT_ANIMATION_TIME = 1f;
    public const float ONE_HALF_TIME = DEFAULT_ANIMATION_TIME / 2;
    public const float ONE_FORTH_TIME = DEFAULT_ANIMATION_TIME / 4;
    public const float SINGLE_TAP_THRESHOLD = 0.25f;
    public const float DRAGGING_THRESHOLD = 0.1f;
    public const float TAP_N_HOLD_THRESHOLD = 0.25f;
    public const float SWIPPING_THRESHOLD = 0.5f;

    /// <summary>
    /// Create an integer variable for script use
    /// and add that name to dictonary below as well
    /// KolpoConfigarator.cs will use this to modify Layer System.
    /// </summary>
    #region Collision Layer Setup
    // Collision Layer ids. 
    // These layer's order must be maintained in Bottom Layer Array
    public const int DEFAULT_LAYER = 0;
    public const int TRANSPARENT_FX_LAYER = 1;
    public const int IGNORE_RAYCAST_LAYER = 2;
    public const int WATER_LAYER = 4;
    public const int UI_LAYER = 5;
    public const int PLAYER_LAYER = 8;
    public const int PLAYER_WEAPON = 9;
    public const int ENEMY_LAYER = 10;
    public const int ENEMY_WEAPON = 11;
    public const int GROUND_LAYER = 12;
    public const int BOUNDARY_LAYER = 13;
    public const int PICKUPS_LAYER = 14;
    public const int DESTINATION_LAYER = 15;
    public const int SENSOR = 16;
    public const int NON_RENDERING_LAYER = 17;

    public static readonly Dictionary<int, string> layerNames = new Dictionary<int, string>
    {
        { 0, "Default" },
        { 1, "TransparentFX" },
        { 2, "Ignore Raycast" },
        { 3, "" },
        { 4, "Water" },
        { 5, "UI" },
        { 6, "" },
        { 7, "" },
        { 8, "PLAYER_LAYER" },
        { 9, "PLAYER_WEAPON" },
        { 10, "ENEMY_LAYER" },
        { 11, "ENEMY_WEAPON" },
        { 12, "GROUND_LAYER" },
        { 13, "BOUNDARY_LAYER" },
        { 14, "PICKUPS_LAYER" },
        { 15, "DESTINATION_LAYER" },
        { 16, "SENSOR" },
        { 17, "NON_RENDERING_LAYER" },
        { 18, "" },
        { 19, "" },
        { 20, "" },
        { 21, "" },
        { 22, "" },
        { 23, "" },
        { 24, "" },
        { 25, "" },
        { 26, "" },
        { 27, "" },
        { 28, "" },
        { 29, "" },
        { 30, "" },
        { 31, "" },
    };
    #endregion Collision Layer Setup
}
