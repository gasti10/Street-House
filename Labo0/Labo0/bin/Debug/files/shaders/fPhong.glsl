//FRAGMENT SHADER
#version 330
#define maxLights 10

struct Material {
	vec3 Ka;
	vec3 Kd;
	vec3 Ks;
	float Shininess;
};

struct Light {
	vec4 position; //Light position in World Space
	vec3 Ia;
	vec3 Ip;
	float coneAngle;
	vec3 coneDirection; //Cone direction in World Space
	int enabled;
};

uniform int numLights;
uniform Light allLights[maxLights];

uniform vec3 cameraPosition; //In World Space.

in vec2 f_TexCoord;
in vec3 fragNormal;
in vec3 fragPos;

uniform float A;
uniform float B;
uniform float C;

out vec4 fColor;

uniform Material material;
uniform mat4 viewMatrix;
uniform sampler2D ColorTex; 

float phongSpecular(vec3 L, vec3 N, float roughness) 
{
	float spec = 0;
	float sDotN = max( dot(L, N), 0.0 );
	vec3 reflectionVector = reflect( -L, N );
	vec3 viewDirection = normalize(cameraPosition - fragPos) ;//en bumpMapping se hace distinto
	if( sDotN > 0.0 )
		spec = pow( max( dot(reflectionVector, viewDirection), 0.0 ), roughness );
	return spec;
}

//modelo de Phong
vec3 phongModel( vec3 N, Light light,vec3 texColor ) 
{
	float attenuation = 1.0;
	vec3 L;
	if (light.position.w == 0.0) { //Directional light
		L = normalize(-light.position.xyz);
		attenuation = 1.0; //no attenuation for directional lights.
	} else { //Positional light (Spot or Point)		
		//paso la LUZ a coord de CAMARA
		vec3 posLuz = (viewMatrix * light.position).xyz;
		L = normalize(light.position.xyz - fragPos);
		
		//Cone restrictions
		vec3 coneDirection = normalize(light.coneDirection);
		vec3 rayDirection = -L;
		float lightToSurfaceAngle = degrees(acos(dot(rayDirection, coneDirection)));
		if (lightToSurfaceAngle <= light.coneAngle) { //Inside cone
			float distanceToLight = length(light.position.xyz - fragPos);
			attenuation = 1.0 / ( A + B * distanceToLight + C * pow(distanceToLight, 2));
		} else {
			attenuation = 0.0;
		}
	}	
	//Ambiente
	vec3 ambient = material.Ka * light.Ia * texColor;

	//Difuso
	float sDotN = max( dot(L, N), 0.0 );
	vec3 diffuse = sDotN * light.Ip * material.Kd * texColor;

	//Especular
	vec3 spec = phongSpecular(L, N, material.Shininess) * material.Ks;
	
	//Retorna el color final con conservacion de energia
	return ambient + attenuation * (diffuse  + spec )  * light.enabled;
}

void main(){
	vec4 fColor_tex = texture2D(ColorTex, f_TexCoord);
	
	//Acumular iluminacion de cada fuente de luz
	vec3 linearColor=vec3(0);
	for(int i=0; i<numLights; i++)
		linearColor += phongModel(fragNormal, allLights[i],fColor_tex.rgb);
	fColor = vec4(linearColor,1.0);
}
