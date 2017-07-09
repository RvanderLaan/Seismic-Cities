using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileSet {

    public static readonly Dictionary<Soil.SoilType, Material> Materials = new Dictionary<Soil.SoilType, Material>
    {
        { Soil.SoilType.Bedrock,    (Material) Resources.Load("Materials/Plates") },
        { Soil.SoilType.Clay,       (Material) Resources.Load("Materials/Clay") },
        { Soil.SoilType.Rock,       (Material) Resources.Load("Materials/Marl") },
        { Soil.SoilType.SoftRock,   (Material) Resources.Load("Materials/Limestone") },
        { Soil.SoilType.Sand,       (Material) Resources.Load("Materials/Sand") },

    };

    public static readonly Dictionary<SoilBlock.BlockShape, GameObject> Shapes = new Dictionary<SoilBlock.BlockShape, GameObject>
    {
        { SoilBlock.BlockShape.Square,              (GameObject) Resources.Load("Shapes/Square") },
        { SoilBlock.BlockShape.TriangleLeftBottom,  (GameObject) Resources.Load("Shapes/TriangleLeftBottom") },
        { SoilBlock.BlockShape.TriangleLeftTop,     (GameObject) Resources.Load("Shapes/TriangleLeftTop") },
        { SoilBlock.BlockShape.TriangleRightBottom, (GameObject) Resources.Load("Shapes/TriangleRightBottom") },
        { SoilBlock.BlockShape.TriangleTopRight,    (GameObject) Resources.Load("Shapes/TriangleTopRight") },
    };
}
