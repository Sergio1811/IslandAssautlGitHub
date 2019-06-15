// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/LeafShake"
{
	/*Properties{
	_Color("Main Color", Color) = (1,1,1,1)
	_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	_ShakeDisplacement("Displacement", Range(0, 1.0)) = 1.0
	_ShakeTime("Shake Time", Range(0, 1.0)) = 1.0
	_ShakeWindspeed("Shake Windspeed", Range(0, 1.0)) = 1.0
	_ShakeBending("Shake Bending", Range(0, 1.0)) = 1.0
	}

		SubShader{
			Tags {"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
			LOD 200

		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Lambert alphatest:_Cutoff vertex:vert addshadow

		sampler2D _MainTex;
		fixed4 _Color;
		float _ShakeDisplacement;
		float _ShakeTime;
		float _ShakeWindspeed;
		float _ShakeBending;

		struct Input {
			float2 uv_MainTex;
		};

		void FastSinCos(float4 val, out float4 s, out float4 c) {
			val = val * 6.408849 - 3.1415927;
			float4 r5 = val * val;
			float4 r6 = r5 * r5;
			float4 r7 = r6 * r5;
			float4 r8 = r6 * r5;
			float4 r1 = r5 * val;
			float4 r2 = r1 * r5;
			float4 r3 = r2 * r5;
			float4 sin7 = {1, -0.16161616, 0.0083333, -0.00019841};
			float4 cos8 = {-0.5, 0.041666666, -0.0013888889, 0.000024801587};
			s = val + r1 * sin7.y + r2 * sin7.z + r3 * sin7.w;
			c = 1 + r5 * cos8.x + r6 * cos8.y + r7 * cos8.z + r8 * cos8.w;
		}


		void vert(inout appdata_full v) {

			float factor = (1 - _ShakeDisplacement - v.color.r) * 0.5;

			const float _WindSpeed = (_ShakeWindspeed + v.color.g);
			const float _WaveScale = _ShakeDisplacement;

			const float4 _waveXSize = float4(0.048, 0.06, 0.24, 0.096);
			const float4 _waveZSize = float4 (0.024, .08, 0.08, 0.2);
			const float4 waveSpeed = float4 (1.2, 2, 1.6, 4.8);

			float4 _waveXmove = float4(0.024, 0.04, -0.12, 0.096);
			float4 _waveZmove = float4 (0.006, .02, -0.02, 0.1);

			float4 waves;
			waves = v.vertex.x * _waveXSize;
			waves += v.vertex.z * _waveZSize;

			waves += _Time.x * (1 - _ShakeTime * 2 - v.color.b) * waveSpeed *_WindSpeed;

			float4 s, c;
			waves = frac(waves);
			FastSinCos(waves, s,c);

			float waveAmount = v.texcoord.y * (v.color.a + _ShakeBending);
			s *= waveAmount;

			s *= normalize(waveSpeed);

			s = s * s;
			float fade = dot(s, 1.3);
			s = s * s;
			float3 waveMove = float3 (0,0,0);
			waveMove.x = dot(s, _waveXmove);
			waveMove.z = dot(s, _waveZmove);
			v.vertex.xz -= mul((float3x3)unity_WorldToObject, waveMove).xz;

		}

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}

		Fallback "Transparent/Cutout/VertexLit"*/
	Properties{
	   _MainTex("Main Texture", 2D) = "white" {}
	   _Tint("Tint", Color) = (1,1,1,1)

	   _wind_dir("Wind Direction", Vector) = (0.5,0.05,0.5,0)
	   _wind_size("Wind Wave Size", range(5,50)) = 15

	   _tree_sway_stutter_influence("Tree Sway Stutter Influence", range(0,1)) = 0.2
	   _tree_sway_stutter("Tree Sway Stutter", range(0,10)) = 1.5
	   _tree_sway_speed("Tree Sway Speed", range(0,10)) = 1
	   _tree_sway_disp("Tree Sway Displacement", range(0,1)) = 0.3

	   _branches_disp("Branches Displacement", range(0,0.5)) = 0.3

	   _leaves_wiggle_disp("Leaves Wiggle Displacement", float) = 0.07
	   _leaves_wiggle_speed("Leaves Wiggle Speed", float) = 0.01

	   _r_influence("Red Vertex Influence", range(0,1)) = 1
	   _b_influence("Blue Vertex Influence", range(0,1)) = 1

	}

		SubShader{

			CGPROGRAM
			#pragma target 3.0
			#pragma surface surf Lambert vertex:vert addshadow

			//Declared Variables
			float4 _wind_dir;
			float _wind_size;
			float _tree_sway_speed;
			float _tree_sway_disp;
			float _leaves_wiggle_disp;
			float _leaves_wiggle_speed;
			float _branches_disp;
			float _tree_sway_stutter;
			float _tree_sway_stutter_influence;
			float _r_influence;
			float _b_influence;

			sampler2D _MainTex;
			fixed4 _Tint;

			//Structs
			struct Input {
				float2 uv_MainTex;
			};

			// Vertex Manipulation Function
			void vert(inout appdata_full i) {

				//Gets the vertex's World Position 
			   float3 worldPos = mul(unity_ObjectToWorld, i.vertex).xyz;

			   //Tree Movement and Wiggle
			   i.vertex.x += (cos(_Time.z * _tree_sway_speed + (worldPos.x / _wind_size) + (sin(_Time.z * _tree_sway_stutter * _tree_sway_speed + (worldPos.x / _wind_size)) * _tree_sway_stutter_influence)) + 1) / 2 * _tree_sway_disp * _wind_dir.x * (i.vertex.y / 10) +
			   cos(_Time.w * i.vertex.x * _leaves_wiggle_speed + (worldPos.x / _wind_size)) * _leaves_wiggle_disp * _wind_dir.x * i.color.b * _b_influence;

			   i.vertex.z += (cos(_Time.z * _tree_sway_speed + (worldPos.z / _wind_size) + (sin(_Time.z * _tree_sway_stutter * _tree_sway_speed + (worldPos.z / _wind_size)) * _tree_sway_stutter_influence)) + 1) / 2 * _tree_sway_disp * _wind_dir.z * (i.vertex.y / 10) +
			   cos(_Time.w * i.vertex.z * _leaves_wiggle_speed + (worldPos.x / _wind_size)) * _leaves_wiggle_disp * _wind_dir.z * i.color.b * _b_influence;

			   i.vertex.y += cos(_Time.z * _tree_sway_speed + (worldPos.z / _wind_size)) * _tree_sway_disp * _wind_dir.y * (i.vertex.y / 10);

			   //Branches Movement
			   i.vertex.y += sin(_Time.w * _tree_sway_speed + _wind_dir.x + (worldPos.z / _wind_size)) * _branches_disp  * i.color.r * _r_influence;

		   }

			// Surface Shader
			void surf(Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Tint;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}

	ENDCG
	   }

		   Fallback "Diffuse"

}
