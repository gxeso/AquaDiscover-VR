using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SpecimenReturnZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        XRGrabInteractable specimen =
            other.GetComponentInParent<XRGrabInteractable>();

        if (specimen == null || !specimen.CompareTag("Specimen"))
            return;

        // Wait until both hands have released it.
        if (!specimen.isSelected)
        {
            Destroy(specimen.gameObject);
        }
    }
}   