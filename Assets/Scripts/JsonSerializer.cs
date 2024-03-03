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
    public TMP_InputField playerName, level, elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Path.Combine(Application.dataPath, "saveData.txt");

        string json = JsonUtility.ToJson(dataObj);
        Debug.Log(json);
        File.WriteAllText(filePath, json);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendButton()
    {
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
            Debug.Log(newGetRoot.playerdata[0].level);

            for (int i = 0; i < newGetRoot.playerdata.Length; i++)
            {
                Debug.Log(newGetRoot.playerdata[i].name);
                Debug.Log(newGetRoot.playerdata[i].level);
                Debug.Log(newGetRoot.playerdata[i].timeElapsed);

            }
        }
    }
}