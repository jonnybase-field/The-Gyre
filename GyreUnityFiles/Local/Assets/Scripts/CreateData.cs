using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CreateData : MonoBehaviour
{

    [Serializable]
    public class ExternalUserData
    {
        public List<userDataJson> users;
    }

    [Serializable]
    public class ExternalImageData
    {
        public List<imageDataJson> images;
    }

    [Serializable]
    public class ExternalGlobalEventData
    {
        public List<globalEventDataJson> globalStory;
    }

    [Serializable]
    public class userDataJson
    {
        public string name;
        public int guideNum;
        public int guideCount;
        public string mediaShown1URL;
        public string mediaShown2URL;
        public string mediaShown3URL;
        public string globalShown1URL;
        public string globalShown2URL;
        public string energyAmount;
        public bool inGyre = false;
    }

    [Serializable]
    public class imageDataJson
    {
        public int imageNumber;
        public string imageDesc1;
        public string imageDesc2;
    }

    [Serializable]
    public class globalEventDataJson
    {
        public int imageNumber;
        public string headline;
        public string text;
    }

    [SerializeField] string jsonURL = "http://google.com"; //The URL to get the Json file from
    [SerializeField] string phpURL;
    [SerializeField] string phpActivateURL;
    [SerializeField] string imageFolderURL;
    [SerializeField] UnityEngine.UI.Image downloadedImageSpace;
    [SerializeField] InputField guideNameInputField;
    [SerializeField] GameObject guideStatsPage;
    public TextMeshProUGUI pollutionStat;
    public TextMeshProUGUI guideNameStat;
    public Button guideStatPageChatButton;
    public Button guideStatPageBackButton;
    private Sprite downloadedSprite;
    public Sprite[] chosenImageSprites = new Sprite[5];
    public Sprite[] chosenEventSprites = new Sprite[4];
    private string userData = "No user data found";
    private string imageData = "No image data found";
    private Coroutine readData;
    private Coroutine sendData;
    public int guideNumber = 3;
    public float contentType = 0;
    public float[] interactionTypes;
    public float hoursADay = 0;
    public float usersAge = 0;
    public string[] imageURLS = new string[3];
    public int[] imageNumbers = new int[3];
    public string[] imageDescriptions;
    public string[] globalStoriesURL = new string[2];
    private string imagesDataFolderURL;
    private string globalStoryDataPath;
    public int[] globalStoryNumbers;
    public string[] globalEventText;
    public float pollutionAmount;
    public string guideName;
    private int guideNum;

    public IEnumerator downloadImagesOnStart()
    {
        yield return StartCoroutine(DownloadImage(imageFolderURL, true, false, "choiceEvent/"));
        yield return StartCoroutine(DownloadImage(imageFolderURL, true, true, "globalStories/"));
        Debug.Log(imagesDataFolderURL);
        yield return StartCoroutine(ReadData(imagesDataFolderURL, false));
        yield return StartCoroutine(readGlobalEventData(globalStoryDataPath));
        gameObject.GetComponent<DialougeManager>().LetsStartButton.GetComponent<Button>().interactable = true;
        yield return null;
    }
    public void readDataButton()
    {
        StartCoroutine(readingData());
    }

    private IEnumerator readingData()
    {
        yield return StartCoroutine(ReadData(jsonURL, true));
        string imageDataFolderURL1 = imageURLS[0];
        Debug.Log(imageDataFolderURL1);
        string imageDataFolderURL2 = imageURLS[1];
        Debug.Log(imageDataFolderURL2);
        for (int i = 1; i < 9; i++)
        {
            string urlSectionToRemove = i + ".png";
            if (imageDataFolderURL1 != null)
            {
                if (imageDataFolderURL1.Contains(urlSectionToRemove))
                {
                    imageDataFolderURL1 = imageDataFolderURL1.Replace(urlSectionToRemove, "imageData.json");
                    imageNumbers[0] = i;
                    Debug.Log("image number 1 is " + i);
                }
            }
            if (imageDataFolderURL2 != null)
            {
                if (imageDataFolderURL2.Contains(urlSectionToRemove))
                {
                    imageDataFolderURL2 = imageDataFolderURL2.Replace(urlSectionToRemove, "imageData.json");
                    imageNumbers[1] = i;
                    Debug.Log("image number 2 is " + i);
                }
            }
        }
        Debug.Log(imageDataFolderURL1);
        yield return StartCoroutine(ReadData(imageDataFolderURL1, false));
    }
    public void sendDataButton()
    {
        sendData = StartCoroutine(SendData(phpURL));
    }

    private IEnumerator DownloadImage(string URL, bool editURL, bool downloadGlobalEvent, string fileType)
    {

        string postEditedURL = "yes";
        if (editURL)
        {
            if (downloadGlobalEvent)
            {
                postEditedURL = generateRandomImageURL(URL, fileType, true);
            }
            else
            {
                postEditedURL = generateRandomImageURL(URL, fileType, false);
            }


        }
        else
        {
            postEditedURL = URL;
        }
        if (!downloadGlobalEvent)
        {
            for (int i = 0; i < imageURLS.Length; i++)
            {
                using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(imageURLS[i]))
                {
                    yield return unityWebRequest.SendWebRequest();
                    if (unityWebRequest.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log("Image download error:" + unityWebRequest.error);
                    }
                    else
                    {
                        Debug.Log("Image download successful");
                        Texture2D downloadedImage = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;
                        chosenImageSprites[i] = Sprite.Create(downloadedImage, new Rect(0f, 0f, downloadedImage.width, downloadedImage.height), new Vector2(0, 0));
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < globalStoriesURL.Length; i++)
            {
                using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(globalStoriesURL[i]))
                {
                    yield return unityWebRequest.SendWebRequest();
                    if (unityWebRequest.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log("Image download error:" + unityWebRequest.error);
                    }
                    else
                    {
                        Debug.Log("Image download successful");
                        Texture2D downloadedImage = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;
                        chosenEventSprites[i] = Sprite.Create(downloadedImage, new Rect(0f, 0f, downloadedImage.width, downloadedImage.height), new Vector2(0, 0));
                    }
                }
            }
        }
    }


    private string generateRandomImageURL(string preEditedURL, string fileType, bool globalEvent)
    {
        int imageNumber = UnityEngine.Random.Range(1, 9);
        string midEditingURL = preEditedURL + fileType;
        if (!globalEvent)
        {
            imagesDataFolderURL = midEditingURL + "imageData.json";
        }
        else
        {

        }
        globalStoryDataPath = midEditingURL + "globalEvents.json";
        midEditingURL += imageNumber.ToString() + ".png";
        if (!globalEvent)
        {
            int urlNum = 0;
            foreach (string url in imageURLS)
            {
                if (url == null)
                {
                    int maxTries = 0;
                    bool gettingURL = true;
                    while (gettingURL)
                    {
                        bool newNumberFound = true;
                        maxTries++;
                        imageNumber = UnityEngine.Random.Range(1, 9);
                        midEditingURL = preEditedURL + fileType;
                        midEditingURL += imageNumber.ToString() + ".png";
                        foreach (string url2 in imageURLS)
                        {
                            if (midEditingURL == url2)
                            {
                                newNumberFound = false;
                            }
                        }
                        if (newNumberFound)
                        {
                            imageURLS[urlNum] = midEditingURL;
                            imageNumbers[urlNum] = imageNumber;
                            gettingURL = false;
                        }
                        if (maxTries > 50)
                        {
                            Debug.Log("max tries reached");
                            break;
                        }
                    }
                }
                Debug.Log("Image URL is " + imageURLS[urlNum] + " and its number is " + imageNumber);
                urlNum++;
            }
        }
        else
        {
            int urlNum = 0;
            foreach (string url in globalStoriesURL)
            {
                if (url == null)
                {
                    int maxTries = 0;
                    bool gettingURL = true;
                    while (gettingURL)
                    {
                        bool newNumberFound = true;
                        maxTries++;
                        imageNumber = UnityEngine.Random.Range(1, 9);
                        midEditingURL = preEditedURL + fileType;
                        midEditingURL += imageNumber.ToString() + ".png";
                        foreach (string url2 in globalStoriesURL)
                        {
                            if (midEditingURL == url2)
                            {
                                newNumberFound = false;
                            }
                        }
                        if (newNumberFound)
                        {
                            globalStoriesURL[urlNum] = midEditingURL;
                            globalStoryNumbers[urlNum] = imageNumber;
                            gettingURL = false;
                        }
                        if (maxTries > 50)
                        {
                            Debug.Log("max tries reached");
                            break;
                        }
                    }
                }
                Debug.Log("Global story URL is " + globalStoriesURL[urlNum]);
                urlNum++;
            }
        }
        return midEditingURL;
    }

    public IEnumerator SendData(string phpAddress)
    {
        List<IMultipartFormSection> dataForm = new List<IMultipartFormSection>();

        dataForm.Add(new MultipartFormDataSection("mediaURL1", imageURLS[0]));
        dataForm.Add(new MultipartFormDataSection("mediaURL2", imageURLS[1]));
        dataForm.Add(new MultipartFormDataSection("mediaURL3", imageURLS[2]));
        dataForm.Add(new MultipartFormDataSection("globalURL1", globalStoriesURL[0]));
        dataForm.Add(new MultipartFormDataSection("globalURL2", globalStoriesURL[1]));
        dataForm.Add(new MultipartFormDataSection("guideModelNumber", guideNumber.ToString()));
        dataForm.Add(new MultipartFormDataSection("energyAmount", pollutionAmount.ToString()));
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(phpAddress, dataForm))
        {
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Image download error:" + unityWebRequest.error);
            }
            else
            {
                Debug.Log("Status Code: " + unityWebRequest.responseCode);
                yield return StartCoroutine(getGuideName());
            }
        }
    }

    public IEnumerator activateGuide()
    {
        List<IMultipartFormSection> dataForm = new List<IMultipartFormSection>();
        dataForm.Add(new MultipartFormDataSection("guideModelNumber", guideNum.ToString()));
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(phpActivateURL, dataForm))
        {
            yield return unityWebRequest.SendWebRequest();

            Debug.Log("Status Code: " + unityWebRequest.responseCode);
        }
    }

    public void toggleMainMenuActive()
    {
        GameObject mainMenu = GameObject.FindGameObjectWithTag("mainMenu");
        if (mainMenu.transform.GetChild(0).gameObject.activeSelf)
        {
            foreach (Transform child in mainMenu.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform child in mainMenu.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        gameObject.GetComponent<DialougeManager>().resetGuide();
    }

    public IEnumerator getGuideName()
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(jsonURL))
        {
            Debug.Log("Attempting Connection");
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error:" + unityWebRequest.error);
            }
            else
            {
                Debug.Log("Connection Successful");
                string guideNames = unityWebRequest.downloadHandler.text;
                ExternalUserData newJasonData = JsonUtility.FromJson<ExternalUserData>(guideNames);
                guideName = newJasonData.users[newJasonData.users.Count - 1].name;
                guideNum = newJasonData.users[newJasonData.users.Count - 1].guideCount;
                Debug.Log(guideNum);
                Debug.Log(guideName);
                yield return guideName;
            }
        }
    }


    private IEnumerator readGlobalEventData(string URL)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(URL))
        {
            Debug.Log("Attempting Connection");
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error:" + unityWebRequest.error);
            }
            else
            {
                Debug.Log("Connection Successful");
                string globalEventData = unityWebRequest.downloadHandler.text;
                ExternalGlobalEventData newJasonData = JsonUtility.FromJson<ExternalGlobalEventData>(globalEventData);
                int storyNum = 0;
                foreach (int number in globalStoryNumbers)
                {
                    foreach (globalEventDataJson value in newJasonData.globalStory)
                    {
                        if (value.imageNumber == number)
                        {
                            Debug.Log("This is the tagline: " + value.text);
                            globalEventText[storyNum] = value.text;
                            storyNum++;
                        }
                    }
                }
                yield return null;
            }
        }
    }

    private IEnumerator ReadData(string URL, bool loadGuide) //The gather data coroutine function
    {
        imageDescriptions = new string[imageURLS.Length * 2];
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(URL))
        {
            Debug.Log(URL);
            Debug.Log("Attempting Connection");
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error:" + unityWebRequest.error);
            }
            else
            {
                Debug.Log("Connection Successful");
                bool doesGuideExist = false;
                if (loadGuide)
                {
                    userData = unityWebRequest.downloadHandler.text;
                    ExternalUserData newJasonData = JsonUtility.FromJson<ExternalUserData>(userData);
                    foreach (userDataJson value in newJasonData.users)
                    {
                        if (value.name == guideNameInputField.text)
                        {
                            gameObject.GetComponent<DialougeManager>().resetGuide();
                            Debug.Log(value.name + " exists");
                            doesGuideExist = true;
                            guideStatsPage.SetActive(true);
                            gameObject.GetComponent<DialougeManager>().onGuideLoadScreen = true;
                            pollutionStat.text += value.energyAmount;
                            guideNameStat.text = value.name;
                            guideStatsPage.GetComponent<DialogueTrigger>().triggerDialogue();
                            foreach (Transform child in gameObject.GetComponent<DialougeManager>().loadMenu.transform)
                            {
                                child.gameObject.SetActive(false);
                            }
                        }
                    }
                    if (!doesGuideExist)
                    {
                        Debug.Log(guideNameInputField.text + " does not exist");
                        StopAllCoroutines();
                    }
                }
                else
                {
                    imageData = unityWebRequest.downloadHandler.text;
                    ExternalImageData newImageJasonData = JsonUtility.FromJson<ExternalImageData>(imageData);
                    int imageNumberChanging = 0;
                    foreach (int imageNum in imageNumbers)
                    {
                        foreach (imageDataJson value in newImageJasonData.images)
                        {

                            if (value.imageNumber == imageNum)
                            {
                                Debug.Log(value.imageDesc1);
                                imageDescriptions[imageNumberChanging] = value.imageDesc1;
                                Debug.Log(value.imageDesc2);
                                imageDescriptions[imageNumberChanging + 1] = value.imageDesc2;
                                imageNumberChanging += 2;
                            }
                        }
                    }
                }
            }
        }
    }
}
