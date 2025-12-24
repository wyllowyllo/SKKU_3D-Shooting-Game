/// <summary>
/// 몬스터/보스 상태 컨트롤러의 공통 인터페이스
/// </summary>
public interface IStateController
{
    
    bool OnDamaged(AttackInfo info);
}
