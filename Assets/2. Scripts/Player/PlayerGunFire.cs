using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{

    [SerializeField] private Transform _fireTransform;
    
  
  

    private void Update()
    {
        // 1. 마우스 왼쪽 버튼 누르면
        if (Input.GetMouseButtonDown(0))
        {
            // 2. Ray를 생성하고 발사할 위치, 방향, 거리를 설정한다.(쏜다)
            Ray ray = new Ray(_fireTransform.position, Camera.main.transform.forward);
            
            // 3. RayCastHit (충돌한 대상의 정보)를 저장할 변수를 생성한다.
            RaycastHit hitInfo = new RaycastHit();
            
            // 4. 발사하고, 충돌했다면... 피격 이펙트 표시
            bool isHit =  Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                Debug.Log("Hit " + hitInfo.transform.name);
            }
        }
    }
}
