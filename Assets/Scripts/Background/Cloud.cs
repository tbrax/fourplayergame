using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud
{

    Transform cloud;
    Vector3 move;
    BackgroundEvent e;
    Transform c;
    SpriteRenderer sprite;

    float checkTimer;
    float checkTimerMax = 5;


    public void setup(BackgroundEvent ee)
    {
        e = ee;
        string path = "Prefabs/Background/Cloud";
        c = Resources.Load<Transform>(path);
        
        makeCloud();
        sprite = cloud.GetComponent<SpriteRenderer>();
        resetCloud();

        startInScreen();
        
    }

    float spawnPosY()
    {
        List<float> bds = e.levelBounds();
        return ((bds[2] + bds[3]) / 2);
    }


    void startInScreen()
    {
        List<float> bds = e.levelBounds();

        float botRand = spawnPosY();
        float topRand = bds[3];

        float posX = Random.Range(bds[0], bds[1]);
        float posY = Random.Range(botRand, topRand);
        cloud.position = new Vector3(posX, posY, 0);
    }
    void makeCloud()
    {
        cloud = GameObject.Instantiate(c, new Vector3(-500, 0, 0), Quaternion.identity);
       

    }

    

    void resetCloud()
    {
        sprite.sprite = e.getImage();

        int side = Random.Range(0, 2);
        if (side == 0)
        {
            side = -1;
        }

        List<float> bds = e.levelBounds();

        float sizeX = sprite.bounds.extents.x;
        float sizeY = sprite.bounds.extents.y;

        float posX = 0;
        if (side == -1)
        {
            //posX = Random.Range(-10.0f, bds[0] - sizeX);
            posX = bds[0] - sizeX*2;
        }
        else
        {
            //posX = Random.Range(-10.0f, bds[0] - sizeX);
            posX = bds[1] + sizeX * 2;
        }

        float botRand = spawnPosY();
        float topRand = bds[3];

        float posY = Random.Range(botRand, topRand);
        float speedX = Random.Range(0.05f, 0.3f);
        speedX = speedX * -1 * side;


        cloud.position = new Vector3(posX, posY, 0);

        float scale = Random.Range(3f, 6f);
        cloud.transform.localScale = new Vector3(scale, scale, 1);
        move = new Vector3(speedX, 0, 0);

    }

    void checkBounds()
    {
        List<float> bds = e.levelBounds();
        float sizeX = sprite.bounds.extents.x;

        if (cloud.position.x < bds[0] - sizeX * 2.2)
        {
            resetCloud();
        }

        if (cloud.position.x > bds[1] + sizeX * 2.2)
        {
            resetCloud();
        }
    }

    void moveCloud(float f)
    {
        cloud.position += move * f;

        checkTimer += f;
        if (checkTimer > checkTimerMax)
        {
            checkTimer = 0;
            checkBounds();
        }
        
    }



    public void update(float f)
    {
        moveCloud(f);

    }

}
