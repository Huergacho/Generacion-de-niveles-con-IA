// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaveMask"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 2
		_PointPosition("PointPosition", Vector) = (0,0,0,0)
		_Radius("Radius", Float) = 0
		_FallOff("FallOff", Float) = 0
		_Hight("Hight", Float) = 2.5
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float3 worldPos;
		};

		uniform float3 _PointPosition;
		uniform float _Radius;
		uniform float _FallOff;
		uniform float _Hight;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float temp_output_17_0 = sin( ( _Time.y + pow( ( distance( _PointPosition , ase_worldPos ) / _Radius ) , _FallOff ) ) );
			v.vertex.xyz += ( temp_output_17_0 * float3(0,1,0) * _Hight );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color21 = IsGammaSpace() ? float4(0.4887595,0,0.8396226,0) : float4(0.2037842,0,0.673178,0);
			float4 color22 = IsGammaSpace() ? float4(0.7955213,0.2227216,0.8584906,0) : float4(0.596264,0.04063099,0.7077566,0);
			float3 ase_worldPos = i.worldPos;
			float temp_output_17_0 = sin( ( _Time.y + pow( ( distance( _PointPosition , ase_worldPos ) / _Radius ) , _FallOff ) ) );
			float4 lerpResult20 = lerp( color21 , color22 , temp_output_17_0);
			o.Albedo = lerpResult20.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;393;941;606;900.5574;135.8491;1;False;False
Node;AmplifyShaderEditor.Vector3Node;1;-937.3247,5.271255;Inherit;False;Property;_PointPosition;PointPosition;5;0;Create;True;0;0;0;False;0;False;0,0,0;-1.9062,0,38;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-952.9767,163.4655;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DistanceOpNode;3;-745.6729,86.77721;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-745.6655,220.7384;Inherit;False;Property;_Radius;Radius;6;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;5;-593.8824,141.5557;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-559.2855,266.3978;Inherit;False;Property;_FallOff;FallOff;7;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;19;-435.1599,-131.4632;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;7;-399.1372,96.40414;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-179.295,-76.5413;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-90.26598,462.3199;Inherit;False;Property;_Hight;Hight;8;0;Create;True;0;0;0;False;0;False;2.5;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;17;-59.42231,32.21211;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;14;-81.83296,304.8887;Inherit;False;Constant;_Vector1;Vector 1;3;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;21;-147.3028,-720.8704;Inherit;False;Constant;_Color0;Color 0;4;0;Create;True;0;0;0;False;0;False;0.4887595,0,0.8396226,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-153.4939,-522.754;Inherit;False;Constant;_Color1;Color 1;4;0;Create;True;0;0;0;False;0;False;0.7955213,0.2227216,0.8584906,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;180.3402,281.2883;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;20;135.4256,-555.7734;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;418.9093,-42.80825;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;WaveMask;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;2;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;5;0;3;0
WireConnection;5;1;6;0
WireConnection;7;0;5;0
WireConnection;7;1;8;0
WireConnection;18;0;19;0
WireConnection;18;1;7;0
WireConnection;17;0;18;0
WireConnection;15;0;17;0
WireConnection;15;1;14;0
WireConnection;15;2;16;0
WireConnection;20;0;21;0
WireConnection;20;1;22;0
WireConnection;20;2;17;0
WireConnection;0;0;20;0
WireConnection;0;11;15;0
ASEEND*/
//CHKSM=32255A7CACBD2CC20EEF05CB33B555D2701B4B17