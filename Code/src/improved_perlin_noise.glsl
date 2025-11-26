// Given a 3d position as a seed, compute an even smoother procedural noise
// value. "Improving Noise" [Perlin 2002].
//
// Inputs:
//   st  3D seed
// Values between  -½ and ½ ?
//
// expects: random_direction, improved_smooth_step
float improved_perlin_noise( vec3 st) 
{
  /////////////////////////////////////////////////////////////////////////////
  vec3 i0 = floor(st);
  vec3 i1 = i0 + vec3(1.0);
  
  vec3 f = fract(st);
  
  vec3 u = improved_smooth_step(f);

  vec3 g000 = random_direction(i0);
  vec3 g100 = random_direction(vec3(i1.x, i0.y, i0.z));
  vec3 g010 = random_direction(vec3(i0.x, i1.y, i0.z));
  vec3 g110 = random_direction(vec3(i1.x, i1.y, i0.z));
  vec3 g001 = random_direction(vec3(i0.x, i0.y, i1.z));
  vec3 g101 = random_direction(vec3(i1.x, i0.y, i1.z));
  vec3 g011 = random_direction(vec3(i0.x, i1.y, i1.z));
  vec3 g111 = random_direction(i1);

  vec3 d000 = f - vec3(0.0, 0.0, 0.0);
  vec3 d100 = f - vec3(1.0, 0.0, 0.0);
  vec3 d010 = f - vec3(0.0, 1.0, 0.0);
  vec3 d110 = f - vec3(1.0, 1.0, 0.0);
  vec3 d001 = f - vec3(0.0, 0.0, 1.0);
  vec3 d101 = f - vec3(1.0, 0.0, 1.0);
  vec3 d011 = f - vec3(0.0, 1.0, 1.0);
  vec3 d111 = f - vec3(1.0, 1.0, 1.0);

  float n000 = dot(g000, d000);
  float n100 = dot(g100, d100);
  float n010 = dot(g010, d010);
  float n110 = dot(g110, d110);
  float n001 = dot(g001, d001);
  float n101 = dot(g101, d101);
  float n011 = dot(g011, d011);
  float n111 = dot(g111, d111);

  float nx00 = mix(n000, n100, u.x);
  float nx10 = mix(n010, n110, u.x);
  float nx01 = mix(n001, n101, u.x);
  float nx11 = mix(n011, n111, u.x);

  float nxy0 = mix(nx00, nx10, u.y);
  float nxy1 = mix(nx01, nx11, u.y);

  float nxyz = mix(nxy0, nxy1, u.z);

  return nxyz; // or (nxyz * 0.5 + 0.5) to map into [0,1]
  /////////////////////////////////////////////////////////////////////////////
}

