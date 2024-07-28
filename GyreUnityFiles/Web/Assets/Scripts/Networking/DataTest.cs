using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DataTest : MonoBehaviour
{
    [Serializable]
    public class comparisons
    {
        public string comparisonTitle;
        public string comparisonMain;
        public float Co2PerUnit;
    }

    [SerializeField] string WebsiteURL = "http://google.com"; //The URL to get the Json file from
    [SerializeField] GameObject playerPrefab; //The player prefab object that is created for each data piece
    [SerializeField] float playerOffset; //The offset each player is created at  
    [SerializeField] bool compareDataSize;
    [SerializeField] float dataCheckRate;
    [SerializeField] comparisons[] comparisonList;
    private Coroutine collectData;
    private Coroutine downloadData;
    private Coroutine processExistingData;
    private string websiteText;
    private int currentUsersAmount;
    private bool initalUsersCreated = false;
    public List<int> currentUsers;


    public float gyreTotalCarbon = 0;

    [SerializeField] GameObject retryConnectionBox;

    [SerializeField] Sprite[] GuideImages;


    [SerializeField] GameObject[] guideOverlayImagesPortrait;
    [SerializeField] GameObject latestGuideImagePortrait;
    [SerializeField] GameObject latestGuideNamePortrait;
    [SerializeField] GameObject totalGyreCarbonPortrait;
    [SerializeField] GameObject guideTotalUIPortrait;
    public GameObject comparisonCarPortrait;
    public GameObject comparisonPhonePortrait;
    public GameObject comparisonKettlePortrait;


            [SerializeField] GameObject[] guideOverlayImagesLandscape;

    [SerializeField] GameObject latestGuideImageLandscape;

    [SerializeField] GameObject latestGuideNameLandscape;
    [SerializeField] GameObject totalGyreCarbonLandscape;
    
    [SerializeField] GameObject guideTotalUILandscape;
    public GameObject comparisonCarLandscape;
    public GameObject comparisonPhoneLandscape;
    public GameObject comparisonKettleLandscape;


    private bool[] imageGotten = new bool[5];
    private int totalGuides = 0;

    [Serializable]
    public class ExternalData
    {
        public string testString;
        public List<userDataJson> users;
    }

    [Serializable]
    public class userDataJson
    {
        public string name;
        public int guideNumber;
        public string mediaURL1;
        public string mediaURL2;
        public string mediaURL3;
        public string globalURL1;
        public string globalURL2;
        public float energyAmount;
        public int guideCount;
        public bool inGyre;
    }

    void Start()
    {
        currentUsers = new List<int>();
        readJson();
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
         
        }
    }
    public void readJson()
    {
        collectData = StartCoroutine(GatherData(WebsiteURL));
        retryConnectionBox.SetActive(false);
    }
    private IEnumerator GatherData(string URL) //The gather data coroutine function
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(URL))
        {
            Debug.Log("Attempting Connection");
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error:" + unityWebRequest.error);
                retryConnectionBox.SetActive(true);
            }
            else
            {
                Debug.Log("Connection Successful");
                websiteText = unityWebRequest.downloadHandler.text;
                ExternalData newJasonData = JsonUtility.FromJson<ExternalData>(websiteText);
                processExistingData = StartCoroutine(processAllJsonData(websiteText, initalUsersCreated));
                currentUsersAmount = newJasonData.users.Count;
            }
        }
    }

    private IEnumerator processAllJsonData(string data, bool haveInitalUsersBeenCreated)
    {
        ExternalData newJasonData = JsonUtility.FromJson<ExternalData>(data);
        int playerCount = 0;
        foreach (userDataJson value in newJasonData.users)
        {
            if (value.inGyre)
            {
                bool newGuide = true;
                foreach (int num in currentUsers)
                {
                    if (newJasonData.users[playerCount].guideCount == num)
                    {
                        newGuide = false;
                    }
                }
                Debug.Log(playerCount + " guide is " +  newGuide);
                if (newGuide)
                {
                    totalGuides++;
                    imageGotten = new bool[5];
                    currentUsers.Add((newJasonData.users[playerCount].guideCount));
                    var newPlayer = Instantiate(playerPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                    newPlayer.name = value.name;
                    gyreTotalCarbon += value.energyAmount;
                    totalGyreCarbonPortrait.GetComponent<TextMeshProUGUI>().text = gyreTotalCarbon.ToString("F1");
                    totalGyreCarbonLandscape.GetComponent<TextMeshProUGUI>().text = gyreTotalCarbon.ToString("F1");
                    guideTotalUIPortrait.GetComponent<TextMeshProUGUI>().text = totalGuides.ToString();
                    guideTotalUILandscape.GetComponent<TextMeshProUGUI>().text = totalGuides.ToString();
                    updateComparison();
                    newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().userName = value.name;
                    newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().imagesShown = new string[5];
                    newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().globalShown = new string[5];
                    newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenImages = new Sprite[5];
                    newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenGlobal = new Sprite[5];
                    newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().guideName.GetComponent<TextMeshProUGUI>().text = value.name;
                    if (value.mediaURL1 != null && value.mediaURL1 != "none")
                    {
                        newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().imagesShown[0] = value.mediaURL1;
                        imageGotten[0] = true;
                    }
                    if (value.mediaURL2 != null && value.mediaURL2 != "none")
                    {
                        newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().imagesShown[1] = value.mediaURL2;
                        imageGotten[1] = true;
                    }
                    if (value.mediaURL3 != null && value.mediaURL3 != "none")
                    {
                        newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().imagesShown[2] = value.mediaURL3;
                        imageGotten[2] = true;
                    }
                    if (value.globalURL1 != null && value.globalURL1 != "none")
                    {
                        newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().globalShown[0] = value.globalURL1;
                        imageGotten[3] = true;
                    }
                    if (value.globalURL2 != null && value.globalURL2 != "none")
                    {
                        newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().globalShown[1] = value.globalURL2;
                        imageGotten[4] = true;
                    }
                    newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().energyAmount = value.energyAmount;
                    newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().models.transform.GetChild(value.guideNumber - 1).gameObject.SetActive(true);
                    if (value.guideNumber == 1)
                    {
                        newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().makeTenticles();
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        if (newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().imagesShown[i] != null && newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().imagesShown[i] != "none")
                        {
                            UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().imagesShown[i]);
                            yield return unityWebRequest.SendWebRequest();
                            if (unityWebRequest.result != UnityWebRequest.Result.Success)
                            {
                                Debug.Log("Image download error:" + unityWebRequest.error);
                            }
                            else
                            {
                                Debug.Log("Image download successful");
                                Texture2D downloadedImage = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;
                                Sprite downloadedSprite = Sprite.Create(downloadedImage, new Rect(0f, 0f, downloadedImage.width, downloadedImage.height), new Vector2(0.5f, 0.5f));
                                newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenImages[i] = downloadedSprite;
                            }
                        }
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        if (newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().globalShown[i] != null && newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().globalShown[i] != "none")
                        {
                            UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().globalShown[i]);
                            yield return unityWebRequest.SendWebRequest();
                            if (unityWebRequest.result != UnityWebRequest.Result.Success)
                            {
                                Debug.Log("Image download error:" + unityWebRequest.error);
                            }
                            else
                            {
                                Debug.Log("Image download successful");
                                Texture2D downloadedImage = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;
                                Sprite downloadedSprite = Sprite.Create(downloadedImage, new Rect(0f, 0f, downloadedImage.width, downloadedImage.height), new Vector2(0.5f, 0.5f));
                                newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenGlobal[i] = downloadedSprite;
                            }
                        }
                    }
                    if (imageGotten[0])
                    {
                        newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().image1.GetComponent<SpriteRenderer>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenImages[0];
                        guideOverlayImagesPortrait[0].SetActive(true);
                        guideOverlayImagesPortrait[0].GetComponent<Image>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenImages[0];
                          guideOverlayImagesLandscape[0].SetActive(true);
                        guideOverlayImagesLandscape[0].GetComponent<Image>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenImages[0];
                    }
                    else
                    {
                        guideOverlayImagesPortrait[0].SetActive(false);                        
                        guideOverlayImagesLandscape[0].SetActive(false);

                    }
                    if (imageGotten[1])
                    {
                        newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().image2.GetComponent<SpriteRenderer>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenImages[1];
                        guideOverlayImagesPortrait[1].SetActive(true);
                        guideOverlayImagesLandscape[1].SetActive(true);
                        guideOverlayImagesPortrait[1].GetComponent<Image>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenImages[1];
                        guideOverlayImagesLandscape[1].GetComponent<Image>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenImages[1];

                    }
                    else
                    {
                        guideOverlayImagesPortrait[1].SetActive(false);
                        guideOverlayImagesLandscape[1].SetActive(false);
                    }
                    if (imageGotten[2])
                    {
                        newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().image3.GetComponent<SpriteRenderer>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenImages[2];
                        guideOverlayImagesPortrait[2].SetActive(true);
                        guideOverlayImagesPortrait[2].GetComponent<Image>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenImages[2];
                        guideOverlayImagesLandscape[2].SetActive(true);
                        guideOverlayImagesLandscape[2].GetComponent<Image>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenImages[2];
                    }
                    else
                    {
                        guideOverlayImagesPortrait[2].SetActive(false);
                         guideOverlayImagesLandscape[2].SetActive(false);
                    }
                    if (imageGotten[3])
                    {
                        newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().image4.GetComponent<SpriteRenderer>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenGlobal[0];
                        guideOverlayImagesPortrait[3].SetActive(true);
                        guideOverlayImagesPortrait[3].GetComponent<Image>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenGlobal[0];
                        guideOverlayImagesLandscape[3].SetActive(true);
                        guideOverlayImagesLandscape[3].GetComponent<Image>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenGlobal[0];
                    }
                    else
                    {
                        guideOverlayImagesPortrait[3].SetActive(false);
                        guideOverlayImagesLandscape[3].SetActive(false);
                    }
                    if (imageGotten[4])
                    {
                        newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().image5.GetComponent<SpriteRenderer>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenGlobal[1];
                        guideOverlayImagesPortrait[4].SetActive(true);
                        guideOverlayImagesPortrait[4].GetComponent<Image>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenGlobal[1];
                          guideOverlayImagesLandscape[4].SetActive(true);
                        guideOverlayImagesLandscape[4].GetComponent<Image>().sprite = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().chosenGlobal[1];
                    }
                    else
                    {
                        guideOverlayImagesPortrait[4].SetActive(false);
                         guideOverlayImagesLandscape[4].SetActive(false);
                    }
                    int randomInt = Random.Range(0, newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().sounds.Length);
                    newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().uniqueSound.clip = newPlayer.transform.GetChild(0).GetComponent<UniqueInfo>().sounds[randomInt];
                    latestGuideImagePortrait.GetComponent<Image>().sprite = GuideImages[value.guideNumber - 1];
                    latestGuideImageLandscape.GetComponent<Image>().sprite = GuideImages[value.guideNumber - 1];
                    latestGuideNamePortrait.GetComponent<TextMeshProUGUI>().text = value.name;
                    latestGuideNameLandscape.GetComponent<TextMeshProUGUI>().text = value.name;
                }
                playerCount++;
            }

        }
        if (!initalUsersCreated)
        {
            InvokeRepeating("readJson", 0, dataCheckRate);
        }
        initalUsersCreated = true;
    }

    public void updateComparison()
    {
        float carNum = gyreTotalCarbon / comparisonList[0].Co2PerUnit;
        comparisonCarPortrait.GetComponent<TextMeshProUGUI>().text = "Equvialent to driving " + System.Math.Round(carNum, 2) + "km";
        comparisonCarLandscape.GetComponent<TextMeshProUGUI>().text = "Equvialent to driving " + System.Math.Round(carNum, 2) + "km";
        float phoneNum = gyreTotalCarbon / comparisonList[1].Co2PerUnit;
        comparisonPhonePortrait.GetComponent<TextMeshProUGUI>().text = System.Math.Round(phoneNum, 2) + " hours of charging";
        comparisonPhoneLandscape.GetComponent<TextMeshProUGUI>().text = System.Math.Round(phoneNum, 2) + " hours of charging";
        float kettleNum = gyreTotalCarbon / comparisonList[2].Co2PerUnit;
        comparisonKettlePortrait.GetComponent<TextMeshProUGUI>().text = "Boiling " + System.Math.Round(kettleNum, 2) + " cups of water";
        comparisonKettleLandscape.GetComponent<TextMeshProUGUI>().text = "Boiling " + System.Math.Round(kettleNum, 2) + " cups of water";

    }

}


