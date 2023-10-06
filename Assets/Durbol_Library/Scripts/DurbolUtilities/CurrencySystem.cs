using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class CurrencySystem : DurbolBehaviour
{
    public string CURRENCY_NAME = "COIN_SYSTEM";
    public Image iconImageUI;
    public Sprite icon;
    public float currencyCollectSpeed = 2f;
    public TMP_Text currencyUI;
    public TMP_Text currencyDeductionUI;
    public ulong startingCurrency = 100;

    ulong totalCurrency;
    public ulong REMAINING_CURRENCY
    {
        get { return totalCurrency; }
    }

    Animator anim;
    IEnumerator runningCoroutine;

    #region ALL UNITY FUNCTIONS

    // Awake is called before Start
    public override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();
        iconImageUI.sprite = icon;

        if (PlayerPrefsX.GetBool(CURRENCY_NAME, true))
        {
            totalCurrency += startingCurrency;
            PlayerPrefsX.SetuLong(CURRENCY_NAME, totalCurrency);
            PlayerPrefsX.SetBool(CURRENCY_NAME, false);
        }
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

        //if (Input.GetMouseButtonDown(0))
        //{
        //    AddCurrency(100000);
        //}
        //else if (Input.GetMouseButtonDown(1))
        //{
        //    DeductCurrency(500);
        //}
    }

    #endregion ALL UNITY FUNCTIONS
    //=================================   
    #region ALL OVERRIDING FUNCTIONS

    public override void OnGameDataLoad()
    {
        base.OnGameDataLoad();

        totalCurrency = PlayerPrefsX.GetuLong(CURRENCY_NAME, 0);
        currencyUI.text = CurrencyFormatter.FormattedRNACount(REMAINING_CURRENCY);
    }

    #endregion ALL OVERRIDING FUNCTIONS
    //=================================
    #region ALL SELF DECLARE FUNCTIONS

    public void AddCurrency(ulong value, float animationDuration = 2)
    {
        IncreaseCurrency(value, animationDuration);
    }

    public void AddCurrency(ulong value, Vector3 worldSpacePoint, Transform uiParent, float animationDuration = 2)
    {
        StartCoroutine(CollectCurrencyIcon(value, worldSpacePoint, uiParent, animationDuration));
    }

    IEnumerator CollectCurrencyIcon(ulong value, Vector3 worldSpacePoint, Transform uiParent, float animationDuration)
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldSpacePoint);

        GameObject go = Instantiate(iconImageUI.gameObject, screenPoint, Quaternion.identity, uiParent);
        Vector3 direction = iconImageUI.transform.position - go.transform.position;
        //direction.y = direction.z;
        //direction.z = 0;
        int count = 0;
        float distance = 1000000f;
        while(true)
        {
            if ((go.transform.position - iconImageUI.transform.position).magnitude > distance)
                break;
            else
                distance = (go.transform.position - iconImageUI.transform.position).magnitude;
            go.transform.position += direction * Time.deltaTime * currencyCollectSpeed;
            count++;
            yield return null;
        }
        Destroy(go);
        IncreaseCurrency(value, animationDuration);
    }

    void IncreaseCurrency(ulong value, float animationDuration)
    {
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
            currencyUI.text = CurrencyFormatter.FormattedRNACount(totalCurrency);
        }
        runningCoroutine = CurrencyFormatter.CountUpAnimation(totalCurrency, totalCurrency + value, currencyUI, animationDuration);
        StartCoroutine(runningCoroutine);

        totalCurrency += value;
        PlayerPrefsX.SetuLong(CURRENCY_NAME, totalCurrency);
    }

    public void DeductCurrency(ulong value, float animationDuration = 2)
    {
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
            currencyUI.text = CurrencyFormatter.FormattedRNACount(totalCurrency);
        }
        //currencyDeductionUI.text = "-" +CurrencyFormatter.FormattedRNACount(value);
        //anim.SetTrigger("Deduct");

        totalCurrency -= value;
        PlayerPrefsX.SetuLong(CURRENCY_NAME, totalCurrency);

        currencyUI.text = CurrencyFormatter.FormattedRNACount(totalCurrency);
    }

    public void Shake()
    {
        anim.SetTrigger("Shake");
    }

    #endregion ALL SELF DECLARE FUNCTIONS

}


