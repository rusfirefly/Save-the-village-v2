using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Buff
{
    public float Attack { get; set; }
    public float Defence { get; set; }
    public float Health { get; set; }

}

public class BuffSkill : MonoBehaviour
{
    [SerializeField] public Canvas _canvasSkill;
    [SerializeField] private Image _buffImage;
    [SerializeField] private Text _textBuff;
    private Buff _eatBuff;

    private void Awake()
    {
        _eatBuff.Attack = 0.35f;
        _eatBuff.Defence = 0.25f;
        _eatBuff.Health = 2f;

        UpdateTextSkill();
    }

    public Buff GetBuff() => _eatBuff;

    private void UpdateTextSkill()
    {
        _textBuff.text = $"ÁÀÔÔ íàñûùåíèÿ\n+{_eatBuff.Health * 100}% hp\n+{_eatBuff.Attack * 100}% atk +{_eatBuff.Defence * 100}% def";
    }

    private void OnValidate()
    {
        UpdateTextSkill();
    }

    public void BuffVisible(bool visible) => _canvasSkill.gameObject.SetActive(visible);

}
