float bump_height( bool is_moon, vec3 s )
{
    /////////////////////////////////////////////////////////////////////////////
    // -------- High frequency roughness (shared by Moon and Earth land) -------
    float hf  = improved_perlin_noise(s * 25.0) * 0.01;
    float hf2 = improved_perlin_noise(s * 60.0) * 0.005;
    float hf3 = improved_perlin_noise(s * 120.0) * 0.002;
    float roughness = hf + hf2 + hf3;


    // ---------------------------- Moon branch ---------------------------------
    if (is_moon)
    {
        float n1 = abs(improved_perlin_noise(s * 8.0));
        float n2 = abs(improved_perlin_noise(s * 20.0));
        float rocky = 0.03 * n1 + 0.015 * n2;

        float c1 = length(s - vec3(0.3, 0.1, 0.2));
        float crater1 = smoothstep(0.25, 0.15, c1);

        float c2 = length(s - vec3(-0.2, -0.3, 0.1));
        float crater2 = smoothstep(0.22, 0.13, c2);

        float c3 = length(s - vec3(0.1, -0.2, -0.4));
        float crater3 = smoothstep(0.30, 0.18, c3);

        float craters = -0.10 * (crater1 + crater2 + crater3);

        float tiny = -0.015 * smoothstep(0.85, 1.0,
                          abs(improved_perlin_noise(s * 35.0)));

        // Moon: full rough
        return rocky + craters + tiny + roughness;
    }


    // ---------------------------- Earth branch --------------------------------
    float continent = improved_perlin_noise(s * 1.5);
    continent += improved_perlin_noise(s * 3.0 + 10.0) * 0.5;
    continent += improved_perlin_noise(s * 6.0 + 20.0) * 0.25;
    continent += improved_perlin_noise(s * 12.0 + 40.0) * 0.15;

    continent = continent * 0.5 + 0.5;

    float sea_level = 0.61;
    float distance_from_coast = continent - sea_level;

    float shelf_start = -0.15;
    float shelf_end = 0.05;

    float base_height;

    if (distance_from_coast < shelf_start)
    {
        // Deep ocean: smooth!
        base_height = -0.035;
    }
    else if (distance_from_coast > shelf_end)
    {
        // Land
        base_height = 0.18;
    }
    else
    {
        float t = (distance_from_coast - shelf_start) /
                  (shelf_end - shelf_start);
        t = smoothstep(0.0, 1.0, t);
        base_height = mix(-0.035, 0.18, t);

        float slope_noise = improved_perlin_noise(s * 8.0 + 100.0) * 0.015;
        slope_noise += improved_perlin_noise(s * 15.0 + 120.0) * 0.008;

        base_height += slope_noise * (1.0 - abs(t - 0.5) * 2.0);
    }


    float ocean_detail = 0.0;
    if (distance_from_coast < shelf_start)
    {
        // Deep ocean ripples – very subtle
        ocean_detail += improved_perlin_noise(s * 15.0) * 0.003;
        ocean_detail += improved_perlin_noise(s * 28.0 + 5.0) * 0.002;
    }

    float land_detail = 0.0;
    if (distance_from_coast > shelf_end)
    {
        land_detail += improved_perlin_noise(s * 5.0 + 50.0) * 0.025;
        land_detail += improved_perlin_noise(s * 10.0 + 70.0) * 0.018;
        land_detail += improved_perlin_noise(s * 18.0 + 90.0) * 0.012;
        land_detail += improved_perlin_noise(s * 32.0 + 110.0) * 0.008;
    }


    // -------- Add roughness ONLY to land (Moon already handled above) ---------
    bool is_land = (distance_from_coast > 0.0);  

if (is_land)
{
    return base_height + ocean_detail + land_detail + roughness;
}
else
{
    return base_height + ocean_detail;
}

}
