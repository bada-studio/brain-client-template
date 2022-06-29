/* 
* Copyright (C) Decartes Corp. All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* 
* @author You sungHyun <1seconds@biscuitlabs.io>
* @created 2022/06/29
* @desc RuleService Template
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class RuleService : Singleton<RuleService>, IService {
    public ServiceType type => ServiceType.Rule;
    public Dictionary<int, G100Rule> g100Rule { get; private set; }

    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        LoadLocalRules();
        return true;
    }

    private void LoadLocalRules() {
        g100Rule = new Dictionary<int, G100Rule>();
        LoadLocalRule<G100Rule>("G100_GameName/G100").ForEach(e => {
            g100Rule.Add(e.index, e);
        });
    }
    
    private List<T> LoadLocalRule<T>(string file) {
        var text = PersistenceUtil.LoadTextResource("Rules/" + file);
        if (string.IsNullOrEmpty(text)) {
            return new List<T>();
        }

        var res = JsonConvert.DeserializeObject<List<T>>(text);
        return res;
    }
}