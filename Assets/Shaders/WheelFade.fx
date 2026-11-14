#if OPENGL
    #define PS_SHADERMODEL ps_3_0
#else
    #define PS_SHADERMODEL ps_5_0
#endif

float4 FadeColor = float4(0.91, 0.85, 0.74, 1.0);
float FadeTop = 0.18;
float FadeBottom = 0.18;

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float y = input.TextureCoordinates.y;

    float top = saturate(1.0 - y / max(FadeTop, 0.001));
    float bottom = saturate(1.0 - (1.0 - y) / max(FadeBottom, 0.001));

    float alpha = max(top, bottom);

    return float4(FadeColor.rgb, alpha * FadeColor.a);
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
