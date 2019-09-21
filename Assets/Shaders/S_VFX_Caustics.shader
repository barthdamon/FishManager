Shader "VFX/S_VFX_Caustics"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
		_Distort ("Distortion", 2D) = "white" {}
		_UVAnim ("UV Animation", vector) = (0,0,0,0)
		_Intensity("Shadow Intensity", float) = 1
		_DisAmount("Distorsion Amount", float) = 0
		_Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {
		
		"RenderType"="Transparent" 
		"Queue" = "Transparent"
	
			}
        
		Blend SrcAlpha OneMinusSrcAlpha
		Fog{ Mode Off }
		Lighting OFF
		ZWrite OFF
		Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"


			sampler2D _MainTex;
			sampler2D _Distort;
			float4 _Distort_ST;
			float4 _MainTex_ST;
			sampler2D _Mask;
			float4 _UVAnim;
			float _Intensity;
			float _DisAmount;
			float4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				
			};

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

          

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) ;
				o.uv1 = TRANSFORM_TEX(v.uv, _Distort) + frac(_Time.y * _UVAnim.xy);
				o.uv2 = v.uv;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 mask = tex2D(_Mask, i.uv2);
				fixed4 distort = tex2D(_Distort, i.uv1);
				distort = ((distort * _DisAmount) * 2) - 1;
				i.uv += distort.xy;
                fixed4 col = tex2D(_MainTex, i.uv);
				col *= mask;
				fixed4 complete;
				complete.a =  (col.rgb * _Intensity);
				complete.rgb = _Color;
				
                
                return complete;
            }
            ENDCG
        }
    }
}
