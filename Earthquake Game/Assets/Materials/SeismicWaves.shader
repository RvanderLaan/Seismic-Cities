// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/SeismicWaves"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 0, 0, 1)
		_Center("Epicenter", Vector) = (0, 0, 0, 0)
		_Speed("Speed", Range(0, 100)) = 10
		_Width("Line width", Range(0, 100)) = 5
		_Range("Maximum range of waves", Range(0, 1000)) = 500
		_Gap("Gap between lines", Int) = 2
		_StartTime("Start time of waves", Float) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			fixed4 _Color;
			float2 _Center;
			float _Speed;
			float _Width;
			float _Range;
			int _Gap;
			float _StartTime;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float2 sqUv = i.uv * _ScreenParams.xy;
				
				float dist = distance(sqUv, _Center.xy);
				
				float actualWidth = dist / _Width;
				float period = _Speed * 6.2831;

				// If distance > time, show the wave
				float isTime = step(actualWidth * period / 2, (_Time.y - _StartTime) * 1000);
				
				float tetha = (1 - _Time % 1);
				float s1 = sin(period * tetha + actualWidth) / 2 + 0.5;
				float s2 = sin(period * tetha + actualWidth * _Gap) / 2 + 0.5;
				float m = max(s1, s2);

				fixed4 newCol = saturate(m * col + (1 - m) * _Color);

				// Fade out
				// float r = dist / _Range;
				// col = saturate((1 - r) * newCol + r * col);

				col = newCol * isTime + col * (1 - isTime);
				return col;
			} 
			ENDCG
		}
	}
}
