using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : DurbolBehaviour
{
    public Transform canvas;
    public Image healthBar;
    public float healthMaxValue = 100f;

    float hitTintValue;

    float health = 100f;

    public float GET_CURRENT_HEALTH
    {
        get { return health; }
    }

    #region ALL UNITY FUNCTIONS

    public override void OnEnable()
    {
        base.OnEnable();
        health = healthMaxValue;
        UpdateVisuals();
    }

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
        if (!gameState.Equals(GameState.GAME_PLAY_STARTED))
            return;
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS


    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void SetActiveUI(bool value)
    {
        canvas.gameObject.SetActive(value);
        healthBar.gameObject.SetActive(value);
    }

    public float Damage(float value)
    {
        hitTintValue = hitTintValue > 0? hitTintValue : 1;
        health -= value;
        
        UpdateVisuals();
        return health;
    }

    public void Gain(float value)
    {
        health += value;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = Mathf.InverseLerp(0, healthMaxValue, health);
            healthBar.color = Color.Lerp(Color.red, Color.green, healthBar.fillAmount);

            healthBar.gameObject.SetActive(health > 0);
            if(canvas != null)
                canvas.gameObject.SetActive(health > 0);
        }
    }

    public void ResetHealth(float healthMaxValue)
    {
        this.healthMaxValue = healthMaxValue;
        health = healthMaxValue;
        healthBar.fillAmount = Mathf.InverseLerp(0, this.healthMaxValue, health);
        UpdateVisuals();
    }
    
    #endregion ALL SELF DECLARE FUNCTIONS

}
