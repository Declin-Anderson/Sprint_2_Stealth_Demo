using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int[,] LoadMap(Texture2D Map, ImageElement color)
    {
        int[,] MapLayout = new int[Map.width,Map.height];

        for (int i = 0; i < Map.width; ++i)
        {
            for (int j = 0; j < Map.height; ++j)
            {
                Color pixel = Map.GetPixel(i, j);

                switch (color)
                {
                    case ImageElement.RED:
                        MapLayout[i, j] = (int)(pixel.r*255);
                        break;
                    case ImageElement.GREEN:
                        MapLayout[i, j] = (int)(pixel.g*255);
                        break;
                    case ImageElement.BLUE:
                        MapLayout[i, j] = (int)(pixel.b*255);
                        break;
                    case ImageElement.ALPHA:
                        MapLayout[i, j] = (int)(pixel.a*255);
                        break;
                    default:
                        break;
                }
                //do something here based on the color.
                //can do pixel.r, pixel.g, pixel.b, pixel.a. These are all in scale 1 rgba meaning that the if you have color
                // (255,54,124) = (255/255,54/255,124/255) = (1,0.21,0.49)
                //So base the logic off of a range of 0.00 to 1.00
            }
        }

        return MapLayout;
    }
}
