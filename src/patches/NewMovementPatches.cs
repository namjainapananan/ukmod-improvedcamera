
using HarmonyLib;
using ukmod_improvedcamera.src.main;
using ULTRAKILL.Portal;
using UnityEngine;

namespace ukmod_improvedcamera.src.patches.newmovementpatches
{
    [HarmonyPatch(typeof(NewMovement),nameof(NewMovement.OnTravel))]
    class OnTravelPatch
    {
        static void Postfix(NewMovement __instance, PortalTravelDetails details)
        {
            MonoSingleton<CameraMovementController>.Instance.viewRollEffect.OnPortalTravesal(details);
        }
    }

    [HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Awake))]
    class AwakePatch
    {
        static void Postfix(NewMovement __instance)
        {
            __instance.rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }
}
