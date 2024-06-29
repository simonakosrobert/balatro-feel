using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{   
    [SerializeField] GameObject magyatroLogo;
    public GameObject touchBlock;
    [SerializeField] GameObject roomBrowser;
    [SerializeField] GameObject singleplayerBtn;
    [SerializeField] GameObject multiplayerBtn;
    [SerializeField] GameObject settingsBtn;
    [SerializeField] GameObject CardSound;

    public AnimationCurve myCurve;
    public AnimationCurve cardCurveSP;
    public AnimationCurve cardCurveMP;
    public AnimationCurve cardCurveSettings;
    float colorChange = 10;
    bool colorUp;

    public float angularSpeed = 0.1f;
    public float circleRad = 0.05f;
    
    private Vector2 fixedPointLogo;
    private Vector2 fixedPointSP;
    private Vector2 fixedPointMP;
    private Vector2 fixedPointSettings;
    private float currentAngle;

    private bool buttonPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        fixedPointLogo = magyatroLogo.transform.position;
        fixedPointSP = singleplayerBtn.transform.position;
        fixedPointMP = multiplayerBtn.transform.position;
        fixedPointSettings = settingsBtn.transform.position;

        cardCurveSP.AddKey(0, 0f);
        cardCurveSP.AddKey(1.5f, 0.02f);
        cardCurveSP.AddKey(2f, 0.04f);
        cardCurveSP.AddKey(3.5f, 0.02f);
        cardCurveSP.AddKey(5, 0f);

        cardCurveMP.AddKey(0, 0f);
        cardCurveMP.AddKey(1, -0.03f);
        cardCurveMP.AddKey(2.5f, -0.07f);
        cardCurveMP.AddKey(4, -0.04f);
        cardCurveMP.AddKey(5, -0.02f);
        cardCurveMP.AddKey(6, -0.005f);
        cardCurveMP.AddKey(7, 0f);

        cardCurveSettings.AddKey(0, 0f);
        cardCurveSettings.AddKey(1.5f, 0.03f);
        cardCurveSettings.AddKey(3f, 0);
        
    }

    public void SingleplayerBtnPressed()
    {
        //TODO implement singleplayer mode
        GameObject cardSound = Instantiate(CardSound);
        Destroy(cardSound, cardSound.GetComponent<AudioSource>().clip.length);
    }

    public void MultiplayerBtnPressed()
    {   
        buttonPressed = true;
        GameObject cardSound = Instantiate(CardSound);
        Destroy(cardSound, cardSound.GetComponent<AudioSource>().clip.length);
        StartCoroutine(Utility.UIElementSwitchEnum(new GameObject[]{magyatroLogo}, new GameObject[]{}, touchBlock, 5, false, false));
        StartCoroutine(Utility.UIElementSwitchEnum(new GameObject[]{settingsBtn, multiplayerBtn, singleplayerBtn}, new GameObject[]{roomBrowser}, touchBlock, 5));
    }

    // Update is called once per frame
    void Update()
    {   

        if (!buttonPressed)
        {   
            circleRad = 0.05f;
            currentAngle += 0.1f * Time.deltaTime;
            Vector2 offsetLogo = new Vector2 (Mathf.Sin (currentAngle), Mathf.Cos (currentAngle)) * circleRad;
            magyatroLogo.transform.position = new Vector3(transform.position.x + offsetLogo.x/2, fixedPointLogo.y - myCurve.Evaluate(Time.time % myCurve.length)/2 + offsetLogo.y/2 , transform.position.z);

            //circleRad = 0.03f;
            //currentAngle += 0.3f * Time.deltaTime;
            Vector2 offsetSP = new Vector2 (Mathf.Sin (currentAngle)* 1.2f, Mathf.Cos (currentAngle)) * circleRad;
            singleplayerBtn.transform.position = new Vector3(transform.position.x + offsetSP.x/2, fixedPointSP.y - myCurve.Evaluate(Time.time % myCurve.length)/2 + offsetSP.y/2, transform.position.z);
            singleplayerBtn.transform.rotation = new Quaternion(singleplayerBtn.transform.rotation.x, singleplayerBtn.transform.rotation.y, 0.02f + cardCurveSP.Evaluate(Time.time % cardCurveSP.length)/10, 1);

            //circleRad = 0.1f;
            //currentAngle += 0.9f * Time.deltaTime;
            Vector2 offsetMP = new Vector2 (Mathf.Sin (currentAngle) * 0.8f, Mathf.Cos (currentAngle)) * circleRad;
            multiplayerBtn.transform.position = new Vector3(transform.position.x + offsetMP.x/2, fixedPointMP.y - myCurve.Evaluate(Time.time % myCurve.length)/2 + offsetMP.y/2, transform.position.z);
            multiplayerBtn.transform.rotation = new Quaternion(multiplayerBtn.transform.rotation.x, multiplayerBtn.transform.rotation.y, cardCurveMP.Evaluate(Time.time % cardCurveMP.length)/10, 1);

            //circleRad = 0.09f;
            //currentAngle += 0.3f * Time.deltaTime;
            Vector2 offsetSettings = new Vector2 (Mathf.Sin (currentAngle) * 0.8f, Mathf.Cos (currentAngle)) * circleRad;
            settingsBtn.transform.position = new Vector3(transform.position.x + offsetSettings.x/2, fixedPointSettings.y - myCurve.Evaluate(Time.time % myCurve.length)/2 + offsetSettings.y/2, transform.position.z);
            settingsBtn.transform.rotation = new Quaternion(settingsBtn.transform.rotation.x, settingsBtn.transform.rotation.y, -0.04f + cardCurveSettings.Evaluate(Time.time % cardCurveSettings.length)/10, 1);
            
        }
        
        if (colorChange >= 100) colorUp = false;
        else if (colorChange <= 10) colorUp = true;

        if (colorUp) colorChange += 0.5f;
        else colorChange -= 0.5f;
 
        magyatroLogo.GetComponent<TMP_Text>().colorGradient = new VertexGradient(new Color32((byte)(255-colorChange), (byte)(0 + colorChange), 146, 255), new Color32((byte)(90 + colorChange), 0, (byte)(113 - colorChange), 255), new Color32((byte)(255 - colorChange), (byte)(59 + colorChange / 2), (byte)(59 + colorChange / 2), 255), new Color32((byte)(255 - colorChange), 82, 0, 255));
        
    }
}
