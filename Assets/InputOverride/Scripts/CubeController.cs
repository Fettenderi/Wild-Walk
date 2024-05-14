using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;

namespace SpatialSys.Samples.InputOverride
{    
    public class CubeController : MonoBehaviour, IAvatarInputActionsListener
    {
        [SerializeField] GameObject cubeVirtualCamera;
        [SerializeField] GameObject startInteractable;

        Vector2 movementVector = Vector2.zero;
        bool isMoving = false;
        float speedMultiplier = .25f;

        private void Update()
        {
            if (isMoving)
            {
                var newPosition = new Vector3();
                newPosition.x = movementVector.x * speedMultiplier;
                newPosition.y = 0f;
                newPosition.z = movementVector.y * speedMultiplier;
                transform.position += newPosition;
            }
        }

        /// <summary>
        /// Function called from the Spatial Interactable's inspector. This begins the input override, sets the target of the camera to the object's transform
        /// and enables the virtual camera that follows the object.
        /// </summary>
        public void OnInteractHandler()
        {
            startInteractable.SetActive(false);
            SpatialBridge.inputService.StartAvatarInputCapture(true, true, true, true, this);
            SpatialBridge.cameraService.SetTargetOverride(transform, SpatialCameraMode.Actor);
            cubeVirtualCamera.SetActive(true);
        }

        #region IAvatarInputActionsListener implementation        

        public void OnAvatarActionInput(InputPhase inputPhase)
        {
            // intentionally not implemented
        }

        public void OnAvatarAutoSprintToggled(bool on)
        {
            // intentionally not implemented
        }

        /// <summary>
        /// Using the jump input override to exit out of being a cube
        /// </summary>
        /// <param name="inputPhase"></param>
        public void OnAvatarJumpInput(InputPhase inputPhase)
        {
            if (inputPhase == InputPhase.OnPressed)
            {
                SpatialBridge.inputService.ReleaseInputCapture(this);
                SpatialBridge.cameraService.ClearTargetOverride();
                startInteractable.SetActive(true);
                cubeVirtualCamera.SetActive(false);
            }
        }

        public void OnAvatarMoveInput(InputPhase inputPhase, Vector2 inputMove)
        {
            if(inputPhase == InputPhase.OnHold)
            {                
                movementVector = inputMove;
                isMoving = true;
            } else if(inputPhase == InputPhase.OnReleased) isMoving = false;
        }

        public void OnAvatarSprintInput(InputPhase inputPhase)
        {
            // intentionally not implemented
        }

        public void OnInputCaptureStarted(InputCaptureType type)
        {
            // intentionally not implemented
        }

        public void OnInputCaptureStopped(InputCaptureType type)
        {
            // intentionally not implemented
        }

        #endregion
    }
}


