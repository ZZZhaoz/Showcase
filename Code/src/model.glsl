// Construct the model transformation matrix. The moon should orbit around the
// origin. The other object should stay still.
//
// Inputs:
//   is_moon  whether we're considering the moon
//   time  seconds on animation clock
// Returns affine model transformation as 4x4 matrix
//
// expects: identity, rotate_about_y, translate, PI
uniform bool is_background;

mat4 rotate_about_x(float a)
{
    float c = cos(a);
    float s = sin(a);

    return mat4(
        1.0, 0.0, 0.0, 0.0,
        0.0,   c,  -s, 0.0,
        0.0,   s,   c, 0.0,
        0.0, 0.0, 0.0, 1.0
    );
}

const float PI = 3.14159265359;

mat4 model(bool is_moon, bool is_sun, float time)
{   
    if (is_background)
        {
        
        return uniform_scale(50.0);
    }

    if (is_sun)
    {
        return translate(vec3(0.0, 0.0, 12.0)) * uniform_scale(4.0);
    }

    float earth_orbit_r = 12.0;
    float earth_orbit_speed  = 2.0 * PI / 12.0;
    float earth_spin_speed   = 2.0 * PI / 5.0;

    mat4 sun_pos = translate(vec3(0.0, 0.0, 12.0));

    mat4 earth_orbit =
        sun_pos *
        rotate_about_y(time * earth_orbit_speed) *
        translate(vec3(earth_orbit_r, 0, 0));

    mat4 earth_spin =
        rotate_about_y(time * earth_spin_speed);

    if (!is_moon)
    {
        return earth_orbit * earth_spin;
    }

    float moon_orbit_r = 1.5;
    float moon_orbit_speed = 2.0 * PI / 4.0;

    mat4 moon_orbit =
        rotate_about_y(time * moon_orbit_speed) *
        rotate_about_x(0.45) *
        translate(vec3(moon_orbit_r, 0, 0));

    mat4 moon_scale = uniform_scale(0.27);

    return earth_orbit * earth_spin * moon_orbit * moon_scale;
}

