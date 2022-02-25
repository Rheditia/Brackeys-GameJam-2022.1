using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float xParallaxFactor;

    private float xStartPosition;
    private float xSpriteLenght;

    private void Start()
    {
        xStartPosition = transform.position.x;
        xSpriteLenght = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        // Distance between the camera itself and the sprite that already moved multiplied by the Parallax factor
        float spriteHorizontalPositionRelativeToCamera = mainCamera.transform.position.x * (1 - xParallaxFactor);
        // Distance moved based on parallax factor
        float horizontalDistanceMoved = mainCamera.transform.position.x * xParallaxFactor;
        transform.position = new Vector3(xStartPosition + horizontalDistanceMoved, transform.position.y, transform.position.z);

        if (spriteHorizontalPositionRelativeToCamera > xStartPosition + xSpriteLenght)
        {
            xStartPosition += xSpriteLenght;
        }
        else if (spriteHorizontalPositionRelativeToCamera < xStartPosition - xSpriteLenght)
        {
            xStartPosition -= xSpriteLenght;
        }
    }
}
