using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightController : MonoBehaviour
{
    public GameObject lightSource;
    public float Duration;
    float RemainTime = 0;

    public bool coroutineCheck;

    public GameObject batteryCanvas;
    public GameObject batteryBar;


    private void Start()
    {
        RemainTime = Duration;
        //batteryCanvas.gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        
    }

    private void Update()
    {
        if(coroutineCheck == false)
        {
            if (Input.GetKeyDown(KeyCode.F) && RemainTime > 0)
            {
                if (lightSource.activeSelf)
                {
                    LightOff();
                }
                else
                {
                    LightOn();
                }
            }

            if (lightSource.activeSelf && RemainTime > 0)
            {
                RemainTime -= Time.deltaTime;

                if (RemainTime <= 0)
                {
                    LightOff();
                }
            }

            if (batteryCanvas.activeSelf && RemainTime > 0)
            {
                batteryBar.transform.localScale = new Vector3(RemainTime / Duration, 1, 1);
            }
        }

        else
        {
            LightOff();
        }
        
        //batteryBar.GetComponent<Image>().rectTransform.localScale = new Vector3(RemainTime/Duration, 0, 0);

    }

    public void LightOn()
    {
        lightSource.SetActive(true);
    }

    public void LightOff()
    {
        lightSource.SetActive(false);
    }

    public IEnumerator Clicker()
    {
        coroutineCheck = true;

        yield return new WaitForSeconds(0.2f);
        coroutineCheck = false;
        LightOn();


    }

    public void ChargeBattery(int Percent)
    {
        batteryBar.transform.localScale = Vector3.one;
        RemainTime = Duration;
        //Debug.Log(RemainTime);
    }
}
