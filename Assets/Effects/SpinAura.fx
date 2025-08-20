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
float uSize;

float4 SpinAura(float2 coords : TEXCOORD0) : COLOR0
{
    float2 uv = coords;
    
    float2 pos = uv - 0.5f;
    pos *= 1.5f;
    float r = length(pos);
    
    
    float a = atan(pos.x);
    float something = uSize;
    float c = pow(sin(a * 8.0 + 12.0 * uTime * sin(something * r) / abs(sin(something * r))) * 0.5 + 0.5, 2.0) * pow(sin(r * something), 2.0) * r;

    float4 color = float4(tex2D(uImage0, uv).rgb, 0.0);
    color += float4(2.0 * c, 2.0 * c, 2.0 * c, 1.0 * c);
	
    return color * 2.5 * (0.65 - pow(r, 2));
}

technique Technique1
{
    pass SpinAura
    {
        PixelShader = compile ps_3_0 SpinAura();
    }
}