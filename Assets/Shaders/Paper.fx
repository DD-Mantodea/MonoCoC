#if OPENGL
    #define PS_SHADERMODEL ps_3_0
#else
    #define PS_SHADERMODEL ps_5_0
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float Hash(float2 p)
{
    p = frac(p * float2(123.34, 456.21));
    p += dot(p, p + 45.32);
    return frac(p.x * p.y);
}

float Noise(float2 p)
{
    float2 i = floor(p);
    float2 f = frac(p);

    // 平滑插值
    f = f * f * (3.0 - 2.0 * f);

    float a = Hash(i);
    float b = Hash(i + float2(1.0, 0.0));
    float c = Hash(i + float2(0.0, 1.0));
    float d = Hash(i + float2(1.0, 1.0));

    return lerp(
        lerp(a, b, f.x),
        lerp(c, d, f.x),
        f.y
    );
}

float FiberNoise(float2 uv)
{
    // 横向纤维：X 方向变化快，Y 方向变化慢
    float2 p = uv * float2(900.0, 65.0);

    float2 i = floor(p);
    float2 f = frac(p);

    f = f * f * (3.0 - 2.0 * f);

    float a = Hash(i);
    float b = Hash(i + float2(1.0, 0.0));
    float c = Hash(i + float2(0.0, 1.0));
    float d = Hash(i + float2(1.0, 1.0));

    float n = lerp(
        lerp(a, b, f.x),
        lerp(c, d, f.x),
        f.y
    );

    return n;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 uv = input.TextureCoordinates;

    // 以中心为原点
    float2 p = uv - float2(0.5, 0.5);

    // 修正宽高比，避免渐变被拉伸
    float aspect = 16.0 / 9.0;
    p.x *= aspect;

    // 计算距离，并归一化到 0~1
    float radius = length(p);
    float maxRadius = length(float2(aspect, 1.0)) * 0.5;
    // 低频边缘扰动
    float edgeNoise = Noise(uv * 5.0) - 0.5;

    // 扰动幅度
    float edgeDistortion = edgeNoise * 0.10;

    // 应用扰动
    float edge = saturate(radius / maxRadius + edgeDistortion);

    // 柔和过渡
    edge = smoothstep(0.2, 1.0, edge);

    // 中央颜色与边缘颜色
    float4 centerColor = float4(0.949, 0.902, 0.804, 1.0);
    float4 edgeColor   = float4(0.863, 0.780, 0.627, 1.0);

    float4 color = lerp(centerColor, edgeColor, edge * 0.35);

    // 低频噪声
    float noise = Noise(uv * 18.0) - 0.5;
    color.rgb += noise * 0.025;

    // 纤维噪声
    // 纤维纹理
    float fiber1 = FiberNoise(uv) - 0.5;

    // 第二层纤维，改变采样位置和方向
    float fiber2 = FiberNoise(uv * float2(0.73, 1.31) + float2(0.37, 0.19)) - 0.5;

    // 混合
    float fiber = fiber1 * 0.7 + fiber2 * 0.3;

    // 增强纤维对比度
    fiber = sign(fiber) * pow(abs(fiber) * 2.0, 1.4);

    // 加入纸张
    color.rgb += fiber * 0.025;

    return color;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
