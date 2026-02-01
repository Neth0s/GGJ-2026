using UnityEngine;

public class WaterScript : MonoBehaviour
{
    [SerializeField] Material webglMaterial;

    private void Awake()
    {
        #if UNITY_WEBGL
        GetComponent<MeshRenderer>().material = webglMaterial;
        #endif
    }
}
