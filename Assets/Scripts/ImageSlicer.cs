using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSlicer
{
    public static Texture2D[,] GetSlices(Texture2D image, int blocksPerLine)
    {
        image = rotateTexture(image, true);
        image = rotateTexture(image, true);

        int imageSize = Mathf.Min(image.width, image.height);
        int blockSize = imageSize / blocksPerLine;

        Texture2D[,] blocks = new Texture2D[blocksPerLine, blocksPerLine];

        for (int y = 0; y < blocksPerLine; y++)
        {
            for (int x = 0; x < blocksPerLine; x++)
            {
                Texture2D block = new Texture2D(blockSize, blockSize);

                block.wrapMode = TextureWrapMode.Clamp;
                block.SetPixels(image.GetPixels(x * blockSize, y * blockSize, blockSize, blockSize));
                block.Apply();
                blocks[x, y] = block;
            }
        }

        return blocks;
    }

     public static Texture2D rotateTexture(Texture2D originalTexture, bool clockwise)
     {
         Color32[] original = originalTexture.GetPixels32();
         Color32[] rotated = new Color32[original.Length];
         int w = originalTexture.width;
         int h = originalTexture.height;
 
         int iRotated, iOriginal;
 
         for (int j = 0; j < h; ++j)
         {
             for (int i = 0; i < w; ++i)
             {
                 iRotated = (i + 1) * h - j - 1;
                 iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                 rotated[iRotated] = original[iOriginal];
             }
         }
 
         Texture2D rotatedTexture = new Texture2D(h, w);
         rotatedTexture.SetPixels32(rotated);
         rotatedTexture.Apply();
         return rotatedTexture;
     }
}
