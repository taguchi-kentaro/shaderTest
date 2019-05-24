Shader "Custom/SimpleShader"
{
	//Unityのインスペクターに表示されるプロパティ
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader
	{
		//SubShader 順番や他のパラメーターを判定するのに使用
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
			//ピクセルシェーダーへの入力パラメーター構造体定義
			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float2 texcoord  : TEXCOORD0;
			};

			//変数宣言　Propertiesと名前を合わせると同じものとして認識してくれる
			sampler2D _MainTex;

			//頂点シェーダー
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				//オブジェク座標系からスクリーン座標までの変換全部これいっぱつ
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				//頂点のUV座標を渡す（この値そのまま渡されるわけではない）
				OUT.texcoord = IN.texcoord;
				return OUT;
			}


			//ピクセルシェーダー
			fixed4 frag(v2f IN) : SV_Target
			{
				//テクスチャの指定UV座標から色を取得
				fixed4 c = tex2D (_MainTex, IN.texcoord);
				return c;
			}
			
		ENDCG
		}
	}
}