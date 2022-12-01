using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

//<summary>
// Class that connects and receives a json from a local server.
//</summary>
public class WebClient : MonoBehaviour
{
    public static Response res = new Response();

    //<summary>
    // The data received from the server stores in a variable Response.
    //</summary>
    IEnumerator SendData(string data)
    {
        WWWForm form = new WWWForm();
        form.AddField("bundle", "the data");
        string url = "http://localhost:8585";
        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            if(www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                res = JsonUtility.FromJson<Response>(www.downloadHandler.text.Replace('\'', '\"'));
            }
        }

    }

    //<summary>
    // Detect changes on the TimeManager and updates time.
    //</summary>
    public void OnEnable(){
        TimeManager.OnMinuteChanged += TimeCheck;
    }

    //<summary>
    // When TimeManager is disabled it start reseting values.
    //</summary>
    public void OnDisable()
    {
        TimeManager.OnMinuteChanged -= TimeCheck;
    }

    //<summary>
    // Every Minute of the TimeManager gets a response from the server.
    //</summary>
    private void TimeCheck()
    {
        if(TimeManager.Minute % 1 == 0)
        {
            Vector3 fakePos = new Vector3(3.44f, 0, -15.707f);
            string json = EditorJsonUtility.ToJson(fakePos);
            StartCoroutine(SendData(json));
        }
        
    }
}