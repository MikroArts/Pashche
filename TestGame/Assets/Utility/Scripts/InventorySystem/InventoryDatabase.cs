using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine.Networking;
using System.Collections;

namespace InventorySystem
{
    public class InventoryDatabase : MonoBehaviour
    {       
        public List<ItemModel> listOfItems = new List<ItemModel>();

        private string path = "https://mikroarts.com//DidYouSeeMyPashche//StreamingAssets//items.json";
        string jsonString;

        void Start()
        {
            StartCoroutine(GetRequest(path));
            //path = Application.streamingAssetsPath + "/Items.json";
            //UnityWebRequest request = UnityWebRequest.Get(path);
            //database = JsonConvert.DeserializeObject<List<Item>>(request.downloadHandler.text);
        }
        public ItemModel ReturnItemById(int id)
        {
            ItemModel i = listOfItems.First(p => p.itemId == id);
            return i;
        }
        public ItemModel ReturnItemByName(string itemName)
        {
            ItemModel i = listOfItems.First(p => p.itemName == itemName);
            return i;
        }

        IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError)
                {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else
                {
                    //database = JsonConvert.DeserializeObject<List<Item>>(webRequest.downloadHandler.text);
                    listOfItems = JsonConvert.DeserializeObject<List<ItemModel>>(webRequest.downloadHandler.text);                    
                }
            }
        }
    }

}