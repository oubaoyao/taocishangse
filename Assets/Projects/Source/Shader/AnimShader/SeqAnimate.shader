// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Yly/YlyUISeqFrameAni"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_RowCount("RowCount", float) = 0
		_ColCount("ColCount", float) = 0
		_Speed("Speed", float) = 30

			//����ui�ϵ�shaderһ�㶼��Ҫ�������ģ������߼������ⱻ��mask����ĸ��ڵ�����ʱû����Ч��
			_StencilComp("Stencil Comparison", Float) = 8
			_Stencil("Stencil ID", Float) = 0
			_StencilOp("Stencil Operation", Float) = 0
			_StencilWriteMask("Stencil Write Mask", Float) = 255
			_StencilReadMask("Stencil Read Mask", Float) = 255

			_ColorMask("Color Mask", Float) = 15
	}
		SubShader
		{
			Tags
			{
				"Queue" = "Transparent" //һ��ui����������Ⱦ���У���Զ������Ⱦ
				"IgnoreProjector" = "True" //����ͶӰ��һ��ui��shaderΪ���Ч�ʶ�������Ϊtrue
				"RenderType" = "Transparent"
				"PreviewType" = "Plane" //������Ԥ��ģʽΪ��Ƭ
				"CanUseSpriteAtlas" = "True" //����_MainTex����ʹ��Sprite(2D and UI)���͵���ͼ
			}

			//����ui�ϵ�shaderһ�㶼��Ҫ�������ģ������߼������ⱻ��mask����ĸ��ڵ�����ʱû����Ч��
			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}

			Cull Off
			Lighting Off //�ص����գ�һ��ui��shaderΪ���Ч�ʶ�����������
			//������������ZTestֻ����ui�ڵ������Σ�����Hierarchy��ͼ�еĲ�Σ���Ϊ���ݽ��в��ԣ�������zֵ
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			//
			Blend SrcAlpha OneMinusSrcAlpha //������ɫֵ = �����ɫֵ * �����ɫAlphaֵa + ������ɫֵ * (1 - a)���������͸����Ʒ����ǰ��Ļ������Ƹ���͸����������������Ч��
			ColorMask[_ColorMask]

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _Speed;
				float _RowCount;
				float _ColCount;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					//�������ң��������½�������֡���� 
					float totalCount = _ColCount * _RowCount; //��֡�������� _RowCount = 2��_ColCount = 5��totalCount = 5 * 2 = 10
					float curIndex = floor((_Time.y * _Speed) % totalCount); //��ǰ�ڼ�֡������ curIndex = 8
					float2 unitSize = float2(1 / _ColCount, 1 / _RowCount); //ÿһ֡��ռ��С���������� unitSize = float2(1/5, 1/2)
					float offsetU = floor(curIndex % _ColCount); //uv���u����ƫ���������� offsetU = floor(8 % 5) = 3
					float offsetV = floor((totalCount - 1 - curIndex) / _ColCount); //uv���v����ƫ���������� offsetV = floor((10 - 1 - 8)/5) = 0
					float2 originUv = float2(offsetU, offsetV) * unitSize; //uv���ƫ�������������� originUv = float2(3 * 1/5, 0 * 1/2) = float2(3/5, 0)
					float2 newUv = originUv + i.uv * unitSize; //newUv = uv���ƫ�������� + uv��С����
					//���� ui�ĸ������µ�uv����������£�
					//���½�newUv = float2(3/5, 0) + uv(0, 0) * float2(1/5, 1/2) = float2(3/5, 0) + float2(0, 0) = float2(3/5, 0)
					//���Ͻ�newUv = float2(3/5, 0) + uv(0, 1) * float2(1/5, 1/2) = float2(3/5, 0) + float2(0, 1/2) = float2(3/5, 1/2)
					//���Ͻ�newUv = float2(3/5, 0) + uv(1, 1) * float2(1/5, 1/2) = float2(3/5, 0) + float2(1/5, 1/2) = float2(4/5, 1/2)
					//���½�newUv = float2(3/5, 0) + uv(1, 0) * float2(1/5, 1/2) = float2(3/5, 0) + float2(1/5, 0) = float2(4/5, 0)
					fixed4 col = tex2D(_MainTex, newUv);
					return col;
				}
				ENDCG
			}
		}
}