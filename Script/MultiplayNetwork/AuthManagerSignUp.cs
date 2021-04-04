using Firebase;
using Firebase.Auth;
using Firebase.Extensions; //파이어 베이스 
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManagerSignUp : MonoBehaviour
{


    public bool IsFirebaseReady { get; private set; }

    public bool IsSignUpOnProgress { get; private set; } //로그인을 한번만 진행하도록


    //해당 값들을 가져오기 위해서 
    public InputField emailField;
    public InputField passwordField;
    public Button signUpButton;

    //외부에서 즉시 접근 가능하도록 ->static
    public static FirebaseApp firebaseApp; //전체 앱 관리
    public static FirebaseAuth firebaseAuth;
    public static FirebaseUser User; //이메일과 패스워드에 대응되는 유저정보를 가져오는거

    public bool SU = false;


    public void Start()
    {
        signUpButton.interactable = false;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result;
            if (result != DependencyStatus.Available)
            {
                Debug.LogError(message: result.ToString());
                IsFirebaseReady = false;
            }
            else
            {
                IsFirebaseReady = true;
                firebaseApp = FirebaseApp.DefaultInstance;
                firebaseAuth = FirebaseAuth.DefaultInstance;
            }

            signUpButton.interactable = IsFirebaseReady;
        });
    }

    public void SignUp()
    {
        if (!IsFirebaseReady || IsSignUpOnProgress || User != null)
        {
            return;
        }

        firebaseAuth.CreateUserWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWith(task =>
        {
            IsSignUpOnProgress = false;
            signUpButton.interactable = true;

            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCanceled)
            {
                Debug.LogError(message: "sign-UP canceled");
            }
            else
            {
                Firebase.Auth.FirebaseUser newUser = task.Result;
                //SU = true;
            }
        });

        SceneManager.LoadScene("SignIn");
        /*
        if (SU)
        {
            SceneManager.LoadScene("SignIn");
        }
        */
    }
    public void Back()
    {
        SceneManager.LoadScene("SignIn");
    }
}