using UnityEngine;

public class WaterScript : MonoBehaviour
{
    [SerializeField] Material webglMaterial;

    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GetComponent<MeshRenderer>().material = webglMaterial;
#endif
    }
}
