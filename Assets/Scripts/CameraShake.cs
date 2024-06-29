using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public AnimationCurve curve;
    public float shakeTime = 0.2f;

    public Dictionary<string, Vector3> shakingDict = new Dictionary<string, Vector3>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cameraShake(0.05f);
        }
    }

    public void cameraShake(float curveKeyValue)
    {
        StartCoroutine(Shake(curveKeyValue, transform, shakeTime, transform.name));
    }

    public void shakeObject(float curveKeyValue, Transform transform, float shakeTime, string objectName)
    {
        StartCoroutine(Shake(curveKeyValue, transform, shakeTime, objectName));
    }

    public IEnumerator Shake(float curveKeyValue, Transform transform, float shakeTime, string objectName)
    {      
        
        Vector3 startPosition = transform.position;

        if (shakingDict.ContainsKey(objectName))
        {
            startPosition = shakingDict[objectName];
        }
        else
        {
            shakingDict[objectName] = startPosition;
        }
        
        float time = 0f;

        curve.ClearKeys();
        curve.AddKey(0f, curveKeyValue);
        curve.AddKey(shakeTime, 0f);

        while (time < shakeTime)
        {
            time += Time.deltaTime;
            float strength = curve.Evaluate(time/shakeTime);
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        // if (shakingDict.ContainsKey(objectName))
        // {
        //     shakingDict.Remove(objectName);
        // }

        transform.position = startPosition;
    }
}
