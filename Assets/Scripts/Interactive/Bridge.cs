using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;

public class Bridge : MonoBehaviour
{
    [SerializeField]
    public int Id;

    private Transform _bridgeRoadTransform;

    [SerializeField]
    private ObjectState _bridgeState;
    [SerializeField]
    public ObjectState BridgeState
    {
        get { return _bridgeState; }
        set
        {
            _bridgeState = value;

            foreach (var zone in Zones)
            {
                zone.ZoneType = BridgeState == ObjectState.Activated ? ZoneType.Walkable : ZoneType.DeathZone;
            }

            _startScaleMarker = _bridgeRoadTransform.localScale;
            _startMoveMarker = _bridgeRoadTransform.localPosition;
            _startLerp = true;
        }
    }

    [SerializeField]
    private List<Zone> Zones;

    // Animation

    private float _lerpTime;

    private Vector3 _startScaleMarker;

    private readonly Vector3 _minScaleMarker = new Vector3(0f, 1f, 1f);
    private readonly Vector3 _maxScaleMarker = new Vector3(1f, 1f, 1f);

    private Vector3 _startMoveMarker;

    private readonly Vector3 _minMoveMarker = new Vector3(-0.547f, 0.02222222f, -0.2777778f);
    private readonly Vector3 _maxMoveMarker = new Vector3(0.02222222f, 0.02222222f, -0.2777778f);

    private bool _startLerp;

    // Use this for initialization
    void Start()
    {
        _bridgeState = ObjectState.Deactivated;

        Zones = new List<Zone>();

        Transform[] allChildren = transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform objT in allChildren)
        {
            if (string.Equals(objT.gameObject.name, "BridgeRoadSprite"))
            {
                _bridgeRoadTransform = objT;

                _bridgeRoadTransform.localScale = BridgeState == ObjectState.Activated ? _maxScaleMarker : _minScaleMarker;
                _bridgeRoadTransform.localPosition = BridgeState == ObjectState.Activated ? _maxMoveMarker : _minMoveMarker;
            }
            else if (objT.gameObject.name.Contains("Zone"))
            {
                var zone = objT.GetComponent<Zone>();
                zone.ZoneType = BridgeState == ObjectState.Activated ? ZoneType.Walkable : ZoneType.DeathZone;

                Zones.Add(zone);
            }
        }
    }

    private void MoveUpdate()
    {
        _bridgeRoadTransform.localPosition = Vector3.Lerp(_startMoveMarker, BridgeState == ObjectState.Activated ? _maxMoveMarker : _minMoveMarker, _lerpTime);
    }

    private void ScaleUpdate(Vector3 endMarker, ref bool referenceBool)
    {
        _bridgeRoadTransform.localScale = Vector3.Lerp(_startScaleMarker, endMarker, _lerpTime);

        MoveUpdate();

        _lerpTime = _lerpTime + Utils.LerpRatio * Utils.LerpSpeed;

        if (!(_lerpTime >= 1)) return;

        _lerpTime = 0f;
        _bridgeRoadTransform.localScale = endMarker;

        referenceBool = false;
    }

    void Update()
    {
        if (_startLerp)
            if (BridgeState == ObjectState.Activated)
                ScaleUpdate(_maxScaleMarker, ref _startLerp);
            else
                ScaleUpdate(_minScaleMarker, ref _startLerp);

    }
}