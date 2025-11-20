using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Fantasista;
using HarmonyLib;
using Scripts.CrownPoint;
using Scripts.OutGame.CrownPoint;
using Scripts.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SkipCoinAndRewardScreen.Plugins
{
    internal class SkipCoinAndRewardHook
    {

        [HarmonyPatch(typeof(CrownPointManager))]
        [HarmonyPatch(nameof(CrownPointManager.GetCrownPointData))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPostfix]
        public static void CrownPointManager_GetCrownPointData_Postfix(CrownPointManager __instance, ref CrownPointData __result)
        {
            if (Plugin.Instance.ConfigSkipCrownPoint.Value)
            {
                Logger.Log("__result.CurrentPoints: " + __result.CurrentPoints);
                Logger.Log("__result.SavedPoints: " + __result.SavedPoints);
                Logger.Log("__result.PointsGained: " + __result.PointsGained);

                __result = new CrownPointData()
                {
                    CurrentPoints = __result.CurrentPoints,
                    SavedPoints = __result.CurrentPoints,
                };
                Logger.Log("__result.PointsGained: " + __result.PointsGained);
            }
            Logger.Log("CrownPointManager_GetCrownPointData_Postfix");
        }



        [HarmonyPatch(typeof(ResultCoinExp))]
        [HarmonyPatch(nameof(ResultCoinExp.Start))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPostfix]
        public static void ResultCoinExp_Start_Postfix(ResultCoinExp __instance)
        {
            List<string> output = new List<string>();
            bool toSkip = true;
            if (Plugin.Instance.ConfigDontSkipOnMaxBankedLevel.Value)
            {
                var bankStockCount = __instance.PlayerData.BankStockCount();
                var playerLevel = __instance.PlayerData.CurrentLevel();
                output.Add("bankStockCount: " + bankStockCount);
                output.Add("playerLevel: " + playerLevel);
                if (bankStockCount == 5 && playerLevel != 400)
                {
                    toSkip = false;
                }
            }

            if (toSkip)
            {
                __instance.gameObject.SetActive(false);
                __instance.m_state = ResultCoinExp.State.Show;
                __instance.Hide();
                __instance.Skip();

                //__instance.cancellationTokenSource.Cancel();
            }

            Logger.Log(output, LogType.Debug);
        }

        [HarmonyPatch(typeof(ResultPlayer))]
        [HarmonyPatch(nameof(ResultPlayer.DisplayDonEffects))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPostfix]
        public static void ResultPlayer_DisplayDonEffects_Postfix(ResultPlayer __instance)
        {
            List<string> output = new List<string>()
            {
                "ResultPlayer_DisplayDonEffects_Postfix",
            };

            __instance.flowerMask.gameObject.SetActive(true);

            Logger.Log(output, LogType.Debug);
        }
    }
}