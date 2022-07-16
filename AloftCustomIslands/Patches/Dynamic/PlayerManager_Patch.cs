using HarmonyLib;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AloftCustomIslands.Patches.Dynamic
{
    [HarmonyPatch(typeof(PlayerManager))]
    internal static class PlayerManager_Patch
    {
        public static s_IslandEditor_Island island = ScriptableObject.CreateInstance<s_IslandEditor_Island>();
        private static string islandPath = "C:\\Sandbox\\max\\DefaultBox\\user\\current\\AppData\\LocalLow\\Astrolabe Interactive\\Aloft\\CustomIslands\\IslandDiscord.json";
        private static bool loaded = false;

        private static Transform playerTrans = null;

        private static bool ValuePressed(Keyboard _keyboard, Key _key)
        {
            bool flag = _keyboard != null && _key > Key.None && _keyboard.allKeys[_key - Key.Space].isPressed;
            return flag;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerManager.CustomFixedUpdate))]
        public static void CustomFixedUpdate_Postfix(PlayerManager __instance)
        {
            if (!GameSession.isInGameSession)
            {
                return;
            }

            PlayerManager playerManager = (PlayerManager)AccessTools.Field(typeof(PlayerMovement), "player").GetValue(__instance.movement);
            playerTrans = playerManager.anatomy.pivot;

            if(ValuePressed(Keyboard.current, Key.L) && !loaded)
            {
                s_IslandEditor_Island.JsonToIsland(File.ReadAllText(islandPath), ref island);

                if (island == null)
                {
                    Debug.Log("[!] could not load island from file");
                    return;
                }
                if(playerTrans == null)
                {
                    Debug.Log("[!] need to wait for player position to update");
                    return;
                }

                if(Level.terrainManager != null)
                {
                    Debug.Log("[i] got terrainManager!");
                    if(Level.terrainManager.platformManager != null)
                    {
                        Debug.Log("[i] got platformManager!");
                        if(Level.terrainManager.platformManager.platformGenerator != null)
                        {
                            Debug.Log("[i] got platformGenerator!");
                            if(Level.terrainManager.platformManager.platformGenerator.islandGenerator != null)
                            {
                                Debug.Log("[i] spawning custom island at player position");

                                PlatformSoul soul = Object.Instantiate<GameObject>(Level.terrainManager.platformManager.platformGenerator.platformSoulPrefab, null).GetComponent<PlatformSoul>();
                                soul.gameObject.name = "custom island";
                                soul.transform.position = playerTrans.position;

                                float seedA = PseudoRandom.Value((float)(256 * 10));
                                float seedB = PseudoRandom.Value((float)(256 * 13));
                                float seedC = PseudoRandom.Value((float)(256 * 17));

                                PlatformGlobalData platformGlobalData = new PlatformGlobalData
                                {
                                    index = 1000,
                                    gameObject = soul.gameObject,
                                    transform = soul.transform,
                                    position = soul.transform.position,
                                    rotation = playerTrans.rotation,
                                    soul = soul,
                                    seedA = seedA,
                                    seedB = seedB,
                                    seedC = seedC,
                                    ID = 1000
                                };

                                soul.platformGlobalData = platformGlobalData;

                                platformGlobalData.CustomStart();
                                platformGlobalData.SetRotation(playerTrans.rotation, true);

                                Level.terrainManager.platformManager.AddPlatform(platformGlobalData);
                            }
                        }
                    }
                }

                loaded = true;

            }
        }
    }
}
