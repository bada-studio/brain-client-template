using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace BCPG9 {
    public class BCPG9_RuleService : Singleton<BCPG9_RuleService>, IService {
        public ServiceType type => ServiceType.Rule;
        public Dictionary<int, BCPG9Rule> bcpg9Rule { get; private set; }

        public async Task<bool> Initialize(ServiceStatePresenter presenter) {
            LoadLocalRules();
            return true;
        }

        private void LoadLocalRules() {
            bcpg9Rule = new Dictionary<int, BCPG9Rule>();
            LoadLocalRule<BCPG9Rule>("G100_GameName/BCPG9").ForEach(e => {
                bcpg9Rule.Add(e.index, e);
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
}