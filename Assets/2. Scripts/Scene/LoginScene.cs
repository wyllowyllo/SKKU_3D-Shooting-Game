using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

public class LoginScene : MonoBehaviour
{
    // 로그인씬 (로그인/회원가입) -> 게임씬

    private enum SceneMode
    {
        Login,
        Register
    }
    
    private SceneMode _mode = SceneMode.Login;
    
    // 비밀번호 확인 오브젝트
    [SerializeField] private GameObject _passwordCofirmObject;
    [SerializeField] private Button _gotoRegisterButton;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _gotoLoginButton;
    [SerializeField] private Button _registerButton;

    [SerializeField] private TextMeshProUGUI _messageTextUI;
    
    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_InputField _passwordConfirmInputField;

    private const string LatestID = "latestID";

    // 정규 표현식 패턴
    private const string EmailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    private const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?])[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]{7,20}$";

    private void Start()
    {
        AddButtonEvents();
        Refresh();
    }

    /// <summary>
    /// SHA256 해시 알고리즘을 사용하여 비밀번호를 암호화
    /// </summary>
    /// <param name="password">평문 비밀번호</param>
    /// <returns>64자리 16진수 해시 문자열</returns>
    private string HashPassword(string password)
    {
        // SHA256 해시 알고리즘 객체 생성 (using으로 자동 리소스 해제)
        using (SHA256 sha256 = SHA256.Create())
        {
            // 1. 문자열을 UTF8 바이트 배열로 변환 후 해시 계산
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            // 2. 바이트 배열을 16진수 문자열로 변환
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                // 각 바이트를 2자리 16진수로 변환하여 추가 (예: 255 -> "ff")
                builder.Append(b.ToString("x2"));
            }

            // 3. 최종 해시 문자열 반환 (64자리)
            return builder.ToString();
        }
    }

    private void AddButtonEvents()
    {
        _gotoRegisterButton.onClick.AddListener(GotoRegister);
        _loginButton.onClick.AddListener(Login);
        _gotoLoginButton.onClick.AddListener(GotoLogin);
        _registerButton.onClick.AddListener(Register);
    }

    private void Refresh()
    {
        // 2차 비밀번호 오브젝트는 회원가입 모드일때만 노출
        _passwordCofirmObject.SetActive(_mode == SceneMode.Register);
        _gotoRegisterButton.gameObject.SetActive(_mode == SceneMode.Login);
        _loginButton.gameObject.SetActive(_mode == SceneMode.Login);
        _gotoLoginButton.gameObject.SetActive(_mode == SceneMode.Register);
        _registerButton.gameObject.SetActive(_mode == SceneMode.Register);
        
        // 마지막에 로그인한 ID로 필드 초기화
        if (PlayerPrefs.HasKey(LatestID))
        {
            _idInputField.text = PlayerPrefs.GetString(LatestID);
        }
        
    }

    private void Login()
    {
        // 로그인
        // 1. 아이디 입력을 확인한다.
        string id = _idInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            _messageTextUI.text = "아이디를 입력해주세요.";
            return;
        }
        
        // 2. 비밀번호 입력을 확인한다.
        string password = _passwordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            _messageTextUI.text = "패스워드를 입력해주세요.";
            return;
        }
        
        // 3. 실제 저장된 아이디-비밀번호 계정이 있는지 확인한다.
        // 3-1. 아이디가 있는지 확인한다.
        if (!PlayerPrefs.HasKey(id))
        {
            _messageTextUI.text = "아이디/비밀번호를 확인해주세요.";
            return;
        }

        // 3-2. 입력된 비밀번호를 해시화하여 저장된 해시와 비교
        string hashedPassword = HashPassword(password);
        string storedHash = PlayerPrefs.GetString(id);
        if (storedHash != hashedPassword)
        {
            _messageTextUI.text = "아이디/비밀번호를 확인해주세요.";
            return;
        }
        
        // 4. 있다면 씬 이동
        PlayerPrefs.SetString(LatestID, id); // 최근 로그인 기록 저장
        SceneManager.LoadScene("LoadingScene");
    }

    private void Register()
    {
        // 회원가입
        // 1. 아이디 입력을 확인한다.
        string id = _idInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            _messageTextUI.text = "아이디를 입력해주세요.";
            return;
        }

        // 1-1. 아이디 이메일 형식 검사
        if (!Regex.IsMatch(id, EmailPattern))
        {
            _messageTextUI.text = "아이디는 이메일 형식이어야 합니다.";
            return;
        }

        // 2. 비밀번호 입력을 확인한다.
        string password = _passwordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            _messageTextUI.text = "패스워드를 입력해주세요.";
            return;
        }

        // 2-1. 비밀번호 규칙 검사
        if (!Regex.IsMatch(password, PasswordPattern))
        {
            _messageTextUI.text = "비밀번호는 7-20자, 영문 대/소문자, 숫자, 특수문자 각 1개 이상 포함해야 합니다.";
            return;
        }

        // 3. 비밀번호 확인 입력을 확인한다.
        string password2 = _passwordConfirmInputField.text;
        if (string.IsNullOrEmpty(password2) || password != password2)
        {
            _messageTextUI.text = "패스워드를 확인해주세요.";
            return;
        }

        // 4. 실제 저장된 아이디-비밀번호 계정이 있는지 확인한다.
        // 4-1. 아이디가 있는지 확인한다.
        if (PlayerPrefs.HasKey(id))
        {
            _messageTextUI.text = "중복된 아이디입니다.";
            return;
        }

        // 5. 비밀번호를 해시화하여 저장
        string hashedPassword = HashPassword(password);
        PlayerPrefs.SetString(id, hashedPassword);
        _messageTextUI.text = "회원가입이 완료되었습니다.";

        GotoLogin();
    }

    private void GotoLogin()
    {
        _mode = SceneMode.Login;
        Refresh();
    }

    private void GotoRegister()
    {
        _mode = SceneMode.Register;
        Refresh();
    }
    
}
