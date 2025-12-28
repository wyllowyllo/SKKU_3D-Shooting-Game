using UnityEngine;
using Redcode.Pools;
using System.Collections;


[RequireComponent(typeof(PlayerInput))]
public class PlayerBombFire : MonoBehaviour
{
   
   [Header("사격 설정")]
   [SerializeField] private Transform _fireTransform;
   [SerializeField] private float _throwPower = 15f;
   
   [Header("투사체 설정")]
   [SerializeField] private Bomb _bombPrefab;
   [SerializeField] private int _maxBombCnt = 5;
   [SerializeField] private int _curBombCnt;
   
   // 참조
   private PlayerInput _input;
   private Camera _cam;
   private Animator _animator;

   // 애니메이션 상태 추적
   private bool _isThrowing = false;
   private bool _hasThrownBomb = false;

   // 애니메이션 상태 해시
   private int _throwStateHash;

   public int MaxBombCnt => _maxBombCnt;

   public int CurBombCnt => _curBombCnt;

   private void Awake()
   {
      Init();
   }

   private void Update()
   {
      Fire();
   }

   private void Init()
   {
      _input = GetComponent<PlayerInput>();
      _cam = Camera.main;
      _animator = GetComponentInChildren<Animator>();

      _curBombCnt = MaxBombCnt;

      // Throw 상태 해시 미리 계산
      _throwStateHash = Animator.StringToHash("Throw");
   }

   private void Fire()
   {
      if (_isThrowing) return; // 이미 던지는 중이면 무시
      if (!_input.BombThrow || CurBombCnt <= 0) return;

      // 코루틴 시작
      StartCoroutine(ThrowBombRoutine());
   }

   private IEnumerator ThrowBombRoutine()
   {
      _isThrowing = true;

      // 애니메이션 트리거 설정
      _animator?.SetTrigger("Throw");

      // 애니메이션 전환 대기 (1프레임)
      yield return null;

      // Throw 상태로 전환될 때까지 대기
      yield return new WaitUntil(() =>
      {
         AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(1);
         return stateInfo.shortNameHash == _throwStateHash;
      });

      // normalizedTime이 0.7에 도달할 때까지 대기
      yield return new WaitUntil(() =>
      {
         AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(1);
         float normalizedTime = stateInfo.normalizedTime % 1f;
         return normalizedTime >= 0.7f;
      });

      // 폭탄 발사
      ThrowBomb();
      Debug.Log("폭탄 발사!");

      // 애니메이션 끝날 때까지 대기
      yield return new WaitUntil(() =>
      {
         AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(1);
         return currentState.shortNameHash != _throwStateHash || currentState.normalizedTime >= 0.95f;
      });

      _isThrowing = false;
   }

   private void ThrowBomb()
   {
      if (CurBombCnt <= 0) return;

      //Bomb bomb = Instantiate(_bombPrefab, _fireTransform.position, Quaternion.identity);
      Bomb bomb = BombFactory.Instance.GetBomb();
      bomb.transform.position = _fireTransform.position;
      bomb.transform.rotation = _fireTransform.rotation;

      Rigidbody bombRigid = bomb.GetComponent<Rigidbody>();
      bombRigid.AddForce(_cam.transform.forward * _throwPower, ForceMode.Impulse);
      _curBombCnt = CurBombCnt - 1;
   }
}
