using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Camera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] _virtualCameraList;
    [SerializeField] private int _UnselectedPriority = 0;
    [SerializeField] private int _selectedPriority = 10;

    private int _currentCamera = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
