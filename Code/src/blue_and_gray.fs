// Set the pixel color to blue or gray depending on is_moon.
//
// Uniforms:
uniform bool is_moon;
// Outputs:
out vec3 color;
void main()
{
  /////////////////////////////////////////////////////////////////////////////
  if (is_moon)
    color = vec3(0.82, 0.75, 0.78);  
  else
    color = vec3(0.28,0.40,0.89);   

  /////////////////////////////////////////////////////////////////////////////
}
