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
        }
    }
}
