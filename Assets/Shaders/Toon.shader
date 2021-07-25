// Toon shader heavily inspired from this tutorial https://roystan.net/articles/toon-shader.html
// This toon shader uses the Blinn-Phong illumination model

Shader "Custom/Toon"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 0.5, 1)
		_MainTex("Main Texture", 2D) = "white" {}	
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.3,0.3,0.3,1)
		// Specular color that tins the specular reflection
		[HDR]
		_SpecularColor("Specular Color", Color) = (0.8,0.8,0.8,1)

		// Controls how reflective the specular reflection is
		_Shininess("Glossiness", Float) = 32
		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
	}
	SubShader
	{
		Pass
		{
			// Setup pass to only receive data on the main directional
			// light and ambient light and to use Forward Rendering
			Tags
			{
				"RenderType"= "Opaque"
				"LightMode" = "ForwardBase"
				"PassFlags" = "OnlyDirectional"
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			// Unity shortcut to compile all variants necessary
			// for forward base rendering
			#pragma multi_compile_fwdbase
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldNormal : NORMAL;
				float3 viewDir : TEXCOORD1;

				// Defined in AutoLight.cginc, creates a 4D value
				// with varying precision and assigns it to 
				// TEXCOORD2 semantic
				SHADOW_COORDS(2)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				// World view direction for specular reflection
				o.viewDir = WorldSpaceViewDir(v.vertex);

				// Defined in AutoLight.cginc, transforms the input
				// vertex's space to the shadow map's space
				TRANSFER_SHADOW(o);
				return o;
			}
			
			float4 _Color;
			float4 _AmbientColor;
			float _Shininess;
			float4 _SpecularColor;
			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;

			float4 frag (v2f i) : SV_Target
			{
				float3 normal = normalize(i.worldNormal);

				// WorldSpaceViewDir doesn't normalize the vector
				float3 viewDir = normalize(i.viewDir);

				// Since we're using Blinn-Phong, tutorial uses Blinn-Phong,
				// the specular reflection is now measured as the dot product
				// between the normal and half vector instead
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);

				// Calculate the dot product between the normal vector
				// and the direction to the light source
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				// Calculate the dot product between the half vector
				// and the normal vector
				float NdotH = dot(halfVector, normal);

				// Defined in AutoLight.cginc, SHADOW_ATTENUATION returns a value
				// between 0 and 1, where 0 indicates no shadow and 1 is fully shadowed
				float shadow = SHADOW_ATTENUATION(i);

				// Divide the lighting into two bands, light and dark to
				// achieve the toon like shading using smoothstep to smoothly
				// blend between the light and dark sides
				float lightStrength = smoothstep(0, 0.01, NdotL * shadow);

				// Include the directional light's color in the calculation
				// _LightColor0 is the main directional light which is declared
				// in Lighting.cginc
				float4 light = lightStrength * _LightColor0;

				// Calculate the specular intensity when the surface is lit
				float specularStrength = pow(NdotH * lightStrength, _Shininess * _Shininess);

				// Use smoothstep to smoothly blend the specular reflections to achieve
				// the toon like specular reflection
				float specularStrengthSmooth = smoothstep(0.005, 0.01, specularStrength);
				float4 specular = specularStrengthSmooth * _SpecularColor;

				// Calculate the dot product of the normal and view direction vector
				// and then inverting it to get the rim
				float4 rimDot = 1 - dot(normal, viewDir);

				// Make the rim lighting only appear on illuminated surfaces of the object
				// and control how far the rim extends along the lit surface
				float rimStrength = rimDot * pow(NdotL, _RimThreshold);

				// Use smoothstep to smoothly blend the rim lighting to achieve the
				// toon like rim lighting
				rimStrength = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimStrength);
				float4 rim = rimStrength * _RimColor;

				float4 sample = tex2D(_MainTex, i.uv);

				return _Color * sample * (_AmbientColor + light + specular + rim);
			}
			ENDCG
		}
		// Cast shadows
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}