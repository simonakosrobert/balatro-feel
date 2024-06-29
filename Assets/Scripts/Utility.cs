using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Alteruna;

public class Utility : AttributesSync
{
    // Start is called before the first frame update

	public static IEnumerator SmoothMovement(GameObject obj, float movement, GameObject touchBlock = null, float speed = 5, bool horizontal = true)
	{	
		//if horizontal = false the movement will be vertical
		Application.targetFrameRate = 60;
		if (touchBlock != null) touchBlock.SetActive(true);

		float elapsedTime = 0;
		Vector3 startingPos;
		Vector3 finalPos;
		float modifier = 1;
		Vector3 horizontalOrVertical = horizontal ? obj.transform.right : obj.transform.up;

		//Vector3 objOrig = obj.transform.position;
		startingPos  = obj.transform.position;
		finalPos = obj.transform.position + (horizontalOrVertical * movement * modifier);
		elapsedTime = 0;
		while (obj.transform.position != finalPos)
		{	
			float stepAmount = Mathf.Pow(elapsedTime * speed, 3f);
			obj.transform.position = Vector3.Lerp(startingPos, finalPos, Mathf.MoveTowards(0f, 1f, stepAmount));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		if (touchBlock != null) touchBlock.SetActive(false);
		yield return null;
	}

	public static IEnumerator SmoothMovementToPoint(GameObject obj, Vector3 startingPos, Vector3 finalPos, GameObject touchBlock = null, float speed = 5)
	{	
		Application.targetFrameRate = 60;
		if (touchBlock != null) touchBlock.SetActive(true);

		float elapsedTime = 0;

		while (obj.transform.position != finalPos)
		{	
			float stepAmount = Mathf.Pow(elapsedTime * speed, 3f);
			obj.transform.position = Vector3.Lerp(startingPos, finalPos, Mathf.MoveTowards(0f, 1f, stepAmount));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		if (touchBlock != null) touchBlock.SetActive(false);
		yield return null;
	}

	public static IEnumerator UIElementMoveAndDisableEnum(GameObject obj, GameObject touchBlock, float speed = 5, bool horizontal = true , bool downRight = true)
	{	
		Application.targetFrameRate = 60;
		touchBlock.SetActive(true);
		float elapsedTime = 0;
		Vector3 startingPos;
		Vector3 finalPos;
		float modifier = downRight ? 1 : -1;
		//float time = 0.3f;

		Vector3 horizontalOrVertical = horizontal ? obj.transform.right : obj.transform.up;


		Vector3 objOrig = obj.transform.position;
		startingPos  = obj.transform.position;
		finalPos = obj.transform.position + (horizontalOrVertical * -15 * modifier);
		elapsedTime = 0;
		while (obj.transform.position != finalPos)
		{	
			float stepAmount = Mathf.Pow(elapsedTime * speed, 3f);
			obj.transform.position = Vector3.Lerp(startingPos, finalPos, Mathf.MoveTowards(0f, 1f, stepAmount));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		obj.SetActive(false);
		obj.transform.position = objOrig;
		
		touchBlock.SetActive(false);
		
		yield return null;
	}

	public static IEnumerator UIElementMoveToStartAndEnableEnum(GameObject obj, Vector3 finalPos, GameObject touchBlock, float speed = 5, bool horizontal = true , bool downRight = true)
	{	
		Application.targetFrameRate = 60;
		touchBlock.SetActive(true);
		float elapsedTime = 0;
		obj.SetActive(true);
		
		float modifier = downRight ? 1 : -1;

		Vector3 horizontalOrVertical = horizontal ? obj.transform.right : obj.transform.up;

		Vector3 startingPos = finalPos + (horizontalOrVertical * -15 * modifier);
		elapsedTime = 0;
		while (obj.transform.position != finalPos)
		{	
			float stepAmount = Mathf.Pow(elapsedTime * speed, 3f);
			obj.transform.position = Vector3.Lerp(startingPos, finalPos, Mathf.MoveTowards(0f, 1f, stepAmount));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		
		touchBlock.SetActive(false);
		
		yield return null;
	}

    public static IEnumerator UIElementSwitchEnum(GameObject[] closeList, GameObject[] openList, GameObject touchBlock, float speed = 5, bool closeDirectionDown = true, bool openDirectionDown = true)
		{	
			Application.targetFrameRate = 60;
			touchBlock.SetActive(true);
			float elapsedTime = 0;
			Vector3 startingPos;
			Vector3 finalPos;
			float modifier = 1;
			//float time = 0.3f;

			if (!closeDirectionDown) modifier = -1;
			else modifier = 1;

			foreach (GameObject obj in closeList)
			{
				Vector3 objOrig = obj.transform.position;
				startingPos  = obj.transform.position;
				finalPos = obj.transform.position + (obj.transform.up * -15 * modifier);
				elapsedTime = 0;
				while (obj.transform.position != finalPos)
				{	
					float stepAmount = Mathf.Pow(elapsedTime * speed, 3f);
					obj.transform.position = Vector3.Lerp(startingPos, finalPos, Mathf.MoveTowards(0f, 1f, stepAmount));
					elapsedTime += Time.deltaTime;
					yield return null;
				}
				obj.SetActive(false);
				obj.transform.position = objOrig;
			}

			if (!openDirectionDown) modifier = -1;
			else modifier = 1;

			foreach (GameObject obj in openList)
			{
				obj.SetActive(true);
				elapsedTime = 0;
				startingPos  = obj.transform.position + (obj.transform.up * 15 * modifier);
				finalPos = obj.transform.position;

				do
				{
				    float stepAmount = Mathf.Pow(elapsedTime * speed, 3f);
					obj.transform.position = Vector3.Lerp(startingPos, finalPos, Mathf.MoveTowards(0f, 1f, stepAmount));
					elapsedTime += Time.deltaTime;
					yield return null;
				} while (obj.transform.position != finalPos);
                
			}

			//SideMenu.SetActive(false);
			//RoomList.SetActive(false);
			//CreateRoomMenuObj.SetActive(true);

			// do
			// {
			// 	//float elapsedTime = Time.time - timeWeStarted;
			// 	float stepAmount = Mathf.Pow(elapsedTime * 5, 3f);
			// 	// FOR VECTOR3s
			// 	CreateRoomMenuObj.transform.position = Vector3.Lerp(startingPos, finalPos, Mathf.MoveTowards(0f, 1f, stepAmount));
			// 	elapsedTime += Time.deltaTime;
			// 	yield return null;
			// } while (CreateRoomMenuObj.transform.position != finalPos);

			
			touchBlock.SetActive(false);

			//SideMenu.transform.position = sideMenuOrig;
			//RoomList.transform.position = roomListOrig;
			
			yield return null;
		}

    public static IEnumerator Shake(float curveKeyValue, Transform transform, float shakeTime, bool shakeForever = false)
    {
        Vector3 startPosition = transform.position;
        float time = 0f;
        float strength = 0;
        AnimationCurve curve = new AnimationCurve();

        curve.ClearKeys();
        curve.AddKey(0f, curveKeyValue);
        curve.AddKey(0.5f, 0f);

        while (time < shakeTime || shakeForever)
        {
            time += Time.deltaTime;
            if (!shakeForever) strength = curve.Evaluate(time/shakeTime);
            else strength = curveKeyValue;
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }

    public static IEnumerator ShakeText(TMP_Text text)
    {
        RectTransform rt = text.rectTransform;
        Vector2 startPos = rt.anchoredPosition;
        while (1 == 1)
        {
            for(float t = 0; t <= 2; t += Time.deltaTime * 5)
            {
                rt.anchoredPosition = startPos + Vector2.up * t;
                yield return null;
            }
            for(float t = 0; t <= 2; t += Time.deltaTime * 5)
            {
                rt.anchoredPosition = startPos + Vector2.up * (1 - t);
                yield return null;
            }
            rt.anchoredPosition = startPos;
        }
    }

}
