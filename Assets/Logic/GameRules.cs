using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;

public class GameRules : MonoBehaviour
{

    public bool isPortalTriggersComplete = false;

    int maxPortalsTriggers;
    public List<GameObject> PortalTriggers;
    public List<GameObject> activatedPortalTriggers;

    public List<GameObject> Boxes;

    void Start()
    {
        activatedPortalTriggers = new List<GameObject>();
    }

    public void addPortalTriggers(GameObject obj)
    {
        PortalTriggers.Add(obj);
        maxPortalsTriggers = PortalTriggers.Count;
    }
    public void addBoxes(GameObject obj)
    {
        Boxes.Add(obj);
    }

    public void activatePortal(int portalIndex)
    {
        for (var i = 0; i < PortalTriggers.Count; i++)
        {
            if (PortalTriggers[i].GetComponent<Logic_PortalTrigger>().index == portalIndex)
            {
                activatedPortalTriggers.Add(PortalTriggers[i]);
                PortalTriggers.Remove(PortalTriggers[i]);
            }
        }

        if (activatedPortalTriggers.Count == maxPortalsTriggers)
        {
            isPortalTriggersComplete = true;
        }
    }

    public void DEactivatePortal(int portalIndex)
    {
        for (var i = 0; i < activatedPortalTriggers.Count; i++)
        {
            if (activatedPortalTriggers[i].GetComponent<Logic_PortalTrigger>().index == portalIndex)
            {
                PortalTriggers.Add(activatedPortalTriggers[i]);
                activatedPortalTriggers.Remove(activatedPortalTriggers[i]);
            }
        }
        if (activatedPortalTriggers.Count != maxPortalsTriggers)
        {
            isPortalTriggersComplete = false;
        }
    }
}