using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Management;


public class TestSwitching : MonoBehaviour
{
    // The name of the VR device to use
    public string vrDeviceName = "Oculus";

    // The name of the VR level scene
    public string vrLevelSceneName = "DDJ Mechanic";

    // The name of the desktop main menu scene
    public string desktopMenuSceneName = "TestingUIScene";

    // A reference to the player camera, if using VR
    public Camera vrPlayerCamera;

    // A reference to the desktop camera, if using desktop mode
    public Camera desktopPlayerCamera;

    // The name of the XR display subsystem
    public string xrSubsystemName = "Oculus";

    // The reference to the XR display subsystem
    private XRDisplaySubsystem xrDisplaySubsystem;

    // Start is called before the first frame update
    void Start()
    {
        // Disable the VR camera and enable the desktop camera
        if (vrPlayerCamera != null)
        {
            vrPlayerCamera.enabled = false;
        }
        if (desktopPlayerCamera != null)
        {
            desktopPlayerCamera.enabled = true;
        }

        // Get the XR display subsystem reference
        List<XRDisplaySubsystem> displaySubsystems = new List<XRDisplaySubsystem>();

    }

    // Switches between VR and desktop modes
    public void SwitchModes()
    {
        if (XRSettings.enabled)
        {
            // Switch from VR to desktop mode
            XRDisplaySubsystem xrDisplay = GetXRDisplaySubsystem();
            // Stop the XR display subsystem
            xrDisplaySubsystem.Stop();

            // Disable the VR camera and enable the desktop camera
            if (vrPlayerCamera != null)
            {
                vrPlayerCamera.enabled = false;
            }
            if (desktopPlayerCamera != null)
            {
                desktopPlayerCamera.enabled = true;
            }

            // Load the desktop main menu scene
            SceneManager.LoadScene(desktopMenuSceneName);
        }
        else
        {
            // Switch from desktop to VR mode
            XRDisplaySubsystem xrDisplay = GetXRDisplaySubsystem();
            // Start the XR display subsystem
            xrDisplaySubsystem.Start();

            // Disable the desktop camera and enable the VR camera
            if (desktopPlayerCamera != null)
            {
                desktopPlayerCamera.enabled = false;
            }
            if (vrPlayerCamera != null)
            {
                vrPlayerCamera.enabled = true;
            }

            // Load the VR level scene
            SceneManager.LoadScene(vrLevelSceneName);
        }
    }

    // Gets the XR display subsystem reference
    private XRDisplaySubsystem GetXRDisplaySubsystem()
    {
        // Get all the loaded XR display subsystems
        List<XRDisplaySubsystem> displaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances(displaySubsystems);

        // Find the subsystem with the specified name
        for (int i = 0; i < displaySubsystems.Count; i++)
        {
            if (displaySubsystems[i].SubsystemDescriptor.id == xrSubsystemName)
            {
                return displaySubsystems[i];
            }
        }

        // Subsystem with the specified name not found
        Debug.LogError("XR display subsystem " + xrSubsystemName + " not found.");
        return null;
    }
}

