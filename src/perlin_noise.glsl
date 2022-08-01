// Given a 3d position as a seed, compute a smooth procedural noise
// value: "Perlin Noise", also known as "Gradient noise".
//
// Inputs:
//   st  3D seed
// Returns a smooth value between (-1,1)
//
// expects: random_direction, smooth_step
float perlin_noise( vec3 st) 
{
  /////////////////////////////////////////////////////////////////////////////
  // Replace with your code 


  //https://en.wikipedia.org/wiki/Perlin_noise
  float product[8];
  vec3 gradient, diff;
  // Determine grid cell coordinates
  int x0 = int(floor(st.x));
  int y0 = int(floor(st.y));
  int z0 = int(floor(st.z));
  int x1 = x0 + 1;
  int y1 = y0 + 1;
  int z1 = z0 + 1;
              // corners
  vec3 corners[8];
  corners[0] = vec3(x0, y0, z0);
  corners[1] = vec3(x1, y0, z0);
  corners[2] = vec3(x0, y1, z0);
  corners[3] = vec3(x1, y1, z0);
  corners[4] = vec3(x0, y0, z1);
  corners[5] = vec3(x1, y0, z1);
  corners[6] = vec3(x0, y1, z1);
  corners[7] = vec3(x1, y1, z1);

  for(int i = 0; i < 8; i++){
    gradient = random_direction(corners[i]);
    diff = st - corners[i];
    product[i] = dot(gradient, diff);
  }

  // interpolation weights:
  vec3 smooth_st = smooth_step(st - corners[0]);

  float ix1 = smooth_st.x * (product[1] - product[0]) + product[0];
  float ix2 = smooth_st.x * (product[3] - product[2]) + product[2];
  float ix3 = smooth_st.x * (product[5] - product[4]) + product[4];
  float ix4 = smooth_st.x * (product[7] - product[6]) + product[6];
  float iy1 = smooth_st.y * (ix2 - ix1) + ix1;
  float iy2 = smooth_st.y * (ix4 - ix3) + ix3;
  float iz = smooth_st.z * (iy2 - iy1) + iy1;

  return iz/sqrt(3);
  /////////////////////////////////////////////////////////////////////////////
}

