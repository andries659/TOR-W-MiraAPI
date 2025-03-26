using UnityEngine;
using HarmonyLib;

namespace TORW.Patches
{
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    [HarmonyPriority(Priority.High)]
    public static class TORWLogoPatch
    {
        public static SpriteRenderer LogoSprite;

        [HarmonyPostfix]
        public static void StartPostfix(MainMenuManager __instance)
        {
           var newparent = __instance.gameModeButtons.transform.parent;
           var Logo = new GameObject("TORWLogo");
           Logo.transform.parent = newparent;
           Logo.transform.localPosition = new(0f, -0.07f, 1f);
           Logo.transform.localScale = new Vector3(0.3f, 0.5f, 0.3f);
           LogoSprite = Logo.AddComponent<SpriteRenderer>();
           LogoSprite.sprite = TORWAssets.ModLogo.LoadAsset();
        }
    }
}