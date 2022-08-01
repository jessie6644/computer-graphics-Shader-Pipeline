// Construct the model transformation matrix. The moon should orbit around the
// origin. The other object should stay still.
//
// Inputs:
//   is_moon  whether we're considering the moon
//   time  seconds on animation clock
// Returns affine model transformation as 4x4 matrix
//
// expects: identity, rotate_about_y, translate, PI, uniform_scale
mat4 model(bool is_moon, float time)
{
  /////////////////////////////////////////////////////////////////////////////
  // Replace with your code 
  if(is_moon){
    float period = 4.0;
    float theta = 2*M_PI*1.0 * time / period;
    return rotate_about_y(theta) * transpose(translate(vec3(2.0, 0, 0))) * uniform_scale(0.3);
  }else{
    return identity();
  }
  /////////////////////////////////////////////////////////////////////////////
}
