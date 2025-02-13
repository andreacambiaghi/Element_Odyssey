Shader "Custom/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1, 1, 0, 1) // Giallo di default
        _OutlineThickness ("Outline Thickness", Range(0, 0.1)) = 0.02
    }
    SubShader
    {
        Tags {"Queue"="Overlay" "RenderType"="Transparent"}
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 texColor = tex2D(_MainTex, i.uv);

                // Controllo trasparenza per il bordo
                float alpha = texColor.a;
                float outlineAlpha = 0.0;

                // Controlla i pixel vicini per creare il contorno
                float2 offsets[8] = {
                    float2(-_OutlineThickness, 0),
                    float2(_OutlineThickness, 0),
                    float2(0, -_OutlineThickness),
                    float2(0, _OutlineThickness),
                    float2(-_OutlineThickness, -_OutlineThickness),
                    float2(-_OutlineThickness, _OutlineThickness),
                    float2(_OutlineThickness, -_OutlineThickness),
                    float2(_OutlineThickness, _OutlineThickness)
                };

                for (int j = 0; j < 8; j++)
                {
                    float4 sampleColor = tex2D(_MainTex, i.uv + offsets[j]);
                    outlineAlpha = max(outlineAlpha, sampleColor.a);
                }

                // Se il pixel corrente è trasparente ma i vicini hanno colore, mostra l'outline
                if (alpha < 0.1 && outlineAlpha > 0.1)
                {
                    return _OutlineColor;
                }

                return texColor;
            }
            ENDCG
        }
    }
}
