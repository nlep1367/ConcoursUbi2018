// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/DistShader" {
	Properties
	{
		_ColorBark("BarkColor", Color) = (0,0,0,1)
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white"
		_EchoRadius("Radius", float) = 30.0
		_BarkRadius("Bark Radius", float) = 0.0
		_ContourWidth("Width of Bark Contour", float) = 0.2
		_DogPosition("Dog Position", vector) = (0,0,0,0)
		_PlayerPosition("Player Position", vector) = (0,0,0,0)
	}

		SubShader
		{
			Tags
		{
			"RenderType" = "Opaque"
		}
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On
			Pass
		{
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"
			//Properties
			sampler2D _MainTex;
			float4 _Color;
			float4 _ColorBark;
			uniform float _EchoRadius;
			uniform float4 _PlayerPosition;
			uniform float _BarkRadius;
			uniform float _ContourWidth;
			uniform float4 _DogPosition;
			
		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			float2 uv : TEXCOORD0;
			float depth : DEPTH;
			float3 bark : TEXCOORD1;
		};

		v2f vert(appdata v)
		{
			v2f o;
			o.uv = v.uv;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.depth = distance(_PlayerPosition, mul(unity_ObjectToWorld, v.vertex).xyz);
			o.bark = mul(unity_ObjectToWorld, v.vertex).xyz;
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			float dist, valBark, val, valCirc, invert;
			float barkContour = _BarkRadius - _ContourWidth;
			//Girl Radius
			val = step(i.depth, _EchoRadius);
			//Distance for the bark
			dist = distance(i.bark, _DogPosition);
			//Edge of the bark radius
			valBark = step(dist, _BarkRadius);
			//Inside of the bark radius
			valCirc = step(dist, barkContour);
			
			//Alpha values
			invert = (i.depth / _EchoRadius)*val;
			//Visible radius
			invert = invert < 0.1 ? 0 : invert;

			//If in bark echo calculate new alpha
			invert = valBark > 0 ? 1 - (valBark - (valCirc*(dist / _BarkRadius))) : invert;
			
			//Get base color
			fixed4 endcolor = valBark > 0 ? _ColorBark : fixed4(0,0,0,1);
			//If in girl visible radius and no bark
			if (val > 0 && valBark < 1)
			{
				float4 test = tex2D(_MainTex, i.uv);
				endcolor = fixed4(test.r*(val), test.g*(val), test.b*(val), 1) *_Color;
			}

			return fixed4(endcolor.r, endcolor.g, endcolor.b, 1 - invert);
		}
			ENDCG
		}
	}

		SubShader
		{
			Tags
		{
			"RenderType" = "Transparent"
		}

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
		{
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"
		sampler2D _MainTex;
		float4 _Color;
		float4 _ColorBark;
		uniform float _EchoRadius;
		uniform float4 _PlayerPosition;
		uniform float _BarkRadius;
		uniform float _ContourWidth;
		uniform float4 _DogPosition;


		struct appdata
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			float2 uv : TEXCOORD0;
			float depth : DEPTH;
			float3 bark : TEXCOORD1;
		};

		v2f vert(appdata v)
		{
			v2f o;
			o.uv = v.uv;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.depth = distance(_PlayerPosition, mul(unity_ObjectToWorld, v.vertex).xyz);
			o.bark = mul(unity_ObjectToWorld, v.vertex).xyz;
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			float dist, valBark, val, valCirc, invert;
			float barkContour = _BarkRadius - _ContourWidth;
			//Girl Radius
			val = step(i.depth, _EchoRadius);
			//Distance for the bark
			dist = distance(i.bark, _DogPosition);
			//Edge of the bark radius
			valBark = step(dist, _BarkRadius);
			//Inside of the bark radius
			valCirc = step(dist, barkContour);

			//Alpha values
			invert = (i.depth / _EchoRadius)*val;
			//Visible radius
			invert = invert < 0.1 ? 0 : invert;

			//If in bark echo calculate new alpha
			invert = valBark > 0 ? 1 - (valBark - (valCirc*(dist / _BarkRadius))) : invert;

			//Get base color
			fixed4 endcolor = valBark > 0 ? _ColorBark : fixed4(0,0,0,1);
			//If in girl visible radius and no bark
			if (val > 0 && valBark < 1)
			{
				float4 test = tex2D(_MainTex, i.uv);
				endcolor = fixed4(test.r*(val), test.g*(val), test.b*(val), test.a) *_Color;
			}

			return fixed4(endcolor.r, endcolor.g, endcolor.b, clamp(endcolor.a - invert, 0, 1));
		}
		ENDCG
		}
	}
			Fallback "Diffuse"
}
