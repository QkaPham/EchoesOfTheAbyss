using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class GlobalVolume : MonoBehaviour
{
    //[SerializeField] private Volume volume;
    //private static DepthOfField depthOfField;
    //private static Bloom bloom;

    //private void Awake()
    //{
    //    volume = GetComponent<Volume>();
    //    volume.profile.TryGet(out depthOfField);
    //    volume.profile.TryGet(out bloom);

    //}
    //public static void ActiveDepthOfField(bool active)
    //{
    //    if (depthOfField == null) return;
    //    //depthOfField.active = active;
    //    //StartCoroutine(DelayActiveDepthOfField(active));
    //}

    //private static IEnumerator DelayActiveDepthOfField(bool active)
    //{
    //    if (active)
    //    {
    //        while (depthOfField.focusDistance.value >= 1)
    //        {
    //            yield return null;
    //            depthOfField.focalLength.value -= Time.deltaTime * 200;
    //        }
    //    }
    //    else
    //    {
    //        while (depthOfField.focusDistance.value <= 300)
    //        {
    //            yield return null;
    //            depthOfField.focalLength.value += Time.deltaTime * 200;
    //        }
    //    }
    //}

    //public static void ActiveBloom(bool active)
    //{
    //    if (bloom == null) return;
    //    bloom.active = active;
    //}
}
