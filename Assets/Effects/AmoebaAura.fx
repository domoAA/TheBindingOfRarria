sampler uImage0 : register(s0); // Contents of the screen.
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
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
float4x2 uSides;

float4 Aura(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    
    float2 center = (0.5f, 0.5f);
    float dist = (distance(center, coords)) * 3.0f;
    
    if (dist < 0.15f)
        return float4(0, 0, 0.6f, 100);

    
    float power = abs(0.3f - max(0, dist));
    
    float4 spike = float4(0.93f, 0.9f, 0.66f, (cos(dist * (sqrt(power) * 15.0f) - uTime * 3.0f) + 0.8f) / (1 + pow(dist * 2, 1.12f)));
    
    if (dist < 0.3f)
        spike.a = max((power * 5.0f), spike.a);
    
    spike.a /= (pow(dist, 3) * 4);
    
    if (spike.a < 0.4f)
        spike.r += (0.6f - spike.a);
    
    if (spike.a < 0.05f)
        spike.a = 0;
    
    spike.a = min(spike.a, 0.8f);
    
    return spike;
}

technique Technique1
{
    pass Aura
    {
        PixelShader = compile ps_2_0 Aura();
    }
}