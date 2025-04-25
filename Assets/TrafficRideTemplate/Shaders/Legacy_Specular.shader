// Orginally writed by ALIyerEdon Summer-Fall 2016
Shader "LightingEffects/Legacy/Diffuse_Specular" 
{
	Properties 
	{
    	// Shader properties that you can see and change it on inspector
		_Color ("Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader // Main shader body
	{

		Tags { "RenderType"="Opaque" }
		LOD 150
		
		CGPROGRAM

		// BlinnPhong lighting for specular shaders + vert function for vertex shader program
		#pragma surface surf BlinnPhong

		// Can run on GLES2+ - Works on all platforms supported by Unity
		#pragma target 2.0

		// This is reWriteded properties on top for use in shader code
		sampler2D _MainTex;
		half _Shininess;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

      // Pixel shader
	  void surf (Input IN, inout SurfaceOutput o) 
	  {

	 		 // Albedo (main texture)
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

            // Gloss and Specular (specular map texture and power+shininess)
            o.Gloss = c.a;
            o.Specular = c.a*_Shininess;
}
		ENDCG
	}
	FallBack "Diffuse"
}
