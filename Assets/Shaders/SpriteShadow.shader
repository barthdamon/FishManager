Shader "Custom/SpriteShadow" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		[PerRendererData]_MainTex ("Sprite Texture", 2D) = "white" {}
		_Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.5

		_Amount("Sheer", Range(-1,1)) = 0.0

	}
	SubShader {
		Tags 
		{ 
			"Queue"="Geometry"
			"RenderType"="TransparentCutout"
		}
		LOD 200

		Cull Off	//Back would be better but doesn't allow flipping the sprite in the renderer

		CGPROGRAM
		// Lambert lighting model, and enable shadows on all light types
		#pragma surface surf Lambert addshadow fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		fixed _Cutoff;
		float _Amount;

		struct Input
		{
			float2 uv_MainTex;
			float4 vertColor : COLOR;
		};

		void vert(inout appdata_full v) 
		{
			//v.vertex.xyz += v.normal * _Amount;
			v.vertex.xyz += v.vertex.y * float4(1, 0, 0, 0) * _Amount;
			v.vertex.xyz += v.vertex.x * v.vertex.y * float4(0, -1, 0, 0) * _Amount;
		}


		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color * 2.0f;
			o.Albedo = c.rgb * IN.vertColor.rgb;
			o.Alpha = c.a;
			clip(o.Alpha - _Cutoff);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
