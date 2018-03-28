Shader "Custom/ObjectHighlight" {

	Properties
	{
		//TOONY COLORS
		_Color("Color", Color) = (0.5,0.5,0.5,1.0)
		_HColor("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_SColor("Shadow Color", Color) = (0.3,0.3,0.3,1.0)

		//DIFFUSE
		_MainTex("Main Texture (RGB)", 2D) = "white" {}

	//TOONY COLORS RAMP
	_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}

	_OutlineColor("Outline color", Color) = (0, 0, 0, 1)
		_OutlineWidth("Outlines width", Range(0.0, 0.03)) = 0.0005
	}

		CGINCLUDE
#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
	};

	struct v2f
	{
		float4 pos : POSITION;
	};

	uniform float _OutlineWidth;
	uniform float4 _OutlineColor;
	uniform sampler2D _MainTex;
	uniform float4 _Color;

	ENDCG

		SubShader
	{
		Tags{ "RenderType" = "Custom/ObjectHighlight" "Queue" = "Transparent" "IgnoreProjector" = "True" }

		Pass //Outline
	{
		ZWrite Off
		Cull Back
		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

		v2f vert(appdata v)
	{
		appdata original = v;
		//Add Outline Width in direction of vertex
		v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz);
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		return o;

	}

	half4 frag(v2f i) : COLOR
	{
		return  _OutlineColor;
	}

		ENDCG
	}
		//Render Geometry on top
		Tags{ "Queue" = "Geometry" }

		CGPROGRAM
#pragma surface surf ToonyColorsCustom
#pragma target 2.0
#pragma glsl

		struct Input
	{
		float2 uv_MainTex;
	};
	//================================================================
	// CUSTOM LIGHTING

	//Lighting-related variables
	fixed4 _HColor;
	fixed4 _SColor;
	sampler2D _Ramp;

	//Custom SurfaceOutput
	struct SurfaceOutputCustom
	{
		fixed3 Albedo;
		fixed3 Normal;
		fixed3 Emission;
		half Specular;
		fixed Alpha;
	};

	inline half4 LightingToonyColorsCustom(SurfaceOutputCustom s, half3 lightDir, half3 viewDir, half atten)
	{
		s.Normal = normalize(s.Normal);
		fixed ndl = max(0, dot(s.Normal, lightDir)*0.5 + 0.5);

		fixed3 ramp = tex2D(_Ramp, fixed2(ndl, ndl));
#if !(POINT) && !(SPOT)
		ramp *= atten;
#endif
		_SColor = lerp(_HColor, _SColor, _SColor.a);	//Shadows intensity through alpha
		ramp = lerp(_SColor.rgb, _HColor.rgb, ramp);
		fixed4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * ramp;
		c.a = s.Alpha;
#if (POINT || SPOT)
		c.rgb *= atten;
#endif
		return c;
	}


	//================================================================
	// SURFACE FUNCTION

	void surf(Input IN, inout SurfaceOutputCustom o)
	{
		fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);

		o.Albedo = mainTex.rgb * _Color.rgb;
		o.Alpha = mainTex.a * _Color.a;

	}

	ENDCG
	}
		Fallback "Diffuse"
		CustomEditor "TCF_MaterialInspector"
}