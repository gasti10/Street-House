//VERTEX SHADER
#version 330

in vec3 vPos;
in vec2 TexCoord;
in vec3 vNormal;
in vec3 vTangente;
in vec3 vBitangente; 

out vec2 f_TexCoord;
out vec3 fragPos;
out vec3 fragNormal;

uniform mat4 projMat;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

void main(){
	//Tengo que usar tang y bitang para que no las descarte y falle al compilar
	vec3 alpedo = (vTangente + vBitangente) * 0.0001;

	 gl_Position = projMat * viewMatrix * modelMatrix * vec4(vPos, 1.0)+ alpedo - alpedo;
	 
	 f_TexCoord = vec2(TexCoord.s, 1 - TexCoord.t);
	 
	 mat4 mvMatrix = viewMatrix * modelMatrix;
	 mat3 normalMatrix = transpose(inverse( mat3(mvMatrix) ));
	 
	 // posicion de los vertices en coordenas de la camara
	 fragPos = vec3( mvMatrix * vec4(vPos,1.0) );
	 //vector N
	 fragNormal = normalize(normalMatrix * vNormal);
	 
}