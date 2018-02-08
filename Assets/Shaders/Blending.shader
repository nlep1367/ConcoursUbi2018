Shader "Custom/Blending"
{
	Properties
	{
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
        _BlendTex("Blending Texture", 2D) = "white" {}
		_BlendValue("BlendValue", Range(0,1)) = 0
	}

	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		Pass 
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BlendTex;
			float _BlendValue;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 show = tex2D(_BlendTex, i.uv);
				
				float moy = (col.r + col.g + col.b) / 3;
				
				if(moy <= _BlendValue)
				{
					if(col.a > _BlendValue)
					{
						col.a = _BlendValue;
					}
				}
				else
				{
					col.a = 0;
				}
				return col;
			}
			ENDCG
		}		
	}
	FallBack "Toony Colors Free/Rim Lighting"
}
