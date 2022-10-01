using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace ThinIce.Helpers
{
    public class Telegram: MonoBehaviour
    {
        public string chat_id = "";
        public string TOKEN = "";
        public string API_URL => $"https://api.telegram.org/bot{TOKEN}/";

        public void GetMe()
        {
            WWWForm form = new WWWForm();
            UnityWebRequest www = UnityWebRequest.Post(API_URL + "getMe", form);
            StartCoroutine(SendRequest(www));
        }

        public void SendFile(byte[] bytes, string filename, string caption = "")
        {
            WWWForm form = new WWWForm();
            form.AddField("chat_id", chat_id);
            form.AddField("caption", caption);
            form.AddBinaryData("document", bytes, filename, "filename");
            UnityWebRequest www = UnityWebRequest.Post(API_URL + "sendDocument?", form);
            StartCoroutine(SendRequest(www));
        }

        public void SendPhoto(byte[] bytes, string filename, string caption = "")
        {
            WWWForm form = new WWWForm();
            form.AddField("chat_id", chat_id);
            form.AddField("caption", caption);
            form.AddBinaryData("photo", bytes, filename, "filename");
            UnityWebRequest www = UnityWebRequest.Post(API_URL + "sendPhoto?", form);
            StartCoroutine(SendRequest(www));
        }

        public new void SendMessage(string text)
        {
            WWWForm form = new WWWForm();
            form.AddField("chat_id", chat_id);
            form.AddField("text", text);
            UnityWebRequest www = UnityWebRequest.Post(API_URL + "sendMessage?", form);
            StartCoroutine(SendRequest(www));
        }

        IEnumerator SendRequest(UnityWebRequest www)
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                var w = www;
                Debug.Log("Success!\n" + www.downloadHandler.text);
            }
        }
    }
}