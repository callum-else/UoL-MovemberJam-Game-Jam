using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public InputManager inputManager;
    public PuzzleManager puzzleManager;
    public MusicManager musicManager;
    public CameraShake camShake;

    public GameObject timerBanner;
    public Text timerText;
    private Animator bannerAnim;

    public Transform monitorObj;
    private Animator monitorAnim;

    private bool countDown = false;
    private bool flashLight = false;
    private float duration = 60;
    private float startTime;

    private void Start()
    {
        monitorAnim = monitorObj.GetComponent<Animator>();
        bannerAnim = timerBanner.GetComponent<Animator>();
    }

    public void showBanner()
    {
        if (!timerBanner.activeSelf)
        {
            timerBanner.SetActive(true);
            startTime = Time.time;
            countDown = true;
            camShake.ShakeCam(3f, .5f);
        }
        musicManager.PlayMusic();
        musicManager.PlayExplosion();
    }

    public void WinGame()
    {
        countDown = false;
        camShake.ShakeCam(0, 0);
        bannerAnim.SetTrigger("GameEnd");
        monitorAnim.SetTrigger("GameWin");
    }

    public void AddTime()
    {
        if(duration - (Time.time - startTime) + 2 < duration)
            startTime += 2;
    }

    private void FixedUpdate()
    {
        if (countDown)
        {
            float remTime = duration - (Time.time - startTime);
            float camShakeAmount = 0;

            if (remTime < duration / 4)
            {
                camShakeAmount = 0.1f;
                musicManager.SetIndex(3);
            }
            else if (remTime < duration / 3)
            {
                camShakeAmount = 0.05f;
                musicManager.SetIndex(2);
            }
            else if (remTime < duration / 2)
            {
                if (!flashLight)
                {
                    monitorAnim.SetTrigger("StartFlashing");
                    flashLight = true;
                }
                camShakeAmount = 0.01f;
                musicManager.SetIndex(1);
            }

            if (duration - (Time.time - startTime) <= 0)
            {
                camShakeAmount = 5f;
                countDown = false;
                inputManager.canType = false;
                musicManager.StopMusic();
                musicManager.PlayBigExplosion();
                monitorAnim.SetTrigger("GameOver");
            }

            if(camShakeAmount > 0)
                camShake.ShakeCam(10f, camShakeAmount);

            timerText.text = (duration - (Time.time - startTime)).ToString("F1");
        }
    }
}
