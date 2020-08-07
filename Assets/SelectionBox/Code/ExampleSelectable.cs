using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Collections;

public class ExampleSelectable : MonoBehaviour, IBoxSelectable
{

    #region Implemented members of IBoxSelectable
    bool _selected = false;
    public bool selected
    {
        get
        {
            return _selected;
        }

        set
        {
            _selected = value;
        }
    }

    bool _preSelected = false;
    public bool preSelected
    {
        get
        {
            return _preSelected;
        }

        set
        {
            _preSelected = value;
        }
    }
    #endregion

    bool once;

    //We want the test object to be either a UI element, a 2D element or a 3D element, so we'll get the appropriate components
    SpriteRenderer thisSprite;
    EditTile thisEditableObject;

    Color color = Color.white;

    void Start()
    {

        thisSprite = this.transform.parent.GetComponent<SpriteRenderer>();
        thisEditableObject = this.transform.GetComponent<EditTile>();
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            //What the game object does with the knowledge that it is selected is entirely up to it.
            //In this case we're just going to change the color.

            if (!preSelected)
            {
                //White if deselected.
                color = Color.white;
                if (thisSprite != null)
                {
                    thisSprite.color = color;
                }
            }

            if (preSelected)
            {
                once = false;
                //Yellow if preselected
                color = Color.yellow;
                if (thisSprite != null)
                {
                    thisSprite.color = color;
                }
            }

            if (selected)
            {
                if (thisEditableObject != null)
                {
                    if (!once)
                    {
                        if (thisSprite != null)
                        {
                            thisSprite.color = color;
                        }
                        thisEditableObject.BroadcastAddOrRemoveEditList();
                        once = true;

                        selected = false;
                        preSelected = false;
                    }
                }
                return;
            }
        }
    }
}
