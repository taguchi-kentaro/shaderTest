Shader "Custom/waveShader"
{
	//Unityのインスペクターに表示されるプロパティ
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Speed("Speed ", Range(1, 1000)) = 5
		_Strength("Strength ", Range(1, 100)) = 1
	}

	SubShader
	{
		
		Tags
		{ 
			"Queue"="Transparent" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
		}

		//カリングしない
		Cull Off
		//ライティングしない
		Lighting Off
		//Z書き込みしない
		ZWrite Off
		//アルファブレンドモードの指定
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			//このシェーダーのVertexシェーダーはvertという関数ではじまることを定義
			#pragma vertex vert
			//このシェーダーのFragmentシェーダーはfragという関数ではじまることを定義
			#pragma fragment frag

			#include "UnityCG.cginc"

			//頂点シェーダーへの入力パラメーターの構造体定義
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float2 texcoord  : TEXCOORD0;
			};

			//変数宣言　Propertiesと名前を合わせると同じものとして認識してくれる
			sampler2D _MainTex;

			float _Speed;

			float _Strength;



			v2f vert(appdata_t IN)
			{
				v2f OUT;
				float offsetY  = sin(IN.vertex.x + _Time * _Speed ) * _Strength;
				IN.vertex.y      += clamp(offsetY, -1, 1);
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				return OUT;
			}



			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = tex2D (_MainTex, IN.texcoord);
				return c;
			}
			
		ENDCG
		}
	}
}
