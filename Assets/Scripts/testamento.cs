using SpatialSys.UnitySDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testamento : MonoBehaviour
{
    public void EquipAvatar()
    {
        SpatialBridge.actorService.localActor.avatar.SetAvatarBody(AssetType.EmbeddedAsset, "Deer");
    }
}
