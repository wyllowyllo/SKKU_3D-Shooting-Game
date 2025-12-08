using System;
using UnityEngine;

public class TryCatchFinallyTest : MonoBehaviour
{
    // 예외 : 런타임중에 발생하는 오류 (참조, 나누기, 인덱스 범위 벗어나기 등)
    
    // Try - Catch 문법 : 예외를 처리하는 기본 분법

    public int Age;

    private void Start()
    {
        if (Age < 0)
        {
            Debug.LogError("사람 나이는 0보다 적을 수 없습니다"); //이렇게만 적어놓으면, 에러 떠서 로그 뜨더라도 그 즉시 사람이 못알아차릴 수 있음.
            throw new Exception("사람 나이는 0보다 적을 수 없습니다"); // 따라서 오류가 날 경우, 이렇게 해서 뜨는 즉시 알아차리도록!
        }
        
        // 인덱스 범위 벗어나므로 오류 발생
        // -> 다른 컴포넌트나 게임 오브젝트에도 영향을 줌으로써 프로그램이 정상적으로 동작 안할 수 있다
        int[] numbers = new int[32];
        //numbers[75] = 1;

        try
        {
            // 예외가 발생할 만한 코드 작성
            int index = 75;
            numbers[index] = 1;
        }
        catch(Exception e)
        {
            // 예외가 발생했을 때 처리해야 할 코드 작성
            int index = numbers.Length - 1;
            numbers[index] = 1;
            Debug.Log("오류 발생해서 방어 코드로 대체함");
            Debug.Log(e.Message);
        }
        finally
        {
            // 이건 옵션임
            // 정상이든 오류이든 실행해야 할 코드 작성
        }
        
        // Try - Catch 구문은 되도록이면 안쓰는 게 좋다. 이유는?
        // - 성능 저하 ( 이게 주된 이유)
        // - 잘못된 알고리즘 (안고치고 그대로 넘어가버릴 수도..)
        
        // 써야 하는 경우 : 내가 제어할 수 없을 때...
        // 제어할 수 없는 상황에서 나타나는 오류는 수십가지이므로 일일이 방어코드를 작성할 수 없다... 그럴 때만 이 문법을 쓴다.
        
        // 1. 네트워크 접근
        // - 로그인, 로그아웃 / 서버 / DB 아이템 저장,불러오기
        // 2. 파일 접근
        // - 용량 충분한지? 파일명 괜찮은지? 권한 있는지? -> 여러가지 이유로 오류가 날 수 있음. 일일이 방어코드를 작성하기 힘듦...
        // 3. DB 접근
        // - 이것도 마찬가지. 에러 이유가 굉장히 많음.
        
    }
}
