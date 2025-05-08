using UnityEngine;
using UnityEditor;

public class ScaleAnimationClip : MonoBehaviour
{
    public AnimationClip clip;
    public float scaleMultiplier = 2000f;

    [ContextMenu("Scale Animation Clip")]
    void ScaleClip()
    {
#if UNITY_EDITOR
        if (clip == null)
        {
            Debug.LogError("No AnimationClip assigned.");
            return;
        }

        // Duplico il clip per sicurezza
        AnimationClip newClip = new AnimationClip();
        EditorUtility.CopySerialized(clip, newClip);
        newClip.name = clip.name + "_Scaled";

        // Processa curve
        foreach (string axis in new[] { "x", "y", "z" })
        {
            string path = "localScale." + axis;
            var curve = AnimationUtility.GetEditorCurve(newClip, EditorCurveBinding.FloatCurve("", typeof(Transform), path));

            if (curve != null)
            {
                for (int i = 0; i < curve.keys.Length; i++)
                {
                    var key = curve.keys[i];
                    key.value *= scaleMultiplier;
                    curve.MoveKey(i, key);
                }
                AnimationUtility.SetEditorCurve(newClip, EditorCurveBinding.FloatCurve("", typeof(Transform), path), curve);
            }
        }

        // Salva nuovo clip come asset
        string pathToSave = "Assets/" + newClip.name + ".anim";
        AssetDatabase.CreateAsset(newClip, pathToSave);
        AssetDatabase.SaveAssets();

        Debug.Log("New scaled clip saved to: " + pathToSave);
#else
        Debug.LogWarning("This can only be run in the Unity Editor.");
#endif
    }
}
