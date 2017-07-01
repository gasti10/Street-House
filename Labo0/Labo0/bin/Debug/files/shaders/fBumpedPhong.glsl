// FRAGMENT SHADER. Simple

#version 330
#define maxLights 7

in vec2 f_TexCoord;

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

float phongSpecular(vec3 lightDirection, vec3 viewDirection, vec3 surfaceNormal, float roughness) 
{
	float spec = 0;
	float sDotN = max( dot(lightDirection, surfaceNormal), 0.0 );
	vec3 reflectionVector = reflect( -lightDirection, surfaceNormal );
	if( sDotN > 0.0 )
		spec = pow( max( dot(reflectionVector, viewDirection), 0.0 ), roughness );
	return spec;
}

//Calculo de la iluminacion por metodo de Phong
vec3 phongModel( vec3 norm, vec3 diffR, Light light, vec3 ViewDir) 
{
	float fAtt = 1.0;
	vec3 LightPos;
	float falloff = 1.0;
	float shadow = 0;

	if(light.position.w == 0)
	{ 
		LightPos = normalize( transpose(inverse(TBN)) * ( (transpose(inverse(viewMatrix)) * -light.position).xyz) );
		
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
			fAtt = (0.5 / ( A + B * distanceToLight + C * pow(distanceToLight, 2)) );

			if(light.coneAngle < 180)
			{
				//Atenuacion de los bordes 
				float innerCone = light.coneAngle*0.75;
				falloff = smoothstep(light.coneAngle, innerCone, lightToSurfaceAngle);
			}
		}//Fuera del cono 
		else 
			fAtt = 0.0;
	}

	//Ambiente
	vec3 ambient = material.Ka * light.Ia * diffR;

	//Difuso
	float sDotN = max( dot(LightPos, norm), 0.0 );
	vec3 diffuse = sDotN * light.Ip * material.Kd * diffR;

	//Especular
	//vec3 spec = cookTorranceSpecular(LightPos, ViewDir, norm, 0.25, 1) * material.Ks;;
	vec3 spec = phongSpecular(LightPos, ViewDir, norm, material.Shininess) * material.Ks;
	//vec3 spec = blinnPhongSpecular(LightPos, ViewDir, norm, material.Shininess * 4) * material.Ks;;

	//Retorna el color final con conservacion de energia
	return (ambient + fAtt * (1 - shadow) * falloff * (diffuse * 0.6 + spec * 0.4) ) * light.enabled;
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

/*
vec3 phongModel( vec3 norm, vec3 diffR, vec3 specR, Light light, vec3 ViewDir) 
{
	float fAtt;
	vec3 LightPos;
	float falloff;

	if(light.direccional==1)
	{ 
		//La Pos de la Luz se interpreta como una direccion centrada en el origen WS
		//Invierto la pos de la luz (en WS) y tengo extremo de la dir (en WS). La paso a CS
		//Invierto la pos de la camara (en WS) para obtener el origen (de WS) en CameraSpace
		//Extremo - Origen, me da la dir de Luz (centrada en origen de mundo) en CS
		//vec3 direccion = ((viewMatrix * -light.position).xyz) - (-cameraPosition);

		//Transformar DIRECCION de la Luz de CameraSpace a TangentSpace
		//LightPos = normalize( TBN *  direccion);

		LightPos = normalize( TBN * ( (viewMatrix * -light.position).xyz) );
	}
	else
	{
		//Transformar POSICION de la Luz de CameraSpace a TangentSpace
		LightPos = normalize( TBN * ( (viewMatrix * light.position).xyz - fPos_CS) );

		//Restricciones del cono de luz
		vec3 coneDirection = normalize(TBN * (mat3(viewMatrix) * light.coneDirection) );
		vec3 rayDirection = -LightPos;
		float lightToSurfaceAngle = degrees(acos(dot(rayDirection, coneDirection)));
		if (lightToSurfaceAngle <= light.coneAngle) 
		{ 
			//Atenuacion a la distancia
			float distanceToLight = length(light.position.xyz);
			fAtt = (0.5 / ( A + B * distanceToLight + C * pow(distanceToLight, 2)) );

			//Atenuacion de los bordes 
			float innerCone = light.coneAngle*0.75;
			falloff = smoothstep(light.coneAngle, innerCone, lightToSurfaceAngle);
		} else 
			fAtt = 0.0;
	}

	//Ambiente
	vec3 ambient = material.Ka * diffR * light.Ia;

	//Difuso
	float sDotN = max( dot(LightPos, norm), 0.0 );
	vec3 diffuse = diffR * sDotN * light.Id;// * material.Kd;

	//Especular
	vec3 r = reflect( -LightPos, norm );
	vec3 spec = vec3(0.0);
	if( sDotN > 0.0 )
		spec = material.Ks * pow( max( dot(r,ViewDir), 0.0 ), material.Shininess ) * light.Is * specR;

	return ambient + fAtt * falloff * (diffuse + spec*0.001) * light.enabled;
}

void main() 
{
	// Lookup the normal from the normal map
	vec4 normal = 2.0*texture2D( NormalMapTex, f_TexCoord ) - 1;

	// The color texture is used as the diff. reflectivity
	vec4 texColor = texture2D( ColorTex, f_TexCoord );

	// The specular texture is used as the spec intensity
	vec4 specular = texture2D( SpecularMapTex, f_TexCoord );

	//Transformar Posicion de CameraSpace a TangentSpace
	vec3 ViewDir = TBN * normalize(-fPos_CS);

	//Acumular iluminacion de cada fuente de luz
	vec3 linearColor=vec3(0);
	for(int i=0; i<numLights; i++)
		linearColor += phongModel(normal.xyz, texColor.rgb, specular.rgb, allLights[i], ViewDir);

	FragColor = vec4( linearColor, 1.0 );
}
*/