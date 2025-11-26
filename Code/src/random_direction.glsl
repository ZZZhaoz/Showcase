// Generate a pseudorandom unit 3D vector
// 
// Inputs:
//   seed  3D seed
// Returns psuedorandom, unit 3D vector drawn from uniform distribution over
// the unit sphere (assuming random2 is uniform over [0,1]²).
//
// expects: random2.glsl, PI.glsl
#define PI 3.14159265358979323846

vec3 random_direction( vec3 seed)
{
  /////////////////////////////////////////////////////////////////////////////
  vec2 r = random2(seed);
  
  float z = 1.0 - 2.0 * r.x;            
  float theta = 2.0 * PI * r.y;         
  float r_xy = sqrt(max(0.0, 1.0 - z*z)); 
  
  return vec3(r_xy * cos(theta), r_xy * sin(theta), z);
  /////////////////////////////////////////////////////////////////////////////
}
