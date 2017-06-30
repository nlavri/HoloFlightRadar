using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Assets;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class DataLoader : MonoBehaviour
{
    private WebClient webClient;
    private FlightsDataReponse flightsDataReponse;

    public GameObject earth;

    // Use this for initialization
    void Start()
    {

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
                
                    flightsDataReponse = JsonConvert.DeserializeObject<FlightsDataReponse>(downloadHandlerText);
                    Debug.Log(flightsDataReponse.Time);
                    if (flightsDataReponse.States != null)
                    {
                        Debug.Log(flightsDataReponse.States.Length);
                        var result = flightsDataReponse.States.Select(FlightItem.FromArray).ToList();
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
