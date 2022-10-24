using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FishingCamera : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] Tilemap map;
    [SerializeField] int chunkSize;

    [SerializeField] EdgeCollider2D bounds;
    
    //how far down chunks get spawned;
    [SerializeField] float chunkPadding;

    [SerializeField] GameObject lure;
    Camera cam;
    Rigidbody2D body;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        body = GetComponent<Rigidbody2D>();

        body.velocity = speed * Vector2.down;

        float ySize = cam.orthographicSize;
        float xSize = ySize * cam.aspect;

        Vector2[] newBounds = new Vector2[5];
        newBounds[0] = new Vector2(xSize, ySize);
        newBounds[1] = new Vector2(-xSize, ySize);
        newBounds[2] = new Vector2(-xSize, -ySize);
        newBounds[3] = new Vector2(xSize, -ySize);
        newBounds[4] = new Vector2(xSize, ySize);

        bounds.points = newBounds;
    }
    private void Update()
    {
        //transform.position = new Vector3(transform.position.x, lure.transform.position.y, transform.position.z);



        //ortho size * 2 is full y height of camera
        if(map.CellToWorld(map.origin).y > (cam.transform.position.y - cam.orthographicSize) - chunkPadding)
        {
            InsertChunk(chunkSize);
        }
    }

    void InsertChunk(int size)
    {
        for (int i = 0; i < size; i++)
        {
            int height = map.origin.y - 1;

            for (int j = 0; j < map.size.x; j++)
            {
                TileBase newTile = map.GetTile(map.origin + new Vector3Int(j, 1, 0));
                map.SetTile(new Vector3Int(map.origin.x + j, height, 0), newTile);
            }
        }
    }


}
