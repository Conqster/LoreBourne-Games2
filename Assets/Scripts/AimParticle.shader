Shader "Custom/AimParticle"
{
    Properties
    {
        _Color("Colour Choice", Color) = (1,1,1,1)
        _Scale("Scale Factor", Range(0,10)) = 1
        _Pow("Power Scale", Range(0.5, 8.0)) = 3
        //
        _WaveSpeed("Wave Speed", Range(0,20)) = 0.5
        _WaveAmp("Amplitiude", Range(0,100)) = 1
        _Sat("Saturation", Range(0,10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite On
            ColorMask 0
        }

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        struct Input
        {
            float3 viewDir;
        };


        fixed4 _Color;
        half _Scale;
        half _Pow;
        //
        half _WaveSpeed;
        half _WaveAmp;
        half _Bandwidth;
        float _Sat;

        void surf(Input IN, inout SurfaceOutput o)
        {
            float t = _Time.x;
            float osci = (0.5 * (sin(t * _WaveSpeed) + 1)) * _WaveAmp;


            half dotp = dot(normalize(IN.viewDir), o.Normal);
            half rim = _Sat - saturate(dotp);
            o.Emission = pow(rim, _Pow) * _Color.rgb * _Scale * osci;
            o.Alpha = pow(rim, _Pow);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
