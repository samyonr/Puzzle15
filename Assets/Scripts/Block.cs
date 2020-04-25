using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public event System.Action<Block> OnBlockPressed;
    public event System.Action OnFinishedMoving;
    public Vector3Int coord;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {  

    }

    public void Init(Vector3Int startingCoord, Texture2D image)
    {
        coord = startingCoord;

        GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
        GetComponent<MeshRenderer>().material.mainTexture = image;
    }

    public void MoveToPisition(Vector3 target, float duration)
    {
        StartCoroutine(AnimateMove(target, duration));
    }

    void OnMouseDown()
    {
        if (OnBlockPressed != null)
        {
            OnBlockPressed(this);
        }
    }

    IEnumerator AnimateMove(Vector3 target, float duration)
    {
        Vector3 initalPos = transform.position;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(initalPos, target, percent);
            yield return null;
        }

        if (OnFinishedMoving != null)
        {
            OnFinishedMoving();
        }
    }
}
