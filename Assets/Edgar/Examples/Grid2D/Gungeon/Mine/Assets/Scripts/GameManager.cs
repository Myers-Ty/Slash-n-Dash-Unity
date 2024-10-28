using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Sprite> PlayerSprites;
    public List<Sprite> SwordSprites;
    public Player player;
    private void Awake()
    {
        instance = this;

    }



}
