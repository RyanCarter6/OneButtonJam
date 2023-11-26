using UnityEngine;

// Used on the ground/background walls/ceiling to make it seem like player is moving
public class ScrollBackground : MonoBehaviour
{
    // Ref to material used by the mesh
    [SerializeField] private Material material;

    // How fast gameobject should scroll
    [SerializeField] private int scrollSpeed;

    // Called once before Update()
    void Start()
    {
        // Lock/Hide cursor as it's not needed
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Sets texture offset to 0 at the start
        material.mainTextureOffset = Vector2.zero;
    }

    // Update is called once per frame to scroll mesh's material
    void Update()
    {
        material.mainTextureOffset += new Vector2(scrollSpeed, 0) * Time.deltaTime;
    }
}
