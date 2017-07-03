// FRAGMENT SHADER. Simple

#version 330
#define maxLights 11

in vec2 f_TexCoord;
in vec3 fnormal;
in mat3 TBN;
in vec3 fPos_CS;

struct Light {
	vec4 position; //Light position in World Space
	vec3 Ia;
	vec3 Ip;
	float coneAngle;
	vec3 coneDirection; //Cone direction in World Space
	int enabled;
};

struct Material {
	vec3 Ka;
	vec3 Kd;
	vec3 Ks;
	float Shininess;
};

uniform sampler2D ColorTex;
uniform sampler2D NormalMapTex;
uniform sampler2D SpecularMapTex;
uniform mat4 viewMatrix;
uniform vec3 cameraPosition; //In World Space.
uniform int numLights;
uniform Light allLights[maxLights];
uniform Material material;

uniform float A;
uniform float B;
uniform float C;

out vec4 FragColor;

// --- SHADOW MAPPING ---
uniform int shadowsOn;
// Sampler del shadow map.
uniform sampler2D uShadowSampler;
// Posicion del fragmento en el espacio de la luz.
in vec4 fragPosLightSpace;

// Calcula la visibilidad del fragmento respecto la luz.
// Retorna 1 si es visible y 0 si no lo es.
float ShadowCalculation(vec4 fragPosLightSpace)
{
	vec3 lightDir = normalize(allLights[0].position.xyz - fragPosLightSpace.xyz);
	float bias    = max(0.0001 * (1.0 - dot(fnormal, lightDir)), 0.00001);
	float shadowDepth = texture(uShadowSampler, fragPosLightSpace.xy).z;
	float fragDepth   = fragPosLightSpace.z;

	float shadow = 0.0;
	vec2 texelSize = 1.0/textureSize(uShadowSampler, 0);
	for (int x = -1; x <= 1; ++x){
		for (int y = -1; y <= 1; ++y){
			float pcfDepth = texture( uShadowSampler, fragPosLightSpace.xy + vec2(x, y) * texelSize).z;
			shadow += fragDepth - bias > pcfDepth ? 1.0f : 0.0f;
		}
	}
	shadow /= 9;
	// Si el fragmento esta fuera del alcance del shadow map entonces es visible.
	if (fragDepth > 1.0)
		shadow = 0.0;

	return shadow;
}

float phongSpecular(vec3 lightDirection, vec3 viewDirection, vec3 N, float roughness) 
{
	float spec = 0;
	float sDotN = max( dot(lightDirection, N), 0.0 );
	vec3 reflectionVector = reflect( -lightDirection, N );
	if( sDotN > 0.0 )
		spec = pow( max( dot(reflectionVector, viewDirection), 0.0 ), roughness );
	return spec;
}

//Calculo de la iluminacion por metodo de Phong
vec3 phongModel( vec3 norm, vec3 diffR, Light light, vec3 ViewDir) 
{
	float attenuation = 1.0;
	vec3 LightPos;
	float falloff = 1.0;
	float shadow = 0;

	if(light.position.w == 0)
	{ 
		LightPos = normalize( transpose(inverse(TBN)) * ( (transpose(inverse(viewMatrix)) * -light.position).xyz) );
		if(shadowsOn == 1)
			shadow = ShadowCalculation(fragPosLightSpace); 
	}
	else
	{
		//Transformar POSICION de la Luz de CameraSpace a TangentSpace
		LightPos = normalize( TBN * ( (viewMatrix * light.position).xyz - fPos_CS) );

		//Restricciones del cono de luz
		vec3 coneDirection = normalize(TBN * (mat3(viewMatrix) * light.coneDirection) );
		vec3 rayDirection = -LightPos;
		float lightToSurfaceAngle = degrees(acos(dot(rayDirection, coneDirection)));
		//Dentro del cono
		if (lightToSurfaceAngle <= light.coneAngle) 
		{ 
			//Atenuacion a la distancia
			float distanceToLight = length(light.position.xyz);
			attenuation = (0.5 / ( A + B * distanceToLight + C * pow(distanceToLight, 2)) );

			if(light.coneAngle < 180)
			{
				//Atenuacion de los bordes 
				float innerCone = light.coneAngle*0.75;
				falloff = smoothstep(light.coneAngle, innerCone, lightToSurfaceAngle);
			}
		}//Fuera del cono 
		else 
			attenuation = 0.0;
	}

	//Ambiente
	vec3 ambient = material.Ka * light.Ia * diffR;

	//Difuso
	float sDotN = max( dot(LightPos, norm), 0.0 );
	vec3 diffuse = sDotN * light.Ip * material.Kd * diffR;

	//Especular
	vec3 spec = phongSpecular(LightPos, ViewDir, norm, material.Shininess) * material.Ks;
	
	//Retorna el color final con conservacion de energia
	return (ambient + attenuation * (1 - shadow) * falloff * (diffuse * 0.6 + spec * 0.4) ) * light.enabled;
}

void main() 
{
	// The color texture is used as the diff. reflectivity
	vec4 texColor = texture2D( ColorTex, f_TexCoord );

	//Descarto los fragmentos con valor de transparencia
	if(texColor.a < 0.5)
		discard;

	// Lookup the normal from the normal map
	vec4 normal = 2.0*texture2D( NormalMapTex, f_TexCoord ) - 1;

	//Transformar Posicion de CameraSpace a TangentSpace
	vec3 ViewDir = TBN * normalize(-fPos_CS);

	//Acumular iluminacion de cada fuente de luz
	vec3 linearColor=vec3(0);
	for(int i=0; i<numLights; i++)
		linearColor += phongModel(normal.xyz, texColor.rgb, allLights[i], ViewDir);

	FragColor = vec4( linearColor, 1.0 );
}

