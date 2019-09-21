Shader "VFX/S_VFX_AnimatedUV"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
		_UVAnim ("UV Animation", vector) = (0,0,0,0)
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
			sampler2D _Mask;
			float4 _MainTex_ST;
			float4 _UVAnim;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
			};

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

          

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex) + frac(_Time.y * _UVAnim.xy);;
				o.uv1 = v.uv1;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 mask = tex2D(_Mask, i.uv1);
                fixed4 col = tex2D(_MainTex, i.uv);
				col.a *= mask;
                
                return col;
            }
            ENDCG
        }
    }
}
