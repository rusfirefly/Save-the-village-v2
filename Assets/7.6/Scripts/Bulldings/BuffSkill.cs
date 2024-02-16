using System;
using UnityEngine;
using UnityEngine.UI;

public class Buff
{
    public float Attack { get; private set; }

    public float Defence { get; private set; }

    public float Health { get; private set; }

    public Buff(float attack, float defence, float health)
    {
        Attack = attack;
        Defence = defence;
        Health = health;
    }
}

public class BuffSkill : MonoBehaviour
{
    public static event Action<Buff> EventBuff;

    [SerializeField] private Canvas _canvasSkill;
    [SerializeField] private Image _buffImage;
    [SerializeField] private Text _textBuff;
    [SerializeField] private Buff _eatBuff;
    private int _percent=100; 

    private void Awake()
    {
        _eatBuff = new Buff(attack: 0.35f, defence: 0.25f,health: 0.2f);
        UpdateTextSkill();
    }

    public Buff GetBuff() => _eatBuff;

    private void UpdateTextSkill()
    {
        _textBuff.text = $"ÁÀÔÔ íàñûùåíèÿ\n+{_eatBuff.Health * _percent}% hp\n+{_eatBuff.Attack * _percent}% atk +{_eatBuff.Defence * _percent}% def";
    }

    public void BuffVisible(bool visible) => _canvasSkill.gameObject.SetActive(visible);

    public void EnableBuff()
    {
        EventBuff?.Invoke(_eatBuff);
    }

    public void DisableBuff()
    {
        Buff deBuff = new Buff(attack: 0,defence: 0,health: 0);
        EventBuff?.Invoke(deBuff);
    }
}
