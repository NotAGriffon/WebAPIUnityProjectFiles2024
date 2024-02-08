using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using TMPro;

public class JsonSerializer : MonoBehaviour
{
    // Variables
    public DataClass dataObj;
    public string filePath;
    public TMP_InputField playerName, level, elapsedTime;

    // Start
    void Start()
    {
        filePath = Path.Combine(Application.dataPath, "saveData.txt");
        dataObj = new DataClass();
        dataObj.level = 1;
        dataObj.timeElapsed = 212f;
        dataObj.name = "Christopher";

        string json = JsonUtility.ToJson(dataObj);

        StartCoroutine(SendWebData(json));

        File.WriteAllText(filePath, json);
    }

    // Update
    void Update()
    {

    }

    // Send Button
    public void SendButton()
    {
        // Local Variables
        var levelData = int.Parse(level.text);
        var timeData = float.Parse(elapsedTime.text);

        DataClass formData = new DataClass();
        formData.level = levelData;
        formData.timeElapsed = timeData;
        formData.name = playerName.text;

        string json = JsonUtility.ToJson(formData);
        filePath = Path.Combine(Application.dataPath, "formData.txt");
        File.WriteAllText(filePath, json);

        StartCoroutine(SendWebData(json));
    }

    public void GetButton()
    {
        StartCoroutine(GetRequest("http://localhost:3000/getUnity"));
    }

    // Send Web Data
    IEnumerator SendWebData(string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/unity", json, "application-json"))
        {
            request.SetRequestHeader("content-type", "application/json");
            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));

            yield return request.SendWebRequest();

            if(request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Data Passed");
            }
            request.uploadHandler.Dispose();
        }
    }

    // Get Request
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest getRequest = UnityWebRequest.Get(uri))
        {
            yield return getRequest.SendWebRequest();

            var newData = System.Text.Encoding.UTF8.GetString(getRequest.downloadHandler.data);
            Debug.Log(newData);
            var newGetRequestData = JsonUtility.FromJson<DataClass>(newData);

            Debug.Log(newGetRequestData.name);
            Debug.Log(newGetRequestData.level);
            Debug.Log(newGetRequestData.timeElapsed);
        }
    }
}