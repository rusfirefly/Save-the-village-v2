using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private SoundSystem _soundSystem;
    //[SerializeField] private HUDSystem _hudSystem;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameMenu _gameMenuSystem;

    private void Start()
    {
        _soundSystem.Initialize();
        _gameManager.Initialize();
        _gameMenuSystem.Initialize();
    }


}
