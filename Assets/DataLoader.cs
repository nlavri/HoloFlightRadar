using System;
using System.Collections;
using System.Linq;
using Assets;
using SimpleJSON;
//using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class DataLoader : MonoBehaviour
{
    public GameObject earth;

    // Use this for initialization
    void Start()
    {
        return;
        GetFlightsData();
        //StartCoroutine(GetFlightsData());
    }

    IEnumerator GetFlightsData()
    {
        while (true)
        {
            var webRequest = UnityWebRequest.Get("https://nlavri:nlavriPass55@opensky-network.org/api/states/all");
            //yield return webRequest.Send();
            webRequest.Send();
            while (!webRequest.isDone && !webRequest.isError)
            {

            }

            if (webRequest.isError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                // Show results as text
                var downloadHandlerText = webRequest.downloadHandler.text;
                Debug.Log(downloadHandlerText);

                try
                {

                    //flightsDataReponse = JsonConvert.DeserializeObject<FlightsDataReponse>(downloadHandlerText);
                    var json = JSON.Parse(downloadHandlerText);
                    Debug.Log(json["time"]);
                    var states = json["states"].AsArray;
                    if (states != null)
                    {
                        //foreach (JSONNode jsonNode in states)
                        //{
                        //    var itemArray = jsonNode.AsArray;
                        //    Debug.Log(itemArray[0]);
                        //}
                        var result = states.Cast<JSONNode>().Select(x => FlightItem.FromArray(x.AsArray)).ToList();
                        Debug.Log(result.First().Icao24);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    throw;
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
