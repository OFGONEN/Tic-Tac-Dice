// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "water"
{
	Properties
	{
		_distortionnormalmap("distortion normal map", 2D) = "white" {}
		_topfoam("top foam", 2D) = "white" {}
		_subfoam("sub foam", 2D) = "white" {}
		_topfoamcolor("top foam color", Color) = (1,1,1,0)
		_watercolor("water color", Color) = (0,0.2348454,0.6792453,0)
		_subfoamcolor("sub foam color", Color) = (0.002135996,0.1569757,0.4528302,1)
		_foamsize("foam size", Float) = 1.5
		_flowstrenght("flow strenght", Float) = 0.02
		_foamspeed("foam speed", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float4 _watercolor;
		uniform float4 _subfoamcolor;
		uniform sampler2D _subfoam;
		uniform float2 _foamspeed;
		uniform sampler2D _distortionnormalmap;
		float4 _distortionnormalmap_TexelSize;
		uniform float _flowstrenght;
		uniform float _foamsize;
		uniform float4 _topfoamcolor;
		uniform sampler2D _topfoam;


		float3 CombineSamplesSharp128_g1( float S0, float S1, float S2, float Strength )
		{
			{
			    float3 va = float3( 0.13, 0, ( S1 - S0 ) * Strength );
			    float3 vb = float3( 0, 0.13, ( S2 - S0 ) * Strength );
			    return normalize( cross( va, vb ) );
			}
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 panner92 = ( 1.0 * _Time.y * _foamspeed + float2( 0,0 ));
			float2 uv_TexCoord72 = i.uv_texcoord + panner92;
			float localCalculateUVsSharp110_g1 = ( 0.0 );
			float2 uv_TexCoord70 = i.uv_texcoord * float2( 0.15,0.15 );
			float2 temp_output_85_0_g1 = ( uv_TexCoord70 + ( _Time.y * ( 0.01 / 2.0 ) ) );
			float2 UV110_g1 = temp_output_85_0_g1;
			float4 TexelSize110_g1 = _distortionnormalmap_TexelSize;
			float2 UV0110_g1 = float2( 0,0 );
			float2 UV1110_g1 = float2( 0,0 );
			float2 UV2110_g1 = float2( 0,0 );
			{
			{
			    UV110_g1.y -= TexelSize110_g1.y * 0.5;
			    UV0110_g1 = UV110_g1;
			    UV1110_g1 = UV110_g1 + float2( TexelSize110_g1.x, 0 );
			    UV2110_g1 = UV110_g1 + float2( 0, TexelSize110_g1.y );
			}
			}
			float4 break134_g1 = tex2D( _distortionnormalmap, UV0110_g1 );
			float S0128_g1 = break134_g1.r;
			float4 break136_g1 = tex2D( _distortionnormalmap, UV1110_g1 );
			float S1128_g1 = break136_g1.r;
			float4 break138_g1 = tex2D( _distortionnormalmap, UV2110_g1 );
			float S2128_g1 = break138_g1.r;
			float temp_output_91_0_g1 = 1.5;
			float Strength128_g1 = temp_output_91_0_g1;
			float3 localCombineSamplesSharp128_g1 = CombineSamplesSharp128_g1( S0128_g1 , S1128_g1 , S2128_g1 , Strength128_g1 );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldTangent = WorldNormalVector( i, float3( 1, 0, 0 ) );
			float3 ase_worldBitangent = WorldNormalVector( i, float3( 0, 1, 0 ) );
			float3x3 ase_tangentToWorldFast = float3x3(ase_worldTangent.x,ase_worldBitangent.x,ase_worldNormal.x,ase_worldTangent.y,ase_worldBitangent.y,ase_worldNormal.y,ase_worldTangent.z,ase_worldBitangent.z,ase_worldNormal.z);
			float3 tangentToWorldDir68_g1 = mul( ase_tangentToWorldFast, localCombineSamplesSharp128_g1 );
			float3 temp_output_73_0 = ( ( float3( uv_TexCoord72 ,  0.0 ) + ( tangentToWorldDir68_g1 * _flowstrenght ) ) * _foamsize );
			float4 lerpResult86 = lerp( _watercolor , _subfoamcolor , tex2D( _subfoam, ( temp_output_73_0 + float3( float2( 0.1,0.1 ) ,  0.0 ) ).xy ));
			float4 lerpResult90 = lerp( lerpResult86 , _topfoamcolor , tex2D( _topfoam, temp_output_73_0.xy ));
			o.Albedo = lerpResult90.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
1920;6;1920;1013;3767.355;1804.932;4.028397;True;False
Node;AmplifyShaderEditor.RangedFloatNode;60;-2426.144,448.3934;Inherit;False;Constant;_flowspeed;flow speed;7;0;Create;True;0;0;0;False;0;False;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-2385.561,548.9073;Inherit;False;Constant;_size;size;6;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;66;-2170.844,342.6927;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;81;-2153.545,180.5358;Inherit;False;Constant;_Vector0;Vector 0;7;0;Create;True;0;0;0;False;0;False;0.15,0.15;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleDivideOpNode;68;-2205.143,416.3932;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;-1953.841,390.6928;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-1960.141,258.5925;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;63;-1702.041,176.7922;Inherit;True;Property;_distortionnormalmap;distortion normal map;5;0;Create;True;0;0;0;False;0;False;6205681387f1856498d9292fea307838;6205681387f1856498d9292fea307838;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleAddOpNode;69;-1682.141,370.5926;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;102;-1422.027,59.59198;Inherit;False;Property;_foamspeed;foam speed;13;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;59;-1310.541,584.5923;Inherit;False;Property;_flowstrenght;flow strenght;12;0;Create;True;0;0;0;False;0;False;0.02;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;61;-1427.541,342.5923;Inherit;True;Normal From Texture;-1;;1;9728ee98a55193249b513caf9a0f1676;13,149,0,147,0,143,0,141,0,139,0,151,0,137,0,153,0,159,0,157,0,155,0,135,0,108,0;4;87;SAMPLER2D;0;False;85;FLOAT2;0,0;False;74;SAMPLERSTATE;0;False;91;FLOAT;1.5;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;92;-1270.631,62.2868;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.05,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-1095.541,370.5923;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;72;-1098.29,62.24269;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-752.9004,199.5381;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-692.5139,416.264;Inherit;False;Property;_foamsize;foam size;11;0;Create;True;0;0;0;False;0;False;1.5;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;83;-463.9635,477.3148;Inherit;False;Constant;_subfoamoffset;sub foam offset;7;0;Create;True;0;0;0;False;0;False;0.1,0.1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-511.0872,198.6392;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexturePropertyNode;84;-250.0489,641.4041;Inherit;True;Property;_subfoam;sub foam;7;0;Create;True;0;0;0;False;0;False;ceac5166b701fb04da2b0ffdbd5be0dc;ceac5166b701fb04da2b0ffdbd5be0dc;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-257.9495,370.1891;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;89;43.71647,279.2061;Inherit;False;Property;_watercolor;water color;9;0;Create;True;0;0;0;False;0;False;0,0.2348454,0.6792453,0;0,0.2348454,0.6792453,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;87;49.71497,451.68;Inherit;False;Property;_subfoamcolor;sub foam color;10;0;Create;True;0;0;0;False;0;False;0.002135996,0.1569757,0.4528302,1;0.002135996,0.1569757,0.4528302,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;85;-6.20311,641.4247;Inherit;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;0;False;0;False;-1;ceac5166b701fb04da2b0ffdbd5be0dc;4335425bd84a52544be6a81ea9848cbc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;74;-530.3246,7.766113;Inherit;True;Property;_topfoam;top foam;6;0;Create;True;0;0;0;False;0;False;ceac5166b701fb04da2b0ffdbd5be0dc;ceac5166b701fb04da2b0ffdbd5be0dc;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.LerpOp;86;382.2886,491.3676;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;58;-258.4788,169.7867;Inherit;True;Property;_flowtexture;flow texture;5;0;Create;True;0;0;0;False;0;False;-1;ceac5166b701fb04da2b0ffdbd5be0dc;4335425bd84a52544be6a81ea9848cbc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;88;370.135,-45.50401;Inherit;False;Property;_topfoamcolor;top foam color;8;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;-2066.491,-263.4665;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;0;False;0;False;9.24;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;18;-708.666,-835.6747;Inherit;False;Property;_maincolor;maincolor;3;0;Create;True;0;0;0;False;0;False;0.1084016,0.2678744,0.7924528,0;0.1084016,0.2678744,0.7924528,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-1044.143,-553.2291;Inherit;False;Property;_Color0;Color 0;1;0;Create;True;0;0;0;False;0;False;0.1241545,0.5607433,0.8490566,0;0.3039783,0.5129928,0.6509434,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VoronoiNode;2;-2525.393,-649.3691;Inherit;True;0;0;1;0;1;False;1;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SamplerNode;51;-1724.056,-644.0585;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;68842673c24adb448a7805a1645c8816;68842673c24adb448a7805a1645c8816;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;9;-2728.675,-611.8694;Inherit;False;Property;_ripplescale;ripple scale;2;0;Create;True;0;0;0;False;0;False;7.48;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-2182.49,-389.4662;Inherit;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;0;False;0;False;1.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;90;648.3997,130.5202;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-666.7358,-472.8886;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-2482.749,-324.8539;Inherit;False;Property;_ripplepower;ripple power;0;0;Create;True;0;0;0;False;0;False;1.38;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-452.6658,-797.3865;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-1982.49,-623.4661;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;6;-2279.394,-628.7867;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;79;1239.158,-39.38817;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;68;0;60;0
WireConnection;68;1;57;0
WireConnection;67;0;66;0
WireConnection;67;1;68;0
WireConnection;70;0;81;0
WireConnection;69;0;70;0
WireConnection;69;1;67;0
WireConnection;61;87;63;0
WireConnection;61;85;69;0
WireConnection;92;2;102;0
WireConnection;64;0;61;0
WireConnection;64;1;59;0
WireConnection;72;1;92;0
WireConnection;71;0;72;0
WireConnection;71;1;64;0
WireConnection;73;0;71;0
WireConnection;73;1;80;0
WireConnection;82;0;73;0
WireConnection;82;1;83;0
WireConnection;85;0;84;0
WireConnection;85;1;82;0
WireConnection;86;0;89;0
WireConnection;86;1;87;0
WireConnection;86;2;85;0
WireConnection;58;0;74;0
WireConnection;58;1;73;0
WireConnection;2;2;9;0
WireConnection;51;1;45;0
WireConnection;90;0;86;0
WireConnection;90;1;88;0
WireConnection;90;2;58;0
WireConnection;17;0;1;0
WireConnection;19;0;18;0
WireConnection;19;1;17;0
WireConnection;45;0;6;0
WireConnection;45;1;46;0
WireConnection;6;0;2;0
WireConnection;6;1;7;0
WireConnection;79;0;90;0
ASEEND*/
//CHKSM=3007EE32E4EA1F2E03BBDFE29C324E7995B77242