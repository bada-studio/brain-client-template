using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IntroSceneController : SceneControllerBase, ServiceStatePresenter {
    public override SceneType sceneType => SceneType.Intro;

    public async void Start() {
        Application.runInBackground = true;

        if (await Boot() == false) {
            SwitchScene(0).RunAsync();
            return;
        }
        
        //mainScene
        SwitchScene(SceneType.G100_GameName).RunAsync();
    }

    public void ShowServiceState(string key) {
    }

    private async Task<bool> Boot() {
        List<IService> services = new List<IService>();
        services.Add(Service.rule);
        return await InitializeServices(services);
    }
    
    private async Task<bool> InitializeServices(List<IService> services) {
        foreach (IService service in services) {
            bool result = await service.Initialize(this);
            if (!result) {
                Debug.LogError(service.type + "service load failed");
                return false;
            } else {
                Debug.Log(service.type + "service ready");
            }
        }

        return true;
    }
}
