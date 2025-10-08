using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour
{
    List<AViewVolume> _activeViewVolumes = new List<AViewVolume>();
    Dictionary<AView, List<AViewVolume>> _volumePerViews = new Dictionary<AView, List<AViewVolume>>();
    
    public static ViewVolumeBlender instance;

    private void Awake()
    {
        if(instance == null) instance = this;
        else
        {
            Debug.LogWarning("Y'a déjà une instance enft");
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        foreach (AViewVolume viewVolume in _activeViewVolumes)
        {
            viewVolume.View.Weight = 0;
        }
        _activeViewVolumes = _activeViewVolumes.OrderBy(x => x.Priority).ThenBy(x => x.UID).ToList();
        foreach (AViewVolume viewVolume in _activeViewVolumes)
        {
            float weight = Mathf.Clamp01(viewVolume.ComputeSelfWeight());
            float remainingWeight = 1 - weight;
            foreach (AViewVolume v in _activeViewVolumes)
            {
                v.View.Weight *= remainingWeight;
            }
            viewVolume.View.Weight += weight;
        }
    }

    public void AddVolume(AViewVolume volume)
    {
        _activeViewVolumes.Add(volume);
        
        if (!_volumePerViews.ContainsKey(volume.View))
        {
            List<AViewVolume> list = new List<AViewVolume>();
            _volumePerViews.Add(volume.View, list);
            volume.View.SetActive(true);
        }
        
        _volumePerViews[volume.View].Add(volume);
    }

    public void RemoveVolume(AViewVolume volume)
    {
        _activeViewVolumes.Remove(volume);
        
        _volumePerViews[volume.View].Remove(volume);
        if (_volumePerViews[volume.View].Count <= 0)
        {
            _volumePerViews.Remove(volume.View);
            _activeViewVolumes.Remove(volume);
        }
        volume.View.SetActive(false);
    }

    private void OnGUI()
    {
        foreach (AViewVolume activeVolume in _activeViewVolumes)
        {
            GUILayout.Label(activeVolume.name);
        }
    }
}
