using System;
using Assets.Scripts.Roguelike.Entities.Player;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    public Player Player;
    public Action HealthChangeAction;
    private Slider _slider;

    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = Player.Health;
        _slider.value = Player.Health;
        
        Player.HealthChangeEvent += delegate ()
        {
            _slider.value = Player.Health;
        };
    }
}
