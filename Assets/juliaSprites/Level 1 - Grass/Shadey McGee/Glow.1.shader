Shader "Unlit/Glow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Colour", Color) = (1,1,1,1)
    }
    SubShader
    {
        Cull Off //Not going to ignore any pixels
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert //vertex function
            #pragma fragment frag //fragment function

            #include "UnityCG.cginc"

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 _Color;
            float4 _MainTex_TexelSize;

            fixed4 frag(v2f i) : COLOR
            {
                half4 c = tex2D(_MainTex, i.uv);
                c.rgb *= c.a; //keep transparent pixels in sprite  transparent
                half4 outlineC = _Color;
                outlineC.a *= ceil(c.a); //round up for whole calue rgb value
                outlineC.rgb *= outlineC.a;

                //alpha of current pixel we are on = tex2D(_MainTex, i.uv).a 
                fixed upAlpha = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a; //alpha value of the pixel above the current one
                fixed downAlpha = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y)).a;
                fixed rightAlpha = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a; 
                fixed leftAlpha = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x, 0)).a; 


                return lerp(outlineC, c, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha));
            }
            
            ENDCG
        }
    }
}
