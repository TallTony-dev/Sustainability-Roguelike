#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix CameraMatrix;
float3 AmbientColor = 0.35;

float3 lightWorldPosition = (0, 0, 0); //needs to be set externally

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


struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 pixelWorldPos = mul(input.Position, CameraMatrix);
	
    float3 lightDirection = normalize((float3)pixelWorldPos - lightWorldPosition);
	
    float4 texColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	
    float3 normal = normalize((2 * tex2D(NormalTextureSampler, input.TextureCoordinates)) - 1);
	
    texColor.r += AmbientColor;

    return texColor;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};