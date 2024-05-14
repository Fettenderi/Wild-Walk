using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSys.UnitySDK;

namespace SpatialSys.Samples.InputOverride
{
    /// <summary>
    /// Normal MonoBehaviour class but also implements Spatial's IAvatarInputActionsListener interface.
    /// This interface is what's used for overriding the default avatar input and applying it to other objects in the scene.
    /// </summary>
    public class DreadmillController : MonoBehaviour, IAvatarInputActionsListener
    {
        [SerializeField] Transform leftPosition;
        [SerializeField] Transform rightPosition;
        [SerializeField] Transform leftSpawn;
        [SerializeField] Transform rightSpawn;
        [SerializeField] GameObject dreadmillCamera;
        [SerializeField] GameObject dreadmillGeometry;
        [SerializeField] GameObject entrancePoint;
        [SerializeField] List<GameObject> capsuleMonsters;

        bool isOnDreadmill = false;
        Transform currentPosition;
        Vector3 dreadmillStartPosition;
        float spawnTimer = 1f;

        // the ID of the embedded avatar animation package
        readonly string _animationID = "1";
        // value used to calculate when to reset the illusion
        readonly float _geometryLength = 15f;
        // the speed at which the geometry moves
        readonly float _geometrySpeed = .15f;

        private void Start()
        {
            // saving the initial start position so it can be used to reset the illusion
            dreadmillStartPosition = dreadmillGeometry.transform.position;
        }
        
        void Update()
        {
            if(isOnDreadmill)
            {
                MovePlayer();
                MoveVisual();
                SpawnCapsuleMonsters();
            }
        }

        /// <summary>
        /// Places the player in the "currentPosition". There's one of two values: the left or right positions (variables above).
        /// </summary>
        void MovePlayer()
        {
            SpatialBridge.actorService.localActor.avatar.SetPositionRotation(currentPosition.position, currentPosition.rotation);
        }
        
        /// <summary>
        /// This function creates the illusion of an endless runner track. The track moves at a slow pace to match the zombie walk animation that's applied to the player.
        /// </summary>
        void MoveVisual()
        {
            dreadmillGeometry.transform.Translate(Vector3.back * _geometrySpeed * Time.deltaTime);
            if(dreadmillGeometry.transform.position.z <= (dreadmillStartPosition.z - _geometryLength))
                dreadmillGeometry.transform.position = dreadmillStartPosition;
        }

        /// <summary>
        /// Function that spawns a capsule prefab every time the timer reaches less than zero. Capsule "monsters" are spawned in one of two positions and move along the negative Z axis.
        /// </summary>
        void SpawnCapsuleMonsters()
        {
            spawnTimer -= Time.deltaTime;
            if(spawnTimer < 0f)
            {
                var monster = capsuleMonsters[Random.Range(0, capsuleMonsters.Count)];
                var spawnPoint = Random.Range(0, 2);
                Instantiate(monster, spawnPoint == 0 ? leftSpawn.position : rightSpawn.position, Quaternion.identity);
                spawnTimer = 1f;
            }
        }

        /// <summary>
        /// Plays the embedded avatar animation
        /// </summary>
        void AutoWalk()
        {
            SpatialBridge.actorService.localActor.avatar.PlayEmote(AssetType.EmbeddedAsset, _animationID, true, true);
        }

        /// <summary>
        /// Function called from the blue cube's Spatial Interactable inspector
        /// </summary>
        public void OnInteractHandler()
        {
            currentPosition = leftPosition;
            MovePlayer();
            AutoWalk();
            isOnDreadmill = true;            
            SpatialBridge.inputService.StartAvatarInputCapture(true, true, true, true, this);
            dreadmillCamera.SetActive(true);
        }

        #region IAvatarInputActionsListener implementations

        public void OnAvatarMoveInput(InputPhase inputPhase, Vector2 inputMove)
        {
            if(inputPhase == InputPhase.OnPressed)
            {
                switch(inputMove.x) 
                {
                    case 1:
                        currentPosition = rightPosition; break;
                    case -1: currentPosition = leftPosition; break;
                    default: break;
                }
            }
        }

        public void OnAvatarJumpInput(InputPhase inputPhase)
        {
            if (inputPhase == InputPhase.OnPressed)
            {
                SpatialBridge.inputService.ReleaseInputCapture(this);
                SpatialBridge.actorService.localActor.avatar.StopEmote();
                dreadmillCamera.SetActive(false);
                isOnDreadmill = false;
                dreadmillGeometry.transform.position = dreadmillStartPosition;
                SpatialBridge.actorService.localActor.avatar.SetPositionRotation(entrancePoint.transform.position, entrancePoint.transform.rotation);
            }
        }

        public void OnAvatarSprintInput(InputPhase inputPhase)
        {
            
        }

        public void OnAvatarActionInput(InputPhase inputPhase)
        {
            
        }

        public void OnAvatarAutoSprintToggled(bool on)
        {
            
        }

        public void OnInputCaptureStarted(InputCaptureType type)
        {
            
        }

        public void OnInputCaptureStopped(InputCaptureType type)
        {
            
        }

        #endregion
    }
}
