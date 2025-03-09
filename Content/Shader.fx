#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix MatrixTransform;
matrix ProjectionMatrix;

float AmbientColor;

float3 lightPosition; // Needs to be set externally
float3 lightColor;
float3 playerLightDirection;

Texture2D SpriteTexture;
Texture2D NormalTexture;

sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

sampler2D NormalTextureSampler = sampler_state
{
    Texture = <NormalTexture>;
};


struct VertexShaderInput
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
    float4 Color : COLOR0;
};


struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
    float3 WorldPosition : TEXCOORD1;
    float4 Color : COLOR0;
};


VertexShaderOutput MainVS(VertexShaderInput input)
{
    VertexShaderOutput output;
    float4 pos = mul(input.Position, MatrixTransform);
    
    output.Position = mul(pos, ProjectionMatrix);
    
    output.TexCoord = input.TexCoord;
    output.WorldPosition = pos;
    output.Color = input.Color;
    return output;
}


float4 MainPS(VertexShaderOutput input) : SV_Target
{
    
    float3 lightDirection = normalize(lightPosition - input.WorldPosition);

    float4 texColor = tex2D(SpriteTextureSampler, input.TexCoord);
    
    float3 normal = normalize((2 * tex2D(NormalTextureSampler, input.TexCoord)) - 1);

    float lightAmount = max(0, (300 / (length(input.WorldPosition - lightPosition) * 0.4 + 100))) * saturate(max(0, dot(normal, -lightDirection)));
    
    texColor.rgb *= AmbientColor + (lightAmount * lightColor);

    return texColor;
}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
