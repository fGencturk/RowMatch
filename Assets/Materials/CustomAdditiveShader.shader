Shader "Additive"
{
    Properties
    {
        _MainTex("Texture to blend", 2D) = "white" {}
        [PerRendererData]
        _AdditiveColor("Main Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Pass
        {
            SetTexture[_MainTex]
            {
                constantColor[_AdditiveColor]
                combine texture + constant
            }
        }
    }
}