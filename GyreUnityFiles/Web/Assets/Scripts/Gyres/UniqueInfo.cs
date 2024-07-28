using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UniqueInfo : MonoBehaviour
{
    public string userName;
    public string[] imagesShown;
    public string[] globalShown;
    public float energyAmount;
    public Sprite[] chosenImages;
    public Sprite[] chosenGlobal;
    public GameObject image1 = null;
    public GameObject image2 = null;
    public GameObject image3 = null;
    public GameObject image4 = null;
    public GameObject image5 = null;
    public GameObject models;
    public AudioClip[] sounds;
    public AudioSource uniqueSound;
    public GameObject guideName;
    private float randomNum;
    private float randomPitch;
    [SerializeField] GameObject[] tentacles;
    [SerializeField] GameObject tentPos1;
    [SerializeField] GameObject tentPos2;
    [SerializeField] GameObject tentPos3;
    [SerializeField] int tentacleAmount;
    [SerializeField] Color startingColour;
    [SerializeField] Color endColour;
    [SerializeField] float lerpTime;
    [SerializeField] GameObject textGameObj;
    private float t = 0;

    private void Awake()
    {
        randomNum = Random.Range(0f, 1f);
        randomPitch = Random.Range(-1f, 2f);
        StartCoroutine(startSound());
    }

    private void Update()
    {
        t += Time.deltaTime / lerpTime;
        Color change = Color.Lerp(startingColour, endColour, t);
        textGameObj.GetComponent<TextMeshProUGUI>().color = change;
    }

    private IEnumerator startSound()
    {
        yield return new WaitForSeconds(randomNum);
        uniqueSound.pitch = randomPitch;
        uniqueSound.Play();
    }

    public void makeTenticles()
    {
        int tentacleNum = Random.Range(0, tentacles.Length);
        Instantiate(tentacles[tentacleNum], tentPos1.transform.position, Quaternion.identity, tentPos1.transform.parent);
        tentacleNum = Random.Range(0, tentacles.Length);
        Instantiate(tentacles[tentacleNum], tentPos2.transform.position, Quaternion.identity, tentPos2.transform.parent);
        tentacleNum = Random.Range(0, tentacles.Length);
        Instantiate(tentacles[tentacleNum], tentPos3.transform.position, Quaternion.identity, tentPos3.transform.parent);
    }


}
