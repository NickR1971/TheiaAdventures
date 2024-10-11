using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************
 * Клас CScenario створює сюжет гри
 * він складається із сюжетних вузлів
 * та умов переходу між ними
 *************************************/
public abstract class CScenario
{
    protected string name;
    protected EGameLevels currentLevel;
    protected int cntNodes;
    protected CScenarioNode[] nodes;
    protected int currentNode;

    public CScenario()
    {
        name = "<empty>";
        currentLevel = EGameLevels.levelRecruit;
        cntNodes = 0;
        currentNode = 0;
    }

    public string GetName() => name;
    public EGameLevels GetCurrentLevel() => currentLevel;
    
    public CScenarioNode GetCurrentNode()
    {
        if (currentNode < cntNodes) return nodes[currentNode];
        
        return null;
    }
}
