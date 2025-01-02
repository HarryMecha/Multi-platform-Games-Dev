using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{


    // Start is called before the first frame update
    [SerializeField] private Dictionary<AudioSource,float> sourcesAndInitial = new Dictionary<AudioSource,float>();

    

    // Update is called once per frame
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
