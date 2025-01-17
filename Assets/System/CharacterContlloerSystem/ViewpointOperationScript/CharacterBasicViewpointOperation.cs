using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Takechi.CharacterController.Address;
using Takechi.CharacterController.KeyInputStete;
using Takechi.CharacterController.Parameters;
using Takechi.ExternalData;
using Takechi.UI.ControlsSettingMenu;
using UnityEngine;
using UnityEngine.Rendering;
using static Takechi.ScriptReference.CryptoRedPath.ReferenceCryptoRedPath;

namespace Takechi.CharacterController.ViewpointOperation
{
    [RequireComponent(typeof(CharacterAddressManagement))]
    [RequireComponent(typeof(CharacterStatusManagement))]
    [RequireComponent(typeof(CharacterKeyInputStateManagement))]
    public class CharacterBasicViewpointOperation : ExternalDataManagement
    {
        #region SerializeField
        [Header("=== CharacterAddressManagement === ")]
        [SerializeField] private CharacterAddressManagement m_characterAddressManagement;
        [Header("=== CharacterStatusManagement ===")]
        [SerializeField] private CharacterStatusManagement m_characterStatusManagement;
        [Header("=== CharacterKeyInputStateManagement ===")]
        [SerializeField] private CharacterKeyInputStateManagement m_characterKeyInputStateManagement;

        #endregion

        #region private variable
        private CharacterAddressManagement addressManagement => m_characterAddressManagement;
        private CharacterStatusManagement statusManagement => m_characterStatusManagement;
        private CharacterKeyInputStateManagement keyInputStateManagement => m_characterKeyInputStateManagement;
        private GameObject avater => addressManagement.GetMyAvater();
        private Camera mainCamera => addressManagement.GetMyMainCamera();
        private ControlsSettingData controlsSettingData = new ControlsSettingData();
        private string cleanpath => Cleanpath.controlsSettingDataPath;
        private Quaternion cameraRot, characterRot;
        private float xSensityvity , ySensityvity = 1f;

        #region Get Function
        public float GetXSensityvity() { return xSensityvity; }
        public float GetYSensityvity() { return ySensityvity; }

        #endregion
        #region Set Finction
        public void SetXSensityvity(float value) 
        {
            xSensityvity = value;
            Debug.Log($" xSensityvity : {value} <color=green>to set</color>.");
        }

        public void SetYSensityvity(float value) 
        {
            ySensityvity = value;
            Debug.Log($" ySensityvity : {value} <color=green>to set</color>.");
        }

        #endregion

        #endregion

        #region cost stract 
        private struct limitedToCamera
        {
            public const float minX = -90f;
            public const float maxX = 35f;
        }

        #endregion


        #region UnityEvent
        private void Reset()
        {
            m_characterStatusManagement = this.transform.GetComponent<CharacterStatusManagement>();
            m_characterAddressManagement = this.transform.GetComponent<CharacterAddressManagement>();
            m_characterKeyInputStateManagement = this.transform.GetComponent<CharacterKeyInputStateManagement>();
        }

        private void Awake()
        {
            controlsSettingData = LoadData(controlsSettingData, cleanpath);
            SetXSensityvity(controlsSettingData.xSensityvity);
            SetYSensityvity(controlsSettingData.ySensityvity);
        }

        private void Start()
        {
            cameraRot = mainCamera.transform.localRotation;
            characterRot = avater.transform.localRotation;
        }

        private void OnEnable()
        {
            statusManagement.InitializeCameraSettings += () =>
            {
                ResetCameraRot();
                Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} :  m_characterStatusManagement.InitializeCameraSettings <color=yellow>ResetCameraRot</color>() <color=green>to add.</color>");
                ResetCharacterRot();
                Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} :  m_characterStatusManagement.InitializeCameraSettings <color=yellow>ResetCharacterRot</color>() <color=green>to add.</color>");
            };

            keyInputStateManagement.InputToViewpoint += (statusManagement, addressManagement, mouseX, mouseY) => { CameraControll(mouseX, mouseY); };
            Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} :  characterKeyInputStateManagement.InputToViewpoint function <color=green>to add.</color>");
        }

        private void OnDisable()
        {
            statusManagement.InitializeCameraSettings -= () =>
            {
                ResetCameraRot();
                Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} :  m_characterStatusManagement.InitializeCameraSettings <color=yellow>ResetCameraRot</color>() <color=green>to remove.</color>");
                ResetCharacterRot();
                Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} :  m_characterStatusManagement.InitializeCameraSettings <color=yellow>ResetCharacterRot</color>() <color=green>to remove.</color>");
            };

            keyInputStateManagement.InputToViewpoint -= (statusManagement, addressManagement, mouseX, mouseY) => { CameraControll(mouseX, mouseY); };
            Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} :  characterKeyInputStateManagement.InputToViewpoint function <color=green>to remove.</color>");
        }

        #endregion

        #region ResetFinction
        public void ResetCameraRot() { cameraRot = Quaternion.identity; }
        public void ResetCharacterRot() { characterRot = Quaternion.identity; }

        #endregion

        #region ViewpointOperationControll
        private void CameraControll(float mouseX, float mouseY) 
        {
            float xRot = mouseX * ySensityvity;
            float yRot = mouseY * xSensityvity;

            cameraRot *= Quaternion.Euler(-yRot, 0, 0);
            characterRot *= Quaternion.Euler(0, xRot, 0);

            cameraRot =
                ClampRotation( cameraRot, limitedToCamera.minX, limitedToCamera.maxX);

            mainCamera.transform.localRotation = cameraRot;
            avater.transform.localRotation = characterRot;
        }

        #endregion

        #region RecursiveFunction 
        /// <summary>
        /// ClampRotation
        /// </summary>
        /// <param name="q"></param>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <returns>　制限のかかった４次元数を、かえします。</returns>
        private Quaternion ClampRotation( Quaternion q, float minX ,float maxX)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1f;

            float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

            angleX = Mathf.Clamp(angleX, minX, maxX);

            q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

            return q;
        }
        #endregion
    }
}