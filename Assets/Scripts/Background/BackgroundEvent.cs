using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundEvent
{


    List<Cloud> clouds;
    LevelBackground back;
    List<Sprite> cloudImages;


    protected void setupClouds()
    {
        clouds = new List<Cloud>();
        int cloud_num = 6;

        for (int i = 0; i < cloud_num; i++)
        {
            Cloud c = new Cloud();
            c.setup(this);
            clouds.Add(c);
        }
    }


    public Sprite getImage(int i)
    {
        return cloudImages[i];
    }

    public Sprite getImage()
    {
        int i = Random.Range(0, cloudImages.Count);

        return cloudImages[i];
    }

    void loadImages()
    {
        cloudImages = new List<Sprite>();

        cloudImages.Add(Resources.Load<Sprite>("Prefabs/Background/CloudShapes/cloud0"));
        cloudImages.Add(Resources.Load<Sprite>("Prefabs/Background/CloudShapes/cloud1"));
        cloudImages.Add(Resources.Load<Sprite>("Prefabs/Background/CloudShapes/cloud2"));
        cloudImages.Add(Resources.Load<Sprite>("Prefabs/Background/CloudShapes/cloud3"));
    }

    public List<float> levelBounds()
    {
        return back.levelBounds();
    }

    protected void updateClouds(float t)
    {
        foreach(Cloud c in clouds)
        {
            c.update(t);
        }
    }

    public void mainSetup(LevelBackground l)
    {
        back = l;
        setup();
    }

    public virtual void setup()
    {
        loadImages();
        setupClouds();
    }

    public virtual void update(float t)
    {
        updateClouds(t);
    }
}
