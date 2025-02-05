using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaysContainer : MonoBehaviour
{
    [SerializeField] private Transform[] _firstLevelPath;
    [SerializeField] private Transform[] _secondLevelPath;
    //[SerializeField] private Transform[] _secondLevelRightPath;

    public Transform[] firstLevelPath => _firstLevelPath;
    public Transform[] secondLevelPath => _secondLevelPath;
   // public Transform[] secondLevelRightPath => _secondLevelRightPath;
}
