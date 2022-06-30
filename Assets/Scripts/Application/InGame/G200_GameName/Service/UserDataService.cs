using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class UserDataService : Singleton<UserDataService>, IService
{
    public ServiceType type => ServiceType.UserData;

    public UserData userData;

    public int BestScore
    {
        get { return userData.bestScore; }
        set { userData.bestScore = Mathf.Max(userData.bestScore, value); }
    }

    public async Task<bool> Initialize(ServiceStatePresenter presenter)
    {
        var text = PersistenceUtil.LoadTextFile("UserData.txt");
        if (string.IsNullOrEmpty(text))
        {
            userData = new UserData
            {
                bestScore = 0
            };
        }
        else
        {
            userData = JsonConvert.DeserializeObject<UserData>(text);
        }

        return true;   
    }

    public bool SaveUserData()
    {
        var text = JsonConvert.SerializeObject(userData);
        return PersistenceUtil.SaveTextFile("UserData.txt", text);
    }
}
