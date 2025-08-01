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
    float dist = distance(center, coords - uSides[int((distance(coords, center) - 0.14f) / 0.9f)]) - 0.14f;
    
    if (dist < 0)
        return float4(0, 0, 6, 10);
    
    float power = 0.37f - max(0, dist);
    
    float4 spike = float4(238, 232, 170, cos(uTime) * power * 255);
    float4 fall = float4(139, 41, 90, sin(uTime) * power * 155);
    
    return color * spike * fall;
}

technique Technique1
{
    pass Aura
    {
        PixelShader = compile ps_2_0 Aura();
    }
}