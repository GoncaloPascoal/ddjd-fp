using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPSystem : MonoBehaviour
{
    private int xp = 0;
    private int level = 1;
    private int levelUpsRemaining = 0;

    public int LevelUpsRemaining
    {
        get { return levelUpsRemaining; }
    }

    [SerializeField]
    private GameObject XpBarItem;
    [SerializeField]
    private Slider XpSlider;
    [SerializeField]
    private TextMeshProUGUI LevelText;
    [SerializeField]
    private TextMeshProUGUI levelUpsRemainingText;


    [SerializeField] private List<Image> _level_up_icons;

    private StatsPlayer _playerStats;

    private InventoryManager _inv_manager;
    
    [SerializeField] public StatsDictionary boostPerLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerStats = GameObject.FindWithTag("Player").GetComponent<StatsPlayer>();
        _inv_manager = GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetButtonDown("InvLevelUpDamage"))
        {
            LevelUpStat(StatName.Damage);
        }
        if (InputManager.GetButtonDown("InvLevelUpArmor"))
        {
            LevelUpStat(StatName.Armor);
        }
        if (InputManager.GetButtonDown("InvLevelUpHealth"))
        {
            LevelUpStat(StatName.Health);
        }
        if (InputManager.GetButtonDown("InvLevelUpStamina"))
        {
            LevelUpStat(StatName.Stamina);
        }
        if (InputManager.GetButtonDown("InvLevelUpStability"))
        {
            LevelUpStat(StatName.Stability);
        }
        if (InputManager.GetButtonDown("InvLevelUpStaminaRecovery"))
        {
            LevelUpStat(StatName.StaminaRecovery);
        }
    }

    public void AddXp(int xp_to_add)
    {
        //var oldXp = xp;
        xp += xp_to_add;
        while (HasLevelledUp())
        {
            LevelUp();
        }

        UpdateXp();

    }

    void LevelUp()
    {
        level++;
        levelUpsRemaining++;
        TurnOnLevelUpIcons();
    }
    
    bool HasLevelledUp()
    {
        return GetXPToLevel(level + 1) <= xp;
    }

    public int GetXpInLevel(int level)
    {
        return GetXPToLevel(level + 1) - GetXPToLevel(level);
    }

    public int GetXPToLevel(int level)
    {
        level = level - 1;
        if (level == 0)
            return 0;
        return (int) (level * Mathf.Sqrt(level)) + 2 * level;
    }

    public void LevelUpStat(StatName statName)
    {
        if(levelUpsRemaining > 0){
            levelUpsRemaining--;
            _playerStats.levelUpBoosts[statName] += boostPerLevel[statName];
            if (levelUpsRemaining == 0)
            {
                TurnOffLevelUpIcons();
            }
            _inv_manager.UpdatePlayerStats();
            UpdateXp();
        }
    }

    public void TurnOffLevelUpIcons()
    {
        foreach (Image icon in _level_up_icons)
        {
            icon.gameObject.SetActive(false);
        }
    }
    
    public void TurnOnLevelUpIcons()
    {
        foreach (Image icon in _level_up_icons)
        {
            icon.gameObject.SetActive(true);
        }
    }

    public void UpdateXp()
    {
        var cur_level_xp = xp - GetXPToLevel(level);

        var xp_requirement_this_level = GetXpInLevel(level);

        XpSlider.value = ((float) cur_level_xp) / xp_requirement_this_level;

        LevelText.text = level.ToString();

        if (levelUpsRemaining > 0)
        {
            levelUpsRemainingText.gameObject.SetActive(true);
            levelUpsRemainingText.text = levelUpsRemaining.ToString();
        }
        else
        {
            levelUpsRemainingText.gameObject.SetActive(false);
        }
    }
    public void ToggleXp()
    {
        if (XpBarItem.activeSelf)
        {
            XpBarItem.SetActive(false);
            return;
        }
        
        XpBarItem.SetActive(true);

        UpdateXp();
    }
    
}
