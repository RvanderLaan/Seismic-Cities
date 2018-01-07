Shader "Custom/Soil" {
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_TexAlpha("Texture opacity", Range(0, 1)) = 0.2
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_BumpStrength("Bump strength", Range(0,1)) = 0.5
	}

	SubShader {
		Tags { "Queue" = "Geometry-1" "RenderType" = "Opaque" }
		// Write '1' to the stencil buffer where this material is rendered
		Stencil {
			Ref 2
			Comp Always
			Pass Replace
		}

		// Render

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Standard shader
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpMap;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		half _TexAlpha;
		fixed4 _Color;
		half _BumpStrength;

		void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color * _TexAlpha + (1 - _TexAlpha) * _Color;
			o.Albedo = c.rgb;
			o.Normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpStrength);
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

/*
// Unlit shader
Pass {
// #pragma vertex vert
// #pragma fragment frag
// #include "UnityCG.cginc"

struct appdata
{
float4 vertex : POSITION;
float2 uv : TEXCOORD0;
};

struct v2f
{
float2 uv : TEXCOORD0;
float4 vertex : SV_POSITION;
};

sampler2D _MainTex;
float4 _MainTex_ST;
float4 _Color;

v2f vert(appdata v)
{
v2f o;
o.vertex = UnityObjectToClipPos(v.vertex);
o.uv = TRANSFORM_TEX(v.uv, _MainTex);
return o;
}

fixed4 frag(v2f i) : SV_Target
{
// sample the texture and apply custom color
fixed4 col = tex2D(_MainTex, i.uv) * _Color;
return col;
}

ENDCG
}
*/
