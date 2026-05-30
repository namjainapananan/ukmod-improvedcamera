
using HarmonyLib;
using UnityEngine;
using ukmod_improvedcamera.src.main;

namespace ukmod_improvedcamera.src.patches.cameracontroller
{
    [HarmonyPatch(typeof(CameraController), nameof(CameraController.Awake))]
    class AwakePatch
    {
        static void Postfix(CameraController __instance)
        {
            __instance.gameObject.AddComponent<CameraMovementController>();
        }
    }

    [HarmonyPatch(typeof(CameraController), nameof(CameraController.LateUpdate))]
    class LateUpdatePatch
    {
        static void Postfix(CameraController __instance)
        {
            __instance.StopShake();
            __instance.tiltRotationZ = 0;
            __instance.tiltRotationZSmooth = 0;
        }
    }

    [HarmonyPatch(typeof(CameraController), nameof(CameraController.CameraShake))]
    class CameraShakePatch
    {
        static void Prefix(float shakeAmount)
        {
            MonoSingleton<CameraMovementController>.Instance.camShakeEffect.StartBasicShake(shakeAmount);
        }
    }

    [HarmonyPatch(typeof(CameraController),nameof(CameraController.ApplyRotations))]
    class ApplyRotationsPatch
    {
        static bool Prefix(CameraController __instance)
        {
            Vector3 camoffset = MonoSingleton<CameraMovementController>.Instance.FinalResult;
            if (GameStateManager.Instance.CameraLocked)
            {
                camoffset = Vector3.zero;
            }
            __instance.player.transform.localRotation = __instance.gravityRotation * Quaternion.AngleAxis(__instance.rotationY, Vector3.up);
            MonoSingleton<NewMovement>.Instance.rb.rotation = __instance.player.transform.rotation;
            __instance.transform.localRotation = Quaternion.AngleAxis(-__instance.rotationX, Vector3.right) *
                                                 Quaternion.AngleAxis(__instance.transitionRotationZ, Vector3.forward) *
                                                 Quaternion.AngleAxis(__instance.tiltRotationZ, Vector3.forward) *
                                                 __instance.rotationOffset *
                                                 Quaternion.Euler(camoffset);
            __instance.hudCamera.transform.GetChild(0).localRotation = Quaternion.Euler(-camoffset);
            return false;
        }
    }
}
