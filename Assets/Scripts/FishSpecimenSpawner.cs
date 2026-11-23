using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSimpleInteractable))]
public class FishSpecimenSpawner : MonoBehaviour
{
    public XRGrabInteractable specimenPrefab;

    private XRSimpleInteractable tankFish;
    private XRGrabInteractable specimen;
    private bool spawning;

    private void Awake()
    {
        tankFish = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        tankFish.selectEntered.AddListener(OnFishSelected);
    }

    private void OnDisable()
    {
        tankFish.selectEntered.RemoveListener(OnFishSelected);
    }

    private void OnFishSelected(SelectEnterEventArgs args)
    {
        if (spawning || specimenPrefab == null)
            return;

        StartCoroutine(GiveSpecimen(args.interactorObject));
    }

    private IEnumerator GiveSpecimen(IXRSelectInteractor interactor)
    {
        spawning = true;

        // Keep only one specimen from this fish.
        if (specimen != null)
            Destroy(specimen.gameObject);

        Transform hand = interactor.GetAttachTransform(tankFish);

        specimen = Instantiate(
            specimenPrefab, hand.position, hand.rotation);

        // Allow the new interactable to register.
        yield return null;

        XRInteractionManager manager = tankFish.interactionManager;

        if (tankFish.IsSelected(interactor))
        {
            manager.SelectExit(interactor, tankFish);
            Physics.SyncTransforms();
            manager.SelectEnter(interactor, specimen);
        }

        spawning = false;
    }
}