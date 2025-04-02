// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32904,y:32704,varname:node_3138,prsc:2|alpha-9414-OUT,refract-7958-OUT;n:type:ShaderForge.SFN_Tex2d,id:6631,x:32236,y:32921,ptovrint:False,ptlb:wenli,ptin:_wenli,varname:node_6631,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Append,id:5401,x:32459,y:32921,varname:node_5401,prsc:2|A-6631-R,B-6631-G;n:type:ShaderForge.SFN_Vector1,id:9414,x:32504,y:32848,varname:node_9414,prsc:2,v1:0;n:type:ShaderForge.SFN_Multiply,id:7958,x:32609,y:33001,varname:node_7958,prsc:2|A-5401-OUT,B-6631-A,C-5206-A,D-9723-OUT;n:type:ShaderForge.SFN_VertexColor,id:5206,x:32400,y:33068,varname:node_5206,prsc:2;n:type:ShaderForge.SFN_Slider,id:9723,x:32319,y:33233,ptovrint:False,ptlb:qiangdu,ptin:_qiangdu,varname:node_9723,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;proporder:6631-9723;pass:END;sub:END;*/

Shader "Shader Forge/niuqu" {
    Properties {
        _wenli ("wenli", 2D) = "white" {}
        _qiangdu ("qiangdu", Range(0, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _wenli; uniform float4 _wenli_ST;
            uniform float _qiangdu;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _wenli_var = tex2D(_wenli,TRANSFORM_TEX(i.uv0, _wenli));
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (float2(_wenli_var.r,_wenli_var.g)*_wenli_var.a*i.vertexColor.a*_qiangdu);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);

                float3 finalColor = 0;
                return fixed4(lerp(sceneColor.rgb, finalColor,0.0),1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
