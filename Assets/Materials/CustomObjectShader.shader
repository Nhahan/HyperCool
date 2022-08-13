Shader "Custom/StandardSpecularSurfaceShader" {
    Properties {
    	_Cutoff( "Mask Clip Value", Float ) = 0.5
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
    	_DisolveGuide("Disolve Guide", 2D) = "white" {}
    	_DissolveAmount("Dissolve Amount", Range( 0 , 1)) = 0
		_Intensity("Intensity", Float) = 0
        _SpecularColor("Specular", Color) = (0.2,0.2,0.2)
    	_Emission("Emission", float) = 0
        _EmissionColor("Color", Color) = (0,0,0)
    }
    SubShader {
//        Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
    	Tags { "RenderType" = "Opaque"}
		Cull Off
        LOD 200
   
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf StandardSpecular fullforwardshadows
 
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
 
        sampler2D _MainTex;
 
        struct Input {
            float2 uv_MainTex;
        };

        fixed4 _EmissionColor;
        half _Glossiness;
        fixed3 _SpecularColor;
        fixed4 _Color;
        uniform float _DissolveAmount;
		uniform sampler2D _DisolveGuide;
		uniform float4 _DisolveGuide_ST;
		uniform sampler2D _BurnRamp;
		uniform float4 _Color0;
		uniform float _Intensity;
		uniform float _Cutoff = 0.7;
 
        void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Specular from specular color
            o.Specular = _SpecularColor;
            // Smoothness come from slider variable
            o.Smoothness = _Glossiness;
            float2 uv_DisolveGuide = IN.uv_MainTex * _DisolveGuide_ST.xy + _DisolveGuide_ST.zw;
			float temp_output_73_0 = ( (-0.6 + (( 1.0 - _DissolveAmount ) - 0.0) * (0.6 - -0.6) / (1.0 - 0.0)) + tex2D( _DisolveGuide, uv_DisolveGuide ).r );
			float clampResult113 = clamp( (-4.0 + (temp_output_73_0 - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float temp_output_130_0 = ( 1.0 - clampResult113 );
			float2 appendResult115 = (float2(temp_output_130_0 , 0.0));
        	o.Emission = c.rgb * _EmissionColor * 2;
			o.Alpha = 1;
			clip( temp_output_73_0 - _Cutoff );
        }
        ENDCG
    }
    FallBack "Diffuse"
}