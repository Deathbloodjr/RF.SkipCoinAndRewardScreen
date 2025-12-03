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
using UnityEngine;
using UnityEngine.UI;

namespace SkipCoinAndRewardScreen.Plugins
{
    internal class SkipCoinAndRewardHook
    {
        // Skip Crown Point screen
        [HarmonyPatch(typeof(CrownPointManager))]
        [HarmonyPatch(nameof(CrownPointManager.GetCrownPointData))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPostfix]
        public static void CrownPointManager_GetCrownPointData_Postfix(CrownPointManager __instance, ref CrownPointData __result)
        {
            List<string> output = new List<string>();
            output.Add("CrownPointManager_GetCrownPointData_Postfix");
            if (Plugin.Instance.ConfigSkipCrownPoint.Value)
            {
                output.Add("__result.CurrentPoints: " + __result.CurrentPoints);
                output.Add("__result.SavedPoints: " + __result.SavedPoints);
                output.Add("__result.PointsGained: " + __result.PointsGained);

                __result = new CrownPointData()
                {
                    CurrentPoints = __result.CurrentPoints,
                    SavedPoints = __result.CurrentPoints,
                };
                output.Add("__result.PointsGained: " + __result.PointsGained);
                Logger.Log(output, LogType.Debug);
            }
        }


        // Skip Coin and Level screen
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
            }

            Logger.Log(output, LogType.Debug);
        }

        [HarmonyPatch(typeof(ResultPlayer._ShowDonCoinAndRewardAsync_d__164))]
        [HarmonyPatch(nameof(ResultPlayer._ShowDonCoinAndRewardAsync_d__164.MoveNext))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPostfix]
        public static void ResultPlayer__ShowDonCoinAndRewardAsync_d__164_MoveNext_Postfix(ResultPlayer._ShowDonCoinAndRewardAsync_d__164 __instance)
        {
            __instance.__4__this.flowerMask.enabled = true;
        }
    }
}