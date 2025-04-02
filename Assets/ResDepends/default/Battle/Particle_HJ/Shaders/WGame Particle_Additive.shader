Shader "WGame Particle/Additive" {
	Properties {
		_TintColor ("Tint Color", Vector) = (0.5,0.5,0.5,0.5)
		_ColorPow ("Color Power", Range(0, 4)) = 1
		_MainTex ("Particle Texture", 2D) = "white" {}
		_ScrollSpeedX ("Scroll Speed X", Range(-10, 10)) = 0
		_ScrollSpeedY ("Scroll Speed Y", Range(-10, 10)) = 0
		[HideInInspector] _MainTex_ST_Proxy ("MainTex_ST Proxy", Vector) = (1,1,0,0)
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 8
	}
	SubShader {
		Pass {
			LOD 100
			Tags { "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha One, SrcAlpha One
			ColorMask RGB 
			ZWrite Off
			Cull Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float4 color : COLOR0;
				float2 texcoord : TEXCOORD0;
			};
			
			float4 _CurvedParam;
			float4 _MainTex_ST;
			float _ScrollSpeedX;
			float _ScrollSpeedY;
			float4 _MainTex_ST_Proxy;
			float4 _TintColor;
			float _ColorPow;
			sampler2D _MainTex;
			
			v2f vert(appdata_full v)
			{
				v2f o;
				float4 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz,1));
				float4 viewPos = mul(unity_MatrixV, worldPos);
				float4 tmp1;
				tmp1.x = viewPos.x * _CurvedParam.y;
				tmp1.x = tmp1.x * tmp1.x;
				tmp1.x = viewPos.y * viewPos.y + tmp1.x;
				tmp1.y = tmp1.x * -_CurvedParam.x;
				tmp1.xzw = float3(0.0, 0.0, 0.0);
				viewPos = viewPos + tmp1;
				o.position = mul(UNITY_MATRIX_P, viewPos);
				
				float4 color = v.color * _TintColor;
				tmp1.x = _ColorPow;
				tmp1.w = 2.0;
				color = color * tmp1.xxxw;
				o.color = color * float4(2.0, 2.0, 2.0, 1.0);
				float4 texcoord = v.texcoord.xyxy * _MainTex_ST.xyxy;
				texcoord.zw = _MainTex_ST.zw + _MainTex_ST_Proxy.zw;
				texcoord.xy = texcoord.xy * _MainTex_ST_Proxy.xy + texcoord.zw;
				tmp1.xw = float2(0.0, 0.0);
				tmp1.yz = float2(_ScrollSpeedX, _ScrollSpeedY) * _Time.yy;
				tmp1 = frac(tmp1);
				texcoord.xy = texcoord.xy + tmp1.xy;
				o.texcoord.xy = tmp1.zw + texcoord.xy;
				
				return o;
			}
			
			float4 frag(v2f inp) : SV_Target
			{
				float4 o;
				float4 tmp0;
				tmp0 = tex2D(_MainTex, inp.texcoord.xy);
				tmp0 = tmp0 * inp.color;
				o.xyz = saturate(tmp0.xyz);
				o.w = tmp0.w;
				return o;
			}
			ENDCG
		}
	}
}