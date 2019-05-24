Shader "Custom/dotShader"
{
	//Unityのインスペクターに表示されるプロパティ
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PixelSize("PixelSize", float) = 1
		_TextureSize("TextureSize", float) = 2048
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

			float _PixelSize;

			float _TextureSize;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				float2 dotsize = 1/2048;
 				float size = _TextureSize / _PixelSize;
				float2 dot = floor(uv * size)  / size;
				fixed4 col = float4(0, 0, 0, 0);
				for(int i = 0 ; i < _PixelSize ; i++){
					for(int j = 0 ; j < _PixelSize; j++){
						col += tex2D(_MainTex, dot + dotsize * float2(i, j)) * (1.0 / (_PixelSize * _PixelSize));
					}
				}
				return col;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord);
				return c;
			}
			
		ENDCG
		}
	}
}
