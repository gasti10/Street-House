#version 330

in vec2 f_TexCoord;
out vec4 fColor;
uniform sampler2D gSampler; 

void main(){

//vec2 flippedCoor = vec2(f_TexCoord.s, 1-f_TexCoord.t);
 // fColor = texture2D(gSampler, flippedCoor);
  fColor = texture2D(gSampler, f_TexCoord);

}
