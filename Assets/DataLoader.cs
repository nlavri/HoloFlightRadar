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
    public GameObject plane;

    // Use this for initialization
    void Start()
    {
        GetFlightsData();
        //StartCoroutine(GetFlightsData());
    }

    IEnumerator GetFlightsData()
    {
        //while (true)
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
                    var json = JSON.Parse(downloadHandlerText);
                    Debug.Log(json["time"]);
                    var states = json["states"].AsArray;
                    if (states != null)
                    {
                        var result = states.Cast<JSONNode>().Select(x => FlightItem.FromArray(x.AsArray)).ToList();
                        var flightItems = result.FirstOrDefault(x => x.Icao24 == "00b22b");
                        //flightItems.Add(new FlightItem()
                        //{
                        //    Latitude = 55.3465f,
                        //    Longitude = 36.9037f,
                        //    CallSign = "REDITEM"
                        //});
                        //lat = 37.8511 long= 56.9136
                        foreach (var flightItem in new[] { flightItems })
                        {
                            if (this.earth != null && this.plane != null && flightItem.Latitude.HasValue && flightItem.Longitude.HasValue)
                            {
                                Debug.Log(flightItem.Latitude);
                                Debug.Log(flightItem.Longitude);

                                var scaleX = this.earth.transform.localScale.x;
                                var radius = this.earth.GetComponent<SphereCollider>().radius;

                                var transformPosition =
                                    Quaternion.AngleAxis(flightItem.Latitude.Value, -Vector3.right) *
                                    Quaternion.AngleAxis(flightItem.Longitude.Value , -Vector3.up) *

                                    //Quaternion.AngleAxis(this.earth.transform.rotation.eulerAngles.y, Vector3.back) *

                                    new Vector3(0, 0, radius * scaleX);


                                Debug.Log(transformPosition);

                                var copy = Instantiate(this.plane);
                                copy.transform.position = this.earth.transform.position + transformPosition;

                                var normal = (copy.transform.position - this.earth.transform.position).normalized;
                                // Here we work out what direction should be pointing forwards.
                                Vector3 forwardsVector =
                                    Vector3.Cross(normal,
                                        new Vector3(Mathf.Cos(Mathf.Deg2Rad * flightItem.Heading.Value),
                                            Mathf.Sin(Mathf.Deg2Rad * flightItem.Heading.Value)));
                                // Finally, compose the two directions back into a single rotation.
                                copy.transform.rotation = Quaternion.LookRotation(forwardsVector, normal);


                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    throw;
                }

            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
