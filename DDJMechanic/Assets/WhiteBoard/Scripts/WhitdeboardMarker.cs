using OVR.OpenVR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class WhitdeboardMarker : MonoBehaviour
{
    [SerializeField] private Transform tip;
    [SerializeField] private int penSize = 5;
    [SerializeField] private AudioManager audioManager;

    private Renderer _renderer;
    private Color[] _colors;
    private float _tipHeight;
    private RaycastHit _touch;
    private WhiteBoard _whiteBoard;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lasttouchRot;
    private bool isSpraying = false;

    private float timer = 0;
    private float timerCutoff = 2.3f;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();
        _tipHeight = tip.localScale.y * 2;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        List<InputDevice> m_device = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right, m_device);
        if (m_device.Count == 1)
        {
            //One device found
            CheckController(m_device[0]);
        }

        if (isSpraying)
        {
            Draw();
        }

        if(timer >= timerCutoff && isSpraying)
        {
            audioManager.PlaySound();
            timer = 0;
        }
        
        
    }
    private void Draw()
    {
      
        if(Physics.Raycast(tip.position, transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("WhiteBoard"))
            {
                if (_whiteBoard == null)
                {
                    _whiteBoard = _touch.transform.GetComponent<WhiteBoard>();
                }
                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _whiteBoard.textureSize.x - (penSize / 2));
                var y = (int)(_touchPos.y * _whiteBoard.textureSize.y - (penSize / 2));

                if (y < 0 || y > _whiteBoard.textureSize.y || x < 0 || x > _whiteBoard.textureSize.x) return;

                if (_touchedLastFrame)
                {
                    _whiteBoard.texture.SetPixels(x, y, penSize, penSize, _colors);

                    for(float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var LerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var LerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _whiteBoard.texture.SetPixels(LerpX, LerpY, penSize, penSize, _colors);
                    }

                    transform.rotation = _lasttouchRot;

                    _whiteBoard.texture.Apply();
                }

                _lastTouchPos = new Vector2(x, y);
                _lasttouchRot = transform.rotation;
                _touchedLastFrame = true;
                return;
            }
        }
        _whiteBoard = null;
        _touchedLastFrame = false;
    }
    private void CheckController(InputDevice d)
    {
        bool PrimaryTriggerDown = false;
        d.TryGetFeatureValue(CommonUsages.triggerButton, out PrimaryTriggerDown);
        if (PrimaryTriggerDown)
        {
            isSpraying = true;
        }
        else
        {
            isSpraying= false;
        }
    }

}
