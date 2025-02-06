using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipCoinAndRewardScreen.Plugins
{
    internal class SkipCoinAndRewardHook
    {
        [HarmonyPatch(typeof(ResultPlayer))]
        [HarmonyPatch(nameof(ResultPlayer.SettingDonCoinAndReward))]
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPostfix]
        public static void ResultPlayer_SettingDonCoinAndReward_Postfix(ResultPlayer __instance)
        {

            __instance.isSkipCoinAndReward = true;
            if (Plugin.Instance.ConfigDontSkipOnMaxBankedLevel.Value)
            {
                var bankStockCount = __instance.resultCoinExp.PlayerData.BankStockCount();
                var playerLevel = __instance.resultCoinExp.PlayerData.CurrentLevel();
                Logger.Log("bankStockCount: " + bankStockCount);
                Logger.Log("playerLevel: " + playerLevel);
                if (bankStockCount == 5 && playerLevel != 400)
                {
                    __instance.isSkipCoinAndReward = false;
                }
            }
        }
    }
}
