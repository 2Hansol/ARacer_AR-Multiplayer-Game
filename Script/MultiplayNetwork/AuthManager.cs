 using Firebase;
using Firebase.Auth;
using Firebase.Extensions; //파이어 베이스 
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{


    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; }//로그인을 한번만 진행하도록
    public bool IsSignUpOnProgress { get; private set; } //로그인을 한번만 진행하도록

    public GameObject mainCanvas;
    public GameObject loginCanvas;
    public GameObject signUpCanvas;

  
    public Button signInButton;
    public Button signupButton;
    public Button loginButton;

    //해당 값들을 가져오기 위해서 
    public InputField login_emailField;
    public InputField login_passwordField;
    public InputField signUp_emailField;
    public InputField signUp_passwordField;

    //public Button BackButton;


    //외부에서 즉시 접근 가능하도록 ->static
    public static FirebaseApp firebaseApp; //전체 앱 관리
    public static FirebaseAuth firebaseAuth;
    public static FirebaseUser User; //이메일과 패스워드에 대응되는 유저정보를 가져오는거


    public void Start()
    {
        mainCanvas.SetActive(true);
        loginCanvas.SetActive(false);
        signUpCanvas.SetActive(false);
    }

    public void SignInStart()
    {
        mainCanvas.SetActive(false);
        loginCanvas.SetActive(true);
        signUpCanvas.SetActive(false);

        signInButton.interactable = false;
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
            signInButton.interactable = IsFirebaseReady;
        });
    }

    public void SignIn()
    {
        loginCanvas.SetActive(true);
        if (!IsFirebaseReady || IsSignInOnProgress || User != null)
        {
            return;
        }

        // 구글이나 애플계정으로 사용해서 로그인 할 수 있도록 firebaseAuth.SignInWithCredentialAsync
        firebaseAuth.SignInWithEmailAndPasswordAsync(login_emailField.text, login_passwordField.text).ContinueWithOnMainThread(
            continuation: (task) =>
            {
                IsSignInOnProgress = false;
                signInButton.interactable = true;

                if(task.IsFaulted)
                {
                    Debug.LogError(task.Exception);
                }
                else if(task.IsCanceled)
                {
                    Debug.LogError(message: "sign-in canceled");
                }
                else
                {
                    User = task.Result;
                    Debug.Log(User.Email);
                    SceneManager.LoadScene("CarSelection");
                    //SceneManager.LoadScene("Lobby");
                }
            });
    }



    public void SignUpStart()
    {
        mainCanvas.SetActive(false);
        loginCanvas.SetActive(false);
        signUpCanvas.SetActive(true);

        signupButton.interactable = false;
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

            signupButton.interactable = IsFirebaseReady;
        });
    }


    public void SignUp()
    {
        mainCanvas.SetActive(false);
        loginCanvas.SetActive(false);
        signUpCanvas.SetActive(true);
        if (!IsFirebaseReady || IsSignUpOnProgress || User != null)
        {
            return;
        }

        firebaseAuth.CreateUserWithEmailAndPasswordAsync(signUp_emailField.text, signUp_passwordField.text).ContinueWith(task =>
        {
            IsSignUpOnProgress = false;
            signupButton.interactable = true;

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

        mainCanvas.SetActive(true);
        loginCanvas.SetActive(false);
        signUpCanvas.SetActive(false);
    }

    public void Back()
    {
        mainCanvas.SetActive(true);
        loginCanvas.SetActive(false);
        signUpCanvas.SetActive(false);
    }

}