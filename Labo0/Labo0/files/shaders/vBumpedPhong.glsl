// VERTEX SHADER. Simple

#version 330

in vec2 TexCoord;
in vec3 vPos;
in vec3 vNormal;
in vec4 vTangente;

out vec3 fnormal;
out vec2 f_TexCoord;
out vec3 fPos_CS;
out mat3 TBN;

uniform mat4 projMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

// --- SHADOW MAPPING ---
// Matriz que convierte al espacio de la luz y lo mapea de [-1,1] a [0,1] para acceder a la textura
uniform mat4 uLightBiasMatrix;
// Posicion del fragmento en el espacio de la luz.
out vec4 fragPosLightSpace;

void main()
{
	mat4 modelViewMatrix = viewMatrix * modelMatrix;
	mat3 normalMatrix = transpose(inverse( mat3(modelViewMatrix) ));

	//Transformar vectores N, T y B de ObjectSpace a CameraSpace
	vec3 N = normalize(normalMatrix * vNormal);
	vec3 T = normalize(normalMatrix * vTangente.xyz);
	vec3 B = normalize(cross(N, T) * vTangente.w);

	//Construir la Matrix de transformacion de CameraSpace a TangentSpace
	TBN = transpose( mat3(T, B, N) );

	//Transformar Posicion de ObjectSpace a CameraSpace
	fPos_CS = vec3( modelViewMatrix * vec4(vPos,1.0) );

	fnormal = vNormal;

	//Invertir la coordenada "y" de textura
	f_TexCoord = vec2(TexCoord.s, 1 - TexCoord.t);

	gl_Position = projMatrix * viewMatrix * modelMatrix * vec4(vPos, 1.0);

	fragPosLightSpace = uLightBiasMatrix * modelMatrix * vec4(vPos, 1.0);
}
