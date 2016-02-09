﻿using UnityEngine;

public enum Swipe { None, Up, Down, Left, Right };

// Left, right, up and down swipes. Taps do not count as swipes.
// Source: http://forum.unity3d.com/threads/swipe-in-all-directions-touch-and-mouse.165416/#post-1516893
public class SwipeManager : MonoBehaviour
{
    public float minSwipeLength = 200f;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    public static Swipe swipeDirection;

    void Update()
    {
        DetectSwipe();
    }

    public void DetectSwipe()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }

            if (t.phase == TouchPhase.Ended)
            {
                secondPressPos = new Vector2(t.position.x, t.position.y);
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                // Make sure it was a legit swipe, not a tap
                if (currentSwipe.magnitude < minSwipeLength)
                {
                    swipeDirection = Swipe.None;
                    return;
                }

                currentSwipe.Normalize();

                // Swipe up
                if (currentSwipe.y > 0 || currentSwipe.x > -0.5f || currentSwipe.x < 0.5f) {
                    swipeDirection = Swipe.Up;
                    // Swipe down
                } else if (currentSwipe.y < 0 || currentSwipe.x > -0.5f || currentSwipe.x < 0.5f) {
                    swipeDirection = Swipe.Down;
                    // Swipe left
                } else if (currentSwipe.x < 0 || currentSwipe.y > -0.5f || currentSwipe.y < 0.5f) {
                    swipeDirection = Swipe.Left;
                    // Swipe right
                } else if (currentSwipe.x > 0 || currentSwipe.y > -0.5f || currentSwipe.y < 0.5f) {
                    swipeDirection = Swipe.Right;
                }
            }
        }
        else {
            swipeDirection = Swipe.None;
        }
    }
}
