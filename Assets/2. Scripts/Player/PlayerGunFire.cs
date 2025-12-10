using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{

    [SerializeField] private Transform _fireTransform;
    [SerializeField] private ParticleSystem _hitEffect;
  
  

    private void Update()
    {
        // 1. 마우스 왼쪽 버튼 누르면
        if (Input.GetMouseButton(0))
        {
            // 2. Ray를 생성하고 발사할 위치, 방향, 거리를 설정한다.(쏜다)
            Ray ray = new Ray(_fireTransform.position, Camera.main.transform.forward);
            
            // 3. RayCastHit (충돌한 대상의 정보)를 저장할 변수를 생성한다.
            RaycastHit hitInfo = new RaycastHit();
            
            // 4. 발사하고, 충돌했다면... 피격 이펙트 표시
            bool isHit =  Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                
                // 파티클 생성과 플레이 방식
                // 1. Instantiate 방식 ( + 풀링) -> 새로 생성(메모리, cpu) -> 한 화면에 여러가지 수정 후 여러 개 그릴 경우 주로 사용
                // 2. 하나를 캐싱해두고 Play ->단점 : 재실행이므로 기존 것이 삭제 -> 한 화면에 한번만 그릴 경우 주로 사용
                // 3. 하나를 캐싱해두고 Emit -> 한 화면에 위치만 수정 후 여러 개 그릴 경우 주로 사용
                
                Debug.Log("Hit " + hitInfo.transform.name);
                //_hitEffect.transform.position = hitInfo.point;
                //_hitEffect.transform.forward = hitInfo.normal;
                //_hitEffect.Play(true);
                
                // 파티클을 어떻게 분출할지 정보를 넘겨줌
                ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
                emitParams.position = hitInfo.point;
                emitParams.rotation3D = Quaternion.LookRotation(hitInfo.normal).eulerAngles;
                _hitEffect.Emit(emitParams, 1);
            }
        }
    }
}
