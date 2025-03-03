Shader "HoloZoo/GlobeOceanShader"
{
	Properties
	{
		_MaskTex("Mask Tex", 2D) = "white" {}
		_Color ("Color (RGBA)", Color) = (0.0, 0.0, 1.0, 0.33)
		_UVRect ("UV Rect", Vector) = (0,0,1,1)
		_Threshold ("Blue Channel Threshold", Range(0, 1)) = 0.1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Cull Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MaskTex;
			fixed4 _Color;
			float4 _UVRect;
			float _Threshold;

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
				o.uv = lerp(_UVRect.xy, _UVRect.xy + _UVRect.zw, v.uv);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 maskColor = tex2D(_MaskTex, i.uv);
				#if !UNITY_COLORSPACE_GAMMA
				maskColor.rgb = LinearToGammaSpace(maskColor.rgb);
				#endif
				
				// Clip pixels where blue channel is below threshold
				clip(maskColor.b - _Threshold);
				
				// Return the specified color
				return _Color;
			}
			ENDCG
		}
	}
}