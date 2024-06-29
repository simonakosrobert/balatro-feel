using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriestessEffect : MonoBehaviour
{   
    Image bgImage;
    float _alpha = 0;
    GameObject touchBlock;
    GameObject tarot;
    // Start is called before the first frame update

    void Awake()
    {   
        tarot = transform.GetChild(1).gameObject;
    }
    void Start()
    {
        bgImage = transform.GetChild(0).GetComponent<Image>();
        GameObject Gamehandler = GameObject.Find("GameHandler");
        if (Gamehandler == null) touchBlock = GameObject.Find("MAIN MENU").GetComponent<MainMenu>().touchBlock;
        else touchBlock = GameObject.Find("GameHandler").GetComponent<GameHandler>().touchBlock;
        touchBlock.SetActive(true);
        StartCoroutine(PriestessEffectEnum());
    }

    public IEnumerator PriestessEffectEnum()
    {
        while (_alpha < 150)
        {
            _alpha += Time.deltaTime * 200;
            bgImage.color = new Color32(0, 0, 0, (byte)_alpha);
            yield return null;
        }
        tarot.SetActive(true);
        StartCoroutine(tarot.GetComponent<TarotCard>().CondenseEnum());

        yield return new WaitForSeconds(2f);

        StartCoroutine(tarot.GetComponent<TarotCard>().DissolveExternalEnum());

        while (_alpha > 0)
        {   
            _alpha -= Time.deltaTime * 200;
            if (_alpha < 0) _alpha = 0;
            bgImage.color = new Color32(0, 0, 0, (byte)_alpha);
            yield return null;
        }
        touchBlock.SetActive(false);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
