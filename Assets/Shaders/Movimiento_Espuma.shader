// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Movimiento_Espuma"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_PatronNoiseEspuma("Patron Noise Espuma", 2D) = "white" {}
		_DistanciadeDesaparicionMenoslejos("Distancia de Desaparicion(Menos+lejos)", Float) = 1
		_ColorEspuma("Color Espuma", Color) = (1,1,1,0)
		_CantidadEspumaNegativoEspuma("CantidadEspuma(Negativo+Espuma)", Float) = 0.1
		_TilingTextura1("TilingTextura1", Vector) = (1.5,1.5,0,0)
		_TilingTextura2("TilingTextura2", Vector) = (0,0,0,0)
		_VeocityTextura2("VeocityTextura2", Vector) = (0,0,0,0)
		_VelocityTextura1("VelocityTextura1", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.5
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform float4 _ColorEspuma;
		uniform sampler2D _PatronNoiseEspuma;
		uniform float2 _VelocityTextura1;
		uniform float2 _TilingTextura1;
		uniform float2 _VeocityTextura2;
		uniform float2 _TilingTextura2;
		uniform float _CantidadEspumaNegativoEspuma;
		uniform float _DistanciadeDesaparicionMenoslejos;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _ColorEspuma.rgb;
			o.Alpha = 1;
			float2 uv_TexCoord3 = i.uv_texcoord * _TilingTextura1;
			float2 panner1 = ( 1.0 * _Time.y * _VelocityTextura1 + uv_TexCoord3);
			float2 uv_TexCoord5 = i.uv_texcoord * _TilingTextura2;
			float2 panner6 = ( 1.0 * _Time.y * _VeocityTextura2 + uv_TexCoord5);
			float clampResult20 = clamp( ( _DistanciadeDesaparicionMenoslejos - _CantidadEspumaNegativoEspuma ) , 0.0 , 1.0 );
			clip( ( i.vertexColor * ( ( ( tex2D( _PatronNoiseEspuma, panner1 ).r + tex2D( _PatronNoiseEspuma, panner6 ).g ) - _CantidadEspumaNegativoEspuma ) / clampResult20 ) ).r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
354.4;476.8;764;335;15.9959;-97.8755;1.6;True;False
Node;AmplifyShaderEditor.Vector2Node;24;-923.4426,657.6825;Float;False;Property;_TilingTextura2;TilingTextura2;6;0;Create;True;0;0;False;0;0,0;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;23;-939.5143,187.4998;Float;False;Property;_TilingTextura1;TilingTextura1;5;0;Create;True;0;0;False;0;1.5,1.5;1.5,1.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;26;-673.6737,786.7224;Float;False;Property;_VeocityTextura2;VeocityTextura2;7;0;Create;True;0;0;False;0;0,0;0.02,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;25;-735.4954,319.9695;Float;False;Property;_VelocityTextura1;VelocityTextura1;8;0;Create;True;0;0;False;0;0,0;0.01,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-737.4725,640.2257;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-748.4484,178.8096;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.5,1.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;9;-852.1566,427.5221;Float;True;Property;_PatronNoiseEspuma;Patron Noise Espuma;1;0;Create;True;0;0;False;0;None;74a888bad6461c549b64da822ac27371;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;1;-513.3874,272.8559;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.01,0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;6;-480.6823,652.184;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.02,0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;19;67.55641,691.7218;Float;False;Property;_DistanciadeDesaparicionMenoslejos;Distancia de Desaparicion(Menos+lejos);2;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-332.7939,249.4494;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;74a888bad6461c549b64da822ac27371;74a888bad6461c549b64da822ac27371;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;70.2284,785.7862;Float;False;Property;_CantidadEspumaNegativoEspuma;CantidadEspuma(Negativo+Espuma);4;0;Create;True;0;0;False;0;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-300.0887,628.7774;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;6b2910686f14f5844bf4707db2d5e2ba;6b2910686f14f5844bf4707db2d5e2ba;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;18;499.4337,765.6086;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;73.35036,513.1903;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;20;682.2693,728.643;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;16;303.5938,573.7429;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;21;851.9241,555.2717;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;15;640.7687,232.6106;Float;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;950.6362,328.0162;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;4;866.8412,18.09917;Float;False;Property;_ColorEspuma;Color Espuma;3;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1173.031,33.13708;Float;False;True;3;Float;ASEMaterialInspector;0;0;Standard;Movimiento_Espuma;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;24;0
WireConnection;3;0;23;0
WireConnection;1;0;3;0
WireConnection;1;2;25;0
WireConnection;6;0;5;0
WireConnection;6;2;26;0
WireConnection;2;0;9;0
WireConnection;2;1;1;0
WireConnection;7;0;9;0
WireConnection;7;1;6;0
WireConnection;18;0;19;0
WireConnection;18;1;17;0
WireConnection;10;0;2;1
WireConnection;10;1;7;2
WireConnection;20;0;18;0
WireConnection;16;0;10;0
WireConnection;16;1;17;0
WireConnection;21;0;16;0
WireConnection;21;1;20;0
WireConnection;22;0;15;0
WireConnection;22;1;21;0
WireConnection;0;0;4;0
WireConnection;0;10;22;0
ASEEND*/
//CHKSM=09B82B983DD42624DBAC0795B9755254E7909469