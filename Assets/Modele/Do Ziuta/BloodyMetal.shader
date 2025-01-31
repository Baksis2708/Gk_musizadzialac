Shader "Custom/BloodyMetal"
{
    Properties
    {
        _MainTex ("Metal Texture", 2D) = "white" {}
        _BloodTex ("Blood Texture", 2D) = "white" {}
        _BloodIntensity ("Blood Intensity", Range(0, 1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _MainTex;
        sampler2D _BloodTex;
        float _BloodIntensity;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BloodTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Pobierz kolory tekstur
            fixed4 metal = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 blood = tex2D(_BloodTex, IN.uv_BloodTex);
            
            // Zmieszaj tekstury
            o.Albedo = lerp(metal.rgb, blood.rgb, _BloodIntensity * blood.a);
          //  o.Metallic = lerp(0.8, 0.0, _BloodIntensity); // Metalowoœæ maleje z krwi¹
        }
        ENDCG
    }
    FallBack "Diffuse"
}