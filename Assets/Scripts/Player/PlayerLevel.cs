using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{

    [SerializeField] public PlayerHealthPoints playerHealth;
    [SerializeField] public LightResource playerLight;

    public int currentLevel;
    [SerializeField] public int maxHealthPerLevel;
    [SerializeField] public float healthGainRatePerLevel;
    public float currentHealthGainRate;
    [SerializeField] public int maxLightPerLevel;
    [SerializeField] public float lightGainRatePerLevel;
    public float currentLightGainRate;

    //Tick Timer
    private float tickRate = 0.05f;
    private float previousTickTime;

    [SerializeField] private bool setLevelDEV = false;
    [SerializeField] private bool levelUpHotkeyDEV = false;





    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 1;
        currentHealthGainRate = 0;
        currentLightGainRate = 0;

        previousTickTime = Time.time;
    }


    
    public void levelUp()
    {
        playerHealth.maximumHealth += maxHealthPerLevel;
        playerHealth.healEntity(1000);
        currentHealthGainRate += healthGainRatePerLevel;
        playerLight.maximumLight = playerLight.maximumLight + maxLightPerLevel;
        playerLight.addLight(1000);
        currentLightGainRate += lightGainRatePerLevel;
        currentLevel++;
    }



    // Update is called once per frame
    void Update()
    {

        //Heal over time and Light gain over time
        if (Time.time - previousTickTime >= tickRate)
        {
            if (!playerHealth.blackout) {
                playerHealth.healEntity(currentHealthGainRate * (Time.time - previousTickTime) /*tickRate*/);
                playerLight.addLight(currentLightGainRate * (Time.time - previousTickTime) /*tickRate*/);
                previousTickTime = Time.time;
            }
        }


        if (setLevelDEV)
        {
            levelUp();
            setLevelDEV = false;
        }

        if (Input.GetKeyDown(KeyCode.L) && levelUpHotkeyDEV)
        {
            levelUp();
        }
    }
}
