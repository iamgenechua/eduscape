using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorStateChanger : MonoBehaviour {

    [SerializeField] private ShipEngineConnectorSegment[] segments;
    [SerializeField] private Material metalMaterial;
    [SerializeField] private Material waterMaterial;

    [SerializeField] private SegmentChangeGlow[] segmentGlows;
    [SerializeField] private float changeDuration;
    [SerializeField] private Material metalGlowMaterial;
    [SerializeField] private Color metalGlowColor;
    [SerializeField] private Material waterGlowMaterial;
    [SerializeField] private Color waterGlowColor;

    private Dictionary<ShipEngineConnectorSegment.State, (Material segmentMaterial, Material glowMaterial, Color glowColor)> stateMaterialsDict
        = new Dictionary<ShipEngineConnectorSegment.State, (Material segmentMaterial, Material glowMaterial, Color glowColor)>();

    public bool IsChanging { get; private set; }

    void Awake() {
        stateMaterialsDict[ShipEngineConnectorSegment.State.METAL] = (metalMaterial, metalGlowMaterial, metalGlowColor);
        stateMaterialsDict[ShipEngineConnectorSegment.State.WATER] = (waterMaterial, waterGlowMaterial, waterGlowColor);
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
        foreach (SegmentChangeGlow glow in segmentGlows) {
            glow.GlowMaterial = stateMaterialsDict[newState].glowMaterial;
            glow.GlowColor = stateMaterialsDict[newState].glowColor;
            StartCoroutine(glow.BrieflyGlow(changeDuration, () => ChangeSegmentStates(newState), () => IsChanging = false));
        }
    }

    private void ChangeSegmentStates(ShipEngineConnectorSegment.State newState) {
        foreach (ShipEngineConnectorSegment segment in segments) {
            segment.ChangeState(newState, stateMaterialsDict[newState].segmentMaterial);
        }
    }
}
