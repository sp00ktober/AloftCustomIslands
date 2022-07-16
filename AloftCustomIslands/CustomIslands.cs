using BepInEx;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace AloftCustomIslands
{
    [BepInPlugin("com.sp00ktober.AloftCustomIslands", "AloftCustomIslands", "0.0.1")]
    public class CustomIslands : BaseUnityPlugin
    {
        private void Awake()
        {
            InitPatches();
        }

        private static void InitPatches()
        {
            Debug.Log("Patching Aloft...");

            try
            {
                Debug.Log("Applying patches from AloftCustomIslands 0.0.1");

                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "com.sp00ktober.de");

                Debug.Log("Patching completed successfully");
            }
            catch (Exception ex)
            {
                Debug.Log("Unhandled exception occurred while patching the game: " + ex);
            }
        }
    }
}
