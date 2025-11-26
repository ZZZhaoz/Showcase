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

  float theta = 0.25 * M_PI * animation_seconds;
  vec4 light = view * vec4(4 * cos(theta), 4, 4 * sin(theta), 1);

  vec3 ka = vec3(0.01);
  vec3 kd = is_moon ? vec3(0.82, 0.75, 0.78) : vec3(0.28, 0.40, 0.89);
  vec3 ks = vec3(0.8);
  float p = is_moon ? 1000.0 : 500.0;

  vec3 T, B;
  tangent(normalize(sphere_fs_in), T, B);
  
  vec3 sphere_norm = normalize(sphere_fs_in);
  float e = 1e-3;
  vec3 bump = bump_position(is_moon, sphere_norm);
  vec3 dp_dt = (bump_position(is_moon, sphere_norm + e * T) - bump) / e;
  vec3 dp_db = (bump_position(is_moon, sphere_norm + e * B) - bump) / e;
  vec3 n = normalize(cross(dp_dt, dp_db));
  
  if (dot(n, sphere_norm) < 0.0) {
    n *= -1.0;
  }

  mat4 model_mat = model(is_moon, animation_seconds);
  n = normalize((transpose(inverse(view)) * transpose(inverse(model_mat)) * vec4(n, 1.0)).xyz);

  vec3 v = normalize(-view_pos_fs_in.xyz);
  vec3 l = normalize(light.xyz - view_pos_fs_in.xyz);
  vec3 h = normalize(v + l);
  
  float diff = max(dot(n, l), 0.0);
  float spec = pow(max(dot(n, h), 0.0), p);
  
  color = ka * kd + diff * kd + ks * spec;
  /////////////////////////////////////////////////////////////////////////////
}
