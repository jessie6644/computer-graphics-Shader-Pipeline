// Set the pixel color using Blinn-Phong shading (e.g., with constant blue and
// gray material color) with a bumpy texture.
// 
// Uniforms:
uniform mat4 view;
uniform mat4 proj;
uniform float animation_seconds;
uniform bool is_moon;
// Inputs:
//                     linearly interpolated from tessellation evaluation shader
//                     output
in vec3 sphere_fs_in;
in vec3 normal_fs_in;
in vec4 pos_fs_in; 
in vec4 view_pos_fs_in; 
// Outputs:
//               rgb color of this pixel
out vec3 color;
// expects: model, blinn_phong, bump_height, bump_position,
// improved_perlin_noise, tangent
void main()
{
  /////////////////////////////////////////////////////////////////////////////
  // Replace with your code 
  vec3 T, B;
  float epsilon = 0.0001;
  float period = 4.0;
  float theta = 2*M_PI*1.0 * animation_seconds/period;
  vec4 ll = view * vec4(-4*sin(theta), 4, 4*cos(theta), 1);     //not sure
  vec3 v = -normalize(view_pos_fs_in.xyz / view_pos_fs_in.w);     // point position in VIEW
  vec3 l = normalize(ll.xyz/ll.w - view_pos_fs_in.xyz/view_pos_fs_in.w);

  float p = 1000;
  vec3 ka = vec3(0.1,0.1,0.1);
  vec3 ks = vec3(1,1,1);

  vec3 bump = bump_position(is_moon, sphere_fs_in);
  tangent(normalize(sphere_fs_in), T, B);
  vec3 n = normalize(cross((bump_position(is_moon, sphere_fs_in + T * epsilon) - bump)/epsilon, 
                      (bump_position(is_moon, sphere_fs_in + B * epsilon) - bump)/epsilon));
  
  if (is_moon) {
    vec3 kd = vec3(0.5,0.5,0.5);
    color = blinn_phong(ka, kd, ks, p, n, v, l);
  }
  else {
    vec3 kd = vec3(0.1,0.1,0.9);
    color = blinn_phong(ka, kd, ks, p, n, v, l);
  }
  /////////////////////////////////////////////////////////////////////////////
}
