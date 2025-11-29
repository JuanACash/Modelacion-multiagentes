using UnityEngine;

public class MaterialDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionDistance = 3f;
    public string targetMaterialName = "TomateInf";

    [Header("Debug")]
    public bool showRay = true;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (showRay)
            Debug.DrawRay(transform.position, transform.forward * detectionDistance, Color.red);

        if (Physics.Raycast(ray, out hit, detectionDistance))
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();

            if (rend == null)
                rend = hit.collider.GetComponentInChildren<Renderer>();

            if (rend != null)
            {
                string matName = rend.sharedMaterial.name;

                if (matName.Contains(targetMaterialName))
                {
                    hit.collider.gameObject.tag = "Infectado";
                }
            }
        }
    }
}