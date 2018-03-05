Shader "Custom/DistShader" {
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white"
		_EchoRadius("Radius", float) = 30.0
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
		sampler2D _MainTex;
		float4 _Color;
		uniform float _EchoRadius;
		uniform float4 _PlayerPosition;
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
			//float3 worldPos : TEXCOORD1;
		};

		v2f vert(appdata v)
		{
			v2f o;
			o.uv = v.uv;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.depth = distance(_PlayerPosition, mul(unity_ObjectToWorld, v.vertex).xyz);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			//CLEAN THIS
			float val = step(i.depth, _EchoRadius);

		float invert = (i.depth / _EchoRadius)*val;
		invert = invert < 0.10 ? 0 : invert;
		invert = val > 0 ? invert : 1;

		//COLOR :D
		float4 test = tex2D(_MainTex, i.uv);

		invert = clamp(test.a - invert, 0.0, 1.0);
		return fixed4(test.r, test.g, test.b, invert)*_Color;
		}
			ENDCG
		}
	}

	//	SubShader
	//	{
	//		Tags
	//	{
	//		"RenderType" = "Transparent"
	//	}

	//		ZWrite Off
	//		Blend SrcAlpha OneMinusSrcAlpha

	//		Pass
	//	{
	//		CGPROGRAM
	//#pragma vertex vert
	//#pragma fragment frag

	//#include "UnityCG.cginc"
	//		sampler2D _MainTex;
	//	float4 _Color;
	//	uniform float _EchoRadius;
	//	uniform float4 _PlayerPosition;

	//	struct appdata
	//	{
	//		float4 vertex : POSITION;
	//		float2 uv : TEXCOORD0;
	//	};

	//	struct v2f
	//	{
	//		float4 vertex : SV_POSITION;
	//		float2 uv : TEXCOORD0;
	//		float depth : DEPTH;
	//	};

	//	v2f vert(appdata v)
	//	{
	//		v2f o;
	//		o.uv = v.uv;
	//		o.vertex = UnityObjectToClipPos(v.vertex);
	//		o.depth = distance(_PlayerPosition, mul(unity_ObjectToWorld, v.vertex).xyz);
	//		return o;
	//	}

	//	fixed4 frag(v2f i) : SV_Target
	//	{
	//		//CLEAN THIS
	//		float val = step(i.depth, _EchoRadius);
	//	float invert = (i.depth / _EchoRadius)*val;
	//	invert = invert < 0.10 ? 0 : invert;
	//	invert = val > 0 ? invert : 1;

	//	//COLOR :D
	//	float4 test = tex2D(_MainTex, i.uv);
	//	invert = clamp(test.a - invert, 0.0, 1.0);
	//	return fixed4(test.r * _Color.r, test.g * _Color.g, test.b *_Color.b, invert);
	//	}
	//		ENDCG
	//	}
	//}
}
