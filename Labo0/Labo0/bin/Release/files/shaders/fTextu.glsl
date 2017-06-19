#version 150

//in vec4 fragColor;
in vec2 f_TexCoord;
out vec4 fColor;
uniform sampler2D gSampler; 

void main(){
  fColor = texture2D(gSampler, f_TexCoord.st);

}
