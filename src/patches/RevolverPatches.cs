
using HarmonyLib;
using ukmod_improvedcamera.src.main;

namespace ukmod_improvedcamera.src.patches
{
    [HarmonyPatch(typeof(Revolver),nameof(Revolver.Shoot))]
    class RevolverPatches
    {
        static void Postfix(Revolver __instance, int shotType)
        {
            float shake = 0.25f;
            if (__instance.altVersion)
            {
                shake *= 2;
            }
            if (shotType == 2)
            {
                shake *= 2;
            }
            MonoSingleton<CameraMovementController>.Instance.camShakeEffect.StartBasicShake(shake);
        }
    }
}
