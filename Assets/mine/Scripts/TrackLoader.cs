using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TrackLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] tracks; 
    public GameObject currentTrack { get; private set; }

    public void LoadTrack(int index)
    {
        if (currentTrack != null)
        {
            Destroy(currentTrack);
        }
        if (index >= 0 && index < tracks.Length)
        {
            currentTrack = Instantiate(tracks[index], tracks[index].transform.position, Quaternion.identity);
        }
    }
}
