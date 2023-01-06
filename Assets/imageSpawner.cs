using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class imageSpawner : MonoBehaviour
{
    // Variables for image tracking 
    private Dictionary<string, GameObject> _instantiatedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager _trackedImageManager;
    public GameObject m_TrackedImagePrefab;
    
    // Variables for vector calculation
    float angle;
    Vector3 vec1, vec2;
    public Transform[] obj;
    
    // Line renderer 
    public LineRenderer lineRend01, lineRend02;
    public Material mat1, mat2;

    private void Awake()
    {
        if (_trackedImageManager == null)
            _trackedImageManager = GetComponent<ARTrackedImageManager>();

    }
    private void OnEnable()
    {
        _trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
    private void OnDisable()
    {
        _trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {   
            if (!_instantiatedPrefabs.ContainsKey(trackedImage.referenceImage.name))
            {
                // Instantiate prefab whenever new reference image is detected
                // Then store instantiated prefab into dictionary according to the name of the image 
                _instantiatedPrefabs[trackedImage.referenceImage.name] = Instantiate(m_TrackedImagePrefab, trackedImage.transform);
            }

            // Get position of each instantiated prefab and store them in obj 
            obj[0] = _instantiatedPrefabs["0"].transform;
            obj[1] = _instantiatedPrefabs["1"].transform;
            obj[2] = _instantiatedPrefabs["2"].transform;

            // Get the vector between two points 
            vec1 = obj[1].position - obj[0].position;
            vec2 = obj[2].position - obj[0].position;

            // Visualize vectors with line renderer 
            lineRend01 = new GameObject("Line01").AddComponent<LineRenderer>();
            lineRend01.startColor = Color.white;
            lineRend01.endColor = Color.white;
            lineRend01.startWidth = 0.01f;
            lineRend01.endWidth = 0.01f;
            lineRend01.positionCount = 2;

            mat1 = new Material(Shader.Find("Standard"));
            mat1.color = Color.white;
            lineRend01.material = mat1;

            lineRend02 = new GameObject("Line02").AddComponent<LineRenderer>();
            lineRend02.startColor = Color.white;
            lineRend02.endColor = Color.white;
            lineRend02.startWidth = 0.01f;
            lineRend02.endWidth = 0.01f;
            lineRend02.positionCount = 2;

            mat2 = new Material(Shader.Find("Standard"));
            mat2.color = Color.white;
            lineRend02.material = mat2;

            lineRend01.SetPosition(0, obj[0].position);
            lineRend01.SetPosition(1, obj[1].position);

            lineRend02.SetPosition(0, obj[0].position);
            lineRend02.SetPosition(1, obj[2].position);

            // Get the angle between two vectors and get angle used for the dorsiflexion test 
            angle = Vector3.Angle(vec1, vec2);
            angle = 90 - angle;
        }

        foreach (var trackedImage in args.updated)
        {
            // Update prefab position to moved detected image position
            _instantiatedPrefabs[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
            obj[0] = _instantiatedPrefabs["0"].transform;
            obj[1] = _instantiatedPrefabs["1"].transform;
            obj[2] = _instantiatedPrefabs["2"].transform;

            // Get the vectors between two points when updated
            vec1 = obj[1].position - obj[0].position;
            vec2 = obj[2].position - obj[0].position;

            // Get the angle between two vectors and get angle used for the dorsiflexion test  
            angle = Vector3.Angle(vec1, vec2);
            angle = 90 - angle;

            // Visualize vectors with line renderer
            lineRend01.SetPosition(0, obj[0].position);
            lineRend01.SetPosition(1, obj[1].position);

            lineRend02.SetPosition(0, obj[0].position);
            lineRend02.SetPosition(1, obj[2].position);

            // Then add UI interaction depending on the angles 
            if (angle < 30)
            {
                // Set the line color to red to indicate poor mobility
                mat1.color = Color.red;
                mat2.color = Color.red;
                lineRend01.material = mat1;
                lineRend02.material = mat2;
            }

            if (30 <= angle && angle < 35)
            {
                // Set the line color to yellow to indicate decent mobility
                mat1.color = Color.yellow;
                mat2.color = Color.yellow;
                lineRend01.material = mat1;
                lineRend02.material = mat2;
            }

            if (angle >= 35)
            {
                // Set the line color to green to indicate good mobility
                mat1.color = Color.green;
                mat2.color = Color.green;
                lineRend01.material = mat1;
                lineRend02.material = mat2;
            }

        }

        foreach (var trackedImage in args.removed)
        {
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.None)
            {
                Destroy(_instantiatedPrefabs[trackedImage.referenceImage.name]);
                _instantiatedPrefabs.Remove(trackedImage.referenceImage.name);
            }         
        }
    }
    
    // Display angles on the top of the screen 
    public void OnGUI()
    {
        GUI.Label(new Rect(100, 100, 400, 400), "Angle: " + angle.ToString());
        GUI.skin.label.fontSize = 60;
    }

}
