using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

    [SerializeField] Sprite timerSpriteNormal;
    [SerializeField] Sprite timerSpriteWarning;
    [SerializeField] Sprite timerSpriteDanger;
    private float timerTime;
    private Image timerImage;

    private void Start()
    {
        timerImage = gameObject.GetComponent<Image>();
    }

    public void UpdateTimer(float currentTime, float maxTime)
    {
        timerImage = gameObject.GetComponent<Image>();
        timerTime = currentTime;
        if (timerTime <= 0)
        {
            timerImage.fillAmount = 0;
        }
        else
        {
            timerImage.fillAmount = timerTime / maxTime;
        }
        if (currentTime < (maxTime / 100) * 30)
        {
            SetAsDanger();
        }
        else if (currentTime < (maxTime / 100) * 50)
        {
            SetAsWarning();
        }
        else
        {
            SetAsNormal();
        }
    }

    void SetAsNormal()
    {
        timerImage.sprite = timerSpriteNormal;
    }

    void SetAsWarning()
    {
        timerImage.sprite = timerSpriteWarning;
    }

    void SetAsDanger()
    {
        timerImage.sprite = timerSpriteDanger;
    }
}
