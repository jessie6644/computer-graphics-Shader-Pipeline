// Generate a procedural planet and orbiting moon. Use layers of (improved)
// Perlin noise to generate planetary features such as vegetation, gaseous
// clouds, mountains, valleys, ice caps, rivers, oceans. Don't forget about the
// moon. Use `animation_seconds` in your noise input to create (periodic)
// temporal effects.
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
  vec3 kd;

  vec3 bump = bump_position(is_moon, sphere_fs_in);
  tangent(normalize(sphere_fs_in), T, B);
  vec3 n = normalize(cross((bump_position(is_moon, sphere_fs_in + T * epsilon) - bump)/epsilon, 
                      (bump_position(is_moon, sphere_fs_in + B * epsilon) - bump)/epsilon));


  // cloud
  vec3 cloud = vec3(5, 5, 5);
  vec3 f = vec3(1, 8, 1);
  float c_cloud = improved_perlin_noise(f * (rotate_about_y(animation_seconds / period) * vec4(sphere_fs_in, 1)).xyz);

  float time_cloud = (animation_seconds - period) / 3;
  if (time_cloud < 0){
    c_cloud = 0;
  }
  else if ((time_cloud < 1 && (time_cloud > 0))){
    c_cloud = time_cloud * c_cloud;
  }

  // mountain 
  float mountain = improved_perlin_noise(10 * sphere_fs_in);
  float phi = M_PI * sphere_fs_in.y;

  // sea
  float sea_level = min(((animation_seconds - 50) * 5*epsilon), epsilon);
  
  

  
  if (is_moon) {
    kd = vec3(0.5,0.5,0.5);
    color = blinn_phong(ka, kd, ks, p, n, v, l);
  } else {
    float height = bump_height(is_moon, sphere_fs_in);
    if (height < sea_level) {
  		ka = vec3(0.05,0.05,0.05);
      kd = vec3(0.2,0.3,0.5);
      ks = vec3(0.8,0.8,0.8);
  	} else {
      kd = vec3(0.88,0.858,0) * cos(phi) + vec3(0.45, 0.75, 0) * (1- cos(phi)) ;
      ka = vec3(0.01,0.01,0.01);
      kd = kd + mountain;
      ks = vec3(0.01,0.01,0.01);
    }
    kd = kd * (1 - c_cloud) + c_cloud * cloud;
    color = blinn_phong(ka, kd, ks, p, n, v, l);
  }
  /////////////////////////////////////////////////////////////////////////////
}
