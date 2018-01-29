#if GAIA_PRESENT && UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using UnityEditor;
using Tenkoku.Effects;

namespace Gaia.GX.TanukiDigital
{
    /// <summary>
    /// Tenkoku Setup
    /// </summary>
    public class Tenkoku_Gaia : MonoBehaviour
    {
        #region Generic informational methods

        /// <summary>
        /// Returns the publisher name if provided. 
        /// This will override the publisher name in the namespace ie Gaia.GX.PublisherName
        /// </summary>
        /// <returns>Publisher name</returns>
        public static string GetPublisherName()
        {
            return "Tanuki Digital";
        }

        /// <summary>
        /// Returns the package name if provided
        /// This will override the package name in the class name ie public class PackageName.
        /// </summary>
        /// <returns>Package name</returns>
        public static string GetPackageName()
        {
            return "Tenkoku";
        }

        #endregion

        #region Methods exposed by Gaia as buttons must be prefixed with GX_

        public static void GX_About()
        {
            EditorUtility.DisplayDialog("About Tenkoku", "Tenkoku is a system for lighting your scenes. Its well matched with Suimono.", "OK");
        }

        /// <summary>
        /// Add Tenkoku to the scene
        /// </summary>
        public static void GX_AddTenkoku()
        {
            //Add the tenkoku prefab to the scene

            //See if we can locate it
            GameObject tenkokuPrefab = Gaia.Utils.GetAssetPrefab("Tenkoku DynamicSky");
            if (tenkokuPrefab == null)
            {
                Debug.LogWarning("Unable to locate Tenkoku - Aborting!");
                return;
            }

            //See if we can locate it
            if (GameObject.Find("Tenkoku DynamicSky") != null)
            {
                Debug.LogWarning("Tenkoku Dynamic Sky already in scene - Aborting!");
                return;
            }

            //See if we can create it
            GameObject tenkokuObj = Instantiate(tenkokuPrefab);
            if (tenkokuObj == null)
            {
                Debug.LogWarning("Unable to creat Tenkoku object - Aborting!");
                return;
            }
            else
            {
                tenkokuObj.name = "Tenkoku DynamicSky";
            }

            //See if we can configure it - via reflection as JS and C# dont play nice
            var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
            if (tenkokuModule != null)
            {
                //Set the reflection probe settings
                FieldInfo enableProbe = tenkokuModule.GetType().GetField("enableProbe", BindingFlags.Public | BindingFlags.Instance);
                if (enableProbe != null) enableProbe.SetValue(tenkokuModule, true);
                FieldInfo reflectionProbeFPS = tenkokuModule.GetType().GetField("reflectionProbeFPS", BindingFlags.Public | BindingFlags.Instance);
                if (reflectionProbeFPS != null) reflectionProbeFPS.SetValue(tenkokuModule, 0.0f);

                //Set scene lighting settings
                //FieldInfo enableProbe = tenkokuModule.GetType().GetField("enableProbe", BindingFlags.Public | BindingFlags.Instance);
                //if (enableProbe != null) enableProbe.SetValue(tenkokuModule, true);
                //RenderSettings.ambientMode = AmbientMode.Flat;
                //RenderSettings.ambientMode = Rendering.AmbientMode.Flat;

                //Add some random clouds
                FieldInfo cloudsAltoStratus = tenkokuModule.GetType().GetField("weather_cloudAltoStratusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (cloudsAltoStratus != null)
                {
                    cloudsAltoStratus.SetValue(tenkokuModule, UnityEngine.Random.Range(0.0f, 0.25f));
                }
                FieldInfo cloudsCirrus = tenkokuModule.GetType().GetField("weather_cloudCirrusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (cloudsCirrus != null)
                {
                    cloudsCirrus.SetValue(tenkokuModule, UnityEngine.Random.Range(0.0f, 0.5f));
                }
                FieldInfo cloudsCumulus = tenkokuModule.GetType().GetField("weather_cloudCumulusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (cloudsCumulus != null)
                {
                    cloudsCumulus.SetValue(tenkokuModule, UnityEngine.Random.Range(0.0f, 0.6f));
                }

                //Set the camera
                Camera camera = Camera.main;
                if (camera == null)
                {
                    camera = FindObjectOfType<Camera>();
                }
                if (camera != null)
                {
                    FieldInfo mainCamera = tenkokuModule.GetType().GetField("mainCamera", BindingFlags.Public | BindingFlags.Instance);
                    if (mainCamera != null)
                    {
                        mainCamera.SetValue(tenkokuModule, camera.transform);
                    }

                    //add fog effect to camera
                    camera.gameObject.AddComponent<TenkokuSkyFog>();

                    //set fog limits based on camera clip distance
                    FieldInfo fogDist = tenkokuModule.GetType().GetField("fogDistance", BindingFlags.Public | BindingFlags.Instance);
                    if (fogDist != null) fogDist.SetValue(tenkokuModule, camera.farClipPlane * 0.99f);  
                }
            }

            //Disable the existing directional light if it exists
            GameObject lightObj = GameObject.Find("Directional Light");
            if (lightObj != null)
            {
                lightObj.SetActive(false);
            }
        }

        public static void GX_SetMorning()
        {
            GameObject tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
            if (tenkokuObj == null)
            {
                Debug.LogWarning("Unable to locate Tenkoku DynamicSky object - Aborting!");
                return;
            }
            //See if we can configure it - via reflection as JS and C# dont play nice
            var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
            if (tenkokuModule != null)
            {
                FieldInfo setHour = tenkokuModule.GetType().GetField("currentHour", BindingFlags.Public | BindingFlags.Instance);
                if (setHour != null) setHour.SetValue(tenkokuModule, 7);
                FieldInfo setMin = tenkokuModule.GetType().GetField("currentMinute", BindingFlags.Public | BindingFlags.Instance);
                if (setMin != null) setMin.SetValue(tenkokuModule, 30);
            }
        }

        /// <summary>
        /// Set the scene light to afternoon
        /// </summary>
        public static void GX_SetNoon()
        {
            GameObject tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
            if (tenkokuObj == null)
            {
                Debug.LogWarning("Unable to locate Tenkoku DynamicSky object - Aborting!");
                return;
            }
            //See if we can configure it - via reflection as JS and C# dont play nice
            var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
            if (tenkokuModule != null)
            {
                FieldInfo setHour = tenkokuModule.GetType().GetField("currentHour", BindingFlags.Public | BindingFlags.Instance);
                if (setHour != null) setHour.SetValue(tenkokuModule, 12);
                FieldInfo setMin = tenkokuModule.GetType().GetField("currentMinute", BindingFlags.Public | BindingFlags.Instance);
                if (setMin != null) setMin.SetValue(tenkokuModule, 0);
            }
        }


        /// <summary>
        /// Set the scene light to evening
        /// </summary>
        public static void GX_SetEvening()
        {
            GameObject tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
            if (tenkokuObj == null)
            {
                Debug.LogWarning("Unable to locate Tenkoku DynamicSky object - Aborting!");
                return;
            }
            //See if we can configure it - via reflection as JS and C# dont play nice
            var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
            if (tenkokuModule != null)
            {
                FieldInfo setHour = tenkokuModule.GetType().GetField("currentHour", BindingFlags.Public | BindingFlags.Instance);
                if (setHour != null) setHour.SetValue(tenkokuModule, 17);
                FieldInfo setMin = tenkokuModule.GetType().GetField("currentMinute", BindingFlags.Public | BindingFlags.Instance);
                if (setMin != null) setMin.SetValue(tenkokuModule, 20);
            }
        }


        /// <summary>
        /// Set the scene light to night
        /// </summary>
        public static void GX_SetNight()
        {
            GameObject tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
            if (tenkokuObj == null)
            {
                Debug.LogWarning("Unable to locate Tenkoku DynamicSky object - Aborting!");
                return;
            }
            //See if we can configure it - via reflection as JS and C# dont play nice
            var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
            if (tenkokuModule != null)
            {
                FieldInfo setHour = tenkokuModule.GetType().GetField("currentHour", BindingFlags.Public | BindingFlags.Instance);
                if (setHour != null) setHour.SetValue(tenkokuModule, 23);
                FieldInfo setMin = tenkokuModule.GetType().GetField("currentMinute", BindingFlags.Public | BindingFlags.Instance);
                if (setMin != null) setMin.SetValue(tenkokuModule, 45);
            }
        }




        /// <summary>
        /// Set scene weather to clear
        /// </summary>
        public static void GX_WeatherClear()
        {
            GameObject tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
            if (tenkokuObj == null)
            {
                Debug.LogWarning("Unable to locate Tenkoku DynamicSky object - Aborting!");
                return;
            }
            //See if we can configure it - via reflection as JS and C# dont play nice
            var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
            if (tenkokuModule != null)
            {
                FieldInfo setCloudAlto = tenkokuModule.GetType().GetField("weather_cloudAltoStratusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudAlto != null) setCloudAlto.SetValue(tenkokuModule, 0.0f);
                FieldInfo setCloudCirrus = tenkokuModule.GetType().GetField("weather_cloudCirrusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudCirrus != null) setCloudCirrus.SetValue(tenkokuModule, 0.0f);
                FieldInfo setCloudCumulus = tenkokuModule.GetType().GetField("weather_cloudCumulusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudCumulus != null) setCloudCumulus.SetValue(tenkokuModule, 0.0f);
                FieldInfo setOvercast = tenkokuModule.GetType().GetField("weather_OvercastAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setOvercast != null) setOvercast.SetValue(tenkokuModule, 0.0f);
                FieldInfo setCloudSpd = tenkokuModule.GetType().GetField("weather_cloudSpeed", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudSpd != null) setCloudSpd.SetValue(tenkokuModule, 0.1f);
                FieldInfo setRainAmt = tenkokuModule.GetType().GetField("weather_RainAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setRainAmt != null) setRainAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setSnowAmt = tenkokuModule.GetType().GetField("weather_SnowAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setSnowAmt != null) setSnowAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setFogAmt = tenkokuModule.GetType().GetField("weather_FogAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setFogAmt != null) setFogAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setWindAmt = tenkokuModule.GetType().GetField("weather_WindAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setWindAmt != null) setWindAmt.SetValue(tenkokuModule, 0.25f);
            }
        }


        /// <summary>
        /// Set scene weather to light clouds
        /// </summary>
        public static void GX_WeatherLightClouds()
        {
            GameObject tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
            if (tenkokuObj == null)
            {
                Debug.LogWarning("Unable to locate Tenkoku DynamicSky object - Aborting!");
                return;
            }
            //See if we can configure it - via reflection as JS and C# dont play nice
            var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
            if (tenkokuModule != null)
            {
                FieldInfo setCloudAlto = tenkokuModule.GetType().GetField("weather_cloudAltoStratusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudAlto != null) setCloudAlto.SetValue(tenkokuModule, 0.1f);
                FieldInfo setCloudCirrus = tenkokuModule.GetType().GetField("weather_cloudCirrusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudCirrus != null) setCloudCirrus.SetValue(tenkokuModule, 0.3f);
                FieldInfo setCloudCumulus = tenkokuModule.GetType().GetField("weather_cloudCumulusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudCumulus != null) setCloudCumulus.SetValue(tenkokuModule, 0.0f);
                FieldInfo setOvercast = tenkokuModule.GetType().GetField("weather_OvercastAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setOvercast != null) setOvercast.SetValue(tenkokuModule, 0.0f);
                FieldInfo setCloudSpd = tenkokuModule.GetType().GetField("weather_cloudSpeed", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudSpd != null) setCloudSpd.SetValue(tenkokuModule, 0.1f);
                FieldInfo setRainAmt = tenkokuModule.GetType().GetField("weather_RainAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setRainAmt != null) setRainAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setSnowAmt = tenkokuModule.GetType().GetField("weather_SnowAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setSnowAmt != null) setSnowAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setFogAmt = tenkokuModule.GetType().GetField("weather_FogAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setFogAmt != null) setFogAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setWindAmt = tenkokuModule.GetType().GetField("weather_WindAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setWindAmt != null) setWindAmt.SetValue(tenkokuModule, 0.25f);
            }
        }


        /// <summary>
        /// Set scene weather to partly cloudy
        /// </summary>
        public static void GX_WeatherPartlyCloudy()
        {
            GameObject tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
            if (tenkokuObj == null)
            {
                Debug.LogWarning("Unable to locate Tenkoku DynamicSky object - Aborting!");
                return;
            }
            //See if we can configure it - via reflection as JS and C# dont play nice
            var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
            if (tenkokuModule != null)
            {
                FieldInfo setCloudAlto = tenkokuModule.GetType().GetField("weather_cloudAltoStratusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudAlto != null) setCloudAlto.SetValue(tenkokuModule, 0.3f);
                FieldInfo setCloudCirrus = tenkokuModule.GetType().GetField("weather_cloudCirrusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudCirrus != null) setCloudCirrus.SetValue(tenkokuModule, 0.6f);
                FieldInfo setCloudCumulus = tenkokuModule.GetType().GetField("weather_cloudCumulusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudCumulus != null) setCloudCumulus.SetValue(tenkokuModule, 0.7f);
                FieldInfo setOvercast = tenkokuModule.GetType().GetField("weather_OvercastAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setOvercast != null) setOvercast.SetValue(tenkokuModule, 0.0f);
                FieldInfo setCloudSpd = tenkokuModule.GetType().GetField("weather_cloudSpeed", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudSpd != null) setCloudSpd.SetValue(tenkokuModule, 0.1f);
                FieldInfo setRainAmt = tenkokuModule.GetType().GetField("weather_RainAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setRainAmt != null) setRainAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setSnowAmt = tenkokuModule.GetType().GetField("weather_SnowAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setSnowAmt != null) setSnowAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setFogAmt = tenkokuModule.GetType().GetField("weather_FogAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setFogAmt != null) setFogAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setWindAmt = tenkokuModule.GetType().GetField("weather_WindAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setWindAmt != null) setWindAmt.SetValue(tenkokuModule, 0.25f);

            }
        }


        /// <summary>
        /// Set scene weather to overcast
        /// </summary>
        public static void GX_WeatherOvercast()
        {
            GameObject tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
            if (tenkokuObj == null)
            {
                Debug.LogWarning("Unable to locate Tenkoku DynamicSky object - Aborting!");
                return;
            }
            //See if we can configure it - via reflection as JS and C# dont play nice
            var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
            if (tenkokuModule != null)
            {
                FieldInfo setCloudAlto = tenkokuModule.GetType().GetField("weather_cloudAltoStratusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudAlto != null) setCloudAlto.SetValue(tenkokuModule, 0.6f);
                FieldInfo setCloudCirrus = tenkokuModule.GetType().GetField("weather_cloudCirrusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudCirrus != null) setCloudCirrus.SetValue(tenkokuModule, 0.8f);
                FieldInfo setCloudCumulus = tenkokuModule.GetType().GetField("weather_cloudCumulusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudCumulus != null) setCloudCumulus.SetValue(tenkokuModule, 1.0f);
                FieldInfo setOvercast = tenkokuModule.GetType().GetField("weather_OvercastAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setOvercast != null) setOvercast.SetValue(tenkokuModule, 1.0f);
                FieldInfo setCloudSpd = tenkokuModule.GetType().GetField("weather_cloudSpeed", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudSpd != null) setCloudSpd.SetValue(tenkokuModule, 0.1f);
                FieldInfo setRainAmt = tenkokuModule.GetType().GetField("weather_RainAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setRainAmt != null) setRainAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setSnowAmt = tenkokuModule.GetType().GetField("weather_SnowAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setSnowAmt != null) setSnowAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setFogAmt = tenkokuModule.GetType().GetField("weather_FogAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setFogAmt != null) setFogAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setWindAmt = tenkokuModule.GetType().GetField("weather_WindAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setWindAmt != null) setWindAmt.SetValue(tenkokuModule, 0.25f);
            }
        }

        /// <summary>
        /// Set scene weather to rain shower
        /// </summary>
        public static void GX_WeatherRainShower()
        {
            GameObject tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
            if (tenkokuObj == null)
            {
                Debug.LogWarning("Unable to locate Tenkoku DynamicSky object - Aborting!");
                return;
            }
            //See if we can configure it - via reflection as JS and C# dont play nice
            var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
            if (tenkokuModule != null)
            {
                FieldInfo setCloudAlto = tenkokuModule.GetType().GetField("weather_cloudAltoStratusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudAlto != null) setCloudAlto.SetValue(tenkokuModule, 0.6f);
                FieldInfo setCloudCirrus = tenkokuModule.GetType().GetField("weather_cloudCirrusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudCirrus != null) setCloudCirrus.SetValue(tenkokuModule, 0.8f);
                FieldInfo setCloudCumulus = tenkokuModule.GetType().GetField("weather_cloudCumulusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudCumulus != null) setCloudCumulus.SetValue(tenkokuModule, 1.0f);
                FieldInfo setOvercast = tenkokuModule.GetType().GetField("weather_OvercastAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setOvercast != null) setOvercast.SetValue(tenkokuModule, 1.0f);
                FieldInfo setCloudSpd = tenkokuModule.GetType().GetField("weather_cloudSpeed", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudSpd != null) setCloudSpd.SetValue(tenkokuModule, 0.1f);
                FieldInfo setRainAmt = tenkokuModule.GetType().GetField("weather_RainAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setRainAmt != null) setRainAmt.SetValue(tenkokuModule, 0.7f);
                FieldInfo setSnowAmt = tenkokuModule.GetType().GetField("weather_SnowAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setSnowAmt != null) setSnowAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setFogAmt = tenkokuModule.GetType().GetField("weather_FogAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setFogAmt != null) setFogAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setWindAmt = tenkokuModule.GetType().GetField("weather_WindAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setWindAmt != null) setWindAmt.SetValue(tenkokuModule, 0.3f);
            }
        }

        /// <summary>
        /// Set scene weather to snow storm
        /// </summary>
        public static void GX_WeatherSnowStorm()
        {
            GameObject tenkokuObj = GameObject.Find("Tenkoku DynamicSky");
            if (tenkokuObj == null)
            {
                Debug.LogWarning("Unable to locate Tenkoku DynamicSky object - Aborting!");
                return;
            }
            //See if we can configure it - via reflection as JS and C# dont play nice
            var tenkokuModule = tenkokuObj.GetComponent("TenkokuModule");
            if (tenkokuModule != null)
            {
                FieldInfo setCloudAlto = tenkokuModule.GetType().GetField("weather_cloudAltoStratusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudAlto != null) setCloudAlto.SetValue(tenkokuModule, 0.6f);
                FieldInfo setCloudCirrus = tenkokuModule.GetType().GetField("weather_cloudCirrusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudCirrus != null) setCloudCirrus.SetValue(tenkokuModule, 0.8f);
                FieldInfo setCloudCumulus = tenkokuModule.GetType().GetField("weather_cloudCumulusAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudCumulus != null) setCloudCumulus.SetValue(tenkokuModule, 1.0f);
                FieldInfo setOvercast = tenkokuModule.GetType().GetField("weather_OvercastAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setOvercast != null) setOvercast.SetValue(tenkokuModule, 1.0f);
                FieldInfo setCloudSpd = tenkokuModule.GetType().GetField("weather_cloudSpeed", BindingFlags.Public | BindingFlags.Instance);
                if (setCloudSpd != null) setCloudSpd.SetValue(tenkokuModule, 0.1f);
                FieldInfo setRainAmt = tenkokuModule.GetType().GetField("weather_RainAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setRainAmt != null) setRainAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setSnowAmt = tenkokuModule.GetType().GetField("weather_SnowAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setSnowAmt != null) setSnowAmt.SetValue(tenkokuModule, 0.7f);
                FieldInfo setFogAmt = tenkokuModule.GetType().GetField("weather_FogAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setFogAmt != null) setFogAmt.SetValue(tenkokuModule, 0.0f);
                FieldInfo setWindAmt = tenkokuModule.GetType().GetField("weather_WindAmt", BindingFlags.Public | BindingFlags.Instance);
                if (setWindAmt != null) setWindAmt.SetValue(tenkokuModule, 0.1f);
            }
        }

        #endregion
    }
}

#endif