using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 1f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private float currentShakeAmount;

    private float startTime;
    private bool shake;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
        currentShakeAmount = shakeAmount;
        //ShakeCam(10f);
    }

    public void ShakeCam(float duration, float amount)
    {
        shake = true;
        shakeDuration = duration;
        shakeAmount = amount;
        currentShakeAmount = shakeAmount;
        startTime = Time.time;
    }

    public void ShakeCam()
    {
        shake = true;
        currentShakeAmount = shakeAmount;
        startTime = Time.time;
    }

    void Update()
    {
        if (shake)
            if (Time.time - startTime < shakeDuration)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * currentShakeAmount;
                currentShakeAmount = Mathf.Lerp(currentShakeAmount, 0, 0.1f);
            }
            else
            {
                shake = false;
                camTransform.localPosition = originalPos;
                currentShakeAmount = shakeAmount;
            }
    }
}