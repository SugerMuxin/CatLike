// 2017.9.21 Clean
Shader "Custom/Particle/ParticleAddColor" 
{
	Properties 
	{
		_TintColor ("Tint Color", Color) = (1.0,1.0,1.0,1.0)
		_MainTex ("Particle Texture", 2D) = "white" {}
	}

	Category 
	{
		Tags 
		{ 
			"Queue"="Transparent+500" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
		}
		
		Blend SrcAlpha One
		Cull Off 
		Lighting Off 
		ColorMask RGB
		ZWrite Off 
		Fog { Color (0,0,0,0) }
		
		SubShader 
		{
			Pass 
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				//#include "Assets/Resources/UnityShaderCompatible.cginc"

				sampler2D _MainTex;
				fixed4 _TintColor;
				
				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};
				
				float4 _MainTex_ST;
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					return o;
				}
				
				fixed4 frag (v2f i) : SV_Target
				{
					return 2.0*i.color* _TintColor * tex2D(_MainTex, i.texcoord);
				}
				ENDCG 
			}
		}	
	}
}
