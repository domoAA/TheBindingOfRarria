sampler uImage0 : register(s0); // Contents of the screen.
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float4 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime; // Should be set to Main.GlobalTime or a fraction of incrementing time for shine effect.
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;
float2 uSize;

float hash1(float n)
{
    return frac(sin(n) * 43758.5453);
}

float2 hash2(float2 p)
{
    p = float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)));
    return frac(sin(p) * 43758.5453);
}

float4 voronoi(float2 x, float w, float di, float iTime)
{
    float2 n = floor(x);
    float2 f = frac(x);
    float4 m = float4(8.0, 0.0, 0.0, 0.0);

    [loop]
    for (int j = -2; j <= 2; j++)
    {
        [loop]
        for (int i = -2; i <= 2; i++)
        {
            float2 g = float2((float) i, (float) j);
            float2 o = hash2(n + g);
            o = 0.5 * sin(iTime + 6.2831 * o);

            float d = 1.2 * length(g - f + o);
            float3 col = 0.2 * float3(1, 2, 7) - 0.2 * float3(1, 0, 0);

            float h = smoothstep(-1.5, 0.3, (m.x - di * d) / w);
            m.x = lerp(m.x, d, h) - h * (1.0 - h) * w / (1.0 + 3.0 * w);
            m.yzw = lerp(m.yzw, col, h) + h * (1.0 - h) * w / (1.0 + 3.0 * w);
        }
    }

    return m;
}

const float4x4 bayer = float4x4(
    0.0, 8.0, 2.0, 10.0,
   12.0, 4.0, 14.0, 6.0,
    3.0, 11.0, 1.0, 9.0,
   15.0, 7.0, 13.0, 5.0) / 16.0;

float4 Sphere(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = float4(0.0, 0.0, 0.0, 0.0);
    
    float2 uv = coords;
    //float pixelSize = 1.5;
    //uv = floor(uv * uSize / pixelSize) / (uSize / pixelSize);
    
    
    uv -= 0.5;
    uv *= 2.2;
    float power = length(uv) * 1.3f + 0.3;
    if (length(uv) > 1.0)
    {
        color = float4(0.0, 0.0, 0.0, 0.0);
        return color;
    }
    uv = lerp(uv, normalize(uv) * ((2.0 * asin(length(uv))) / 3.1415926), 0.5);
    float3 n = float3(uv, sqrt(1.0 - uv.x * uv.x - uv.y * uv.y));
    uv = normalize(uv) * (2.0 * asin(length(uv)) / 3.1415926);
    
    uv -= float2(uTime * 0.4, uTime * 0.15);
    
    float max = 6.0;
    //align matrix with window
    float2 bayerUV = floor(uv * pow(2.0, max));
    
    //float bayerPattern = GetBayerFromCoordLevel(bayerUV);
    float bayerPattern = bayer[(int) (30 * abs(uv.x)) % 4][(int) (30 * abs(uv.y)) % 4];
    
    float4 dither = float4(bayerPattern, bayerPattern, bayerPattern, 1.0);

    
    float2 p = uv;
    float scale = 4.0;

    float4 v1 = voronoi(0.7 * scale * p, 0.3, 1.5, uTime);
    float4 v2 = voronoi(2.0 * scale * p, 0.3, 0.8, uTime);
    float4 v3 = voronoi(4.0 * scale * p, 0.3, 0.4, uTime);
    float4 v = (2.0 * v1 + 0.5 * v2 + 0.5 * v3) / 3.0;

    float3 col = sqrt(v.yzw); // gamma correction

    if (col.g > 0.64 && col.g < 0.655)
        col = 1.5 * float3(0.239, 0.714, 0.984);
    else if (col.g >= 0.655)
        col = 1.7 * float3(0.239, 0.714, 0.984);
    
    
    color = uColor * tex2D(uImage0, uv + 0.5) * pow(power * 2, 2) / 4 * float4(col, 1.0);
    
    return color;
}

technique Technique1
{
    pass Sphere
    {
        PixelShader = compile ps_3_0 Sphere();
    }
}