#version 330

in vec3 vPos;
in vec2 TexCoord;

uniform mat4 projMat;
uniform mat4 mvMat;

out vec2 f_TexCoord;

void main(){
gl_Position = projMat *mvMat * vec4(vPos, 1.0);
 f_TexCoord = TexCoord;
}