using UnityEngine;

public static class BossPrototypeVisuals
{
    private static Sprite placeholderSprite;
    private static Material defaultLineMaterial;

    public static Sprite PlaceholderSprite
    {
        get
        {
            if (placeholderSprite == null)
            {
                Texture2D texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();

                placeholderSprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, 1, 1),
                    new Vector2(0.5f, 0.5f),
                    1f
                );
            }

            return placeholderSprite;
        }
    }

    public static Material DefaultLineMaterial
    {
        get
        {
            if (defaultLineMaterial == null)
            {
                Shader shader = Shader.Find("Sprites/Default");

                if (shader == null)
                {
                    shader = Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default");
                }

                defaultLineMaterial = new Material(shader);
            }

            return defaultLineMaterial;
        }
    }
}