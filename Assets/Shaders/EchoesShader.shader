Shader "Custom/EchoesShader" {
	Properties{
		_ColorBackGround("ColorBG", Color) = (1,1,1,1)
		_Color("Color Echo", Color) = (0,0,0,1)
		_EchoRadius("Radius", float) = 30.0
		_EchoWidth("Echo Line Width", float) = 0.2
		_EchoCenter("EchoCenter", vector) = (0,0,0,0)
	}
		SubShader{
			Tags
			{
				"RenderType" = "Custom/EchoesShader"
				"DisableBatching" = "True"
			}
			LOD 200
		ZWrite Off
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		
	Pass{
			CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
			float _EchoRadius;
			float _EchoWidth;
			float4 _EchoCenter;
			float4 _Color;
			float4 _ColorBackGround;


			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 pos : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.pos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}


			fixed4 frag(v2f i) : SV_Target
			{
				float dist, valEcho, valCirc;
				float EchoContour = _EchoRadius - _EchoWidth;
				//Distance for the echo
				dist = distance(i.pos, _EchoCenter);
				//Edge of the bark radius
				valEcho = step(dist, _EchoRadius);
				//Inside of the bark radius
				valCirc = step(dist, EchoContour);

				//Get base color
				fixed4 endcolor = valEcho > 0 ? fixed4(_Color.r, _Color.g, _Color.b, (valEcho - (valCirc*(dist / _EchoRadius)))) : _ColorBackGround;

				return endcolor;
			}
			ENDCG

		}
	}
}
