// Based on http://makegamessa.com/discussion/2796/shader-help-material-alpha-fade-based-on-distance-from-camera

Shader "Custom/UnlitDistanceFade"
{
	Properties
	{
		_Color("Color Tint", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Alpha (A)", 2D) = "white"
		_MinVisDistance("Min visibility distance", Float) = 5
		_MaxVisDistance("Max visibility distance", Float) = 10
	}

	SubShader
	{
		
		Tags{ "RenderType" = "Transparent" }
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		// Render

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct appdata_full
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
				float4 color    : COLOR0;
			};

			struct v2f
			{
				half4 pos       : POSITION;
				float4 color    : COLOR0;
				half2 uv        : TEXCOORD0;
			};

			float4		_Color;
			sampler2D	_MainTex;
			float       _MinVisDistance;
			float       _MaxVisDistance;

			v2f vert(appdata_full v)
			{
				v2f o;

				o.pos = mul((half4x4)UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord.xy;
				o.color = _Color;

				//distance falloff
				half3 viewDirW = _WorldSpaceCameraPos - mul((half4x4)unity_ObjectToWorld, v.vertex);
				half viewDist = length(viewDirW);
				half falloff = saturate((viewDist - _MinVisDistance) / (_MaxVisDistance - _MinVisDistance));
				o.color.a *= (1.0f - falloff);
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				fixed4 color = tex2D(_MainTex, i.uv) * i.color;
				return color;
			}

			ENDCG
		}

		//Pass {
		//	SetTexture[_MainTex] {
		//		//distance falloff
		//		half3 viewDirW = _WorldSpaceCameraPos - mul((half4x4)_Object2World, v.vertex);
		//		half viewDist = length(viewDirW);
		//		half falloff = saturate((viewDist - _MinVisDistance) / (_MaxVisDistance - _MinVisDistance));

		//		o.color.a *= (1.0f - falloff);
		//		ConstantColor[_Color]
		//		Combine Texture * constant
		//	}
		//}
		
	}
	Fallback "Unlit"
}
