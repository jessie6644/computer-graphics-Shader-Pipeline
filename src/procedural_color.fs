// Set the pixel color to an interesting procedural color generated by mixing
// and filtering Perlin noise of different frequencies.
//
// Uniforms:
uniform mat4 view;
uniform mat4 proj;
uniform float animation_seconds;
uniform bool is_moon;
// Inputs:
in vec3 sphere_fs_in;
in vec3 normal_fs_in;
in vec4 pos_fs_in; 
in vec4 view_pos_fs_in; 
// Outputs:
out vec3 color;

// expects: blinn_phong, perlin_noise
void main()
{
  /////////////////////////////////////////////////////////////////////////////
  // Replace with your code 
  float period = 4.0;
  float theta = 2*M_PI*1.0 * animation_seconds/period;
  vec4 ll = view * vec4(-4*sin(theta), 4, 4*cos(theta), 1);     //not sure
  vec3 v = -normalize(view_pos_fs_in.xyz / view_pos_fs_in.w);     // point position in VIEW
  vec3 l = normalize(ll.xyz/ll.w - view_pos_fs_in.xyz/view_pos_fs_in.w);
  vec3 n = normalize(normal_fs_in);
  float noise = fract((perlin_noise(20*sphere_fs_in) + 100)) * 7;
  float p = 1000;
  vec3 ka = vec3(0.1,0.1,0.1);
  vec3 ks = vec3(1,1,1);

  if (is_moon) {
    vec3 kd = vec3(0.5,0.5,0.5) * noise;
    color = blinn_phong(ka, kd, ks, p, n, v, l);
  }
  else {
    vec3 kd = vec3(0.1,0.1,0.9) * noise;
    color = blinn_phong(ka, kd, ks, p, n, v, l);
  }
  /////////////////////////////////////////////////////////////////////////////
}
