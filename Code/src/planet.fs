// Generate a procedural planet and orbiting moon
uniform mat4 view;
uniform mat4 proj;
uniform float animation_seconds;
uniform bool is_moon;
uniform bool is_sun;
uniform bool is_background;  
uniform vec3 sun_world_pos;

in vec3 sphere_fs_in;
in vec3 normal_fs_in;
in vec4 pos_fs_in;
in vec4 view_pos_fs_in;

out vec3 color;

// expects: model, blinn_phong, bump_height, bump_position,
// improved_perlin_noise, tangent

void main()
{
    vec3 sphere_norm = normalize(sphere_fs_in);
    

if (is_background)
{
    vec3 dir = normalize(sphere_fs_in);


    vec3 background = vec3(0.01, 0.01, 0.03);
    float stars = 0.0;

    float s1 = improved_perlin_noise(dir * 40.0);
    s1 = pow(max(s1, 0.0), 8.0);
    stars += s1 * 3.0;

    float s2 = improved_perlin_noise(dir * 120.0);
    s2 = pow(max(s2, 0.0), 5.0);
    stars += s2 * 2.0;

    float s3 = improved_perlin_noise(dir * 300.0);
    s3 = pow(max(s3, 0.0), 4.0);
    stars += s3 * 1.2;

    vec3 star_color = vec3(1.0);

    color = background + star_color * stars * 1.2;
    return;
}

    vec3 sun_view = (view * vec4(sun_world_pos, 1.0)).xyz;
    vec3 l = normalize(sun_view - view_pos_fs_in.xyz);
    vec3 v = normalize(-view_pos_fs_in.xyz);
    vec3 h = normalize(v + l);

    vec3 bump_pos = bump_position(is_moon, sphere_norm);
    vec3 bump_n = normalize(cross(dFdx(bump_pos), dFdy(bump_pos)));
    vec3 n = normalize(normal_fs_in);
    bump_n = normalize(mix(n, bump_n, is_moon ? 0.3 : 0.5));

    vec3 ka = vec3(0.02);
    vec3 ks = vec3(0.8);
    float p = is_moon ? 600.0 : 300.0;
    float height = bump_height(is_moon, sphere_norm);
    vec3 kd;

    if(is_sun)
    {
        vec3 base_color = vec3(1.8, 0.25, 0.05);

        float n1 = improved_perlin_noise(sphere_fs_in * 3.0 + animation_seconds * 0.5);
        float n2 = improved_perlin_noise(sphere_fs_in * 8.0 + animation_seconds * 0.8);
        float n3 = improved_perlin_noise(sphere_fs_in * 15.0 - animation_seconds * 1.2);
        float n4 = improved_perlin_noise(sphere_fs_in * 25.0 + animation_seconds * 1.5);

        float cn = n1*0.5 + n2*0.25 + n3*0.15 + n4*0.1;
        cn = cn * 0.5 + 0.5;
        cn = pow(cn, 0.6);

        vec3 col = base_color * (0.8 + cn * 2.5);

        float spots = improved_perlin_noise(sphere_fs_in * 4.0 + animation_seconds * 0.2);
        col *= (0.3 + 0.7 * smoothstep(0.25, 0.55, spots));

        float detail = improved_perlin_noise(sphere_fs_in * 20.0 - animation_seconds * 2.0);
        col *= (0.9 + detail * 0.3);

        float fres = 1.0 - max(dot(normalize(normal_fs_in), v), 0.0);

        col += vec3(2.5,0.8,0.2) * pow(fres,4.0) * 1.5;
        col += vec3(2.0,1.0,0.4) * pow(fres,2.0);
        col += vec3(1.2,0.8,0.4) * pow(fres,1.0) * 0.5;
        col += vec3(0.5,0.3,0.15) * pow(fres,0.6) * 0.3;

        color = col;
        return;
    }

    if(is_moon)
    {
        float t = clamp(height * 5.0 + 0.5, 0.0, 1.0);
        kd = mix(vec3(0.3), vec3(0.85,0.82,0.80), t);
    }
    else
    {

        if(height < -0.015)
            kd = vec3(0.0,0.1,0.4);
        else if(height < 0.0)
            kd = vec3(0.0,0.3,0.7);
        else if(height < 0.03)
            kd = mix(vec3(0.0,0.4,0.8), vec3(0.8,0.75,0.5), height/0.03);
        else if(height < 0.10)
            kd = vec3(0.2,0.6,0.2);
        else if(height < 0.14)
            kd = mix(vec3(0.2,0.6,0.2), vec3(0.45,0.4,0.25), (height-0.10)/0.04);
        else if(height < 0.18)
            kd = mix(vec3(0.45,0.4,0.25), vec3(0.65,0.65,0.6), (height-0.14)/0.04);
        else if(height < 0.22)
            kd = mix(vec3(0.65,0.65,0.6), vec3(0.85,0.88,0.92), (height-0.18)/0.04);
        else
            kd = vec3(0.92,0.94,0.97);

        vec3 cloud_coord = sphere_norm * 2.0 + vec3(animation_seconds * 0.03,0,0);

        float region = improved_perlin_noise(cloud_coord * 0.4)*0.5+0.5;
        float n1 = improved_perlin_noise(cloud_coord * 1.5)*0.5+0.5;
        float n2 = improved_perlin_noise(cloud_coord * 3.0)*0.5+0.5;
        float n3 = improved_perlin_noise(cloud_coord * 9.0)*0.5+0.5;
        float n4 = improved_perlin_noise(cloud_coord * 18.0)*0.5+0.5;

        vec3 shadow_coord = sphere_norm - normalize(l) * 0.06;

        float sh1 = improved_perlin_noise(shadow_coord * 1.5)*0.5+0.5;
        float sh2 = improved_perlin_noise(shadow_coord * 3.0)*0.5+0.5;
        float sh3 = improved_perlin_noise(shadow_coord * 9.0)*0.5+0.5;

        float shadow_cloud = smoothstep(0.2,0.6, sh1*0.55 + sh2*0.35 + sh3*0.1);
        kd *= (1.0 - shadow_cloud*0.45);

        float clouds = region*(0.55*n1 + 0.3*n2 + 0.1*n3 + 0.05*n4);
        clouds += 0.05*n3*n4;
        clouds = pow(clouds, 1.1);

        float cloud_mask = smoothstep(0.15,0.55, clouds);
        kd = mix(kd, vec3(0.96,0.98,1.0), cloud_mask*1.2)
           + vec3(0.96,0.98,1.0)*0.12*cloud_mask;
    }


    if(!is_moon)
    {
        float lat = abs(sphere_norm.y);
        float ice = smoothstep(0.75, 0.9, lat);
        kd = mix(kd, vec3(0.95,0.97,1.0), ice);
    }

    float diff = max(dot(bump_n, l), 0.0);
    float spec = pow(max(dot(bump_n, h), 0.0), p);

    vec3 planet_color = ka + kd * diff + ks * spec;

    if (!is_moon)
    {
        float night = 1.0 - smoothstep(0.05, 0.25, diff);
        float flatland = smoothstep(0.02, 0.08, max(0.0, 0.08 - height));

        vec3 city_coord = sphere_norm * 60.0;
        float cA = improved_perlin_noise(city_coord);
        float cB = improved_perlin_noise(city_coord*2.0 + 13.7);
        float cC = improved_perlin_noise(city_coord*4.0 + 27.2);

        float city_pattern = cA*0.6 + cB*0.3 + cC*0.1;
        city_pattern = smoothstep(0.40, 0.75, city_pattern);

        float flicker = improved_perlin_noise(city_coord*10.0 + animation_seconds*2.0);
        flicker = flicker * 0.15 + 0.85;

        vec3 night_light = vec3(1.3,1.1,0.8) * city_pattern * night * flatland * flicker;

        planet_color += night_light;
    }

    color = planet_color;
}