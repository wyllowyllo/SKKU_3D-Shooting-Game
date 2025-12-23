using System.Collections;
using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    private enum GoldState
    {
        Scattered,  // 흩어지는 상태
        Idle,       // 대기 상태
        Collecting  // 수집 중
    }

    [Header("Gold Settings")]
    [SerializeField] private int _goldValue;
    [SerializeField] private float _collectRadius = 3f;
    [SerializeField] private float _collectSpeed = 7f;
    [SerializeField] private float _rotationSpeed = 360f;

    [Header("Scatter Settings")]
    [SerializeField] private float _scatterSettleTime = 2f;

    private Rigidbody _rigidbody;
    private Collider[] _colliders;
    private GoldState _currentState;
    private Transform _playerTransform;
    private Vector3 _startPosition;
    private float _bezierProgress;
    private bool _isInitialized = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _colliders = GetComponents<Collider>();
    }

    public void Initialize(int value, Vector3 scatterDirection, float scatterForce)
    {
        _goldValue = value;
        _currentState = GoldState.Scattered;
        _playerTransform = null;
        _bezierProgress = 0f;
        _isInitialized = true;

        // Rigidbody 활성화 및 물리 적용
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            // 흩뿌리기 효과
            _rigidbody.AddForce(scatterDirection * scatterForce, ForceMode.Impulse);
            _rigidbody.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
        }

        // Collider 활성화
        foreach (var col in _colliders)
        {
            col.enabled = true;
        }

        // 일정 시간 후 Idle 상태로 전환
        StartCoroutine(TransitionToIdle());
    }

    private IEnumerator TransitionToIdle()
    {
        yield return new WaitForSeconds(_scatterSettleTime);

        // 속도가 충분히 느려졌거나 시간이 지나면 Idle 상태로
        if (_currentState == GoldState.Scattered)
        {
            _currentState = GoldState.Idle;

            // 물리 정지
            if (_rigidbody != null)
            {
                _rigidbody.linearVelocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }
    }

    private void Update()
    {
        if (!_isInitialized) return;

        switch (_currentState)
        {
            case GoldState.Idle:
                // 회전 효과
                transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
                break;

            case GoldState.Collecting:
                UpdateBezierMovement();
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Idle 상태일 때만 수집 시작
        if (_currentState != GoldState.Idle) return;

        // 플레이어 감지
        if (other.CompareTag("Player"))
        {
            PlayerStat playerStat = other.GetComponent<PlayerStat>();
            if (playerStat != null)
            {
                StartCollection(other.transform);
            }
        }
    }

    private void StartCollection(Transform player)
    {
        _currentState = GoldState.Collecting;
        _playerTransform = player;
        _startPosition = transform.position;
        _bezierProgress = 0f;

        // 물리 비활성화
        if (_rigidbody != null)
        {
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }
    }

    private void UpdateBezierMovement()
    {
        if (_playerTransform == null)
        {
            OnCollected();
            return;
        }

        _bezierProgress += Time.deltaTime * _collectSpeed;

        if (_bezierProgress >= 1f)
        {
            OnCollected();
            return;
        }

        // 베지어 곡선 계산 (Quadratic Bezier)
        float t = _bezierProgress;
        float oneMinusT = 1f - t;

        // 플레이어의 가슴 높이로 수집
        Vector3 endPosition = _playerTransform.position + Vector3.up * 1f;

        // 컨트롤 포인트: 중간 지점에서 위로 올라간 위치 (아치 효과)
        Vector3 midPoint = (_startPosition + endPosition) * 0.5f;
        Vector3 controlPoint = midPoint + Vector3.up * 2f;

        // Quadratic Bezier 공식: B(t) = (1-t)²P0 + 2(1-t)tP1 + t²P2
        Vector3 position = (oneMinusT * oneMinusT * _startPosition) +
                          (2f * oneMinusT * t * controlPoint) +
                          (t * t * endPosition);

        transform.position = position;

        // 회전 효과
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime * 2f);
    }

    private void OnCollected()
    {
        if (!_isInitialized) return;

        // 플레이어에게 골드 추가
        if (_playerTransform != null)
        {
            PlayerStat playerStat = _playerTransform.GetComponent<PlayerStat>();
            if (playerStat != null)
            {
                playerStat.AddGold(_goldValue);
            }
        }

        // 풀로 반환
        _isInitialized = false;
        _currentState = GoldState.Idle;

        // Collider 비활성화
        foreach (var col in _colliders)
        {
            col.enabled = false;
        }

        GoldFactory.Instance.ReturnGoldCoin(this);
    }

    private void OnDisable()
    {
        // 코루틴 정리
        StopAllCoroutines();
    }
}
