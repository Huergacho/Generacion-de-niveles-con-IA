// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SphereMask"
{
	Properties
	{
		_PointPosition("Point Position", Vector) = (0,0,0,0)
		_Radius("Radius", Float) = 1
		_FallOff("FallOff", Float) = 1
		_Hight("Hight", Range( 0 , 10)) = 1
		_Color0("Color 0", Color) = (0,0,0,0)
		_Color1("Color 1", Color) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
		};

		uniform float3 _PointPosition;
		uniform float _Radius;
		uniform float _FallOff;
		uniform float _Hight;
		uniform float4 _Color1;
		uniform float4 _Color0;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float temp_output_5_0 = ( distance( _PointPosition , ase_worldPos ) / _Radius );
			v.vertex.xyz += ( ( 1.0 - saturate( pow( temp_output_5_0 , _FallOff ) ) ) * float3(0,1,0) * _Hight );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float temp_output_5_0 = ( distance( _PointPosition , ase_worldPos ) / _Radius );
			float4 lerpResult27 = lerp( _Color1 , _Color0 , saturate( temp_output_5_0 ));
			o.Albedo = lerpResult27.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;308;893;691;758.6302;385.7595;2.174519;True;True
Node;AmplifyShaderEditor.Vector3Node;1;-1000.068,50.37502;Inherit;False;Property;_PointPosition;Point Position;0;0;Create;True;0;0;0;False;0;False;0,0,0;4.775984,-0.02999995,-5.916996;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-983.3226,302.7749;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DistanceOpNode;4;-577.3427,119.1289;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-597.5383,324.7585;Inherit;False;Property;_Radius;Radius;1;0;Create;True;0;0;0;False;0;False;1;4.68;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;5;-339.5151,189.3607;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-393.1661,412.7668;Inherit;False;Property;_FallOff;FallOff;2;0;Create;True;0;0;0;False;0;False;1;7.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;8;-182.9305,289.1671;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;34;-4.244711,300.4085;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;45;224.2524,-50.88351;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;17;-187.051,602.7358;Inherit;False;Constant;_Position;Position;4;0;Create;True;0;0;0;False;0;False;0,1,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;33;132.4462,330.009;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;-66.68513,-152.0072;Inherit;False;Property;_Color0;Color 0;4;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;1.681226,-449.9085;Inherit;False;Property;_Color1;Color 1;5;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-117.0544,845.8159;Inherit;False;Property;_Hight;Hight;3;0;Create;True;0;0;0;False;0;False;1;1.5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;354.9048,356.0589;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;27;450.0107,-212.457;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1093.489,78.98273;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SphereMask;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;1;0
WireConnection;4;1;2;0
WireConnection;5;0;4;0
WireConnection;5;1;6;0
WireConnection;8;0;5;0
WireConnection;8;1;9;0
WireConnection;34;0;8;0
WireConnection;45;0;5;0
WireConnection;33;0;34;0
WireConnection;18;0;33;0
WireConnection;18;1;17;0
WireConnection;18;2;19;0
WireConnection;27;0;30;0
WireConnection;27;1;29;0
WireConnection;27;2;45;0
WireConnection;0;0;27;0
WireConnection;0;11;18;0
ASEEND*/
//CHKSM=72E3389C2666E72A9E79ECE27E0AE92F03856AA7