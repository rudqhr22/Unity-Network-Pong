using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; }

    public InputField emailField;
    public InputField passwordField;
    public Button signInButton;

    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;

    public static FirebaseUser User;

    public void Start()
    {   
        signInButton.interactable = false;
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            DependencyStatus dependencyStatus = task.Result;
            if(dependencyStatus != DependencyStatus.Available)
            {
                Debug.LogError(task.Result.ToString());
                IsFirebaseReady = false;
            }
            else
            {
                Debug.Log(task.Result.ToString());
                IsFirebaseReady = true;
                firebaseApp = FirebaseApp.DefaultInstance;
                firebaseAuth = FirebaseAuth.DefaultInstance;
                signInButton.interactable = true;
            }
            
        });
    }

    public void SignIn()
    {
        if(!IsFirebaseReady || IsSignInOnProgress || User != null)
        {
            return;
        }

        IsSignInOnProgress = true;
        signInButton.interactable = false;


        Debug.Log(emailField.text + " " + passwordField.text);

        firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(task=> {

            Debug.Log("SignInWithEmailAndPasswordAsync");

            IsSignInOnProgress = false;
            signInButton.interactable = true;

            if(task.IsFaulted)
            {
                Debug.LogError(task.Result.ToString() + " " + task.Exception);                
            }
            else if(task.IsCanceled)
            {
                Debug.LogError(task.Result.ToString());
            }
            else
            {
                Debug.Log("LObby");
                User = task.Result;                                     //FirebaseUser
                SceneManager.LoadScene("Lobby");
                Debug.Log("Login success");
            }
        });


    }
}