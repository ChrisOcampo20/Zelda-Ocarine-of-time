using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager sharedInstance;

    public List<LevelBlock> allTheLevelBlock = new List<LevelBlock>();

    public List<LevelBlock> currentLevelBlock = new List<LevelBlock>();

    public Transform levelstartPosition;

    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }   
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateInitialBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddLevelBlock()
    {
        int randomIdx = Random.Range(0, allTheLevelBlock.Count);

        LevelBlock block;

        Vector3 spawnPosition = Vector3.zero;

        if(currentLevelBlock.Count == 0)
        {
            block = Instantiate(allTheLevelBlock[0]);
            spawnPosition = levelstartPosition.position;
        }
        else
        {
            block = Instantiate(allTheLevelBlock[randomIdx]);
            spawnPosition = currentLevelBlock[currentLevelBlock.Count - 1].exitPoint.position;
        }

        block.transform.SetParent(this.transform, false);

        Vector3 correction = new Vector3(spawnPosition.x - block.startPoint.position.x, spawnPosition.y - block.startPoint.position.y, 0);
        block.transform.position = correction;
        currentLevelBlock.Add(block);

       
            
    }


    public void RemoveLevelBlock()
    {
        LevelBlock oldBlock = currentLevelBlock[0];
        currentLevelBlock.Remove(oldBlock);
        Destroy(oldBlock.gameObject);
    }


    public void RemoveAllLevelBlock()
    {
        while (currentLevelBlock.Count>0)
        {
            RemoveLevelBlock();
        }
    }

    public void GenerateInitialBlock()
    {
        for(int i = 0; i <2; i++)
        {
            AddLevelBlock();
        }
    }


}
