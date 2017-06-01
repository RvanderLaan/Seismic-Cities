using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilBlock : MonoBehaviour {

   
    public enum BlockShape { Square, TriangleLeftBottom, TriangleRightBottom, TriangleLeftTop, TriangleTopRight };
    
    public Soil.SoilType type = Soil.SoilType.Bedrock;
    public BlockShape shape = BlockShape.Square;

    public SoilBlock(BlockShape shape, Soil.SoilType type)
    {
        this.type = type;
        this.shape = shape;
    }
}
