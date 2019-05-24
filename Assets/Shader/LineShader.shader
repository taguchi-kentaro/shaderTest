Shader "Custom/LineShader"
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
                float2 dotsize = 1/_TextureSize;
                fixed4 basecol = tex2D(_MainTex, uv); 
                fixed4 col = float4(0, 0, 0, 0);
                
                col += tex2D(_MainTex, uv + dotsize * float2(-1, -1)) * (1.0 / 8);
                col += tex2D(_MainTex, uv + dotsize * float2(0, -1)) * (1.0 / 8);
                col += tex2D(_MainTex, uv + dotsize * float2(1, -1)) * (1.0 / 8);
                col += tex2D(_MainTex, uv + dotsize * float2(-1, 0)) * (1.0 / 8);
                col += tex2D(_MainTex, uv + dotsize * float2(1, 0)) * (1.0 / 8);
                col += tex2D(_MainTex, uv + dotsize * float2(-1, 1)) * (1.0 / 8);
                col += tex2D(_MainTex, uv + dotsize * float2(0, 1)) * (1.0 / 8);
                col += tex2D(_MainTex, uv + dotsize * float2(1, 1)) * (1.0 / 8);
                
                fixed4 ret = abs(basecol - col);
                float average = (ret.r + ret.g + ret.b);
                average = 1.0 - average;
                ret = fixed4(average,average,average,col.a);

				return ret;
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
