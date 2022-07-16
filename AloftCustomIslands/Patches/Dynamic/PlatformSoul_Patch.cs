using HarmonyLib;
using UnityEngine;

namespace AloftCustomIslands.Patches.Dynamic
{
    [HarmonyPatch(typeof(PlatformSoul))]
    internal class PlatformSoul_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlatformSoul), "CreatePlatform")]
        public static bool CreatePlatform_Prefix(PlatformSoul __instance, ref PlatformAbstract __result, ScriptableIslandGeneratorTEMP islandData, PlatformGlobalData globalData)
        {
            if(__instance.platformGlobalData.ID == 1000)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(islandData.islandPrefab, __instance.gameObject.transform);
                PlatformAbstract component = gameObject.GetComponent<PlatformAbstract>();
                gameObject.transform.position = __instance.platformGlobalData.position;
                component.pivot.rotation = __instance.platformGlobalData.rotation;
                component.globalData = globalData;
                component.LoadIslandData(PlayerManager_Patch.island, __instance.platformGlobalData.seedA, __instance.platformGlobalData.seedB, __instance.platformGlobalData.seedC);

                __result = component;

                return false;
            }

            return true;
        }
    }
}
