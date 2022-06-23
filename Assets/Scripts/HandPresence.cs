#namespaces are given here:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics InputDeviceCharacteristics;
    private InputDevice targetDevice;
    public GameObject handModelPrefab;
    public GameObject controllerPrefab;
    private GameObject spawnedController;
    private GameObject spawnedHandController;
    private bool showController = false;
    private Animator animator;

    void Start()
    {
        tryGetDevice();
    }

    void tryGetDevice()
    {
        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics, inputDevices);
        if (inputDevices.Count > 0)
        {
            targetDevice = inputDevices[0];
        }

        spawnedController = Instantiate(controllerPrefab, transform);

        spawnedHandController = Instantiate(handModelPrefab, transform);
        animator = spawnedHandController.GetComponent<Animator>();
    }

    void updateAnimatorValues()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            animator.SetFloat("Grip", gripValue);
        }
        else
        {
            animator.SetFloat("Grip", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            animator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            animator.SetFloat("Trigger", 0);
        }
    }

    void Update()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
        {
            showController = !showController;
        };

        if (!targetDevice.isValid)
        {
            tryGetDevice();
        }
        else
        {
            if (showController)
            {
                spawnedHandController.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedController.SetActive(false);
                spawnedHandController.SetActive(true);
                updateAnimatorValues();
            }

        }


    }
}
   
