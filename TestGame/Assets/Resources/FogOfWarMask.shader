Shader "Custom/FogOfWarMask" {
	Properties{
		_Color("Color", Color) = (145, 145, 145, 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BlurPower("BlurPower", float) = 0.002
		_PaintTex("Paint Texture (RGBA)", 2D) = "white" {} 
		_BlendAmount("Blend Amount", Range(0.0, 1.0)) = 1.0
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		//Lighting off
		//LOD 200

		CGPROGRAM
#pragma surface surf Lambert alpha:fade
#pragma target 3.0

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, float aten){

			fixed4 color;
			color.rgb = s.Albedo;
			color.a = s.Alpha;
			return color;
		}

		float _BlurPower;
		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _PaintTex;
		fixed _BlendAmount;		

	struct Input {
		float2 uv_MainTex;
		float2 uv_PaintTex;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		half4 baseColor1 = tex2D(_MainTex, IN.uv_MainTex + float2(-_BlurPower, 0));
		half4 baseColor2 = tex2D(_MainTex, IN.uv_MainTex + float2(0, -_BlurPower));
		half4 baseColor3 = tex2D(_MainTex, IN.uv_MainTex + float2(_BlurPower, 0));
		half4 baseColor4 = tex2D(_MainTex, IN.uv_MainTex + float2(0, _BlurPower));
		half4 baseColor = 0.25 * (baseColor1 + baseColor2 + baseColor3 + baseColor4);

		fixed4 paint = tex2D(_PaintTex, IN.uv_MainTex);

		o.Albedo = baseColor.rgb;
		o.Albedo = lerp((_Color.rgb * baseColor.b), paint, _Color.a * _BlendAmount);
		o.Alpha = _Color.a - baseColor.g;
	}
	ENDCG
	}

}