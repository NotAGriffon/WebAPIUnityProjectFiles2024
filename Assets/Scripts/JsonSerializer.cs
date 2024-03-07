using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.Networking;
public class JsonSerializer : MonoBehaviour
{
    public DataClass dataObj;
    public string filePath;
    public TMP_InputField playerID, screenName, firstName, lastName, date, score;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Path.Combine(Application.dataPath, "saveData.txt");

        string jsonOBJ = JsonUtility.ToJson(dataObj);
        Debug.Log(jsonOBJ);
        File.WriteAllText(filePath, jsonOBJ);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendButton()
    {
        var dateData = int.Parse(date.text);
        var scoreData = int.Parse(score.text);

        PlayerDatum formData = new PlayerDatum();
        formData.screenName = screenName.text;
        formData.firstName = firstName.text;
        formData.lastName = lastName.text;
        formData.date = dateData;
        formData.score = scoreData;

        string json =  JsonUtility.ToJson(formData);
        Debug.Log(json);
        filePath = Path.Combine(Application.dataPath, "formData.txt");
        File.WriteAllText(filePath, json);
        StartCoroutine(SendWebData(json));
    }

    public void EditButton()
    {
        var dateData = int.Parse(date.text);
        var scoreData = int.Parse(score.text);

        DataClass formData = new DataClass();
        formData._id = playerID.text;
        formData.screenName = screenName.text;
        formData.firstName = firstName.text;
        formData.lastName = lastName.text;
        formData.date = dateData;
        formData.score = scoreData;

        string json = JsonUtility.ToJson(formData);
        filePath = Path.Combine(Application.dataPath, "formDataUpdate.txt");
        File.WriteAllText(filePath, json);
        StartCoroutine(EditRecord(json));
    }

    public void DeleteButton()
    {
        DataClass formData = new DataClass();
        formData._id = playerID.text;
        string json = JsonUtility.ToJson(formData);
        filePath = Path.Combine(Application.dataPath, "formData.txt");
        File.WriteAllText(filePath, json);
        StartCoroutine(DeleteRecord(json));
    }

    public void GetButton()
    {
        StartCoroutine(GetRequest("http://localhost:3000/getUnity"));
    }

    public void FindButton()
    {
        StartCoroutine(NameSearch("http://localhost:3000/getUnity"));
    }

    IEnumerator SendWebData(string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/unity", json, "application-json"))
        {
            request.SetRequestHeader("content-type", "application/json");

            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Data Posted");
            }
            request.uploadHandler.Dispose();
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest getRequest = UnityWebRequest.Get(uri))
        {
            yield return getRequest.SendWebRequest();

            var newData = System.Text.Encoding.UTF8.GetString(getRequest.downloadHandler.data);
            Debug.Log(newData);
            var newGetRequestData = JsonUtility.FromJson<DataClass>(newData);
            Debug.Log(newGetRequestData);
            Debug.Log(newGetRequestData);

            var newGetRoot = JsonUtility.FromJson<Root>(newData);

            List<string> nameList = new List<string>();

            for (int i = 0; i < newGetRoot.playerdata.Length; i++)
            {
                nameList.Add(newGetRoot.playerdata[i].screenName);
            }
            nameList.Sort();

            for (int j = 0; j < nameList.Count; j++)
            {
                for(int i = 0; i < newGetRoot.playerdata.Length; i++)
                {
                    if(newGetRoot.playerdata[i].screenName == nameList[j])
                    {
                        Debug.Log(newGetRoot.playerdata[i].screenName);
                        Debug.Log(newGetRoot.playerdata[i].firstName);
                        Debug.Log(newGetRoot.playerdata[i].lastName);
                        Debug.Log(newGetRoot.playerdata[i].date);
                        Debug.Log(newGetRoot.playerdata[i].score);
                    }
                }
            }
        }
    }

    IEnumerator NameSearch(string uri)
    {
        using (UnityWebRequest getRequest = UnityWebRequest.Get(uri))
        {
            yield return getRequest.SendWebRequest();

            var newData = System.Text.Encoding.UTF8.GetString(getRequest.downloadHandler.data);
            var newGetRequestData = JsonUtility.FromJson<DataClass>(newData);

            var screenNameToSearch = screenName.text;
            bool screenNameFound = false;

            var newGetRoot = JsonUtility.FromJson<Root>(newData);

            for (int i = 0; i < newGetRoot.playerdata.Length; i++)
            {
                if (screenNameToSearch == newGetRoot.playerdata[i].screenName)
                {
                    Debug.Log(newGetRoot.playerdata[i].screenName);
                    Debug.Log(newGetRoot.playerdata[i].firstName);
                    Debug.Log(newGetRoot.playerdata[i].lastName);
                    Debug.Log(newGetRoot.playerdata[i].date);
                    Debug.Log(newGetRoot.playerdata[i].score);

                    screenNameFound = true;
                }
            }

            if (screenNameFound == false)
            {
                Debug.Log("Name not found.");
            }
        }
    }

    IEnumerator EditRecord(string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/updateUnity", json, "application-json"))
        {
            request.SetRequestHeader("content-type", "application/json");

            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Data Posted");
            }
            request.uploadHandler.Dispose();
        }
    }

    IEnumerator DeleteRecord(string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/deleteUnity", json, "application-json"))
        {
            request.SetRequestHeader("content-type", "application/json");

            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Data Posted");
            }
            request.uploadHandler.Dispose();
        }
    }
}