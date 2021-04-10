using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorStateChanger : MonoBehaviour {

    [SerializeField] private ShipEngineConnectorSegment[] segments;
    [SerializeField] private SegmentChangeGlow[] segmentGlows;
    [SerializeField] private float changeDuration;

    [SerializeField] private ConnectorStateData[] stateData;
    [SerializeField] private GameObject metalIndicator;
    [SerializeField] private GameObject waterIndicator;

    private Dictionary<ShipEngineConnectorSegment.State, (ConnectorStateData stateDataEntry, GameObject indicator)> stateMaterialsDict
        = new Dictionary<ShipEngineConnectorSegment.State, (ConnectorStateData stateDataEntry, GameObject indicator)>();

    public bool IsChanging { get; private set; }

    void Awake() {
        stateMaterialsDict[ShipEngineConnectorSegment.State.METAL] =
            (System.Array.Find(stateData, stateDataEntry => stateDataEntry.State == ShipEngineConnectorSegment.State.METAL),
            metalIndicator);
        stateMaterialsDict[ShipEngineConnectorSegment.State.WATER] =
            (System.Array.Find(stateData, stateDataEntry => stateDataEntry.State == ShipEngineConnectorSegment.State.WATER),
            waterIndicator);
    }

    void Update() {

    }

    public void HitByElement(Element element) {
        if (element.ElementType == Element.Type.METAL || element.ElementType == Element.Type.WATER) {
            switch (element.ElementType) {
                case Element.Type.METAL:
                    ChangeSegmentStatesOverTime(ShipEngineConnectorSegment.State.METAL);
                    break;
                case Element.Type.WATER:
                    ChangeSegmentStatesOverTime(ShipEngineConnectorSegment.State.WATER);
                    break;
                default:
                    throw new System.ArgumentException($"No connector segment state corresponds to element of type {element.ElementType}.");
            }
        }
    }

    private void ChangeSegmentStatesOverTime(ShipEngineConnectorSegment.State newState) {
        if (IsChanging) {
            return;
        }

        IsChanging = true;

        if (newState == ShipEngineConnectorSegment.State.METAL) {
            metalIndicator.SetActive(true);
            waterIndicator.SetActive(false);
        } else if (newState == ShipEngineConnectorSegment.State.WATER) {
            metalIndicator.SetActive(false);
            waterIndicator.SetActive(true);
        }

        stateMaterialsDict[newState].indicator.SetActive(true);
        foreach (SegmentChangeGlow glow in segmentGlows) {
            glow.GlowMaterial = stateMaterialsDict[newState].stateDataEntry.GlowMaterial;
            glow.GlowColor = stateMaterialsDict[newState].stateDataEntry.GlowColor;
            StartCoroutine(glow.BrieflyGlow(changeDuration, () => ChangeSegmentStates(newState), () => IsChanging = false));
        }
    }

    private void ChangeSegmentStates(ShipEngineConnectorSegment.State newState) {
        foreach (ShipEngineConnectorSegment segment in segments) {
            segment.ChangeState(newState, stateMaterialsDict[newState].stateDataEntry.SegmentMaterial);
        }
    }
}
