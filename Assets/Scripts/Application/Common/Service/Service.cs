using System.Threading.Tasks;
using UnityEngine;

public class Service : SingletonGameObject<Service> {
    // Services
    //-------------------------------------------------------------------------
    public static SceneController scene { get; set; }
    public static RuleService rule => RuleService.instance;
    
    private void Awake() {
        DontDestroyOnLoad(gameObject);
        
        InitLogHandler();
    }

    private void InitLogHandler() {
        Debug.Log("SERVICE STARTED!");
    }
}

public static class AsyncRunner { 
    public static async void RunAsync(this Task task) {
        await task;
    }
}
