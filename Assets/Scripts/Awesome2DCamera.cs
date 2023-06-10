using UnityEngine;
using System.Collections;
 
public class Awesome2DCamera : MonoBehaviour {
 
    // the target the camera should follow (usually the player)
    public Transform target;
 
    // the camera distance (z position)
    public float distance = -10f;
 
    // the height the camera should be above the target (AKA player)
    public float height = 0f;
 
    // damping is the amount of time the camera should take to go to the target
    public float damping = 5.0f;
 
    // map maximum X and Y coordinates. (the final boundaries of your map/level)
    public float mapX = 100.0f;
    public float mapY = 100.0f;
 
    // just private var for the map boundaries
    private float minX = 0f;
    private float maxX = 0f;
    private float minY = 0f;
    private float maxY = 0f;
  
    void Start () {
        // the map MinX and MinY are the position that the camera STARTS
        minX = transform.position.x;
        minY = transform.position.y;
        // the desired max boundaries
        maxX = mapX;
        maxY = mapY;
    }
 
    void Update () {
 
        // get the position of the target (AKA player)
        Vector3 wantedPosition = target.TransformPoint(0, height, distance);
 
        // check if it's inside the boundaries on the X position
        wantedPosition.x = (wantedPosition.x < minX) ? minX : wantedPosition.x;
        wantedPosition.x = (wantedPosition.x > maxX) ? maxX : wantedPosition.x;
 
        // check if it's inside the boundaries on the Y position
        wantedPosition.y = (wantedPosition.y < minY) ? minY : wantedPosition.y;
        wantedPosition.y = (wantedPosition.y > maxY) ? maxY : wantedPosition.y;

        // set the camera to go to the wanted position in a certain amount of time
        transform.position = Vector3.Lerp (transform.position, wantedPosition, (Time.deltaTime * damping));
    }
}
/*
The MIT License (MIT)

Copyright (c) 2015 Emerson Carvalho <@emersonbroga>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/