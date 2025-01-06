using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    #region Fields
    [SerializeField] private Dictionary<AudioSource,float> sourcesAndInitial = new Dictionary<AudioSource,float>();
    #endregion


    /*
     * The OnSliderChange funtion takes a value, based on the current position of the slider and find all the audio sources within a scene, checking if they no longer exist in the scene and multiply their current volume by this slider value.
     */
    public void OnSliderChange(float value)
    {
        
        List<AudioSource> sources = new List<AudioSource>();
        sources = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None).ToList<AudioSource>();
        List<AudioSource> sourcesToDelete = new List<AudioSource>();
        foreach (var source in sourcesAndInitial.Keys)
        {
            if (!sources.Contains(source))
            sourcesToDelete.Add(source);
        }
        
        foreach (var source in sourcesToDelete)
        {
           sourcesAndInitial.Remove(source);
        }


        foreach (AudioSource source in sources)
        {
            if (!sourcesAndInitial.ContainsKey(source)) { 
                sourcesAndInitial.Add(source, source.volume);
        }
        }
        foreach (var source in sourcesAndInitial)
        {
            source.Key.volume = source.Value * value;
        }
    }
}