public class CurrencyFormatter
{
    public static string FormattedRNACount(int rnaCount, int decimalPlaces = 2)
    {
        if (((long)rnaCount) >= 1000000000000000)
        {
            return $"{((double)rnaCount / 1000000000000000.0).ToString($"F{decimalPlaces}")}Q";
        }
        else if (((long)rnaCount) >= 1000000000000)
        {
            return $"{((double)rnaCount / 1000000000000.0).ToString($"F{decimalPlaces}")}T";
        }
        else if (rnaCount >= 1000000000)
        {
            return $"{((double)rnaCount / 1000000000.0).ToString($"F{decimalPlaces}")}B";
        }
        else if (rnaCount >= 1000000)
        {
            return $"{((double)rnaCount / 1000000.0).ToString($"F{decimalPlaces}")}M";
        }
        else if (rnaCount >= 1000)
        {
            return $"{((double)rnaCount / 1000.0).ToString($"F{decimalPlaces}")}K";
        }
        else
        {
            return rnaCount.ToString();
        }
    }

    public static string FormattedRNACount(ulong rnaCount, int decimalPlaces = 2)
    {
        if (rnaCount >= 1000000000000000)
        {
            return $"{((double)rnaCount / 1000000000000000.0).ToString($"F{decimalPlaces}")}Q";
        }
        else if (rnaCount >= 1000000000000)
        {
            return $"{((double)rnaCount / 1000000000000.0).ToString($"F{decimalPlaces}")}T";
        }
        else if (rnaCount >= 1000000000)
        {
            return $"{((double)rnaCount / 1000000000.0).ToString($"F{decimalPlaces}")}B";
        }
        else if (rnaCount >= 1000000)
        {
            return $"{((double)rnaCount / 1000000.0).ToString($"F{decimalPlaces}")}M";
        }
        else if (rnaCount >= 1000)
        {
            return $"{((double)rnaCount / 1000.0).ToString($"F{decimalPlaces}")}K";
        }
        else
        {
            return rnaCount.ToString();
        }
    }

    public static IEnumerator CountUpAnimation(int startVal, int endVal, TMP_Text textComponent, float duration, System.Action OnComplete = null)
    {
        int deltaVal = endVal - startVal;
        int perFrameUpdate = (int)(((float)deltaVal / (float)duration) * Time.deltaTime);
        if(perFrameUpdate < 1)
        {
            perFrameUpdate = 1;
            duration = Time.deltaTime / perFrameUpdate * deltaVal;
        }
        int currentVal = startVal;
        float startTime = Time.time;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            currentVal += perFrameUpdate;
            if (currentVal < endVal)
            {
                textComponent.text = FormattedRNACount(currentVal);
            }
        }

        textComponent.text = FormattedRNACount(endVal);

        OnComplete?.Invoke();
    }

    public static IEnumerator CountUpAnimation(ulong startVal, ulong endVal, TMP_Text textComponent, float duration, System.Action OnComplete = null)
    {
        ulong deltaVal = endVal - startVal;
        int perFrameUpdate = (int)(((double)deltaVal / (double)duration) * Time.deltaTime);
        if (perFrameUpdate < 1)
        {
            perFrameUpdate = 1;
            duration = Time.deltaTime / perFrameUpdate * deltaVal;
        }
        ulong currentVal = startVal;
        float startTime = Time.time;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            currentVal += (ulong)perFrameUpdate;
            if (currentVal < endVal)
            {
                textComponent.text = FormattedRNACount(currentVal);
            }
        }

        textComponent.text = FormattedRNACount(endVal);

        OnComplete?.Invoke();
    }

    public static IEnumerator CountUpTimeAnimation(int timeInSeconds, TMP_Text textComponent, float duration, System.Action OnComplete = null)
    {
        int perFrameUpdate = (int)(((float)timeInSeconds / duration) * Time.deltaTime);
        perFrameUpdate = perFrameUpdate >= 1 ? perFrameUpdate : 1;
        int currentVal = 0;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            currentVal += perFrameUpdate;
            if (currentVal < timeInSeconds)
            {
                textComponent.text = FormattedTime(currentVal);
            }
        }

        textComponent.text = FormattedTime(timeInSeconds);

        OnComplete?.Invoke();
    }

    static string FormattedTime(int seconds)
    {
        int secs = seconds % 60;
        int mins = seconds / 60;
        int hrs = mins / 60;
        mins = mins % 60;

        return $"{hrs.ToString("00")}hrs : {mins.ToString("00")}mins : {secs.ToString("00")}secs";
    }
}
