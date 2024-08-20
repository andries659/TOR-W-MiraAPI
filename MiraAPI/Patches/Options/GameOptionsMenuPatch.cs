﻿using HarmonyLib;
using Il2CppSystem.Collections.Generic;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using Reactor.Localization.Utilities;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MiraAPI.Patches.Options;


[HarmonyPatch(typeof(GameOptionsMenu))]
public static class GameOptionsMenuPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(GameOptionsMenu.CreateSettings))]
    public static bool SettingsPatch(GameOptionsMenu __instance)
    {
        if (GameSettingMenuPatches.CurrentSelectedMod == 0) return true;

        __instance.MapPicker.gameObject.SetActive(false);

        float num = 2.1f;
        
        var filteredGroups = GameSettingMenuPatches.SelectedMod.OptionGroups.Where(x => x.GroupVisible.Invoke() && x.AdvancedRole is null);
        
        foreach (IModdedOptionGroup group in filteredGroups)
        {
            CategoryHeaderMasked categoryHeaderMasked = Object.Instantiate(__instance.categoryHeaderOrigin, Vector3.zero, Quaternion.identity, __instance.settingsContainer);
            categoryHeaderMasked.SetHeader(CustomStringName.CreateAndRegister(group.GroupName), 20);
            if (group.GroupColor != Color.clear)
            {
                categoryHeaderMasked.Background.color = group.GroupColor;
                categoryHeaderMasked.Title.color = group.GroupColor.DarkenColor();
            }
            categoryHeaderMasked.transform.localScale = Vector3.one * 0.63f;
            categoryHeaderMasked.transform.localPosition = new Vector3(-0.903f, num, -2f);
            num -= 0.63f;

            var filteredOpts = ModdedOptionsManager.GroupedOptions[group].Where(x=>x.Visible.Invoke()).ToList();
            
            foreach (var opt in filteredOpts)
            {
                OptionBehaviour newOpt = opt.CreateOption(__instance.checkboxOrigin, __instance.numberOptionOrigin, __instance.stringOptionOrigin, __instance.settingsContainer);
                newOpt.transform.localPosition = new Vector3(0.952f, num, -2f);
                newOpt.SetClickMask(__instance.ButtonClickMask);

                SpriteRenderer[] componentsInChildren = newOpt.GetComponentsInChildren<SpriteRenderer>(true);
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    if (group.GroupColor != Color.clear) componentsInChildren[i].color = group.GroupColor;
                    componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, 20);
                }

                foreach (TextMeshPro textMeshPro in newOpt.GetComponentsInChildren<TextMeshPro>(true))
                {
                    if (group.GroupColor != Color.clear) textMeshPro.color = group.GroupColor.DarkenColor();
                    textMeshPro.fontMaterial.SetFloat(ShaderID.StencilComp, 3f);
                    textMeshPro.fontMaterial.SetFloat(ShaderID.Stencil, 20);
                }

                __instance.Children.Add(newOpt);

                num -= 0.45f;
                newOpt.Initialize();
            }
        }

        var filteredOptions = GameSettingMenuPatches.SelectedMod.Options.Where(x => x.AdvancedRole is null && !x.HasGroup).ToList();

        if (filteredOptions.Count > 0)
        {

            CategoryHeaderMasked ungroupedHeader = Object.Instantiate(__instance.categoryHeaderOrigin, Vector3.zero,
                Quaternion.identity, __instance.settingsContainer);
            ungroupedHeader.SetHeader(CustomStringName.CreateAndRegister("Ungrouped Options"), 20);
            ungroupedHeader.transform.localScale = Vector3.one * 0.63f;
            ungroupedHeader.transform.localPosition = new Vector3(-0.903f, num, -2f);

            num -= 0.63f;


            foreach (var opt in filteredOptions)
            {
                OptionBehaviour newOpt = opt.CreateOption(__instance.checkboxOrigin, __instance.numberOptionOrigin,
                    __instance.stringOptionOrigin, __instance.settingsContainer);
                newOpt.transform.localPosition = new Vector3(0.952f, num, -2f);
                newOpt.SetClickMask(__instance.ButtonClickMask);
                newOpt.SetUpFromData(newOpt.data, 20);

                SpriteRenderer[] componentsInChildren = newOpt.GetComponentsInChildren<SpriteRenderer>(true);
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, 20);
                }

                foreach (TextMeshPro textMeshPro in newOpt.GetComponentsInChildren<TextMeshPro>(true))
                {
                    textMeshPro.fontMaterial.SetFloat(ShaderID.StencilComp, 3f);
                    textMeshPro.fontMaterial.SetFloat(ShaderID.Stencil, 20);
                }

                __instance.Children.Add(newOpt);

                num -= 0.45f;

                newOpt.Initialize();
            }
        }

        __instance.scrollBar.SetYBoundsMax(-num - 1.65f);

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(GameOptionsMenu.Initialize))]
    public static bool InitPatch(GameOptionsMenu __instance)
    {
        if (__instance.Children == null || __instance.Children.Count == 0)
        {
            __instance.MapPicker.gameObject.SetActive(true);
            __instance.MapPicker.Initialize(20);
            BaseGameSetting mapNameSetting = GameManager.Instance.GameSettingsList.MapNameSetting;
            __instance.MapPicker.SetUpFromData(mapNameSetting, 20);
            __instance.Children = new List<OptionBehaviour>();
            __instance.Children.Add(__instance.MapPicker);
            __instance.CreateSettings();
            __instance.cachedData = GameOptionsManager.Instance.CurrentGameOptions;
            foreach (var optionBehaviour in __instance.Children)
            {
                if (AmongUsClient.Instance && !AmongUsClient.Instance.AmHost)
                {
                    optionBehaviour.SetAsPlayer();
                }

                if (optionBehaviour.IsCustom()) continue;

                optionBehaviour.OnValueChanged = new System.Action<OptionBehaviour>(__instance.ValueChanged);
            }
        }

        return false;
    }
}