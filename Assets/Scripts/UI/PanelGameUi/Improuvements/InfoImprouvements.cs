using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoImprouvements : MonoBehaviour
{
    private TopSkillManager parentTopSkill;

    public void setInfo(TopSkillManager _parentTopSkill)
    {
        parentTopSkill = _parentTopSkill;
    }

    public void improuvementBuy()
    {
        parentTopSkill.amelioration();
    }
}
