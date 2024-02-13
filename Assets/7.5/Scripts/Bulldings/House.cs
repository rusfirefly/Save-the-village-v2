using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class House : MonoBehaviour, ISelecteble
{
    public static event Action<int> HouseBought;
    public static event Action BuildComplete;
    public static event Action<Vector3> NeedEngineer;
    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private Text _priceText;
    private Material _default;
    private SpriteRenderer _spriteRender;
    private SoundClip _sound;
    private bool _isSelected;
    private bool _isBulding;
    private bool _isComplete;
    private float _currentTime;
    [SerializeField] private float _timeBulding;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _needWoodSound;
    [SerializeField] private AudioClip _buildingComplete;
    [SerializeField] private Canvas _canvasPrice;
    [SerializeField] private Canvas _canvasProgress;
    [SerializeField] private int _priceBuilding;
    [SerializeField] private Text _progressBuildingText;
    private WorkManEngineer unit;

    private void Start()
    {
        GetSoundComponent();
        SetPriceText($"{_priceBuilding}");
    }

    private void Update()
    {
        BuldingHouse();
    }

    private void OnValidate()
    {
        GetAnimatorComponent();
        SetPriceText($"{_priceBuilding}");
    }

    private void GetSoundComponent()=> _sound = gameObject.GetComponent<SoundClip>();

    private void BuldingHouse()
    {
        if (_isComplete) return;
        if (!_isBulding) return;

        _currentTime += Time.deltaTime;
        SetProgressBuldingText($"{(_timeBulding - _currentTime):F0}s");
        if (_currentTime>=_timeBulding)
        {
            BuildComplete?.Invoke();

            CanvasPriceTextVisible(false);
            CanvasProgressVisible(false);
            PlayAnimationComplete();
            PlaySoundBuildingComplite();
            unit.StopWork();

            _isComplete = true;
            _isBulding = false;

            _currentTime -= _timeBulding;
        }
    }

    private void CanvasPriceTextVisible(bool visible) => _canvasPrice.gameObject.SetActive(visible);

    private void CanvasProgressVisible(bool visible)=> _canvasProgress.gameObject.SetActive(visible);

    private void PlayAnimationComplete()=> _animator.SetBool("CompleteBuild", true);

    private void PlaySoundBuildingComplite() => _sound.PlaySound(_buildingComplete);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "worker" && !_isBulding && _isSelected)
        {
            _isBulding = true;
            _canvasPrice.gameObject.SetActive(false);
            _canvasProgress.gameObject.SetActive(true);
            ProgressBuildingTextVisible(true);
            unit = collision.gameObject.GetComponent<WorkManEngineer>();
            unit.StartWork();
        }
    }

    private void OnMouseDown()
    {
        if (WorkManEngineer.CountWork == Population.EngineerCountTotal) return;
        if (Storage.Wood < _priceBuilding)
        {
            _sound.PlaySound(_needWoodSound);
            return;
        }

        if (!_isBulding)
        {
            _isSelected = true;
            HouseBought?.Invoke(_priceBuilding);
            NeedEngineer?.Invoke(gameObject.transform.position);
        }
    }

    public void DeSelected()
    {
        _isSelected = false;
        _spriteRender.material = _default;
        SelectedBuilding.selectedObject = null;
    }

    public void Selected()
    {
        if (!_isSelected)
        {
            _isSelected = true;
            _spriteRender.material = _outlineMaterial;
            SelectedBuilding.OnSelected(gameObject);
        }
    }

    public void Reload()
    {
        _canvasPrice.gameObject.SetActive(true);
        _canvasProgress.gameObject.SetActive(false);
        _animator.SetBool("CompleteBuild", false);
        _isComplete = false;
    }

    private void SetProgressBuldingText(string text)=> _progressBuildingText.text = text;

    private void ProgressBuildingTextVisible(bool visible) => _progressBuildingText.gameObject.SetActive(visible);

    private void SetPriceText(string text) => _priceText.text = text;

    private void GetAnimatorComponent()=> _animator ??= gameObject.GetComponent<Animator>();
}
