texture2D trailTexture; //trail shape, never directly set here, use GraphicsDevice.Textures[0]

sampler2D uShape = sampler_state
{
    Texture = <trailTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4x4 uTransform; //transformed matrix used for perspective
float4 uColor = float4(1, 1, 1, 1); //vec4 for the color near the base
float4 uEndColor = float4(1, 1, 1, 1); //vec4 for the color near the tip
float uLerpPower = 0; //controls how quickly it pulls towards the second color

float2 lerpPull = float2(1, 1);

struct VSInput
{
    float2 Pos : POSITION0;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

struct PSInput
{
    float4 Pos : SV_POSITION;
    float4 Color : COLOR0;
    float3 Texcoord : TEXCOORD0;
};

PSInput VertexShaderFunction(VSInput input)
{
    PSInput output;
    
    output.Texcoord = input.Texcoord;
    output.Color = input.Color;
    output.Pos = mul(float4(input.Pos, 0, 1), uTransform);
    
    return output;
}

float4 PixelShaderFunction(PSInput input) : COLOR0
{
    float3 coords = input.Texcoord; // the TexCoord values (x is completion, y is 0 or 1, z is alpha)
    float4 image = tex2D(uShape, float2(coords.x, coords.y)) * input.Color; //the pixel
    
    float4 imageAlpha = image * coords.z; //remember passing z in the drawcode? here it is used to preserve opacity.
    float t = saturate(coords.y * 0.85 + coords.x * 0.45); //lerp value used for creating the gradient.
    float lerpValue = saturate(t + uLerpPower); //added an optional value for colors to shift more towards one than the other, ensure this stays between the two colors though...
    float4 offset = lerp(uColor, uEndColor, 1 - lerpValue); //the color to apply to the sampled image
    
    return imageAlpha * offset; //return the new color.
}

float4 PixelShaderFunction2(PSInput input) : COLOR0
{
    float3 coords = input.Texcoord; 
    float4 image = tex2D(uShape, float2(coords.x, coords.y)) * input.Color; 
    
    float4 imageAlpha = image * coords.z; 
    float t = saturate((coords.y * lerpPull.y * 0.5) + (coords.x * lerpPull.x * 0.5));
    float lerpValue = saturate(t + uLerpPower); 
    float4 offset = lerp(uColor, uEndColor, 1 - lerpValue);
    
    return imageAlpha * offset;
}

technique Technique1
{
    pass TrailColor
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }

    pass TrailColor2
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction2();
    }
}
