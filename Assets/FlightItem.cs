﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;

namespace Assets
{
    public class FlightItem
    {
        /// <summary>
        /// Unique ICAO 24-bit address of the transponder in hex string representation.
        /// </summary>
        public string Icao24 { get; set; }

        /// <summary>
        /// Callsign of the vehicle (8 chars). Can be null if no callsign has been received.
        /// </summary>
        public string CallSign  { get; set; }

        /// <summary>
        /// Country name inferred from the ICAO 24-bit address.
        /// </summary>
        public string OriginCountry { get; set; }

        /// <summary>
        /// Unix timestamp (seconds) for the last position update.Can be null if no position report was received by OpenSky within the past 15s.
        /// </summary>
        public long? TimePosition { get; set; }

        /// <summary>
        /// Unix timestamp (seconds) for the last velocity update.Can be null if no velocity report was received by OpenSky within the past 15s.
        /// </summary>
        public long? TimeVelocity { get; set; }

        /// <summary>
        /// WGS-84 longitude in decimal degrees. Can be null.
        /// </summary>
        public float? Longitude { get; set; }

        /// <summary>
        /// WGS-84 latitude in decimal degrees. Can be null.
        /// </summary>
        public float? Latitude { get; set; }

        /// <summary>
        /// Barometric or geometric altitude in meters.Can be null.
        /// </summary>
        public float? Altitude { get; set; }

        /// <summary>
        /// Value which indicates if the position was retrieved from a surface position report.
        /// </summary>
        public bool OnGround { get; set; }

        /// <summary>
        /// Velocity over ground in m/s.Can be null.
        /// </summary>
        public float? Velocity { get; set; }

        /// <summary>
        ///  Heading in decimal degrees clockwise from north (i.e.north= 0°). Can be null.
        /// </summary>
        public float? Heading { get; set; }

        /// <summary>
        /// Vertical rate in m/s.A positive value indicates that the airplane is climbing, a negative value indicates that it descends.Can be null.
        /// </summary>
        public float? VerticalRotates { get; set; }

        /// <summary>
        /// IDs of the receivers which contributed to this state vector.Is null if no filtering for sensor was used in the request.
        /// </summary>
        public int[] SensorIds { get; set; }

        public static FlightItem FromArray(JSONArray array)
        {
            var result = new FlightItem();

            result.Icao24 = array[0].IsNull ? null : array[0].Value;
            result.CallSign = array[1].IsNull ? null : array[1].Value;
            result.OriginCountry = array[2].IsNull ? null : array[2].Value;
            result.TimePosition = array[3].IsNull ? (int?)null : array[3].AsInt; 
            result.TimeVelocity = array[4].IsNull ? (int?)null : array[4].AsInt; 
            result.Longitude = array[5].IsNull ? (float?)null : array[5].AsFloat;
            result.Latitude = array[6].IsNull ? (float?)null : array[6].AsFloat;
            result.Altitude = array[7].IsNull ? (float?)null : array[7].AsFloat;
            result.OnGround = !array[8].IsNull && array[8].AsBool;
            result.Velocity = array[9].IsNull ? (float?)null : array[9].AsFloat;
            result.Heading = array[10].IsNull ? (float?)null : array[10].AsFloat;
            result.VerticalRotates = array[11].IsNull ? (float?)null : array[11].AsFloat;
            result.SensorIds = new int[0];

            return result;
        }
    }
}
