using UnityEngine;
using System.Collections;

public class EditTile : MonoBehaviour {

    Tile parent;
    SpriteRenderer thisSprite;

    public BroadcastPropertyToView broadcast;

    public void AwakeThis(Tile tile) {
        parent = tile;
        thisSprite = this.GetComponent<SpriteRenderer>();
    }

    void OnMouseUp()
    {
        if (!Input.GetKey(KeyCode.LeftShift)){

            BroadcastAddOrRemoveEditList();    
        }
    }

    public void BroadcastAddOrRemoveEditList() {
        if (broadcast != null)
        {
            broadcast.AddOrRemove_EditList(parent);
        }
        if (thisSprite.enabled)
        {
            thisSprite.enabled = false;
        }
        else
        {
            thisSprite.enabled = true;
        }
    }
}
