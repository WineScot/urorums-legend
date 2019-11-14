using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFollowCamera : MonoBehaviour
{
    // ATTRIBUTES
    // camera component
    private GameObject hero;
    private MovingControl movingControl;
    private Rigidbody2D rb2d;
    // variables
    private float x_distance = 0;
    private float y_distance = 0;
    private float x_cameraPosition = 0;
    private float y_cameraPosition = 0;
    private string x_direction = "left";
    private string y_direction = "up";
    private float x_speed = 0;
    private float y_speed = 0;
    private float x_heroSpeed = 0;
    private float y_heroSpeed = 0;
    private bool followHeroMode = true;
    void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
        rb2d = base.GetComponent<Rigidbody2D>();
        movingControl = hero.GetComponent<MovingControl>();
                                                          // Odczytywanie save'a
        if (PlayerPrefs.HasKey("HeroPositionX"))          // Jeśli wybrana została opcja NewGame, to PlayerPrefs jest puste
            transform.position = new Vector3(PlayerPrefs.GetFloat("HeroPositionX"), PlayerPrefs.GetFloat("HeroPositionY"), -10);
        PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        x_cameraPosition = base.transform.position.x;
        y_cameraPosition = base.transform.position.y;
        if (followHeroMode)
        { 
            SetHeroPosition();
            FollowHero();
        }
    }

    private void SetHeroPosition()
    {
        x_distance = movingControl.GetPosition().x - x_cameraPosition;
        y_distance = movingControl.GetPosition().y - y_cameraPosition;
        if (x_distance > 0) x_direction = "right";
        else
        {
            x_direction = "left";
            x_distance = -x_distance;
        }
        if (y_distance > 0) y_direction = "up";
        else
        {
            y_direction = "down";
            y_distance = -y_distance;
        }

    }

    public ref bool GetFollowHeroMode()
    {
        return ref followHeroMode;
    }

    private void FollowHero()
    {
        x_speed = y_speed = 0;
        x_heroSpeed = (movingControl.CurrentHorizontalSpeed()) > 5 ? (movingControl.CurrentHorizontalSpeed()) : x_heroSpeed;
        y_heroSpeed = Mathf.Abs(movingControl.CurrentVerticalSpeed()) > 15 ? Mathf.Abs(movingControl.CurrentVerticalSpeed()) : y_heroSpeed;
        if(y_direction == "down")
        {
            if (y_distance < 8.2f) y_speed = 0;
            else for (int i = 1; i < 50; i++)
                {
                    if (y_distance - 8.2f > i) y_speed += (y_heroSpeed/4);
                }
        }
        else
        {
            if (y_distance < 10.2f) y_speed = 0;
            else for (int i = 0; i < 20; i++)
                {
                    if (y_distance - 10.2f > i) y_speed += (y_heroSpeed/6);
                }
        }

        if (x_distance <= 5) x_speed = 0;
        else for(int i=1;i<50;i++)
            {
                if (x_distance > 5 * i) x_speed += (x_heroSpeed / 5);
            }

        Move(x_speed, y_speed, x_direction, y_direction);

    }

    public void Move(float x_moveSpeed,float y_moveSpeed,string x_direction,string y_direction)
    {
        Vector2 movement;
        if (x_direction == "left") x_moveSpeed = -x_moveSpeed;
        if (y_direction == "down") y_moveSpeed = -y_moveSpeed;
        movement = new Vector2(x_moveSpeed, y_moveSpeed);
        rb2d.velocity = movement;
    }
} 