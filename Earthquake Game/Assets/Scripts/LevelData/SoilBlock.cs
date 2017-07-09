using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoilBlock {

    [SerializeField]
    public enum BlockShape { Square, TriangleLeftBottom, TriangleRightBottom, TriangleLeftTop, TriangleTopRight };

    [SerializeField]
    public Soil.SoilType type = Soil.SoilType.Bedrock;

    [SerializeField]
    public BlockShape shape = BlockShape.Square;

    public GameObject gameObject;

    public SoilBlock(BlockShape shape, Soil.SoilType type)
    {
        this.type = type;
        this.shape = shape;
    }
}
